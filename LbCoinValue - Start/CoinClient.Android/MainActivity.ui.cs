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

        public Button RefreshButton
        {
            get
            {
                return _refreshButton
                       ?? (_refreshButton = FindViewById<Button>(Resource.Id.RefreshButton));
            }
        }

        private TextView _valueLabelBtc;

        public TextView ValueLabelBtc
        {
            get
            {
                return _valueLabelBtc
                       ?? (_valueLabelBtc = FindViewById<TextView>(Resource.Id.ValueLabelBtc));
            }
        }

        private ImageView _arrowImageBtc;

        public ImageView ArrowImageBtc
        {
            get
            {
                return _arrowImageBtc
                       ?? (_arrowImageBtc = FindViewById<ImageView>(Resource.Id.ArrowImageBtc));
            }
        }

        private TextView _valueLabelEth;

        public TextView ValueLabelEth
        {
            get
            {
                return _valueLabelEth
                       ?? (_valueLabelEth = FindViewById<TextView>(Resource.Id.ValueLabelEth));
            }
        }

        private ImageView _arrowImageEth;

        public ImageView ArrowImageEth
        {
            get
            {
                return _arrowImageEth
                       ?? (_arrowImageEth = FindViewById<ImageView>(Resource.Id.ArrowImageEth));
            }
        }
    }
}