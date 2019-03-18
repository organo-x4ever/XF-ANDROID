using System;
using System.Collections.Generic;
using System.Linq;

namespace com.organo.x4ever.Utilities
{
    public static class EnumUtil
    {
        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}