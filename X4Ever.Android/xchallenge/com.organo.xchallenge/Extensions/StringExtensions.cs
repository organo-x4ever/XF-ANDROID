﻿using Xamarin.Forms;

namespace com.organo.xchallenge.Extensions
{
    public static class StringExtensions
    {
        public static string CapitalizeForAndroid(this string str)
        {
            return Device.RuntimePlatform == Device.Android ? str.ToUpper() : str;
        }
    }
}