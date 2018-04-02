
namespace OpenDDSharp.ShapesDemo.Model
{
    public class ReaderFilterConfig
    {
        #region Properties
        public int X0 { get; set; }
        public int Y0 { get; set; }
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public bool Enabled { get; set; }
        public FilterKind FilterKind { get; set; }
        #endregion

        #region Constructors
        public ReaderFilterConfig()
        {
            X0 = 100;
            X1 = 200;
            Y0 = 100;
            Y1 = 200;
            Enabled = false;
            FilterKind = FilterKind.Outside;
        }
        #endregion
    }
}
