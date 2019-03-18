using System;
using Xamarin.Forms;

namespace com.organo.xchallenge.Effects.Control
{
    public interface IControlEntity
    {
        Type ControlType { get; set; }
        View ControlView { get; set; }
        string ControlID { get; set; }
    }
}