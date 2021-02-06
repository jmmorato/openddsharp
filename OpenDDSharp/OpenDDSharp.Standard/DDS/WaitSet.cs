/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

OpenDDSharp is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using OpenDDSharp.Helpers;

namespace OpenDDSharp.DDS
{
    /// <summary>
    /// A WaitSet object allows an application to wait until one or more of the attached <see cref="Condition" /> objects has a <see cref="Condition.TriggerValue" /> of
    /// <see langword="true"/> or else until the timeout expires.
    /// </summary>
    /// <remarks>
    /// WaitSet has no factory. This is because it is not necessarily associated with a single DomainParticipant and could be used to wait on
    /// <see cref="Condition" /> objects associated with different <see cref="DomainParticipant" /> objects.
    /// </remarks>
    public class WaitSet
    {
        #region Fields
        private readonly IntPtr _native;
        private readonly ConcurrentDictionary<IntPtr, Condition> _conditions;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="WaitSet"/> class.
        /// </summary>
        public WaitSet() : this(NewWaitSet())
        {
        }

        internal WaitSet(IntPtr native)
        {
            _native = native;
            _conditions = new ConcurrentDictionary<IntPtr, Condition>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// <para>This operation allows an application thread to wait for the occurrence of certain conditions. If none of the conditions attached
        /// to the <see cref="WaitSet" /> have a <see cref="Condition.TriggerValue" /> of <see langword="true"/>, the wait operation will block suspending the calling thread.</para>
        /// <para>The wait operation will wait infinite time for the conditions.</para>
        /// </summary>
        /// <remarks>
        /// It is not allowed for more than one application thread to be waiting on the same <see cref="WaitSet" />. If the wait operation is invoked on a
        /// <see cref="WaitSet" /> that already has a thread blocking on it, the operation will return immediately with the value <see cref="ReturnCode.PreconditionNotMet" />.
        /// </remarks>
        /// <param name="activeConditions">
        /// The collection of <see cref="Condition" />s with the <see cref="Condition.TriggerValue" /> equals <see langword="true"/> when the thread is unblocked.
        /// </param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode Wait(ICollection<Condition> activeConditions)
        {
            Duration duration = default;
            duration.Seconds = Duration.InfiniteSeconds;
            duration.NanoSeconds = Duration.InfiniteNanoseconds;

            return Wait(activeConditions, duration);
        }

        /// <summary>
        /// This operation allows an application thread to wait for the occurrence of certain conditions. If none of the conditions attached
        /// to the <see cref="WaitSet" /> have a <see cref="Condition.TriggerValue" /> of <see langword="true"/>, the wait operation will block suspending the calling thread.
        /// </summary>
        /// <remarks>
        /// <para>It is not allowed for more than one application thread to be waiting on the same <see cref="WaitSet" />. If the wait operation is invoked on a
        /// <see cref="WaitSet" /> that already has a thread blocking on it, the operation will return immediately with the value <see cref="ReturnCode.PreconditionNotMet" />.</para>
        /// <para>The wait operation takes a timeout argument that specifies the maximum duration for the wait. It this duration is exceeded and
        /// none of the attached <see cref="Condition" /> objects is <see langword="true"/>, wait will return with the return code <see cref="ReturnCode.Timeout" />.</para>
        /// </remarks>
        /// <param name="activeConditions">
        /// The collection of <see cref="Condition" />s with the <see cref="Condition.TriggerValue" /> equals <see langword="true"/> when the thread is unblocked.
        /// </param>
        /// <param name="timeout">Maximum duration for the wait.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode Wait(ICollection<Condition> activeConditions, Duration timeout)
        {
            if (activeConditions == null)
            {
                return ReturnCode.BadParameter;
            }
            activeConditions.Clear();

            IntPtr seq = IntPtr.Zero;
            ReturnCode ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.Wait86(_native, ref seq, timeout),
                                                         () => UnsafeNativeMethods.Wait64(_native, ref seq, timeout));

            if (ret == ReturnCode.Ok && !seq.Equals(IntPtr.Zero))
            {
                ICollection<IntPtr> lst = new Collection<IntPtr>();
                seq.PtrToSequence(ref lst);
                foreach (IntPtr ptr in lst)
                {
                    if (_conditions.TryGetValue(ptr, out var condition))
                    {
                        activeConditions.Add(condition);
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Attaches a <see cref="Condition" /> to the <see cref="WaitSet" />.
        /// </summary>
        /// <remarks>
        /// <para>It is possible to attach a <see cref="Condition" /> on a <see cref="WaitSet" /> that is currently being waited upon (via the wait operation). In this case, if the
        /// <see cref="Condition" /> has a <see cref="Condition.TriggerValue" /> of <see langword="true"/>, then attaching the condition will unblock the <see cref="WaitSet" />.</para>
        /// <para>Adding a <see cref="Condition" /> that is already attached to the <see cref="WaitSet" /> has no effect.</para>
        /// </remarks>
        /// <param name="cond">The <see cref="Condition" /> to be attached.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode AttachCondition(Condition cond)
        {
            if (cond == null)
            {
                return ReturnCode.BadParameter;
            }

            ReturnCode ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.AttachCondition86(_native, cond.ToNative()),
                                                         () => UnsafeNativeMethods.AttachCondition64(_native, cond.ToNative()));

            if (ret == ReturnCode.Ok)
            {
                _conditions.AddOrUpdate(cond.ToNative(), cond, (p, t) => cond);
            }

            return ret;
        }

        /// <summary>
        /// Detaches a <see cref="Condition" /> from the <see cref="WaitSet" />.
        /// </summary>
        /// <remarks>
        /// If the <see cref="Condition" /> was not attached to the <see cref="WaitSet" />, the operation will return <see cref="ReturnCode.PreconditionNotMet" />.
        /// </remarks>
        /// <param name="cond">The <see cref="Condition" /> to be detached.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode DetachCondition(Condition cond)
        {
            if (cond == null)
            {
                return ReturnCode.BadParameter;
            }

            ReturnCode ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.DetachCondition86(_native, cond.ToNative()),
                                                         () => UnsafeNativeMethods.DetachCondition64(_native, cond.ToNative()));

            if (ret == ReturnCode.Ok)
            {
                _conditions.TryRemove(cond.ToNative(), out _);
            }

            return ret;
        }

        /// <summary>
        /// Retrieves the list of attached conditions.
        /// </summary>
        /// <param name="attachedConditions">The collection of <see cref="Condition" />s to be filled up.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode GetConditions(ICollection<Condition> attachedConditions)
        {
            if (attachedConditions == null)
            {
                return ReturnCode.BadParameter;
            }
            attachedConditions.Clear();

            IntPtr seq = IntPtr.Zero;
            ReturnCode ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.GetConditions86(_native, ref seq),
                                                         () => UnsafeNativeMethods.GetConditions64(_native, ref seq));

            if (ret == ReturnCode.Ok && !seq.Equals(IntPtr.Zero))
            {
                IList<IntPtr> lst = new List<IntPtr>();
                MarshalHelper.PtrToSequence(seq, ref lst);
                if (lst != null)
                {
                    foreach (IntPtr ptrCondition in lst)
                    {
                        if (_conditions.TryGetValue(ptrCondition, out var condition))
                        {
                            attachedConditions.Add(condition);
                        }
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Convenience method for detaching multiple conditions, for example when shutting down.
        /// </summary>
        /// <param name="conditions">The collection of <see cref="Condition" />s to be detached.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode DetachConditions(ICollection<Condition> conditions)
        {
            if (conditions == null)
            {
                return ReturnCode.Ok;
            }

            IntPtr seq = IntPtr.Zero;
            ICollection<IntPtr> pointers = conditions.Select(c => c.ToNative()).ToList();
            MarshalHelper.SequenceToPtr(pointers, ref seq);
            ReturnCode ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.DetachConditions86(_native, seq),
                                                         () => UnsafeNativeMethods.DetachConditions64(_native, seq));

            if (ret == ReturnCode.Ok)
            {
                foreach (var ptr in pointers)
                {
                    _conditions.TryRemove(ptr, out _);
                }
            }

            return ret;
        }

        private static IntPtr NewWaitSet()
        {
            return MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.NewWaitset86(),
                                               () => UnsafeNativeMethods.NewWaitset64());
        }
        #endregion

        #region UnsafeNativeMethods
        /// <summary>
        /// This class suppresses stack walks for unmanaged code permission. (System.Security.SuppressUnmanagedCodeSecurityAttribute is applied to this class.)
        /// This class is for methods that are potentially dangerous. Any caller of these methods must perform a full security review to make sure that the usage
        /// is secure because no stack walk will be performed.
        /// </summary>
        [SuppressUnmanagedCodeSecurity]
        private static class UnsafeNativeMethods
        {
            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "WaitSet_New", CallingConvention = CallingConvention.StdCall)]
            public static extern IntPtr NewWaitset64();

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "WaitSet_New", CallingConvention = CallingConvention.StdCall)]
            public static extern IntPtr NewWaitset86();

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "WaitSet_Wait", CallingConvention = CallingConvention.StdCall)]
            public static extern ReturnCode Wait64(IntPtr ws, ref IntPtr seq, [MarshalAs(UnmanagedType.Struct), In] Duration duration);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "WaitSet_Wait", CallingConvention = CallingConvention.StdCall)]
            public static extern ReturnCode Wait86(IntPtr ws, ref IntPtr seq, [MarshalAs(UnmanagedType.Struct), In] Duration duration);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "WaitSet_AttachCondition", CallingConvention = CallingConvention.StdCall)]
            public static extern ReturnCode AttachCondition64(IntPtr ws, IntPtr condition);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "WaitSet_AttachCondition", CallingConvention = CallingConvention.StdCall)]
            public static extern ReturnCode AttachCondition86(IntPtr ws, IntPtr condition);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "WaitSet_DetachCondition", CallingConvention = CallingConvention.StdCall)]
            public static extern ReturnCode DetachCondition64(IntPtr ws, IntPtr condition);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "WaitSet_DetachCondition", CallingConvention = CallingConvention.StdCall)]
            public static extern ReturnCode DetachCondition86(IntPtr ws, IntPtr condition);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "WaitSet_GetConditions", CallingConvention = CallingConvention.StdCall)]
            public static extern ReturnCode GetConditions64(IntPtr ws, ref IntPtr seq);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "WaitSet_GetConditions", CallingConvention = CallingConvention.StdCall)]
            public static extern ReturnCode GetConditions86(IntPtr ws, ref IntPtr seq);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "WaitSet_DetachConditions", CallingConvention = CallingConvention.StdCall)]
            public static extern ReturnCode DetachConditions64(IntPtr ws, IntPtr seq);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "WaitSet_DetachConditions", CallingConvention = CallingConvention.StdCall)]
            public static extern ReturnCode DetachConditions86(IntPtr ws, IntPtr seq);
        }
        #endregion
    }
}