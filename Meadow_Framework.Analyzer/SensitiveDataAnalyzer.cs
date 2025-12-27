using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using static Microsoft.CodeAnalysis.LanguageNames;

namespace Meadow_Framework.Analyzer;

/// <summary>
///
/// </summary>
[DiagnosticAnalyzer(CSharp)]
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

    private static readonly ImmutableHashSet<string> DefaultSensitiveNames =
        ImmutableHashSet.Create(
            StringComparer.OrdinalIgnoreCase,
            "Name",
            "Email",
            "Password",
            "Phone",
            "Address");

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
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterCompilationStartAction(startContext =>
        {
            var sensitiveNames = GetSensitiveNames(startContext.Options);

            startContext.RegisterSymbolAction(
                ctx => AnalyzeProperty(ctx, sensitiveNames),
                SymbolKind.Property);
        });
    }

    private static void AnalyzeProperty(
        SymbolAnalysisContext context,
        ImmutableHashSet<string> sensitiveNames)
    {
        var property = (IPropertySymbol)context.Symbol;

        // Only string properties
        if (property.Type.SpecialType != SpecialType.System_String)
            return;

        // Name heuristic
        if (!sensitiveNames.Contains(property.Name))
            return;

        // Already annotated
        if (HasSensitiveAttribute(property))
            return;

        context.ReportDiagnostic(
            Diagnostic.Create(
                Rule,
                property.Locations[0],
                property.Name));
    }

    private static ImmutableHashSet<string> GetSensitiveNames(AnalyzerOptions options)
    {
        if (options.AnalyzerConfigOptionsProvider.GlobalOptions
            .TryGetValue("dotnet_diagnostic.SD001.sensitive_names", out string? value))
        {
            IEnumerable<string> names = value
                .Split([','], StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x));

            List<string> enumerable = names.ToList();
            return enumerable.Any()
                ? enumerable.ToImmutableHashSet(StringComparer.OrdinalIgnoreCase)
                : DefaultSensitiveNames;
        }

        return DefaultSensitiveNames;
    }

    private static bool HasSensitiveAttribute(ISymbol symbol)
    {
        return symbol.GetAttributes()
            .Any(a => a.AttributeClass?.Name == "SensitiveDataAttribute");
    }
}
