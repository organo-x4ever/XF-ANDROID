
using System.Threading.Tasks;
using Xamarin.Forms;

namespace com.organo.x4ever.Extensions
{
    public static class SliderValue
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="slider">The Slider Control</param>
        /// <param name="maxValue">Increase upto slider minimum value</param>
        /// <param name="minValue">Start increasing from slide minimum value</param>
        /// <param name="delay">Slider take time to update (in Milliseconds)</param>
        /// <returns></returns>
        public static async void SetMinValueAsync(this Slider slider, double maxValue, double minValue = 0,
            short delay = 1)
        {
            if (delay == 0)
                SetSliderValueAsync(slider, maxValue);
            else
            {
                double i = minValue;
                do
                {
                    i++;
                    SetSliderValueAsync(slider, i);
                    await Task.Delay(delay);
                } while (i < maxValue);
            }
        }

        private static void SetSliderValueAsync(Slider slider, double value)
        {
            slider.Value = value;
        }
    }
}