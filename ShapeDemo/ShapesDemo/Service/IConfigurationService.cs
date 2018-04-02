
using OpenDDSharp.ShapesDemo.Model;

namespace OpenDDSharp.ShapesDemo.Service
{
    public interface IConfigurationService
    {
        #region Properties
        WriterQosConfig WriterQosConfig { get; }
        ReaderQosConfig ReaderQosConfig { get; }
        ReaderFilterConfig ReaderFilterConfig { get; }
        #endregion
    }
}
