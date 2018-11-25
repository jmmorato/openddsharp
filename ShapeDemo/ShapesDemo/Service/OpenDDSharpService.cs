using System;
using System.Windows;
using System.Collections.Generic;
using OpenDDSharp.DDS;
using OpenDDSharp.OpenDDS.DCPS;
using OpenDDSharp.ShapesDemo.Model;
using OpenDDSharp.org.omg.dds.demo;
using CommonServiceLocator;
using OpenDDSharp.OpenDDS.RTPS;

namespace OpenDDSharp.ShapesDemo.Service
{
    public sealed class OpenDDSharpService : IOpenDDSharpService
    {
        #region Constants
        private const string RTPS_DISCOVERY = "RtpsDiscovery";
        private const string TYPE_NAME = "ShapeType";
        private const string SQUARE_TOPIC_NAME = "Square";
        private const string CIRCLE_TOPIC_NAME = "Circle";
        private const string TRIANGLE_TOPIC_NAME = "Triangle";
        private string FILTER_OUTSIDE = "(x BETWEEN %0 AND %1) AND (y BETWEEN %2 AND %3)";
        private string FILTER_INSIDE = "(x < %0) OR(x > %1) OR(y< %2) OR(y > %3)";
        #endregion

        #region Events
        public event EventHandler<SquareType> SquareUpdated;
        public event EventHandler<CircleType> CircleUpdated;
        public event EventHandler<TriangleType> TriangleUpdated;
        #endregion

        #region Fields
        private bool _disposed;
        private IConfigurationService _config;
        private DomainParticipantFactory _domainFactory;
        private DomainParticipant _participant;
        private Publisher _publisher;
        private Subscriber _subscriber;        
        private Topic _squareTopic;
        private Topic _circleTopic;
        private Topic _triangleTopic;        
        private List<ShapeWaitSet> _shapeWaitSets;
        private List<ShapeDynamic> _shapeDynamics;        
        private int _cfCircleCount;
        private int _cfSquareCount;
        private int _cfTriangleCount;
        private RtpsDiscovery _disc;
        private TransportConfig _tConfig;
        private TransportInst _inst;
        private RtpsUdpInst _rui;
        #endregion

        #region Constructors
        public OpenDDSharpService()
        {
            _config = ServiceLocator.Current.GetInstance<IConfigurationService>();

            _disc = new RtpsDiscovery(RTPS_DISCOVERY);
            ParticipantService.Instance.AddDiscovery(_disc);
            ParticipantService.Instance.DefaultDiscovery = RTPS_DISCOVERY;

            long ticks = DateTime.Now.Ticks;
            string configName = "openddsharp_rtps_interop";
            string instName = "internal_openddsharp_rtps_transport";

            _tConfig = TransportRegistry.Instance.CreateConfig(configName);
            _inst = TransportRegistry.Instance.CreateInst(instName, "rtps_udp");
            _rui = new RtpsUdpInst(_inst);
            _tConfig.Insert(_inst);
            TransportRegistry.Instance.GlobalConfig = _tConfig;

            _domainFactory = ParticipantService.Instance.GetDomainParticipantFactory("-DCPSDebugLevel", "10", "-ORBLogFile", "LogFile.log", "-ORBDebugLevel", "10");

            _participant = _domainFactory.CreateParticipant(0);
            if (_participant == null)
            {
                throw new Exception("Could not create the participant");
            }
            
            ShapeTypeTypeSupport support = new ShapeTypeTypeSupport();                
            ReturnCode result = support.RegisterType(_participant, TYPE_NAME);
            if (result != ReturnCode.Ok)
            {
                throw new Exception("Could not register type: " + result.ToString());
            }
                                                        
            _squareTopic = _participant.CreateTopic(SQUARE_TOPIC_NAME, TYPE_NAME);
            if (_squareTopic == null)
            {
                throw new Exception("Could not create square topic");
            }

            _circleTopic = _participant.CreateTopic(CIRCLE_TOPIC_NAME, TYPE_NAME);
            if (_circleTopic == null)
            {
                throw new Exception("Could not create circle topic");
            }

            _triangleTopic = _participant.CreateTopic(TRIANGLE_TOPIC_NAME, TYPE_NAME);
            if (_triangleTopic == null)
            {
                throw new Exception("Could not create triangle topic");
            }

            _publisher = _participant.CreatePublisher();
            if (_publisher == null)
            {
                throw new Exception("Could not create publisher");
            }

            _subscriber = _participant.CreateSubscriber();
            if (_subscriber == null)
            {
                throw new Exception("Could not create subscriber");
            }

            _shapeWaitSets = new List<ShapeWaitSet>();
            _shapeDynamics = new List<ShapeDynamic>();

            _cfCircleCount = 0;
            _cfSquareCount = 0;
            _cfTriangleCount = 0;
        }        
        #endregion

