using CoinClient.ViewModel;
using GalaSoft.MvvmLight.Helpers;
using System;
using System.Collections.Generic;
using UIKit;

namespace CoinValue.Ios
{
    public partial class ViewController : UIViewController
    {
        private const string BtcTemplate = "BTC: {0}U$";
        private const string EthTemplate = "ETH: {0}U$";

        private List<Binding> _bindings = new List<Binding>();

        private MainViewModel Vm => Application.Locator.Main;

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

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
                        ValueLabelBtc.Text = string.Format(BtcTemplate, "0.0");
                    }
                    else
                    {
                        ValueLabelBtc.Text = string.Format(BtcTemplate, Vm.Btc.Model.CurrentValue);
                    }
                }));

            _bindings.Add(this.SetBinding(
                () => Vm.Btc)
                .WhenSourceChanges(() =>
                {
                    if (Vm.Btc == null)
                    {
                        ArrowImageBtc.Image = UIImage.FromBundle("ArrowFlat");
                    }
                    else
                    {
                        if (Vm.Btc.IsFlatTrendVisible)
                        {
                            ArrowImageBtc.Image = UIImage.FromBundle("ArrowFlat");
                        }

                        if (Vm.Btc.IsUpTrendVisible)
                        {
                            ArrowImageBtc.Image = UIImage.FromBundle("ArrowUp");
                        }

                        if (Vm.Btc.IsDownTrendVisible)
                        {
                            ArrowImageBtc.Image = UIImage.FromBundle("ArrowDown");
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
                        ValueLabelEth.Text = string.Format(EthTemplate, "0.0");
                    }
                    else
                    {
                        ValueLabelEth.Text = string.Format(EthTemplate, Vm.Eth.Model.CurrentValue);
                    }
                }));

            _bindings.Add(this.SetBinding(
                () => Vm.Eth)
                .WhenSourceChanges(() =>
                {
                    if (Vm.Eth == null)
                    {
                        ArrowImageEth.Image = UIImage.FromBundle("ArrowFlat");
                    }
                    else
                    {
                        if (Vm.Eth.IsFlatTrendVisible)
                        {
                            ArrowImageEth.Image = UIImage.FromBundle("ArrowFlat");
                        }

                        if (Vm.Eth.IsUpTrendVisible)
                        {
                            ArrowImageEth.Image = UIImage.FromBundle("ArrowUp");
                        }

                        if (Vm.Eth.IsDownTrendVisible)
                        {
                            ArrowImageEth.Image = UIImage.FromBundle("ArrowDown");
                        }
                    }
                }));

            RefreshButton.SetCommand(Application.Locator.Main.RefreshCommand);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}