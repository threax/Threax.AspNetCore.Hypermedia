using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Threax.AspNetCore.Halcyon.Ext.UIAttrs
{
    public enum OperationType
    {
        And,
        Or,
        Equal,
        NotEqual,
        GreaterThan,
        LessThan,
        GreaterThanOrEqual,
        LessThanOrEqual
    }

    public class DisplayExpressionNode
    {
        private static readonly Dictionary<ExpressionType, OperationType> OperationMap = new Dictionary<ExpressionType, OperationType>
        {
            { ExpressionType.AndAlso, OperationType.And },
            { ExpressionType.OrElse, OperationType.Or },
            { ExpressionType.Equal, OperationType.Equal },
            { ExpressionType.NotEqual, OperationType.NotEqual },
            { ExpressionType.GreaterThan, OperationType.GreaterThan },
            { ExpressionType.LessThan, OperationType.LessThan },
            { ExpressionType.GreaterThanOrEqual, OperationType.GreaterThanOrEqual },
            { ExpressionType.LessThanOrEqual, OperationType.LessThanOrEqual }
        };

        public DisplayExpressionNode(Expression expression)
        {
            BinaryExpression binaryExpression;
            MemberExpression memberExpression;
            ConstantExpression constantExpression = null;

            switch (expression.NodeType)
            {
                case ExpressionType.AndAlso:
                case ExpressionType.OrElse:
                    binaryExpression = expression as BinaryExpression;
                    Left = new DisplayExpressionNode(binaryExpression.Left);
                    Right = new DisplayExpressionNode(binaryExpression.Right);
                    break;
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.LessThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LessThanOrEqual:
                    binaryExpression = expression as BinaryExpression;
                    if((memberExpression = binaryExpression.Left as MemberExpression) != null)
                    {
                        constantExpression = binaryExpression.Right as ConstantExpression;
                    }
                    else if((memberExpression = binaryExpression.Right as MemberExpression) != null)
                    {
                        constantExpression = binaryExpression.Left as ConstantExpression;
                    }
                    else
                    {
                        throw new NotSupportedException($"Display expressions must have one member expression.");
                    }

                    if (constantExpression == null)
                    {
                        throw new NotSupportedException($"Display expressions only support a member expression on one side. You must use a constant on the other side.");
                    }

                    Test = new Dictionary<String, Object>() { { memberExpression.Member.Name, constantExpression.Value } };

                    break;
                default:
                    throw new NotSupportedException($"Display Expressions do not support Linq expression type {expression.NodeType}. Please modify your expression tree to only include supported operations.");
            }

            this.Operation = OperationMap[expression.NodeType];
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DisplayExpressionNode Left { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DisplayExpressionNode Right { get; set; }

        /// <summary>
        /// Use a dictionary to put out the test conditions so the property name is serialized correctly by the
        /// json serializer based on its current settings.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<String, Object> Test { get; set; }

        [JsonProperty]
        public OperationType Operation { get; set; }
    }
}