        #region Methods

        #region Public Methods
        public void Publish(Shape shape, Rect constraint, int speed)
        {
            if (shape is CircleType)
            {
                _shapeDynamics.Add(new ShapeDynamic(CreateCircleWriter(), shape, constraint, speed));
            }
            else if (shape is SquareType)
            {
                _shapeDynamics.Add(new ShapeDynamic(CreateSquareWriter(), shape, constraint, speed));
            }
            else if (shape is TriangleType)
            {
                _shapeDynamics.Add(new ShapeDynamic(CreateTriangleWriter(), shape, constraint, speed));
            }                           
        }

        public void Subscribe(ShapeKind shapeKind)
        {
            if (!_disposed)
            {
                switch (shapeKind)
                {
                    case ShapeKind.Circle:
                        _shapeWaitSets.Add(CreateCircleReader());
                        break;
                    case ShapeKind.Square:
                        _shapeWaitSets.Add(CreateSquareReader());
                        break;
                    case ShapeKind.Triangle:
                        _shapeWaitSets.Add(CreateTriangleReader());
                        break;
                }
            }
        }
        #endregion

        #region Private Methods
        private ShapeTypeDataWriter CreateSquareWriter()
        {
            DataWriterQos qos = GetDataWriterQos();
            DataWriter writer = _publisher.CreateDataWriter(_squareTopic);
            if (writer == null)
            {
                throw new Exception("Could not create square data writer");
            }

            ShapeTypeDataWriter squareDataWriter = new ShapeTypeDataWriter(writer);
            return squareDataWriter;
        }

        private ShapeTypeDataWriter CreateCircleWriter()
        {
            DataWriterQos qos = GetDataWriterQos();
            DataWriter writer = _publisher.CreateDataWriter(_circleTopic, qos);
            if (writer == null)
            {
                throw new Exception("Could not create circle data writer");
            }

            ShapeTypeDataWriter circleDataWriter = new ShapeTypeDataWriter(writer);
            return circleDataWriter;
        }        

        private ShapeTypeDataWriter CreateTriangleWriter()
        {
            DataWriterQos qos = GetDataWriterQos();
            DataWriter writer = _publisher.CreateDataWriter(_triangleTopic, qos);
            if (writer == null)
            {
                throw new Exception("Could not create triangle data writer");
            }

            ShapeTypeDataWriter triangleDataWriter = new ShapeTypeDataWriter(writer);
            return triangleDataWriter;
        }        

        private ShapeWaitSet CreateSquareReader()
        {
            DataReaderQos qos = GetDataReaderQos();

            ITopicDescription topic = null;
            if (_config.ReaderFilterConfig.Enabled)
            {
                string filter = _config.ReaderFilterConfig.FilterKind == FilterKind.Inside ? FILTER_INSIDE : FILTER_OUTSIDE;
                topic = _participant.CreateContentFilteredTopic("CFSquare" + (++_cfSquareCount), 
                                                                _squareTopic, filter,
                                                                _config.ReaderFilterConfig.X0.ToString(),
                                                                _config.ReaderFilterConfig.X1.ToString(),
                                                                _config.ReaderFilterConfig.Y0.ToString(),
                                                                _config.ReaderFilterConfig.Y1.ToString());
            }
            else
            {
                topic = _squareTopic;
            }

            DataReader reader =_subscriber.CreateDataReader(topic, qos);
            if (reader == null)
            {
                throw new Exception("Could not create square data reader");
            }

            ShapeTypeDataReader squareDataReader = new ShapeTypeDataReader(reader);
            return new ShapeWaitSet(squareDataReader, SquareTopicDataAvailable);
        }        

