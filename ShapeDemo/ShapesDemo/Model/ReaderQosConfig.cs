using OpenDDSharp.DDS;

namespace OpenDDSharp.ShapesDemo.Model
{
    public class ReaderQosConfig
    {
        #region Properties
        public ReliabilityQosPolicyKind ReliabilityKind { get; set; }
        public OwnershipQosPolicyKind OwnershipKind { get; set; }
        public DurabilityQosPolicyKind DurabilityKind { get; set; }
        public HistoryQosPolicyKind HistoryKind { get; set; }
        public int HistoryDepth { get; set; }
        public int MinimumSeparation { get; set; }
        #endregion

        #region Constructors
        public ReaderQosConfig()
        {
            ReliabilityKind = ReliabilityQosPolicyKind.BestEffortReliabilityQos;
            OwnershipKind = OwnershipQosPolicyKind.SharedOwnershipQos;
            DurabilityKind = DurabilityQosPolicyKind.VolatileDurabilityQos;
            HistoryKind = HistoryQosPolicyKind.KeepLastHistoryQos;
            HistoryDepth = 1;
            MinimumSeparation = 0;
        }
        #endregion
    }
}
