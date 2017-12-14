using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public enum OperationType
    {
        And,
        Or,
        Not,
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
            { ExpressionType.LessThanOrEqual, OperationType.LessThanOrEqual },
            { ExpressionType.Not, OperationType.Not }
        };

        public DisplayExpressionNode(Expression expression)
        {
            BinaryExpression binaryExpression = null;
            MemberExpression memberExpression = null;
            ConstantExpression constantExpression = null;
            UnaryExpression unaryExpression = null;

            switch (expression.NodeType)
            {
                case ExpressionType.AndAlso:
                case ExpressionType.OrElse:
                    binaryExpression = expression as BinaryExpression;
                    Left = new DisplayExpressionNode(binaryExpression.Left);
                    Right = new DisplayExpressionNode(binaryExpression.Right);
                    this.Operation = OperationMap[expression.NodeType];
                    break;

                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.LessThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LessThanOrEqual:
                    binaryExpression = expression as BinaryExpression;
                    if ((memberExpression = binaryExpression.Left as MemberExpression) != null)
                    {
                        constantExpression = FindConstantExpression(binaryExpression.Right);
                    }
                    else if ((memberExpression = binaryExpression.Right as MemberExpression) != null)
                    {
                        constantExpression = FindConstantExpression(binaryExpression.Left);
                    }
                    else
                    {
                        //Enums come in as unary convert expressions over member expressions, so check for that also
                        if ((unaryExpression = binaryExpression.Left as UnaryExpression) != null)
                        {
                            memberExpression = unaryExpression.Operand as MemberExpression;
                            constantExpression = FindConstantExpression(binaryExpression.Right);
                        }
                        else if ((unaryExpression = binaryExpression.Right as UnaryExpression) != null)
                        {
                            memberExpression = unaryExpression.Operand as MemberExpression;
                            constantExpression = FindConstantExpression(binaryExpression.Left);
                        }
                    }

                    if (memberExpression == null)
                    {
                        throw new NotSupportedException($"Display expressions must have one member expression.");
                    }

                    if (constantExpression == null)
                    {
                        throw new NotSupportedException($"Display expressions only support a member expression on one side. You must use a constant on the other side.");
                    }

                    //See if the value is an enum, and if so convert it back
                    var memberTypeInfo = memberExpression.Type.GetTypeInfo();
                    if (memberTypeInfo.IsEnum)
                    {
                        Test = new Dictionary<String, Object>() { {
                                memberExpression.Member.Name,
                                Enum.ToObject(memberExpression.Type, constantExpression.Value)
                                //Enum.Parse(memberExpression.Type, Enum.GetName(memberExpression.Type, constantExpression.Value)) //Alternative method
                            } };
                    }
                    else
                    {
                        Test = new Dictionary<String, Object>() { { memberExpression.Member.Name, constantExpression.Value } };
                    }

                    this.Operation = OperationMap[expression.NodeType];
                    break;

                case ExpressionType.MemberAccess:
                    memberExpression = expression as MemberExpression;
                    if (memberExpression.Member.MemberType == MemberTypes.Property)
                    {
                        var propInfo = memberExpression.Member as PropertyInfo;
                        if (propInfo.PropertyType == typeof(bool))
                        {
                            Test = new Dictionary<String, Object>() { { memberExpression.Member.Name, true } };
                        }
                        else
                        {
                            throw new NotSupportedException("Only boolean properties are supported for member expressions.");
                        }
                    }
                    else
                    {
                        throw new NotSupportedException("The member expression must be a property.");
                    }
                    this.Operation = OperationType.Equal;
                    break;

                case ExpressionType.Not:
                    unaryExpression = expression as UnaryExpression;
                    Left = new DisplayExpressionNode(unaryExpression.Operand);
                    this.Operation = OperationMap[expression.NodeType];
                    break;
                default:
                    throw new NotSupportedException($"Display Expressions do not support Linq expression type {expression.NodeType}. Please modify your expression tree to only include supported operations.");
            }
        }

        private ConstantExpression FindConstantExpression(Expression source)
        {
            UnaryExpression unaryExpression = null;
            var constantExpression = source as ConstantExpression;
            if (constantExpression == null) //If the constant expression is null, there might be a nullable enum or value, try to get the real constant value out
            {
                unaryExpression = source as UnaryExpression;
                if (unaryExpression != null)
                {
                    constantExpression = unaryExpression.Operand as ConstantExpression;
                }
            }
            return constantExpression;
        }

        /// <summary>
        /// The left side of the expression.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DisplayExpressionNode Left { get; set; }

        /// <summary>
        /// The right side of the expression.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DisplayExpressionNode Right { get; set; }

        /// <summary>
        /// The test condition, the key is the property to lookup and the value is the value.
        /// </summary>
        /// <remarks>
        /// Use a dictionary to put out the test conditions so the property name is serialized correctly by the
        /// json serializer based on its current settings.
        /// </remarks>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<String, Object> Test { get; set; }

        /// <summary>
        /// The type of operation this node performes.
        /// </summary>
        [JsonProperty]
        public OperationType Operation { get; set; }
    }
}
