using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace com.organo.x4ever.Effects.Control
{
    public interface IAnimation
    {
        int LoadDelay { get; set; }
        uint AnimationSpeed { get; set; }
        Easing EasingIn { get; set; }
        Easing EasingOut { get; set; }

        Task Add(View view);

        Task Clear();

        /// <summary>
        /// Parent View of all the controls
        /// </summary>
        /// <param name="viewParent">
        /// </param>
        /// <returns>
        /// </returns>
        Task Animate(StackLayout viewParent);

        /// <summary>
        /// Animate all the controls defined in Control List
        /// </summary>
        /// <param name="viewParent">
        /// </param>
        /// <returns>
        /// </returns>
        Task Animate(List<ControlEntity> controlList);

        /// <summary>
        /// Children controls will be animated from Control List
        /// </summary>
        /// <returns>
        /// </returns>
        Task Animate();
    }
}