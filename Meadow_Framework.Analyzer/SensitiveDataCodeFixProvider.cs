using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Meadow_Framework.Analyzer;

/// <summary>
/// 
/// </summary>
[ExportCodeFixProvider(LanguageNames.CSharp)]
[Shared]
public sealed class SensitiveDataCodeFixProvider : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds =>
        ImmutableArray.Create(SensitiveDataPropertyAnalyzer.DiagnosticId);

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public override FixAllProvider? GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    /// <summary>
    ///
    /// </summary>
    /// <param name="context"></param>
    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var diagnostic = context.Diagnostics.Single();
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken);
        if (root == null) return;

        var node = root.FindNode(diagnostic.Location.SourceSpan);

        if (node is not PropertyDeclarationSyntax property)
            return;

        context.RegisterCodeFix(
            CodeAction.Create(
                "Add [SensitiveData]",
                c => AddAttributeAsync(context.Document, root, property, c),
                nameof(SensitiveDataCodeFixProvider)),
            diagnostic);
    }

    private static Task<Document> AddAttributeAsync(
        Document document,
        SyntaxNode root,
        PropertyDeclarationSyntax property,
        CancellationToken cancellationToken)
    {
        var attribute =
            SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("SensitiveData"));

        var attributeList =
            SyntaxFactory.AttributeList(
                SyntaxFactory.SingletonSeparatedList(attribute));

        var newProperty =
            property.AddAttributeLists(attributeList);

        var newRoot = root.ReplaceNode(property, newProperty);
        return Task.FromResult(document.WithSyntaxRoot(newRoot));
    }
}
