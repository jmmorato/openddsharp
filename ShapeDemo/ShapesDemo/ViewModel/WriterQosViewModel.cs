using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using OpenDDSharp.DDS;
using OpenDDSharp.ShapesDemo.Model;
using OpenDDSharp.ShapesDemo.Service;

namespace OpenDDSharp.ShapesDemo.ViewModel
{
    public class WriterQosViewModel : ViewModelBase
    {
        #region Fields
        private IConfigurationService _config;
        private ReliabilityQosPolicyKind _reliabilityKind;
        private OwnershipQosPolicyKind _ownershipKind;
        private DurabilityQosPolicyKind _durabilityKind;
        private int _ownershipStrength;
        private int _transportPriority;
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
        public int OwnershipStrength
        {
            get { return _ownershipStrength; }
            set
            {
                Set(ref _ownershipStrength, value);
            }
        }
        public int TransportPriority
        {
            get { return _transportPriority; }
            set
            {
                Set(ref _transportPriority, value);
            }
        }

        public RelayCommand OkCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        #endregion

        #region Constructors
        public WriterQosViewModel(IConfigurationService config)
        {
            _config = config;

            _reliabilityKind = _config.WriterQosConfig.ReliabilityKind; 
            _ownershipKind = _config.WriterQosConfig.OwnershipKind;
            _durabilityKind = _config.WriterQosConfig.DurabilityKind;
            _ownershipStrength = _config.WriterQosConfig.OwnershipStrength;
            _transportPriority = _config.WriterQosConfig.TransportPriority;

            OkCommand = new RelayCommand(Ok, () => true);
            CancelCommand = new RelayCommand(Cancel, () => true);
        }
        #endregion

        #region Methods
        private void Ok()
        {
            _config.WriterQosConfig.ReliabilityKind = _reliabilityKind;
            _config.WriterQosConfig.OwnershipKind = _ownershipKind;
            _config.WriterQosConfig.DurabilityKind = _durabilityKind;
            _config.WriterQosConfig.OwnershipStrength = _ownershipStrength;
            _config.WriterQosConfig.TransportPriority = _transportPriority;

            Messenger.Default.Send(new RequestCloseMessage(this, true), this);
        }

        private void Cancel()
        {
            Messenger.Default.Send(new RequestCloseMessage(this, false), this);
        }
        #endregion
    }
}
