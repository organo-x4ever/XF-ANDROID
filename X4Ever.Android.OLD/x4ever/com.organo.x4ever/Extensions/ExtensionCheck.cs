using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.organo.x4ever.Extensions
{
    public static class ExtensionCheck
    {
        public static bool IsExtension(this string value, string extension)
        {
            return value.ToLower().EndsWith(extension.ToLower());
        }

        public static bool IsExtension(this string value, string[] extensions)
        {
            foreach (var extension in extensions)
                if (value.ToLower().EndsWith(extension.ToLower()))
                    return true;
            return false;
        }
    }
}