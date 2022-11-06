using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LoginGoogle
{
    public partial class MainPage : ContentPage
    {
        private string authenticationUrl = "http://localhost:5000/mobileauth";
        private string _AuthToken;

        public string AuthToken
        {
            get => _AuthToken;
            set
            {
                if (value == _AuthToken) return;
                _AuthToken = value;
                OnPropertyChanged();
            }
        }

        public MainPage()
        {
            InitializeComponent();
        }

        async void Button_Clicked(object sender, EventArgs e)
        {
            string scheme = "Google";
            try
            {
                WebAuthenticatorResult r = null;

                if (scheme.Equals("Apple")
                    && DeviceInfo.Platform == DevicePlatform.iOS
                    && DeviceInfo.Version.Major >= 13)
                {
                    r = await AppleSignInAuthenticator.AuthenticateAsync();
                }
                else if (scheme.Equals("Google"))
                {
                    var authUrl = new Uri(authenticationUrl + scheme);
                    var callbackUrl = new Uri("xamarinessentials://");

                    r = await WebAuthenticator.AuthenticateAsync(authUrl, callbackUrl);
                }

                AuthToken = r?.AccessToken ?? r?.IdToken;
            }
            catch (Exception ex)
            {
                AuthToken = string.Empty;
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "Ok");
            }
        }
    }
}