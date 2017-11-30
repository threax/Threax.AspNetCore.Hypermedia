using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            switch (prop.Type)
            {
                case JsonObjectType.Boolean:
                case JsonObjectType.Integer:
                case JsonObjectType.Number:
                    return true;
                case JsonObjectType.Object:
                    //Try to search for the type directly, usually not going to work
                    foreach(var assembly in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        var type = assembly.GetType(prop.Format);
                        if (type != null)
                        {
                            return type.IsValueType;
                        }
                    }
                    //Now search by each type's name, this will potentially have problems with collisions since it ignores namespaces
                    foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        var type = assembly.GetTypes().Where(i => i.Name == prop.Format).FirstOrDefault();
                        if(type != null)
                        {
                            return type.IsValueType;
                        }
                    }
                    //Could not find anything, assume ref type
                    return false;
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
    }
}
