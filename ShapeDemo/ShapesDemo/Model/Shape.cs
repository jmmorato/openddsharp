using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using OpenDDSharp.DDS;

namespace OpenDDSharp.ShapesDemo.Model
{
    public class Shape : INotifyPropertyChanged
    {
        #region Fields
        private string _color;
        private int _x;
        private int _y;
        private int _size;
        private InstanceHandle _publicationHandle;        
        private bool _isPublished;
        #endregion

        #region Properties
        public string Color
        {
            get { return _color; }
            set
            {
                _color = value;
                NotifyPropertyChanged();
            }
        }

        public int X
        {
            get { return _x; }
            set
            {
                _x = value;
                NotifyPropertyChanged();
            }
        }

        public int Y
        {
            get { return _y; }
            set
            {
                _y = value;
                NotifyPropertyChanged();
            }
        }

        public int Size
        {
            get { return _size; }
            set
            {
                _size = value;
                NotifyPropertyChanged();
            }
        }

        public InstanceHandle PublicationHandle
        {
            get { return _publicationHandle; }
            set
            {
                _publicationHandle = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsPublished
        {
            get { return _isPublished; }
            set
            {
                _isPublished = value;
                NotifyPropertyChanged();
            }
        }
        #endregion      

        #region INotifyPropertyChanged Members
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        
        private void NotifyPropertyChanged([CallerMemberName] String name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}
