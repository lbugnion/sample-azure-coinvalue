// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace CoinValue.Ios
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ArrowImageBtc { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ArrowImageEth { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton RefreshButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ValueLabelBtc { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ValueLabelEth { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ArrowImageBtc != null) {
                ArrowImageBtc.Dispose ();
                ArrowImageBtc = null;
            }

            if (ArrowImageEth != null) {
                ArrowImageEth.Dispose ();
                ArrowImageEth = null;
            }

            if (RefreshButton != null) {
                RefreshButton.Dispose ();
                RefreshButton = null;
            }

            if (ValueLabelBtc != null) {
                ValueLabelBtc.Dispose ();
                ValueLabelBtc = null;
            }

            if (ValueLabelEth != null) {
                ValueLabelEth.Dispose ();
                ValueLabelEth = null;
            }
        }
    }
}