using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Threax.AspNetCore.Models;

namespace Threax.ModelGen
{
    public static class JsonSchemaExtensions
    {
        public static string GetClrType(this JsonProperty prop)
        {
            String type = GetNonArrayType(prop.Type, prop.Format);

            if (IsType(prop.Type, JsonObjectType.Array))
            {
                type = $"List<{GetNonArrayType(prop.Item.Type, prop.Item.Format)}>";
            }

            return type;
        }

        /// <summary>
        /// Determine if the property represents a type that is a clr value type as opposed to a reference type.
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static bool IsClrValueType(this JsonProperty prop)
        {
            //See if the clr type was provided
            Object fullClrType = prop.GetClrFullTypeName();
            if (fullClrType != null)
            {
                //Look for type in all loaded assemblies
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    var type = assembly.GetType(fullClrType.ToString());
                    if (type != null)
                    {
                        return type.IsValueType;
                    }
                }
            }

            //Otherwise go off the schema type
            switch (prop.Type)
            {
                case JsonObjectType.Boolean:
                case JsonObjectType.Integer:
                case JsonObjectType.Number:
                    return true;
                default:
                    return false;
            }
        }

        public static string GetNonArrayType(JsonObjectType types, String format)
        {
            var loweredFormat = format;
            if (loweredFormat != null)
            {
                loweredFormat = loweredFormat.ToLowerInvariant();
            }

            var type = "String";

            if (IsType(types, JsonObjectType.Integer) || IsType(types, JsonObjectType.Number))
            {
                String simpleType = "decimal";
                if (IsType(types, JsonObjectType.Integer))
                {
                    simpleType = "int";
                    switch (loweredFormat)
                    {
                        case "int16":
                            simpleType = "short";
                            break;
                        case "int32":
                            simpleType = "int";
                            break;
                        case "int64":
                            simpleType = "long";
                            break;
                    }
                }
                if (IsType(types, JsonObjectType.Number))
                {
                    simpleType = "decimal";
                    switch (loweredFormat)
                    {
                        case "int16":
                            simpleType = "short";
                            break;
                        case "int32":
                            simpleType = "int";
                            break;
                        case "int64":
                            simpleType = "long";
                            break;
                        case "single":
                            simpleType = "float";
                            break;
                        case "double":
                            simpleType = "double";
                            break;
                        case "decimal":
                            simpleType = "decimal";
                            break;
                    }
                }
                type = simpleType + GetNullable(types);
            }
            if (IsType(types, JsonObjectType.Boolean))
            {
                type = "bool" + GetNullable(types);
            }
            if (IsType(types, JsonObjectType.String) || IsType(types, JsonObjectType.File))
            {
                type = "String";
                switch (loweredFormat)
                {
                    case "date":
                    case "time":
                    case "date-time":
                        type = "DateTime" + GetNullable(types); //Overrides completely since date time can be nullable
                        break;
                    case "guid": //Guids come in as strings sometimes
                        type = "Guid";
                        break;
                }
            }

            if (IsType(types, JsonObjectType.Object))
            {
                type = "Object";
                if (format != null) //Use format to specify the real object type
                {
                    type = format;
                }
            }

            return type;
        }

        public static String GetNullable(JsonObjectType types)
        {
            String extra = "";
            if (IsType(types, JsonObjectType.Null))
            {
                extra += "?";
            }
            return extra;
        }

        public static bool IsType(this JsonProperty prop, JsonObjectType type)
        {
            return IsType(prop.Type, type);
        }

        public static bool IsType(JsonObjectType types, JsonObjectType type)
        {
            //Since none is 0, treat it special
            if(type == JsonObjectType.None)
            {
                return types == JsonObjectType.None;
            }
            return (types & type) == type;
        }

        public const String ClrFullTypeName = "x-clr-fullname";

        public static String GetClrFullTypeName(this JsonProperty prop)
        {
            Object data = null;
            if (prop.ExtensionData?.TryGetValue(ClrFullTypeName, out data) == true)
            {
                return data?.ToString();
            }
            return null;
        }

        public static void SetClrFullTypeName(this JsonProperty prop, String fullName)
        {
            prop.EnsureExtensions();
            prop.ExtensionData[ClrFullTypeName] = fullName;
        }

        public static void EnsureExtensions(this JsonSchema4 schema)
        {
            if(schema.ExtensionData == null)
            {
                schema.ExtensionData = new Dictionary<String, Object>();
            }
        }

        /// <summary>
        /// Determine if a particular schema or property is on all types of models or not.
        /// </summary>
        /// <param name="schema"></param>
        public static bool OnAllModelTypes(this JsonSchema4 schema)
        {
            return schema.CreateEntity() && schema.CreateInputModel() && schema.CreateViewModel();
        }
    }
}