        private ShapeWaitSet CreateCircleReader()
        {
            DataReaderQos qos = GetDataReaderQos();

            ITopicDescription topic = null;
            if (_config.ReaderFilterConfig.Enabled)
            {
                string filter = _config.ReaderFilterConfig.FilterKind == FilterKind.Inside ? FILTER_INSIDE : FILTER_OUTSIDE;
                topic = _participant.CreateContentFilteredTopic("CFCircle" + (++_cfCircleCount), 
                                                                _circleTopic, filter,  
                                                                _config.ReaderFilterConfig.X0.ToString(),
                                                                _config.ReaderFilterConfig.X1.ToString(),
                                                                _config.ReaderFilterConfig.Y0.ToString(),
                                                                _config.ReaderFilterConfig.Y1.ToString());
            }
            else
            {
                topic = _circleTopic;
            }

            DataReader reader = _subscriber.CreateDataReader(topic, qos);
            if (reader == null)
            {
                throw new Exception("Could not create circle data reader");
            }

            ShapeTypeDataReader circleDataReader = new ShapeTypeDataReader(reader);
            return new ShapeWaitSet(circleDataReader, CircleTopicDataAvailable);
        }

        private ShapeWaitSet CreateTriangleReader()
        {
            DataReaderQos qos = GetDataReaderQos();

            ITopicDescription topic = null;
            if (_config.ReaderFilterConfig.Enabled)
            {
                string filter = _config.ReaderFilterConfig.FilterKind == FilterKind.Inside ? FILTER_INSIDE : FILTER_OUTSIDE;
                topic = _participant.CreateContentFilteredTopic("CFTriangle" + (++_cfTriangleCount), 
                                                                _triangleTopic, filter, 
                                                                _config.ReaderFilterConfig.X0.ToString(),
                                                                _config.ReaderFilterConfig.X1.ToString(),
                                                                _config.ReaderFilterConfig.Y0.ToString(),
                                                                _config.ReaderFilterConfig.Y1.ToString());
            }
            else
            {
                topic = _triangleTopic;
            }

            DataReader reader = _subscriber.CreateDataReader(topic, qos);
            if (reader == null)
            {
                throw new Exception("Could not create triangle data reader");
            }

            ShapeTypeDataReader triangleDataReader = new ShapeTypeDataReader(reader);
            return new ShapeWaitSet(triangleDataReader, TriangleTopicDataAvailable);
        }

        private DataWriterQos GetDataWriterQos()
        {
            DataWriterQos dataWriterQos = new DataWriterQos();
            _publisher.GetDefaultDataWriterQos(dataWriterQos);

            dataWriterQos.Reliability.Kind = _config.WriterQosConfig.ReliabilityKind;
            dataWriterQos.Ownership.Kind = _config.WriterQosConfig.OwnershipKind;
            dataWriterQos.OwnershipStrength.Value = _config.WriterQosConfig.OwnershipStrength;
            dataWriterQos.Durability.Kind = _config.WriterQosConfig.DurabilityKind;
            dataWriterQos.TransportPriority.Value = _config.WriterQosConfig.TransportPriority;

            return dataWriterQos;
        }

