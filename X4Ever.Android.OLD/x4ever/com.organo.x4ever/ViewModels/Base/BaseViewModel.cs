using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using com.organo.x4ever.Localization;
using com.organo.x4ever.Services;
using Xamarin.Forms;

namespace com.organo.x4ever.ViewModels.Base
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public INavigation Navigation { get; set; }

        private static IInformationMessageServices InformationServices =>
            DependencyService.Get<IInformationMessageServices>();

        public BaseViewModel(INavigation navigation = null)
        {
            Navigation = navigation;
        }

        public bool IsInitialized { get; set; }

        private LayoutOptions _layoutOptions = LayoutOptions.Center;

        /// <summary>
        /// Gets or sets the "LayoutOptions" property
        /// </summary>
        /// <value>
        /// The user account.
        /// </value>
        public const string LayoutOptionsPropertyName = "LayoutOptions";

        public LayoutOptions LayoutOptions
        {
            get { return _layoutOptions; }
            set { SetProperty(ref _layoutOptions, value, LayoutOptionsPropertyName); }
        }

        public async Task PushModalAsync(Page page)
        {
            if (Navigation != null)
                await Navigation.PushModalAsync(page);
        }

        public async Task PopModalAsync()
        {
            if (Navigation != null)
                await Navigation.PopModalAsync();
            //await App.CurrentApp.MainPage.Navigation.PopModalAsync();
        }

        public async Task PushAsync(Page page)
        {
            if (Navigation != null)
                await Navigation.PushAsync(page);
        }

        public async Task PopAsync()
        {
            if (Navigation != null)
                await Navigation.PopAsync();
        }

        public void SetActivityResource(bool showEditable = true, bool showBusy = false, bool showMessage = false,
            bool showError = false, string busyMessage = "", string message = "", string errorMessage = "",
            bool modalWindow = false)
        {
            IsMessage = IsError = false;
            MessageText = ErrorMessage = string.Empty;
            IsEditable = showEditable;
            IsBusy = showBusy;
            BusyMessage = showBusy && string.IsNullOrEmpty(busyMessage)
                ? TextResources.ProcessingPleaseWait
                : busyMessage;
            if (!modalWindow)
            {
                if (showMessage && !string.IsNullOrEmpty(message))
                    InformationServices.ShortAlert(message);
                if (showError && !string.IsNullOrEmpty(errorMessage))
                    InformationServices.LongAlert(errorMessage);
            }
            else
            {
                IsMessage = showMessage;
                IsError = showError;
                MessageText = message;
                ErrorMessage = errorMessage;
            }
        }

        private bool _canLoadMore;

        /// <summary>
        /// Gets or sets the "IsBusy" property
        /// </summary>
        /// <value>
        /// The isbusy property.
        /// </value>
        public const string CanLoadMorePropertyName = "CanLoadMore";

        public bool CanLoadMore
        {
            get => _canLoadMore;
            set => SetProperty(ref _canLoadMore, value, CanLoadMorePropertyName);
        }

        private bool _isEditable = true;

        /// <summary>
        /// Gets or sets the "IsEditable" property
        /// </summary>
        /// <value>
        /// The isEditable property.
        /// </value>
        public const string IsEditablePropertyName = "IsEditable";

        public bool IsEditable
        {
            get { return _isEditable; }
            set { SetProperty(ref _isEditable, value, IsEditablePropertyName); }
        }

        private bool _isBusy;

        /// <summary>
        /// Gets or sets the "IsBusy" property
        /// </summary>
        /// <value>
        /// The isbusy property.
        /// </value>
        public const string IsBusyPropertyName = "IsBusy";

        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value, IsBusyPropertyName); }
        }

        private string _busyMessage = string.Empty;

        /// <summary>
        /// Gets or sets the "BusyMessage" property
        /// </summary>
        /// <value>
        /// The busy message.
        /// </value>
        public const string BusyMessagePropertyName = "BusyMessage";

        public string BusyMessage
        {
            get { return _busyMessage; }
            set { SetProperty(ref _busyMessage, value, BusyMessagePropertyName); }
        }

        private bool _isError;

        /// <summary>
        /// Gets or sets the "IsError" property
        /// </summary>
        /// <value>
        /// The iserror property.
        /// </value>
        public const string IsErrorPropertyName = "IsError";

        public bool IsError
        {
            get { return _isError; }
            set { SetProperty(ref _isError, value, IsErrorPropertyName); }
        }

        private string _errorMessage = string.Empty;

        /// <summary>
        /// Gets or sets the "ErrorMessage" property
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public const string ErrorMessagePropertyName = "ErrorMessage";

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { SetProperty(ref _errorMessage, value, ErrorMessagePropertyName); }
        }

        private bool _isMessage;

        /// <summary>
        /// Gets or sets the "IsMessage" property
        /// </summary>
        /// <value>
        /// The isMessage property.
        /// </value>
        public const string IsMessagePropertyName = "IsMessage";

        public bool IsMessage
        {
            get { return _isMessage; }
            set { SetProperty(ref _isMessage, value, IsMessagePropertyName); }
        }

        private string _messageText = string.Empty;

        /// <summary>
        /// Gets or sets the "MessageText" property
        /// </summary>
        /// <value>
        /// The Message Text message.
        /// </value>
        public const string MessageTextPropertyName = "MessageText";

        public string MessageText
        {
            get { return _messageText; }
            set { SetProperty(ref _messageText, value, MessageTextPropertyName); }
        }

        private string title = string.Empty;

        /// <summary>
        /// Gets or sets the "Title" property
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public const string TitlePropertyName = "Title";

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value, TitlePropertyName); }
        }

        private string _subTitle = string.Empty;

        /// <summary>
        /// Gets or sets the "Subtitle" property
        /// </summary>
        public const string SubtitlePropertyName = "Subtitle";

        public string Subtitle
        {
            get { return _subTitle; }
            set { SetProperty(ref _subTitle, value, SubtitlePropertyName); }
        }

        private string _icon = null;

        /// <summary>
        /// Gets or sets the "Icon" of the viewmodel
        /// </summary>
        public const string IconPropertyName = "Icon";

        public string Icon
        {
            get { return _icon; }
            set { SetProperty(ref _icon, value, IconPropertyName); }
        }

        private Style _entryStyle;

        /// <summary>
        /// Gets or sets the "EntryStyle" property
        /// </summary>
        /// <value>
        /// The EntryStyle property.
        /// </value>
        public const string EntryStylePropertyName = "EntryStyle";

        public Style EntryStyle
        {
            get { return _entryStyle; }
            set { SetProperty(ref _entryStyle, value, EntryStylePropertyName); }
        }

        #region Signature

        private string Signature => "GURPREET DEOL";
        
        #endregion Signature

        private Style _entryStyleError;

        /// <summary>
        /// Gets or sets the "EntryStyleError" property
        /// </summary>
        /// <value>
        /// The EntryStyleError property.
        /// </value>
        public const string EntryStyleErrorPropertyName = "EntryStyleError";

        public Style EntryStyleError
        {
            get { return _entryStyleError; }
            set { SetProperty(ref _entryStyleError, value, EntryStyleErrorPropertyName); }
        }

        protected void SetProperty<U>(
            ref U backingStore, U value,
            string propertyName,
            Action onChanged = null,
            Action<U> onChanging = null)
        {
            if (EqualityComparer<U>.Default.Equals(backingStore, value))
                return;

            if (onChanging != null)
                onChanging(value);

            OnPropertyChanging(propertyName);

            backingStore = value;

            if (onChanged != null)
                onChanged();

            OnPropertyChanged(propertyName);
        }

        #region INotifyPropertyChanging implementation

        public event PropertyChangingEventHandler PropertyChanging;

        #endregion INotifyPropertyChanging implementation

        public void OnPropertyChanging(string propertyName)
        {
            if (PropertyChanging == null)
                return;

            PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
        }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion INotifyPropertyChanged implementation

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private short timeDelay => 1000;
        private ICommand _developerCommand;
        private DateTime _currentTime;
        private short _tapAttempt;

        public ICommand DeveloperCommand
        {
            get
            {
                return _developerCommand ?? (_developerCommand = new Command((obj) =>
                {
                    if (_tapAttempt == 0)
                        _currentTime = DateTime.Now;
                    if (_currentTime.AddMilliseconds(timeDelay) >= DateTime.Now)
                        _tapAttempt++;
                    else
                    {
                        _tapAttempt = 0;
                        _currentTime = DateTime.Now;
                    }

                    if (_tapAttempt == 3)
                    {
                        DependencyService.Get<IInformationMessageServices>().ShortAlert(Signature);
                    }
                }));
            }
        }
    }
}