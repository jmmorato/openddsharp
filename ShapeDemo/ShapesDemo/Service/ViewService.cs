using System;
using System.Linq;
using System.Windows;
using System.Diagnostics;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using OpenDDSharp.ShapesDemo.Model;

namespace OpenDDSharp.ShapesDemo.Service
{
    public class ViewService : IViewService
    {
        #region Fields
        private bool _disposed;
        private readonly Dictionary<Type, Type> _viewMap;
        private readonly List<Window> _openedWindows;
        #endregion

        #region Constructors
        public ViewService()
        {
            _viewMap = new Dictionary<Type, Type>();
            _openedWindows = new List<Window>();
        }
        #endregion

        #region Methods

        #region Public Methods
        public void RegisterView(Type windowType, Type viewModelType)
        {
            if (_disposed)
                return;

            lock (_viewMap)
            {
                if (_viewMap.ContainsKey(viewModelType))
                    throw new ArgumentException("ViewModel already registered");
                
                _viewMap[viewModelType] = windowType;
            }
        }

        [DebuggerStepThrough]
        public void OpenWindow(ViewModelBase viewModel)
        {
            if (_disposed)
                return;

            // Create window for that view tabModel.
            Window window = CreateWindow(viewModel);

            // Open the window.
            window.Show();
        }

        [DebuggerStepThrough]
        public bool? OpenDialog(ViewModelBase viewModel)
        {
            if (_disposed)
                return null;

            // Create window for that view tabModel.
            Window window = CreateWindow(viewModel);

            // Open the window and return the result.
            return window.ShowDialog();
        }
        #endregion

        #region Private Methods
        private Window CreateWindow(ViewModelBase viewModel)
        {
            Type windowType;
            lock (_viewMap)
            {
                if (!_viewMap.ContainsKey(viewModel.GetType()))
                    throw new ArgumentException("ViewModel not registered");
                windowType = _viewMap[viewModel.GetType()];
            }

            var window = (Window)Activator.CreateInstance(windowType);
            window.DataContext = viewModel;            
            window.Closed += OnClosed;

            if (Application.Current != null && !Equals(Application.Current.MainWindow, window))
            {
                if (Application.Current.MainWindow.IsLoaded)
                    window.Owner = Application.Current.MainWindow;
            }
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            lock (_openedWindows)
            {
                _openedWindows.Add(window);
            }

            // Listen for the close event
            Messenger.Default.Register<RequestCloseMessage>(window, viewModel, OnRequestClose);

            return window;
        }

        private void OnRequestClose(RequestCloseMessage message)
        {
            var window = _openedWindows.SingleOrDefault(w => w.DataContext == message.ViewModel);
            if (window != null)
            {
                Messenger.Default.Unregister<RequestCloseMessage>(window, message.ViewModel, OnRequestClose);
                if (message.DialogResult != null)
                {
                    // Trying to set the dialog result of the non-modal window will
                    // result in an InvalidOperationException
                    window.DialogResult = message.DialogResult;
                }
                window.Close();
            }
        }

        private void OnClosed(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            window.Closed -= OnClosed;

            lock (_openedWindows)
            {
                _openedWindows.Remove(window);
            }
        }
        #endregion

        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            if (!_disposed)
            {
                _viewMap.Clear();

                _openedWindows.Clear();

                _disposed = true;
            }
        }
        #endregion
    }
}
