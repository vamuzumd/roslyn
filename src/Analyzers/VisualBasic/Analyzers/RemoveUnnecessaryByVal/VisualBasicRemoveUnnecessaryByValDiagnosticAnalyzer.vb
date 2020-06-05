﻿' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Collections.Immutable
Imports Microsoft.CodeAnalysis.CodeStyle
Imports Microsoft.CodeAnalysis.Diagnostics
Imports Microsoft.CodeAnalysis.VisualBasic.Syntax

Namespace Microsoft.CodeAnalysis.VisualBasic.RemoveUnnecessaryByVal
    <DiagnosticAnalyzer(LanguageNames.VisualBasic)>
    Friend Class VisualBasicRemoveUnnecessaryByValDiagnosticAnalyzer
        Inherits AbstractBuiltInCodeStyleDiagnosticAnalyzer

        Private Shared ReadOnly s_descriptor As New DiagnosticDescriptor(
            id:=IDEDiagnosticIds.RemoveUnnecessaryByValDiagnosticId,
            title:="",
            messageFormat:="",
            category:="",
            defaultSeverity:=DiagnosticSeverity.Hidden,
            isEnabledByDefault:=True,
            description:="",
            helpLinkUri:="",
            WellKnownDiagnosticTags.Unnecessary)

        Public Sub New()
            MyBase.New(
                diagnosticId:=IDEDiagnosticIds.RemoveUnnecessaryByValDiagnosticId,
                [option]:=Nothing,
                title:=New LocalizableResourceString(NameOf(VisualBasicAnalyzersResources.Remove_ByVal), VisualBasicAnalyzersResources.ResourceManager, GetType(VisualBasicAnalyzersResources)))
        End Sub

        Protected Overrides Sub InitializeWorker(context As AnalysisContext)
            context.RegisterSyntaxNodeAction(
                Sub(syntaxContext As SyntaxNodeAnalysisContext)
                    Dim parameterSyntax = TryCast(syntaxContext.Node, ParameterSyntax)
                    If parameterSyntax Is Nothing Then
                        Return
                    End If
                    For Each modifier In parameterSyntax.Modifiers
                        If modifier.IsKind(SyntaxKind.ByValKeyword) Then
                            syntaxContext.ReportDiagnostic(Diagnostic.Create(s_descriptor, modifier.GetLocation()))
                        End If
                    Next

                End Sub, SyntaxKind.Parameter)
        End Sub

        Public Overrides Function GetAnalyzerCategory() As DiagnosticAnalyzerCategory
            Return DiagnosticAnalyzerCategory.SemanticSpanAnalysis
        End Function
    End Class
End Namespace
