using Android.App;
using Android.OS;
using CoinClient.ViewModel;
using GalaSoft.MvvmLight.Helpers;
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Push;
using System.Collections.Generic;

namespace CoinClient
{
    [Activity(Label = "Coin watcher", MainLauncher = true, Icon = "@drawable/icon")]
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

            // Using WhenSourceChanges here because there is a bug in MVVM Light
            // for Binding<double, string> when complex property path has a null
            // instance on the path.
            // TODO Update this code when the bug is fixed
            // https://github.com/lbugnion/mvvmlight/issues/9
            _bindings.Add(this.SetBinding(
                () => Vm.Btc)
                .WhenSourceChanges(() =>
                {
                    if (Vm.Btc == null)
                    {
                        ValueLabelBtc.Text = "0.0";
                    }
                    else
                    {
                        ValueLabelBtc.Text = Vm.Btc.Model.CurrentValue.ToString();
                    }
                }));

            _bindings.Add(this.SetBinding(
                () => Vm.Btc)
                .WhenSourceChanges(() =>
                {
                    if (Vm.Btc == null)
                    {
                        ArrowImageBtc.SetImageResource(Resource.Drawable.ArrowFlat);
                    }
                    else
                    {
                        if (Vm.Btc.IsFlatTrendVisible)
                        {
                            ArrowImageBtc.SetImageResource(Resource.Drawable.ArrowFlat);
                        }

                        if (Vm.Btc.IsUpTrendVisible)
                        {
                            ArrowImageBtc.SetImageResource(Resource.Drawable.ArrowUp);
                        }

                        if (Vm.Btc.IsDownTrendVisible)
                        {
                            ArrowImageBtc.SetImageResource(Resource.Drawable.ArrowDown);
                        }
                    }
                }));

            // Using WhenSourceChanges here because there is a bug in MVVM Light
            // for Binding<double, string> when complex property path has a null
            // instance on the path.
            // TODO Update this code when the bug is fixed
            // https://github.com/lbugnion/mvvmlight/issues/9
            _bindings.Add(this.SetBinding(
                () => Vm.Eth)
                .WhenSourceChanges(() =>
                {
                    if (Vm.Eth == null)
                    {
                        ValueLabelEth.Text = "0.0";
                    }
                    else
                    {
                        ValueLabelEth.Text = Vm.Eth.Model.CurrentValue.ToString();
                    }
                }));

            _bindings.Add(this.SetBinding(
                () => Vm.Eth)
                .WhenSourceChanges(() =>
                {
                    if (Vm.Eth == null)
                    {
                        ArrowImageEth.SetImageResource(Resource.Drawable.ArrowFlat);
                    }
                    else
                    {
                        if (Vm.Eth.IsFlatTrendVisible)
                        {
                            ArrowImageEth.SetImageResource(Resource.Drawable.ArrowFlat);
                        }

                        if (Vm.Eth.IsUpTrendVisible)
                        {
                            ArrowImageEth.SetImageResource(Resource.Drawable.ArrowUp);
                        }

                        if (Vm.Eth.IsDownTrendVisible)
                        {
                            ArrowImageEth.SetImageResource(Resource.Drawable.ArrowDown);
                        }
                    }
                }));

            RefreshButton.SetCommand(App.Locator.Main.RefreshCommand);
        }
    }
}