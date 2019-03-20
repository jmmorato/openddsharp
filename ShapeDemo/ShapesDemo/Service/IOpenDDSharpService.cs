using System;
using System.Windows;
using OpenDDSharp.ShapesDemo.Model;

namespace OpenDDSharp.ShapesDemo.Service
{
    public interface IOpenDDSharpService : IDisposable
    {
        #region Events
        event EventHandler<SquareType> SquareUpdated;
        event EventHandler<CircleType> CircleUpdated;
        event EventHandler<TriangleType> TriangleUpdated;
        #endregion

        #region Properties
        InteroperatibilityProvider Provider { get; set; }
        #endregion

        #region Methods
        void Publish(Shape shape, Rect constraint, int speed);

        void Subscribe(ShapeKind shapeKind);
        #endregion
    }
}
