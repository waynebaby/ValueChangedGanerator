﻿using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace ValueChangedGanerator.DataModels
{
    static class SyntaxExtensions
    {
        private static readonly SyntaxToken PartialToken = Token(SyntaxKind.PartialKeyword);

        public static ClassDeclarationSyntax AddPartialModifier(this ClassDeclarationSyntax typeDecl)
        {
            if (typeDecl.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword))) return typeDecl;
            return typeDecl.AddModifiers(new[] { PartialToken });
        }

        public static ClassDeclarationSyntax GetPartialTypeDelaration(this ClassDeclarationSyntax typeDecl)
            => CSharpSyntaxTree.ParseText($@"
partial class {typeDecl.Identifier.Text}
{{
}}
").GetRoot().ChildNodes().OfType<ClassDeclarationSyntax>().First();
    }
}
