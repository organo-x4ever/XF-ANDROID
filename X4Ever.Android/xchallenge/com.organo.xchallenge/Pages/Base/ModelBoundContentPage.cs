﻿using com.organo.xchallenge.ViewModels.Base;
using Xamarin.Forms;

namespace com.organo.xchallenge.Pages.Base
{
    /// <summary>
    /// A generically typed ContentPage that enforces the type of its BindingContext according to TViewModel.
    /// </summary>
    public abstract class ModelBoundContentPage<TViewModel> : ContentPage where TViewModel : BaseViewModel
    {
        public static readonly string TAG = typeof(TViewModel).FullName;
        /// <summary>
        /// Gets the generically typed ViewModel from the underlying BindingContext.
        /// </summary>
        /// <value>
        /// The generically typed ViewModel.
        /// </value>
        protected TViewModel ViewModel
        {
            get { return base.BindingContext as TViewModel; }
        }

        /// <summary>
        /// Sets the underlying BindingContext as the defined generic type.
        /// </summary>
        /// <value>
        /// The generically typed ViewModel.
        /// </value>
        /// <remarks>
        /// Enforces a generically typed BindingContext, instead of the underlying loosely
        /// object-typed BindingContext.
        /// </remarks>
        public new TViewModel BindingContext
        {
            set
            {
                base.BindingContext = value;
                base.OnPropertyChanged("BindingContext");
            }
        }
    }
}