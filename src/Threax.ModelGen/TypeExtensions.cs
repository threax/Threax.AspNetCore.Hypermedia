using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen
{
    static class TypeExtensions
    {
        public static String GetTypeAsNullable(this Type t)
        {
            if (t.IsValueType)
            {
                return $"{t.Name}?";
            }
            else
            {
                return t.Name;
            }
        }

        private static HashSet<Type> NumericTypes = new HashSet<Type>()
        {
            typeof(byte), typeof(sbyte), typeof(ushort), typeof(uint), typeof(ulong), typeof(short), typeof(int), typeof(long), typeof(decimal), typeof(double), typeof(float)
        };

        public static bool IsNumeric(this Type t)
        {
            return NumericTypes.Contains(t);
        }
    }
}