        private DataReaderQos GetDataReaderQos()
        {
            DataReaderQos dataReaderQos = new DataReaderQos();
            _subscriber.GetDefaultDataReaderQos(dataReaderQos);

            dataReaderQos.Reliability.Kind = _config.ReaderQosConfig.ReliabilityKind;
            dataReaderQos.Ownership.Kind = _config.ReaderQosConfig.OwnershipKind;
            dataReaderQos.Durability.Kind = _config.ReaderQosConfig.DurabilityKind;
            dataReaderQos.History.Kind = _config.ReaderQosConfig.HistoryKind;
            dataReaderQos.History.Depth = _config.ReaderQosConfig.HistoryDepth;            
            dataReaderQos.TimeBasedFilter.MinimumSeparation = new DDS.Duration
            {
                Seconds = _config.ReaderQosConfig.MinimumSeparation,
                NanoSeconds = 0
            };

            return dataReaderQos;
        }

        private void SquareTopicDataAvailable(ShapeTypeDataReader dr)
        {            
            List<ShapeType> samples = new List<ShapeType>();
            List<SampleInfo> infos = new List<SampleInfo>();
            ReturnCode error = dr.Take(samples, infos);
            if (error == ReturnCode.Ok)
            {
                for (int i = 0; i < samples.Count; i++)
                {
                    SampleInfo info = infos[i];
                    ShapeType sample = samples[i];
                    if (info.ValidData)
                    {                        
                        int x = sample.x - (sample.shapesize / 2);
                        int y = sample.y - (sample.shapesize / 2);
                        SquareType square = new SquareType
                        {
                            Color = sample.color,                            
                            X = x,
                            Y = y,
                            Size = sample.shapesize,
                            PublicationHandle = info.PublicationHandle
                        };
                        
                        SquareUpdated?.Invoke(this, square);
                        
                    }
                }
            }
        }

        private void CircleTopicDataAvailable(ShapeTypeDataReader dr)
        {            
            List<ShapeType> samples = new List<ShapeType>();
            List<SampleInfo> infos = new List<SampleInfo>();
            ReturnCode error = dr.Take(samples, infos);
            if (error == ReturnCode.Ok)
            {
                for (int i = 0; i < samples.Count; i++)
                {                    
                    SampleInfo info = infos[i];                    
                    ShapeType sample = samples[i];
                    if (info.ValidData)
                    {
                        int x = sample.x - (sample.shapesize / 2);
                        int y = sample.y - (sample.shapesize / 2);                        
                        CircleType square = new CircleType
                        {
                            Color = sample.color,
                            X = x,
                            Y = y,
                            Size = sample.shapesize,
                            PublicationHandle = info.PublicationHandle
                        };

                        CircleUpdated?.Invoke(this, square);

                    }
                }
            }
        }

        private void TriangleTopicDataAvailable(ShapeTypeDataReader dr)
        {
            List<ShapeType> samples = new List<ShapeType>();
            List<SampleInfo> infos = new List<SampleInfo>();
            ReturnCode error = dr.Take(samples, infos);
            if (error == ReturnCode.Ok)
            {
                for (int i = 0; i < samples.Count; i++)
                {
                    SampleInfo info = infos[i];
                    ShapeType sample = samples[i];
                    if (info.ValidData)
                    {
                        int x = sample.x - (sample.shapesize / 2);
                        int y = sample.y - (sample.shapesize / 2);
                        TriangleType square = new TriangleType
                        {
                            Color = sample.color,
                            X = x,
                            Y = y,
                            Size = sample.shapesize,
                            PublicationHandle = info.PublicationHandle
                        };

                        TriangleUpdated?.Invoke(this, square);
                    }
                }
            }
        }
        #endregion

        #endregion

        #region IDisposable Members
        [System.STAThread]
        public void Dispose()
        {
            if (!_disposed)
            {
                foreach (ShapeWaitSet waitSet in _shapeWaitSets)
                    waitSet.Dispose();

                _shapeWaitSets.Clear();

                foreach (ShapeDynamic shape in _shapeDynamics)
                    shape.Dispose();

                _shapeDynamics.Clear();

                if (_participant != null)
                {
                    _participant.DeleteContainedEntities();
                    _domainFactory.DeleteParticipant(_participant);                    
                }
                
                ParticipantService.Instance.Shutdown();

                _disposed = true;
            }
        }
        #endregion
    }    
}
