using GalaSoft.MvvmLight.Helpers;
using System;

using UIKit;

namespace CoinValue.Ios
{
    public partial class ViewController : UIViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            RefreshButton.SetCommand(Application.Locator.Main.RefreshCommand);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}