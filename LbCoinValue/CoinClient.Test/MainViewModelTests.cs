using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoinClient.ViewModel;
using System.ComponentModel;

namespace CoinClient.Test
{
    [TestClass]
    public class MainViewModelTests
    {
        private bool _propertyChangedUpWasCalled;
        private bool _propertyChangedFlatWasCalled;
        private bool _propertyChangedDownWasCalled;
        private bool _propertyChangedValueWasCalled;
        private bool _propertyChangedIsBusyWasCalled;

        [TestMethod]
        public void MainViewModel_TestTrend_ShouldBeUp()
        {
            const double testValue = 1234.5;
            const int trendValue = 1;

            _propertyChangedUpWasCalled = false;
            _propertyChangedFlatWasCalled = false;
            _propertyChangedDownWasCalled = false;

            var service = new TestCoinService(testValue, trendValue);

            var vm = new MainViewModel(service);

            vm.PropertyChanged += MainViewModelPropertyChanged;

            vm.RefreshCommand.Execute(null);

            Assert.IsTrue(_propertyChangedUpWasCalled);
            Assert.IsTrue(_propertyChangedFlatWasCalled);
            Assert.IsFalse(_propertyChangedDownWasCalled);

            Assert.IsTrue(vm.IsUpTrendVisible);
            Assert.IsFalse(vm.IsFlatTrendVisible);
            Assert.IsFalse(vm.IsDownTrendVisible);
        }

        private void MainViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(MainViewModel.IsUpTrendVisible):
                    _propertyChangedUpWasCalled = true;
                    break;

                case nameof(MainViewModel.IsFlatTrendVisible):
                    _propertyChangedFlatWasCalled = true;
                    break;

                case nameof(MainViewModel.IsDownTrendVisible):
                    _propertyChangedDownWasCalled = true;
                    break;

                case nameof(MainViewModel.IsBusy):
                    _propertyChangedIsBusyWasCalled = true;
                    break;

                case nameof(MainViewModel.CurrentCoinValue):
                    _propertyChangedValueWasCalled = true;
                    break;
            }
        }

        [TestMethod]
        public void MainViewModel_TestTrend_ShouldBeDown()
        {
            const double testValue = 1234.5;
            const int trendValue = -1;

            _propertyChangedUpWasCalled = false;
            _propertyChangedFlatWasCalled = false;
            _propertyChangedDownWasCalled = false;

            var service = new TestCoinService(testValue, trendValue);

            var vm = new MainViewModel(service);

            vm.PropertyChanged += MainViewModelPropertyChanged;

            vm.RefreshCommand.Execute(null);

            Assert.IsFalse(_propertyChangedUpWasCalled);
            Assert.IsTrue(_propertyChangedFlatWasCalled);
            Assert.IsTrue(_propertyChangedDownWasCalled);

            Assert.IsFalse(vm.IsUpTrendVisible);
            Assert.IsFalse(vm.IsFlatTrendVisible);
            Assert.IsTrue(vm.IsDownTrendVisible);
        }

        [TestMethod]
        public void MainViewModel_TestTrend_ShouldBeFlat()
        {
            const double testValue = 1234.5;
            const int trendValue = 0;

            _propertyChangedUpWasCalled = false;
            _propertyChangedFlatWasCalled = false;
            _propertyChangedDownWasCalled = false;

            var service = new TestCoinService(testValue, trendValue);

            var vm = new MainViewModel(service);

            vm.PropertyChanged += MainViewModelPropertyChanged;

            vm.RefreshCommand.Execute(null);

            Assert.IsFalse(_propertyChangedUpWasCalled);
            Assert.IsFalse(_propertyChangedFlatWasCalled);
            Assert.IsFalse(_propertyChangedDownWasCalled);

            Assert.IsFalse(vm.IsUpTrendVisible);
            Assert.IsTrue(vm.IsFlatTrendVisible);
            Assert.IsFalse(vm.IsDownTrendVisible);
        }

        [TestMethod]
        public void MainViewModel_TestCurrentValue_ShouldBeSet()
        {
            const double testValue = 1234.5;
            const int trendValue = 0;

            _propertyChangedValueWasCalled = false;

            var service = new TestCoinService(testValue, trendValue);

            var vm = new MainViewModel(service);

            vm.PropertyChanged += MainViewModelPropertyChanged;

            vm.RefreshCommand.Execute(null);

            Assert.IsTrue(_propertyChangedValueWasCalled);
            Assert.AreEqual(testValue, vm.CurrentCoinValue);
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
