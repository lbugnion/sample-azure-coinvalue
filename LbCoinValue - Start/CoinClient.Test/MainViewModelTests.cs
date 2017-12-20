using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoinClient.ViewModel;
using System.ComponentModel;

namespace CoinClient.Test
{
    [TestClass]
    public class MainViewModelTests
    {
        private bool _propertyChangedBtcWasCalled;
        private bool _propertyChangedEthWasCalled;
        private bool _propertyChangedIsBusyWasCalled;

        [TestMethod]
        public void MainViewModel_TestTrend_ShouldBeUp()
        {
            const double testValue = 1234.5;
            const int trendValue = 1;

            _propertyChangedEthWasCalled = false;
            _propertyChangedBtcWasCalled = false;

            var service = new TestCoinService(testValue, trendValue);

            var vm = new MainViewModel(service);

            vm.PropertyChanged += MainViewModelPropertyChanged;
            vm.RefreshCommand.Execute(null);

            Assert.IsTrue(_propertyChangedBtcWasCalled);
            Assert.IsTrue(_propertyChangedEthWasCalled);

            Assert.IsTrue(vm.Btc.IsUpTrendVisible);
            Assert.IsFalse(vm.Btc.IsFlatTrendVisible);
            Assert.IsFalse(vm.Btc.IsDownTrendVisible);

            Assert.IsTrue(vm.Eth.IsUpTrendVisible);
            Assert.IsFalse(vm.Eth.IsFlatTrendVisible);
            Assert.IsFalse(vm.Eth.IsDownTrendVisible);
        }

        private void MainViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(MainViewModel.Btc):
                    _propertyChangedBtcWasCalled = true;
                    break;

                case nameof(MainViewModel.Eth):
                    _propertyChangedEthWasCalled = true;
                    break;

                case nameof(MainViewModel.IsBusy):
                    _propertyChangedIsBusyWasCalled = true;
                    break;
            }
        }

        [TestMethod]
        public void MainViewModel_TestTrend_ShouldBeDown()
        {
            const double testValue = 1234.5;
            const int trendValue = -1;

            _propertyChangedEthWasCalled = false;
            _propertyChangedBtcWasCalled = false;

            var service = new TestCoinService(testValue, trendValue);

            var vm = new MainViewModel(service);

            vm.PropertyChanged += MainViewModelPropertyChanged;
            vm.RefreshCommand.Execute(null);

            Assert.IsTrue(_propertyChangedBtcWasCalled);
            Assert.IsTrue(_propertyChangedEthWasCalled);

            Assert.IsFalse(vm.Btc.IsUpTrendVisible);
            Assert.IsFalse(vm.Btc.IsFlatTrendVisible);
            Assert.IsTrue(vm.Btc.IsDownTrendVisible);

            Assert.IsFalse(vm.Eth.IsUpTrendVisible);
            Assert.IsFalse(vm.Eth.IsFlatTrendVisible);
            Assert.IsTrue(vm.Eth.IsDownTrendVisible);
        }

        [TestMethod]
        public void MainViewModel_TestTrend_ShouldBeFlat()
        {
            const double testValue = 1234.5;
            const int trendValue = 0;

            _propertyChangedEthWasCalled = false;
            _propertyChangedBtcWasCalled = false;

            var service = new TestCoinService(testValue, trendValue);

            var vm = new MainViewModel(service);

            vm.PropertyChanged += MainViewModelPropertyChanged;
            vm.RefreshCommand.Execute(null);

            Assert.IsTrue(_propertyChangedBtcWasCalled);
            Assert.IsTrue(_propertyChangedEthWasCalled);

            Assert.IsFalse(vm.Btc.IsUpTrendVisible);
            Assert.IsTrue(vm.Btc.IsFlatTrendVisible);
            Assert.IsFalse(vm.Btc.IsDownTrendVisible);

            Assert.IsFalse(vm.Eth.IsUpTrendVisible);
            Assert.IsTrue(vm.Eth.IsFlatTrendVisible);
            Assert.IsFalse(vm.Eth.IsDownTrendVisible);
        }

        [TestMethod]
        public void MainViewModel_TestCurrentValue_ShouldBeSet()
        {
            const double testValue = 1234.5;
            const int trendValue = 0;

            _propertyChangedEthWasCalled = false;
            _propertyChangedBtcWasCalled = false;

            var service = new TestCoinService(testValue, trendValue);

            var vm = new MainViewModel(service);

            vm.PropertyChanged += MainViewModelPropertyChanged;

            vm.RefreshCommand.Execute(null);

            Assert.AreEqual(testValue, vm.Btc.Model.CurrentValue);
            Assert.AreEqual(testValue, vm.Eth.Model.CurrentValue);
        }

        [TestMethod]
        public void MainViewModel_TestIsBusy_ShouldSwitch()
        {
            const double testValue = 1234.5;
            const int trendValue = 0;

            _propertyChangedIsBusyWasCalled = false;

            var service = new TestCoinService(testValue, trendValue);

            var vm = new MainViewModel(service);

            var raiseCanExecuteChangedWasCalled = false;

            vm.PropertyChanged += MainViewModelPropertyChanged;

            vm.RefreshCommand.CanExecuteChanged += (s, e) =>
            {
                raiseCanExecuteChangedWasCalled = true;
            };

            Assert.IsFalse(vm.IsBusy);

            vm.RefreshCommand.Execute(null);

            Assert.IsTrue(raiseCanExecuteChangedWasCalled);
            Assert.IsTrue(_propertyChangedIsBusyWasCalled);
            Assert.IsFalse(vm.IsBusy);
        }
    }

}
