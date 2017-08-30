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
using CoinClient.ViewModel;

namespace CoinClient
{
    public static class App
    {
        private static ViewModelLocator _locator;

        public static ViewModelLocator Locator => _locator ?? (_locator = new ViewModelLocator());
    }
}