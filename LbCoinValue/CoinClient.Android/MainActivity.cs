using Android.App;
using Android.Widget;
using Android.OS;
using GalaSoft.MvvmLight.Helpers;
using CoinClient.ViewModel;
using System.Collections.Generic;
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Push;

namespace CoinClient
{
    [Activity(Label = "Bitcoin watcher", MainLauncher = true, Icon = "@drawable/icon")]
    public partial class MainActivity
    {
        private MainViewModel Vm => App.Locator.Main;
        private List<Binding> _bindings = new List<Binding>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Notifications

            // This should come before MobileCenter.Start() is called
            Push.PushNotificationReceived += (sender, e) => 
            {
                // Add the notification message and title to the message
                var summary = $"Push notification received:"
                    + $"\n\tNotification title: {e.Title}" 
                    + $"\n\tMessage: {e.Message}";

                // If there is custom data associated with the notification,
                // print the entries
                if (e.CustomData != null)
                {
                    summary += "\n\tCustom data:\n";
                    foreach (var key in e.CustomData.Keys)
                    {
                        summary += $"\t\t{key} : {e.CustomData[key]}\n";
                    }
                }

                // Send the notification summary to debug output
                System.Diagnostics.Debug.WriteLine(summary);
            };

            MobileCenter.Start("585a1865-5171-45b5-9a5e-40923798232d", typeof(Push));

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            _bindings.Add(this.SetBinding(
                () => Vm.CurrentCoinValue,
                () => ValueLabel.Text));

            _bindings.Add(this.SetBinding(
                () => Vm.IsUpTrendVisible)
                .WhenSourceChanges(() =>
                {
                    if (Vm.IsUpTrendVisible)
                    {
                        ArrowImage.SetImageResource(Resource.Drawable.ArrowUp);
                    }
                }));

            _bindings.Add(this.SetBinding(
                () => Vm.IsFlatTrendVisible)
                .WhenSourceChanges(() =>
                {
                    if (Vm.IsFlatTrendVisible)
                    {
                        ArrowImage.SetImageResource(Resource.Drawable.ArrowFlat);
                    }
                }));

            _bindings.Add(this.SetBinding(
                () => Vm.IsDownTrendVisible)
                .WhenSourceChanges(() =>
                {
                    if (Vm.IsDownTrendVisible)
                    {
                        ArrowImage.SetImageResource(Resource.Drawable.ArrowDown);
                    }
                }));

            RefreshButton.SetCommand(App.Locator.Main.RefreshCommand);
        }
    }
}

