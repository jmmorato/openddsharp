using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace OpenDDSharp.ShapesDemo.Model
{
    public class FilterConfigMessage : MessageBase
    {
        #region Properties
        public ViewModelBase ViewModel { get; set; }
        #endregion

        #region Constructors
        public FilterConfigMessage(object sender) : base(sender) { }
        #endregion
    }
}
