using System;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using OpenDDSharp.ShapesDemo.Model;
using OpenDDSharp.ShapesDemo.Service;
using GalaSoft.MvvmLight.Messaging;

namespace OpenDDSharp.ShapesDemo.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Fields        
        private IOpenDDSharpService _dds;
        private IViewService _view;
        private IConfigurationService _config;
        private readonly object _lockPublishedSquares = new object();
        private readonly object _lockPublishedCircles = new object();
        private readonly object _lockPublishedTriangles = new object();
        private readonly object _lockSubscribedSquares = new object();
        private readonly object _lockSubscribedCircles = new object();
        private readonly object _lockSubscribedTriangles = new object();
        private ObservableCollection<SquareType> _publishedSquares;
        private ObservableCollection<CircleType> _publishedCircles;
        private ObservableCollection<TriangleType> _publishedTriangles;
        private ObservableCollection<SquareType> _subscribedSquares;
        private ObservableCollection<CircleType> _subscribedCircles;
        private ObservableCollection<TriangleType> _subscribedTriangles;
        private ShapeKind _selectedSubscriberShape;
        private ShapeKind _selectedPublisherShape;        
        private ShapeColor _selectedColor;
        private Random _random;
        private Rect _rect;
        private int _selectedSize;
        private int _selectedSpeed;
        #endregion

        #region Properties
        public CompositeCollection Shapes { get; }

        public ShapeKind SelectedSubscriberShape
        {
            get { return _selectedSubscriberShape; }
            set { Set(ref _selectedSubscriberShape, value); }
        }

        public ShapeKind SelectedPublisherShape
        {
            get { return _selectedPublisherShape; }
            set { Set(ref _selectedPublisherShape, value); }
        }

        public ShapeColor SelectedColor
        {
            get { return _selectedColor; }
            set { Set(ref _selectedColor, value); }
        }

        public int SelectedSize
        {
            get { return _selectedSize; }
            set { Set(ref _selectedSize, value); }
        }

        public int SelectedSpeed
        {
            get { return _selectedSpeed; }
            set { Set(ref _selectedSpeed, value); }
        }

        public ReaderFilterConfig FilterConfig
        {
            get { return _config.ReaderFilterConfig; }
        }

        public RelayCommand WriterQosCommand { get; private set; }
        public RelayCommand PublishShapeCommand { get; private set; }        
        public RelayCommand ReaderQosCommand { get; private set; }
        public RelayCommand ReaderFilterCommand { get; private set; }
        public RelayCommand SubscribeShapeCommand { get; private set; }        
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainWindowViewModel(IOpenDDSharpService dds, IViewService view, IConfigurationService config)
        {
            _dds = dds;
            _dds.SquareUpdated += DdsSquareUpdated;
            _dds.CircleUpdated += DdsCircleUpdated;
            _dds.TriangleUpdated += DdsTriangleUpdated;

            _view = view;
            _config = config;

            _publishedSquares = new ObservableCollection<SquareType>();
            _publishedCircles = new ObservableCollection<CircleType>();
            _publishedTriangles = new ObservableCollection<TriangleType>();
            _subscribedSquares = new ObservableCollection<SquareType>();            
            _subscribedCircles = new ObservableCollection<CircleType>();
            _subscribedTriangles = new ObservableCollection<TriangleType>();

            Shapes = new CompositeCollection
            {
                new CollectionContainer() { Collection = _publishedSquares },
                new CollectionContainer() { Collection = _publishedCircles },
                new CollectionContainer() { Collection = _publishedTriangles },
                new CollectionContainer() { Collection = _subscribedSquares },
                new CollectionContainer() { Collection = _subscribedCircles },
                new CollectionContainer() { Collection = _subscribedTriangles }
            };
            
            PublishShapeCommand = new RelayCommand(PublishShape, () => true);
            WriterQosCommand = new RelayCommand(WriterQos, () => true);
            ReaderQosCommand = new RelayCommand(ReaderQos, () => true);
            ReaderFilterCommand = new RelayCommand(ReaderFilter, () => true);            
            SubscribeShapeCommand = new RelayCommand(SubscribeShape, () => true);

            _rect = new Rect(0, 0, 321, 361);            
            _random = new Random();
            _selectedSubscriberShape = ShapeKind.Circle;
            _selectedPublisherShape = ShapeKind.Circle;
            _selectedColor = ShapeColor.Green;
            _selectedSize = 45;
            _selectedSpeed = 45;

            BindingOperations.EnableCollectionSynchronization(_publishedSquares, _lockPublishedSquares);
            BindingOperations.EnableCollectionSynchronization(_publishedCircles, _lockPublishedCircles);
            BindingOperations.EnableCollectionSynchronization(_publishedTriangles, _lockPublishedTriangles);
            BindingOperations.EnableCollectionSynchronization(_subscribedSquares, _lockSubscribedSquares);
            BindingOperations.EnableCollectionSynchronization(_subscribedCircles, _lockSubscribedCircles);
            BindingOperations.EnableCollectionSynchronization(_subscribedTriangles, _lockSubscribedTriangles);

            Messenger.Default.Register<FilterConfigMessage>(this, OnFilterConfigMessage);
        }        
        #endregion

        #region Methods
        private void WriterQos()
        {
            _view.OpenDialog(new WriterQosViewModel(_config));
        }

        private void PublishShape()
        {
            switch(_selectedPublisherShape)
            {
                case ShapeKind.Circle:
                    CircleType circle = new CircleType
                    {
                        Color = _selectedColor.ToString().ToUpper(),
                        Size = _selectedSize,
                        X = (int)(_random.NextDouble() * 300),
                        Y = (int)(_random.NextDouble() * 300),
                        IsPublished = true                            
                    };
                    
                    _publishedCircles.Add(circle);
                    _dds.Publish(circle, _rect, _selectedSpeed / 9);
                    break;
                case ShapeKind.Square:
                    SquareType square = new SquareType
                    {
                        Color = _selectedColor.ToString().ToUpper(),
                        Size = _selectedSize,
                        X = (int)(_random.NextDouble() * 300),
                        Y = (int)(_random.NextDouble() * 300),
                        IsPublished = true                    
                    };
                    
                    _publishedSquares.Add(square);

                    _dds.Publish(square, _rect, _selectedSpeed / 9);
                    break;
                case ShapeKind.Triangle:
                    TriangleType triangle = new TriangleType
                    {
                        Color = _selectedColor.ToString().ToUpper(),
                        Size = _selectedSize,
                        X = (int)(_random.NextDouble() * 300),
                        Y = (int)(_random.NextDouble() * 300),
                        IsPublished = true                        
                    };
                    
                    _publishedTriangles.Add(triangle);

                    _dds.Publish(triangle, _rect, _selectedSpeed / 9);
                    break;
            }
            
        }

        private void ReaderQos()
        {
            _view.OpenDialog(new ReaderQosViewModel(_config));
        }

        private void ReaderFilter()
        {
            _view.OpenDialog(new ReaderFilterViewModel(_config));
        }        

        private void SubscribeShape()
        {
            _dds.Subscribe(_selectedSubscriberShape);
        }

        private void DdsSquareUpdated(object sender, SquareType e)
        {
            SquareType square = _subscribedSquares.FirstOrDefault(s => s.Color == e.Color && s.PublicationHandle == e.PublicationHandle);
            if (square != null)
            {
                square.X = e.X;
                square.Y = e.Y;
            }
            else
            {
                _subscribedSquares.Add(e);
            }
        }

        private void DdsCircleUpdated(object sender, CircleType e)
        {
            CircleType circle = _subscribedCircles.FirstOrDefault(s => s.Color == e.Color && s.PublicationHandle == e.PublicationHandle);
            if (circle != null)
            {
                circle.X = e.X;
                circle.Y = e.Y;                
            }
            else
            {
                _subscribedCircles.Add(e);
            }
        }

        private void DdsTriangleUpdated(object sender, TriangleType e)
        {
            TriangleType triangle = _subscribedTriangles.FirstOrDefault(s => s.Color == e.Color && s.PublicationHandle == e.PublicationHandle);
            if (triangle != null)
            {
                triangle.X = e.X;
                triangle.Y = e.Y;               
            }
            else
            {
                _subscribedTriangles.Add(e);
            }
        }

        public void OnFilterConfigMessage(FilterConfigMessage obj)
        {
            RaisePropertyChanged("FilterConfig");
        }

        public override void Cleanup()
        {
            Messenger.Default.Unregister<FilterConfigMessage>(this, OnFilterConfigMessage);

            _dds.SquareUpdated -= DdsSquareUpdated;
            _dds.CircleUpdated -= DdsCircleUpdated;
            _dds.TriangleUpdated -= DdsTriangleUpdated;

            PublishShapeCommand = null;
            WriterQosCommand = null;
            ReaderQosCommand = null;
            ReaderFilterCommand = null;
            SubscribeShapeCommand = null;

            base.Cleanup();
        }
        #endregion
    }
}