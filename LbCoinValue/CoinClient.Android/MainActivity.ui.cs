using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace CoinClient
{
    public partial class MainActivity : Activity
    {

        private Button _refreshButton;

        /// <summary>
        /// Gets the RefreshButton UI element.
        /// </summary>
        public Button RefreshButton
        {
            get
            {
                return _refreshButton
                       ?? (_refreshButton = FindViewById<Button>(Resource.Id.RefreshButton));
            }
        }

        private TextView _valueLabel;

        /// <summary>
        /// Gets the ValueLabel UI element.
        /// </summary>
        public TextView ValueLabel
        {
            get
            {
                return _valueLabel
                       ?? (_valueLabel = FindViewById<TextView>(Resource.Id.ValueLabel));
            }
        }

        private ImageView _arrowImage;

        /// <summary>
        /// Gets the ArrowImage UI element.
        /// </summary>
        public ImageView ArrowImage
        {
            get
            {
                return _arrowImage
                       ?? (_arrowImage = FindViewById<ImageView>(Resource.Id.ArrowImage));
            }
        }
    }
}