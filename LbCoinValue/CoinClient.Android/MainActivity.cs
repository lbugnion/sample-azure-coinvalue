using Android.App;
using Android.OS;
using CoinClient.ViewModel;
using GalaSoft.MvvmLight.Helpers;
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Push;
using System.Collections.Generic;

namespace CoinClient
{
    [Activity(Label = "Bitcoin watcher", MainLauncher = true, Icon = "@drawable/icon")]
    public partial class MainActivity
    {
        private List<Binding> _bindings = new List<Binding>();

        private MainViewModel Vm => App.Locator.Main;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Notifications

            // This should come before MobileCenter.Start() is called
            Push.PushNotificationReceived += (sender, e) =>
            {
                // Instantiate the builder and set notification elements:
                var builder = new Notification.Builder(this)
                    .SetContentTitle(e.Title)
                    .SetContentText(e.Message)
                    .SetSmallIcon(Resource.Drawable.notification_icon);

                // Build the notification:
                Notification notification = builder.Build();

                // Get the notification manager:
                NotificationManager notificationManager
                    = GetSystemService(NotificationService) as NotificationManager;

                // Publish the notification:
                const int notificationId = 0;
                notificationManager.Notify(notificationId, notification);
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