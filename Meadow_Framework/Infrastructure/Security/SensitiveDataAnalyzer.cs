using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Meadow_Framework.Infrastructure.Security;

/// <summary>
///
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class SensitiveDataAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "SD001";

    private static readonly DiagnosticDescriptor Rule = new(
        DiagnosticId,
        "Consider marking sensitive property",
        "Property '{0}' may contain sensitive data. Consider applying [SensitiveData].",
        "Security",
        DiagnosticSeverity.Info,
        isEnabledByDefault: true);

    /// <summary>
    ///
    /// </summary>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

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

        if (!IsStringOrDecimal(property.Type)) return;
        if (!IsDerivedFrom(property.ContainingType, "IntegrationBaseEvent")) return;

        // Heuristic: property names that may be sensitive
        var sensitiveNames = new[] { "Name", "Email", "SSN", "Password", "UserName" };
        if (!sensitiveNames.Contains(property.Name)) return;

        // Skip if already marked
        var hasAttribute = property.GetAttributes().Any(a => a.AttributeClass?.Name == "SensitiveDataAttribute");
        if (!hasAttribute)
        {
            var diagnostic = Diagnostic.Create(Rule, property.Locations[0], property.Name);
            context.ReportDiagnostic(diagnostic);
        }
    }

    private static bool IsDerivedFrom(ITypeSymbol type, string baseTypeName)
    {
        while (type != null)
        {
            if (type.Name == baseTypeName) return true;
            type = type.BaseType;
        }
        return false;
    }

    private static bool IsStringOrDecimal(ITypeSymbol type)
    {
        if (type.SpecialType == SpecialType.System_String || type.SpecialType == SpecialType.System_Decimal)
            return true;

        // Handle List<string>
        if (type is INamedTypeSymbol namedType &&
            namedType.IsGenericType &&
            namedType.TypeArguments.Length == 1 &&
            namedType.TypeArguments[0].SpecialType == SpecialType.System_String)
        {
            return true;
        }

        return false;
    }
}