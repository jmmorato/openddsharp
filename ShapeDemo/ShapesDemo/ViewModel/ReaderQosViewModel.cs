using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using OpenDDSharp.DDS;
using OpenDDSharp.ShapesDemo.Model;
using OpenDDSharp.ShapesDemo.Service;

namespace OpenDDSharp.ShapesDemo.ViewModel
{
    public class ReaderQosViewModel : ViewModelBase
    {
        #region Fields
        private IConfigurationService _config;
        private ReliabilityQosPolicyKind _reliabilityKind;
        private OwnershipQosPolicyKind _ownershipKind;
        private DurabilityQosPolicyKind _durabilityKind;
        private HistoryQosPolicyKind _historyKind;
        private int _historyDepth;
        private int _minSeparation;
        #endregion

        #region Properties
        public ReliabilityQosPolicyKind ReliabilityKind
        {
            get { return _reliabilityKind; }
            set
            {
                Set(ref _reliabilityKind, value);
            }
        }
        public OwnershipQosPolicyKind OwnershipKind
        {
            get { return _ownershipKind; }
            set
            {
                Set(ref _ownershipKind, value);
            }
        }
        public DurabilityQosPolicyKind DurabilityKind
        {
            get { return _durabilityKind; }
            set
            {
                Set(ref _durabilityKind, value);
            }
        }

        public HistoryQosPolicyKind HistoryKind
        {
            get { return _historyKind; }
            set
            {
                Set(ref _historyKind, value);
            }
        }

        public int HistoryDepth
        {
            get { return _historyDepth; }
            set
            {
                Set(ref _historyDepth, value);
            }
        }

        public int MinimumSeparation
        {
            get { return _minSeparation; }
            set
            {
                Set(ref _minSeparation, value);
            }
        }

        public RelayCommand OkCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        #endregion

        #region Constructors
        public ReaderQosViewModel(IConfigurationService config)
        {
            _config = config;

            _reliabilityKind = _config.ReaderQosConfig.ReliabilityKind;
            _ownershipKind = _config.ReaderQosConfig.OwnershipKind;
            _durabilityKind = _config.ReaderQosConfig.DurabilityKind;
            _historyKind = _config.ReaderQosConfig.HistoryKind;
            _historyDepth = _config.ReaderQosConfig.HistoryDepth;
            _minSeparation = _config.ReaderQosConfig.MinimumSeparation;

            OkCommand = new RelayCommand(Ok, () => true);
            CancelCommand = new RelayCommand(Cancel, () => true);
        }
        #endregion

        #region Methods
        private void Ok()
        {
            _config.ReaderQosConfig.ReliabilityKind = _reliabilityKind;
            _config.ReaderQosConfig.OwnershipKind = _ownershipKind;
            _config.ReaderQosConfig.DurabilityKind = _durabilityKind;
            _config.ReaderQosConfig.HistoryKind = _historyKind;
            _config.ReaderQosConfig.HistoryDepth = _historyDepth;
            _config.ReaderQosConfig.MinimumSeparation = _minSeparation;

            Messenger.Default.Send(new RequestCloseMessage(this, true), this);
        }

        private void Cancel()
        {
            Messenger.Default.Send(new RequestCloseMessage(this, false), this);
        }
        #endregion
    }
}
