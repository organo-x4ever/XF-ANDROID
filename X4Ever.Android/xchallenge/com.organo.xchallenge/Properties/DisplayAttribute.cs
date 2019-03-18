﻿using System;

namespace com.organo.xchallenge.Properties.Attributes
{
    public class DisplayAttribute : Attribute
    {
        public string Name { get; private set; }

        public DisplayAttribute(string name)
        {
            Name = name;
        }
    }
}