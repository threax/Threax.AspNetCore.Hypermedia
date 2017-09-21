using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Halcyon.Ext.ValueProviders
{
    /// <summary>
    /// This class will create pretty titles from a string. The default version will make sure the first character is
    /// upper case and will add a space after each transition form lower to upper case. If you need to customize your
    /// field name further use the DisplayAttribute, which will prevent this generator from being used.
    /// </summary>
    public class AutoTitleGenerator : IAutoTitleGenerator
    {
        public String CreateTitle(String name)
        {
            if(name == null || name.Length == 0)
            {
                return name;
            }

            bool currentLower = false;
            bool lastLower = false;
            var title = new StringBuilder(name.Length + 5); //Slightly larger buffer, wont have to reallocate unless there are more than 5 spaces that need to be added.
            title.Append(char.ToUpper(name[0]));
            for (int i = 1; i < name.Length; ++i) //Add a space between each transition from lower case to upper case in the name
            {
                var current = name[i];
                currentLower = char.IsLower(current);
                if (lastLower && !currentLower)
                {
                    title.Append(" ");
                    current = char.ToUpper(current);
                }
                lastLower = currentLower;
                title.Append(current);
            }

            return title.ToString();
        }
    }
}
