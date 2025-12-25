using System.Collections.Immutable;
using System.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;

namespace Meadow_Framework.Infrastructure.Security;

/// <summary>
///
/// </summary>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(SensitiveDataCodeFixProvider)), Shared]
public class SensitiveDataCodeFixProvider : CodeFixProvider
{
    /// <summary>
    ///
    /// </summary>
    public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(SensitiveDataAnalyzer.DiagnosticId);

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    /// <summary>
    ///
    /// </summary>
    /// <param name="context"></param>
    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        if (root == null) return;

        var diagnostic = context.Diagnostics.First();
        var diagnosticSpan = diagnostic.Location.SourceSpan;

        var propertyDecl = root.FindToken(diagnosticSpan.Start)
            .Parent
            .AncestorsAndSelf()
            .OfType<PropertyDeclarationSyntax>()
            .FirstOrDefault();
        if (propertyDecl == null) return;

        context.RegisterCodeFix(
            CodeAction.Create(
                "Add [SensitiveData]",
                cancellationToken => AddSensitiveDataAttributeAsync(context.Document, propertyDecl, cancellationToken),
                nameof(SensitiveDataCodeFixProvider)),
            diagnostic);
    }

    private async Task<Document> AddSensitiveDataAttributeAsync(Document document, PropertyDeclarationSyntax propertyDecl, CancellationToken cancellationToken)
    {
        var editor = await DocumentEditor.CreateAsync(document, cancellationToken).ConfigureAwait(false);

        var attribute = Microsoft.CodeAnalysis.CSharp.SyntaxFactory.Attribute(Microsoft.CodeAnalysis.CSharp.SyntaxFactory.ParseName("SensitiveData"));
        var attributeList = Microsoft.CodeAnalysis.CSharp.SyntaxFactory.AttributeList(Microsoft.CodeAnalysis.CSharp.SyntaxFactory.SingletonSeparatedList(attribute))
            .WithTrailingTrivia(Microsoft.CodeAnalysis.CSharp.SyntaxFactory.Whitespace("\n"));

        editor.AddAttribute(propertyDecl, attributeList);

        return editor.GetChangedDocument();
    }
}