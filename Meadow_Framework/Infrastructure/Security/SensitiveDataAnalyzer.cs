using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Meadow_Framework.Infrastructure.Security;

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

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.RegisterSymbolAction(AnalyzeProperty, SymbolKind.Property);
    }

    private static void AnalyzeProperty(SymbolAnalysisContext context)
    {
        var property = (IPropertySymbol)context.Symbol;

        // Only string or decimal properties (you can expand types)
        if (property.Type.SpecialType != SpecialType.System_String && property.Type.SpecialType != SpecialType.System_Decimal)
            return;

        // Only properties inside IntegrationBaseEvent subclasses
        var containingClass = property.ContainingType;
        if (!containingClass.BaseType?.Name.EndsWith("IntegrationBaseEvent") ?? true)
            return;

        // Heuristic: property names that are likely sensitive
        var sensitiveNames = new[] { "Name", "Email", "SSN", "Password", "Price" };
        if (!sensitiveNames.Contains(property.Name))
            return;

        // Only report if [SensitiveData] is not already applied
        var hasAttribute = property.GetAttributes().Any(a => a.AttributeClass?.Name == "SensitiveDataAttribute");
        if (!hasAttribute)
        {
            var diagnostic = Diagnostic.Create(Rule, property.Locations[0], property.Name);
            context.ReportDiagnostic(diagnostic);
        }
    }
}