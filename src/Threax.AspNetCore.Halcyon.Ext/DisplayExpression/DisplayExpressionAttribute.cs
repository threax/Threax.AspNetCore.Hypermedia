using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DisplayExpressionAttribute : CustomizeSchemaAttribute, ISchemaCustomizer
    {
        private const String DisplayIfName = "x-display-if";

        private String expressionName;

        public DisplayExpressionAttribute(String expressionName)
            : base(null)
        {
            this.expressionName = expressionName;
        }

        public Task Customize(SchemaCustomizerArgs args)
        {
            var schemaProp = args.SchemaProperty;

            if (schemaProp.ExtensionData == null)
            {
                schemaProp.ExtensionData = new Dictionary<String, Object>();
            }

            var expressionFieldInfo = args.TypeProperty.DeclaringType.GetTypeInfo().DeclaredFields.First(i => i.Name == expressionName);
            var expression = expressionFieldInfo.GetValue(null) as LambdaExpression;
            schemaProp.ExtensionData.Add(DisplayIfName, new DisplayExpressionNode(expression.Body));

            return Task.FromResult(0);
        }
    }
}
