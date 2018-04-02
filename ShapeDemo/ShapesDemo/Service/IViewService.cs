using GalaSoft.MvvmLight;
using System;

namespace OpenDDSharp.ShapesDemo.Service
{
    public interface IViewService : IDisposable
    {
        #region Methods
        void RegisterView(Type windowType, Type viewModelType);
        void OpenWindow(ViewModelBase viewModel);
        bool? OpenDialog(ViewModelBase viewModel);
        #endregion
    }
}
