using System;
using System.Threading;
using System.Collections.Generic;
using OpenDDSharp.DDS;
using OpenDDSharp.org.omg.dds.demo;

namespace OpenDDSharp.ShapesDemo.Model
{
    internal class ShapeWaitSet : IDisposable
    {
        #region Fields
        private bool _disposed;
        private WaitSet _waitSet;
        private Action<ShapeTypeDataReader> _dataAvailableFunc;
        private ShapeTypeDataReader _dataReader;
        private StatusCondition _statusCondition;
        private GuardCondition _cancelCondition;
        private Thread _thread;
        #endregion

        #region Constructors
        public ShapeWaitSet(ShapeTypeDataReader dataReader, Action<ShapeTypeDataReader> dataAvailableFunc)
        {
            _dataAvailableFunc = dataAvailableFunc;
            _dataReader = dataReader;

            _waitSet = new WaitSet();
            _thread = new Thread(DoThreadActivity)
            {
                IsBackground = true
            };

            _cancelCondition = new GuardCondition();
            _statusCondition = dataReader.StatusCondition;
            _waitSet.AttachCondition(_statusCondition);
            _waitSet.AttachCondition(_cancelCondition);
            _statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
            _thread.Start();
        }
        #endregion

        #region Methods
        private void DoThreadActivity()
        {
            while (true)
            {
                ICollection<Condition> conditions = new List<Condition>();
                Duration duration = new Duration
                {
                    Seconds = Duration.InfiniteSeconds
                };
                _waitSet.Wait(conditions, duration);

                foreach (Condition cond in conditions)
                {
                    if (cond == _cancelCondition && cond.TriggerValue)
                    {
                        // We reset the cancellation condition because it is a good practice, but in this implementation it probably doesn't change anything.
                        _cancelCondition.TriggerValue = false;

                        // The thread activity has been canceled. We exit even if another condition (data available)
                        // is active: no point extracting data if we're going down anyway.
                        return;
                    }

                    if (cond == _statusCondition && cond.TriggerValue)
                    {
                        StatusCondition sCond = (StatusCondition)cond;
                        StatusMask mask = sCond.EnabledStatuses;
                        if ((mask & StatusKind.DataAvailableStatus) != 0)
                            _dataAvailableFunc(_dataReader);
                    }
                }
            }
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            if (!_disposed)
            {
                _cancelCondition.TriggerValue = true;
                // We join the thread here because it may still be updating data (== using reader). _cancelCondition at true is the out condition.
                _thread.Join(500);

                _waitSet.DetachCondition(_cancelCondition);
                _waitSet.DetachCondition(_statusCondition);                

                _disposed = true;
            }
        }
        #endregion
    }
}
