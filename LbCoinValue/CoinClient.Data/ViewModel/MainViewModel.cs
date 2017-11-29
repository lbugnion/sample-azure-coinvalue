using CoinClient.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;

namespace CoinClient.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private CoinTrendViewModel _btc = null;
        private CoinTrendViewModel _eth = null;
        private string _errorMessage;
        private bool _isBusy;
        private RelayCommand _refreshCommand;
        private ICoinService _service;

        public CoinTrendViewModel Btc
        {
            get
            {
                return _btc;
            }
            set
            {
                Set(ref _btc, value);
            }
        }

        public CoinTrendViewModel Eth
        {
            get
            {
                return _eth;
            }
            set
            {
                Set(ref _eth, value);
            }
        }

        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                Set(ref _errorMessage, value);
            }
        }

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                if (Set(ref _isBusy, value))
                {
                    RefreshCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public RelayCommand RefreshCommand
        {
            get
            {
                return _refreshCommand
                    ?? (_refreshCommand = new RelayCommand(
                    async () =>
                    {
                        IsBusy = true;

                        try
                        {
                            var btcModel = await _service.GetTrend(CoinTrend.SymbolBtc);
                            Btc = new CoinTrendViewModel(btcModel);

                            var ethModel = await _service.GetTrend(CoinTrend.SymbolEth);
                            Eth = new CoinTrendViewModel(ethModel);

                            ErrorMessage = string.Empty;
                        }
                        catch (Exception ex)
                        {
                            Btc = null;
                            Eth = null;
                            ErrorMessage = ex.Message;
                        }

                        IsBusy = false;
                    },
                    () => !IsBusy));
            }
        }

        public MainViewModel(ICoinService service)
        {
            _service = service;
        }
    }
}