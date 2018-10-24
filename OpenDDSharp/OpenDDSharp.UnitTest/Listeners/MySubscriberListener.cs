/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS.
Copyright (C) 2018 Jose Morato

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

OpenDDSharp is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
using System;
using OpenDDSharp.DDS;
using OpenDDSharp.OpenDDS.DCPS;

namespace OpenDDSharp.UnitTest.Listeners
{
    public class MySubscriberListener : SubscriberListener
    {
        public Action<DataReader, BudgetExceededStatus> BudgetExceeded;
        public Action<DataReader> ConnectionDeleted;
        public Action<DataReader> DataAvailable;
        public Action<Subscriber> DataOnReaders;
        public Action<DataReader, LivelinessChangedStatus> LivelinessChanged;
        public Action<DataReader, RequestedDeadlineMissedStatus> RequestedDeadlineMissed;
        public Action<DataReader, RequestedIncompatibleQosStatus> RequestedIncompatibleQos;
        public Action<DataReader, SampleLostStatus> SampleLost;
        public Action<DataReader, SampleRejectedStatus> SampleRejected;
        public Action<DataReader, SubscriptionDisconnectedStatus> SubscriptionDisconnected;
        public Action<DataReader, SubscriptionLostStatus> SubscriptionLost;
        public Action<DataReader, SubscriptionMatchedStatus> SubscriptionMatched;
        public Action<DataReader, SubscriptionReconnectedStatus> SubscriptionReconnected;

        public override void OnBudgetExceeded(DataReader reader, BudgetExceededStatus status)
        {
            BudgetExceeded?.Invoke(reader, status);
        }

        public override void OnDataAvailable(DataReader reader)
        {
            DataAvailable?.Invoke(reader);
        }

        public override void OnDataOnReaders(Subscriber subscriber)
        {
            DataOnReaders?.Invoke(subscriber);
        }

        public override void OnLivelinessChanged(DataReader reader, LivelinessChangedStatus status)
        {
            LivelinessChanged?.Invoke(reader, status);
        }

        public override void OnRequestedDeadlineMissed(DataReader reader, RequestedDeadlineMissedStatus status)
        {
            RequestedDeadlineMissed?.Invoke(reader, status);
        }

        public override void OnRequestedIncompatibleQos(DataReader reader, RequestedIncompatibleQosStatus status)
        {
            RequestedIncompatibleQos?.Invoke(reader, status);
        }

        public override void OnSampleLost(DataReader reader, SampleLostStatus status)
        {
            SampleLost?.Invoke(reader, status);
        }

        public override void OnSampleRejected(DataReader reader, SampleRejectedStatus status)
        {
            SampleRejected?.Invoke(reader, status);
        }

        public override void OnSubscriptionDisconnected(DataReader reader, SubscriptionDisconnectedStatus status)
        {
            SubscriptionDisconnected?.Invoke(reader, status);
        }

        public override void OnSubscriptionLost(DataReader reader, SubscriptionLostStatus status)
        {
            SubscriptionLost?.Invoke(reader, status);
        }

        public override void OnSubscriptionMatched(DataReader reader, SubscriptionMatchedStatus status)
        {
            SubscriptionMatched?.Invoke(reader, status);
        }

        public override void OnSubscriptionReconnected(DataReader reader, SubscriptionReconnectedStatus status)
        {
            SubscriptionReconnected?.Invoke(reader, status);
        }
    }
}
