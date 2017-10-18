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

