using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace SensitiveDataAnalyzer;

/// <summary>
///
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class SensitiveDataPropertyAnalyzer : DiagnosticAnalyzer
{
    /// <summary>
    ///
    /// </summary>
    public const string DiagnosticId = "SD001";

    private static readonly DiagnosticDescriptor Rule =
        new DiagnosticDescriptor(
            DiagnosticId,
            "Sensitive data property should be annotated",
            "Property '{0}' looks like sensitive data and should be marked with [SensitiveData]",
            "Security",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

    private static readonly ImmutableHashSet<string> SensitiveNames =
        ImmutableHashSet.Create(
            StringComparer.OrdinalIgnoreCase,
            "Name",
            "email",
            "password",
            "phone",
            "address");

    /// <summary>
    ///
    /// </summary>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
        ImmutableArray.Create(Rule);

    /// <summary>
    ///
    /// </summary>
    /// <param name="context"></param>
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterSymbolAction(AnalyzeProperty, SymbolKind.Property);
    }

    private static void AnalyzeProperty(SymbolAnalysisContext context)
    {
        var property = (IPropertySymbol)context.Symbol;

        // Only strings
        if (property.Type.SpecialType != SpecialType.System_String)
            return;

        // Name heuristic
        if (!SensitiveNames.Contains(property.Name))
            return;

        // Already has [SensitiveData]
        if (HasSensitiveAttribute(property))
            return;

        var diagnostic = Diagnostic.Create(
            Rule,
            property.Locations[0],
            property.Name);

        context.ReportDiagnostic(diagnostic);
    }

    private static bool HasSensitiveAttribute(ISymbol symbol)
    {
        return Enumerable.Any(symbol.GetAttributes(), attr => attr.AttributeClass?.Name == "SensitiveDataAttribute");
    }
}
