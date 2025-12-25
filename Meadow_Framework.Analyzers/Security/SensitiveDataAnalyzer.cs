using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Meadow_Framework.Analyzers.Security;

/// <summary>
///
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class SensitiveDataAnalyzer : DiagnosticAnalyzer
{
    /// <summary>
    ///
    /// </summary>
    public const string DiagnosticId = "SD001";

    private static readonly DiagnosticDescriptor Rule = new(
        DiagnosticId,
        "Consider marking sensitive property",
        "Property '{0}' may contain sensitive data. Consider applying [SensitiveData].",
        "Security",
        DiagnosticSeverity.Info,
        isEnabledByDefault: true);

    private static readonly ImmutableHashSet<string> SensitiveNames =
        ImmutableHashSet.Create(StringComparer.OrdinalIgnoreCase,
            "Name", "FirstName", "LastName", "Email", "SSN", "Password",
            "UserName", "Username", "Login", "ApiKey", "Secret", "Token");

    /// <summary>
    ///
    /// </summary>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        => ImmutableArray.Create(Rule);

    /// <summary>
    ///
    /// </summary>
    /// <param name="context"></param>
    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.RegisterSymbolAction(AnalyzeProperty, SymbolKind.Property);
    }

    private static void AnalyzeProperty(SymbolAnalysisContext context)
    {
        var property = (IPropertySymbol)context.Symbol;

        if (!SensitiveNames.Contains(property.Name))
            return;

        if (!IsStringOrDecimal(property.Type))
            return;

        if (!DerivesFromTypeWithName(property.ContainingType, "IntegrationBaseEvent"))
            return;

        if (property.GetAttributes()
                    .Any(a => a.AttributeClass?.Name is "SensitiveDataAttribute" or "SensitiveData"))
            return;

        var diagnostic = Diagnostic.Create(Rule, property.Locations[0], property.Name);
        context.ReportDiagnostic(diagnostic);
    }

    private static bool DerivesFromTypeWithName(ITypeSymbol? type, string simpleName)
    {
        while (type != null)
        {
            if (type.Name == simpleName)
                return true;
            type = type.BaseType;
        }
        return false;
    }

    private static bool IsStringOrDecimal(ITypeSymbol type)
    {
        if (type.SpecialType is SpecialType.System_String or SpecialType.System_Decimal)
            return true;

        if (type is INamedTypeSymbol { IsGenericType: true } nt &&
            nt.TypeArguments.Length == 1 &&
            nt.TypeArguments[0].SpecialType == SpecialType.System_String)
            return true;

        return false;
    }
}