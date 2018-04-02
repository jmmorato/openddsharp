using OpenDDSharp.ShapesDemo.Model;

namespace OpenDDSharp.ShapesDemo.Service
{
    public class ConfigurationService : IConfigurationService
    {
        #region Properties
        public WriterQosConfig WriterQosConfig { get; }
        public ReaderQosConfig ReaderQosConfig { get; }
        public ReaderFilterConfig ReaderFilterConfig { get; }
        #endregion

        #region Constructors
        public ConfigurationService()
        {
            WriterQosConfig = new WriterQosConfig();
            ReaderQosConfig = new ReaderQosConfig();
            ReaderFilterConfig = new ReaderFilterConfig();
        }
        #endregion
    }
}
