using System;
using Xamarin.Forms;

namespace com.organo.x4ever.Effects.Control
{
    public class ControlEntity : IControlEntity
    {
        public Type ControlType { get; set; }
        public View ControlView { get; set; }
        public string ControlID { get; set; }
    }
}