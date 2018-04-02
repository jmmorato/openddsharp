using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using OpenDDSharp.ShapesDemo.Model;
using OpenDDSharp.ShapesDemo.Service;

namespace OpenDDSharp.ShapesDemo.ViewModel
{
    public class ReaderFilterViewModel : ViewModelBase
    {
        #region Fields
        private IConfigurationService _config;
        #endregion

        #region Properties
        public int X0 { get; set; }
        public int Y0 { get; set; }
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public bool Enabled { get; set; }
        public FilterKind FilterKind { get; set; }

        public RelayCommand OkCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        #endregion

        #region Constructors
        public ReaderFilterViewModel(IConfigurationService config)
        {
            _config = config;

            X0 = _config.ReaderFilterConfig.X0;
            X1 = _config.ReaderFilterConfig.X1;
            Y0 = _config.ReaderFilterConfig.Y0;
            Y1 = _config.ReaderFilterConfig.Y1;
            Enabled = _config.ReaderFilterConfig.Enabled;
            FilterKind = _config.ReaderFilterConfig.FilterKind;

            OkCommand = new RelayCommand(Ok, () => true);
            CancelCommand = new RelayCommand(Cancel, () => true);
        }
        #endregion

        #region Methods
        private void Ok()
        {
            _config.ReaderFilterConfig.X0 = X0;
            _config.ReaderFilterConfig.X1 = X1;
            _config.ReaderFilterConfig.Y0 = Y0;
            _config.ReaderFilterConfig.Y1 = Y1;
            _config.ReaderFilterConfig.Enabled = Enabled;
            _config.ReaderFilterConfig.FilterKind = FilterKind;

            Messenger.Default.Send(new FilterConfigMessage(this));
            Messenger.Default.Send(new RequestCloseMessage(this, true), this);
        }

        private void Cancel()
        {
            Messenger.Default.Send(new RequestCloseMessage(this, false), this);            
        }
        #endregion
    }
}
