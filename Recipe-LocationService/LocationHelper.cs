using System;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Windows;

namespace Microsoft.Phone.Recipes.LocationService
{
    public class LocationHelper : INotifyPropertyChanging, INotifyPropertyChanged
    {
        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;
        private string _PermissionRequestTitle;
        private string _PermissionRequestMessage;
        private bool _permissionToUseLocation;
        private readonly static string ISO_STORAGE_KEY = "Microsoft.Phone.Recipes.LocationService.PermissionToUseLocation";
        private static AskMeControl popup;
        private const string _defaultRequestMessage = "Is it OK to use the Location Service?";
        private const string _defaultRequestTitle = "Verify Location Service";
        private UIElement pageContent;

        /// <summary>
        /// Initializes a new instance of the LocationHelper class.
        /// </summary>
        public LocationHelper()
        {
        }

        public void VerifyLocationPermission()
        {
            var isOk = GetPermissionFromIsoStore();
            if (isOk == null)
            {
                AskForPermission();
            }
            else
            {
                PermissionToUseLocation = (isOk.HasValue && isOk == true);
            }
        }

        private void ProcessButtonClick(bool result)
        {
            PermissionToUseLocation = result;
            Application.Current.RootVisual = pageContent;
        }

        private void AskForPermission()
        {
            popup = new AskMeControl();

            popup.OnButtonClick += (sender, e) =>
            {
                ProcessButtonClick(e.UserApproves);
            };

            pageContent = Application.Current.RootVisual;
            Application.Current.RootVisual = popup;
        }

        private void SavePermissionToIsoStore(bool? value)
        {
            IsolatedStorageSettings.ApplicationSettings[ISO_STORAGE_KEY] = value;
            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        private bool? GetPermissionFromIsoStore()
        {
            var hasKey = IsolatedStorageSettings.ApplicationSettings.Contains(ISO_STORAGE_KEY);
            if (hasKey)
            {
                var value = IsolatedStorageSettings.ApplicationSettings[ISO_STORAGE_KEY];
                bool boolValue;
                if (Boolean.TryParse(value.ToString(), out boolValue))
                {
                    return boolValue;
                }
            }
            return null;
        }

        #region Properties

        public string PermissionRequestTitle
        {
            get { return _PermissionRequestTitle; }
            set
            {
                if (_PermissionRequestTitle != value)
                {
                    RaisePropertyChanging("PermissionRequestTitle");
                    _PermissionRequestTitle = value;
                    RaisePropertyChanged("PermissionRequestTitle");
                }
            }
        }

        public string PermissionRequestMessage
        {
            get { return _PermissionRequestMessage; }
            set
            {
                if (_PermissionRequestMessage != value)
                {
                    RaisePropertyChanging("PermissionRequestMessage");
                    _PermissionRequestMessage = value;
                    RaisePropertyChanged("PermissionRequestMessage");
                }
            }
        }

        public bool PermissionToUseLocation
        {
            get
            {
                return _permissionToUseLocation;
            }
            set
            {
                if (_permissionToUseLocation != value)
                {
                    RaisePropertyChanging("PermissionToUseLocation");
                    _permissionToUseLocation = value;
                    RaisePropertyChanged("PermissionToUseLocation");
                    SavePermissionToIsoStore(value);
                }
            }
        }

        #endregion Properties

        #region Events

        private void RaisePropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
        }

        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion Events
    }
}