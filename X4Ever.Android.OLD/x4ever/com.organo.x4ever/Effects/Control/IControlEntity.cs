using System;
using Xamarin.Forms;

namespace com.organo.x4ever.Effects.Control
{
    public interface IControlEntity
    {
        Type ControlType { get; set; }
        View ControlView { get; set; }
        string ControlID { get; set; }
    }
}