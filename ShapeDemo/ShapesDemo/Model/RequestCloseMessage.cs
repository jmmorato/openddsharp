using GalaSoft.MvvmLight;

namespace OpenDDSharp.ShapesDemo.Model
{
    public class RequestCloseMessage
    {
        #region Properties
        public ViewModelBase ViewModel { get; set; }
        public bool? DialogResult { get; set; }
        #endregion

        #region Constructors
        public RequestCloseMessage(ViewModelBase viewModel, bool? dialogResult = null)
        {
            ViewModel = viewModel;
            DialogResult = dialogResult;
        }
        #endregion
    }
}
