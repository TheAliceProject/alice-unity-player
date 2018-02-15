using System;
using System.Reflection;
using System.IO;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using UnityEngine;

namespace GenerateCodeDom
{

    public class GenerateClass {

        private CodeCompileUnit targetUnit;
        private CodeTypeDeclaration targetClass;

        public static void Main(string className)
        {
            GenerateClass newClass = new GenerateClass(className);
            newClass.AddFields();
            newClass.AddProperties();
            newClass.AddMethod();
            newClass.AddConstructor();
            newClass.GenerateCSharpCode(className + ".cs");
        }

        public GenerateClass(string className)
        {
            targetUnit = new CodeCompileUnit();
            CodeNamespace namespaceName = new CodeNamespace("CodeDomClass");
            namespaceName.Imports.Add(new CodeNamespaceImport("System"));
            targetClass = new CodeTypeDeclaration(className);
            targetClass.IsClass = true;
            targetClass.TypeAttributes =
                TypeAttributes.Public | TypeAttributes.Sealed;
            namespaceName.Types.Add(targetClass);
            targetUnit.Namespaces.Add(namespaceName);
        }

        public void AddFields()
        {
            CodeMemberField valueField = new CodeMemberField();
            valueField.Attributes = MemberAttributes.Private;
            valueField.Name = "valueField";
            valueField.Type = new CodeTypeReference(typeof(System.Double));
            valueField.Comments.Add(new CodeCommentStatement("Object value"));
            targetClass.Members.Add(valueField);
        }

        public void AddProperties()
        {
            CodeMemberProperty valueProperty = new CodeMemberProperty();
            valueProperty.Attributes = MemberAttributes.Private | MemberAttributes.Final;
            valueProperty.Name = "valueProperty";
            valueProperty.HasGet = true;
            valueProperty.Type = new CodeTypeReference(typeof(System.Double));
            valueProperty.GetStatements.Add(new CodeMethodReturnStatement(
                new CodeFieldReferenceExpression(
                    new CodeThisReferenceExpression(), "valueField"
            )));
            targetClass.Members.Add(valueProperty);


            CodeBinaryOperatorExpression valueExpress = 
                new CodeBinaryOperatorExpression(
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(), "valueField"
                    ),
                    CodeBinaryOperatorType.Multiply,
                    new CodePrimitiveExpression(4.0)
                );
            // NOT ADDED TO ANYTHING YET
        }

        public void AddMethod()
        {
            CodeMemberMethod toStringMethod = new CodeMemberMethod();
            toStringMethod.Attributes = MemberAttributes.Public | MemberAttributes.Override;
            toStringMethod.Name = "ToString";
            toStringMethod.ReturnType = new CodeTypeReference(typeof(System.String));

            CodeFieldReferenceExpression valueRef = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "valueProperty");
            CodeMethodReturnStatement returnStatement = new CodeMethodReturnStatement();

            string formattedOutput = "Object value: {0}";
            returnStatement.Expression = new CodeMethodInvokeExpression(
                new CodeTypeReferenceExpression("System.String"), "Format",
                new CodePrimitiveExpression(formattedOutput),
                valueRef);
            toStringMethod.Statements.Add(returnStatement);
            targetClass.Members.Add(toStringMethod);
        }

        public void AddConstructor()
        {
            CodeConstructor constructor = new CodeConstructor();
            constructor.Attributes = MemberAttributes.Public | MemberAttributes.Final;

            constructor.Parameters.Add(new CodeParameterDeclarationExpression(typeof(System.Double), "value"));
            CodeFieldReferenceExpression valueRef = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "valueField");
            constructor.Statements.Add(new CodeAssignStatement(valueRef, new CodeArgumentReferenceExpression("value")));
            targetClass.Members.Add(constructor);
        }

        public void GenerateCSharpCode(string fileName)
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BracingStyle = "C";
            using (StreamWriter sourceWriter = new StreamWriter(fileName))
            {
                provider.GenerateCodeFromCompileUnit(
                    targetUnit, sourceWriter, options);
            }
        }
    }

}