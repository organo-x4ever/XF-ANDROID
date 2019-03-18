using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.organo.x4ever.Extensions
{
    public static class SetValueAnimation
    {
        public static async Task SetAsync(this short value, short max, short min = 0, short delay = 50)
        {
            short i = min;
            do
            {
                i++;
                value = i;
                await Task.Delay(delay);
            } while (i < max);
        }
    }
}