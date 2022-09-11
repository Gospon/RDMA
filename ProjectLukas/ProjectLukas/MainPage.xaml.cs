using Firebase.Auth;
using Newtonsoft.Json;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ProjectLukas
{
    public partial class MainPage : ContentPage
    {
        public string webAPIkey = "AIzaSyARGgG-cBAaQeJuyVAy9RCPyrpAB3PyY_I";

        public MainPage()
        {
            InitializeComponent();
        }

        async void LoginClicked(System.Object sender, System.EventArgs e)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(webAPIkey));
            try
            {
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(UserLoginEmail.Text, UserLoginPassword.Text);
                var content = await auth.GetFreshAuthAsync();
                var serializedcontent = JsonConvert.SerializeObject(content);
                Preferences.Set("MyFirebaseRefreshToken", serializedcontent);
                await Navigation.PushAsync(new Dashboard());
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "An Error Has Occurred", "OK");
            }
        }

        async void SignUpClicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(webAPIkey));
                var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(UserNewEmail.Text, UserNewPassword.Text);
                string getToken = auth.FirebaseToken;
                await App.Current.MainPage.DisplayAlert("Alert", "Signed Up Successfully", "OK");
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "An Error Has Occurred", "OK");
            }

        }

        private void OnSignUp(object sender, EventArgs e)
        {
            UserNewEmail.IsVisible = true;
            UserNewPassword.IsVisible = true;
            SignUpButton.IsVisible = true;
            LoginLabel.IsVisible = true;
            UserLoginEmail.IsVisible = false;
            UserLoginPassword.IsVisible = false;
            LogInButton.IsVisible = false;
            SignUpLabel.IsVisible = false;
        }

        private void OnLogIn(object sender, EventArgs e)
        {
            UserNewEmail.IsVisible = false;
            UserNewPassword.IsVisible = false;
            SignUpButton.IsVisible = false;
            LoginLabel.IsVisible = false;
            UserLoginEmail.IsVisible = true;
            UserLoginPassword.IsVisible = true;
            LogInButton.IsVisible = true;
            SignUpLabel.IsVisible = true;
        }
    }
}
