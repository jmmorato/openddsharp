using OpenDDSharp.DDS;

namespace OpenDDSharp.ShapesDemo.Model
{
    public class WriterQosConfig
    {
        #region Properties
        public ReliabilityQosPolicyKind ReliabilityKind { get; set; }
        public OwnershipQosPolicyKind OwnershipKind { get; set; }
        public DurabilityQosPolicyKind DurabilityKind { get; set; }
        public int OwnershipStrength { get; set; }
        public int TransportPriority { get; set; }        
        #endregion

        #region Constructors
        public WriterQosConfig()
        {
            ReliabilityKind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            OwnershipKind = OwnershipQosPolicyKind.SharedOwnershipQos;
            DurabilityKind = DurabilityQosPolicyKind.VolatileDurabilityQos;
            OwnershipStrength = 50;
            TransportPriority = 50;
        }
        #endregion
    }
}
