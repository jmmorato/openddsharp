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
using System.Runtime.InteropServices;
using System.Security;
using OpenDDSharp.Helpers;

#if NET7_0_OR_GREATER
using System.Runtime.CompilerServices;
#endif

namespace OpenDDSharp.DDS;

/// <summary>
/// Abstract class that can be implemented by an application-provided class and then registered with
/// the <see cref="Topic" /> such that the application can be notified of relevant status changes.
/// </summary>
public abstract class TopicListener : IDisposable
{
    #region Delegates
    private delegate void OnInconsistentTopicDelegate(IntPtr topic, ref InconsistentTopicStatus status);
    #endregion

    #region Fields
    private readonly IntPtr _native;
    private bool _disposed;

    private GCHandle _gchInconsistentTopic;
    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="TopicListener"/> class.
    /// </summary>
    protected TopicListener()
    {
        OnInconsistentTopicDelegate onInconsistentTopic = OnInconsistentTopicHandler;
        _gchInconsistentTopic = GCHandle.Alloc(onInconsistentTopic);

        _native = UnsafeNativeMethods.NewTopicListener(Marshal.GetFunctionPointerForDelegate(onInconsistentTopic));
    }

    /// <summary>
    /// Finalizes an instance of the <see cref="TopicListener"/> class.
    /// </summary>
    ~TopicListener()
    {
        Dispose(false);
    }
    #endregion

    #region Methods
    /// <summary>
    /// <para>Handles the <see cref="StatusKind.InconsistentTopicStatus" /> communication status.</para>
    /// <para>The <see cref="StatusKind.InconsistentTopicStatus" /> indicates that a <see cref="Topic" /> was attempted
    /// to be registered that already exists with different characteristics. Typically, the existing
    /// <see cref="Topic" /> may have a different type associated with it.</para>
    /// </summary>
    /// <param name="topic">The <see cref="Topic" /> that triggered the event.</param>
    /// <param name="status">The current <see cref="InconsistentTopicStatus" />.</param>
    public abstract void OnInconsistentTopic(Topic topic, InconsistentTopicStatus status);

    private void OnInconsistentTopicHandler(IntPtr topic, ref InconsistentTopicStatus status)
    {
        Entity entity = EntityManager.Instance.Find(topic);

        Topic t = null;
        if (entity != null)
        {
            t = entity as Topic;
        }

        OnInconsistentTopic(t, status);
    }

    internal IntPtr ToNative()
    {
        return _native;
    }
    #endregion

    #region IDisposable Members
    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="DataReaderListener" />.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing,
    /// releasing, or resetting unmanaged resources.
    /// </summary>
    /// <param name="disposing">True to free managed resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;

        ReleaseUnmanagedResources();
    }

    private void ReleaseUnmanagedResources()
    {
        UnsafeNativeMethods.DisposeTopicListener(_native);

        if (_gchInconsistentTopic.IsAllocated)
        {
            _gchInconsistentTopic.Free();
        }

        _native.ReleaseNativePointer();
    }
    #endregion
}

/// <summary>
/// This class suppresses stack walks for unmanaged code permission.
/// (System.Security.SuppressUnmanagedCodeSecurityAttribute is applied to this class.)
/// This class is for methods that are potentially dangerous. Any caller of these methods must perform a full
/// security review to make sure that the usage
/// is secure because no stack walk will be performed.
/// </summary>
[SuppressUnmanagedCodeSecurity]
internal static partial class UnsafeNativeMethods
{
#if NET7_0_OR_GREATER
    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TopicListener_New")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr NewTopicListener(IntPtr onInconsistentTopic);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TopicListener_Dispose")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr DisposeTopicListener(IntPtr native);
#else
    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "TopicListener_New", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr NewTopicListener(IntPtr onInconsistentTopic);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "TopicListener_Dispose", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr DisposeTopicListener(IntPtr native);
#endif
}