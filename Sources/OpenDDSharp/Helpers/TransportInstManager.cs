/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
using System;
using System.Collections.Concurrent;
using OpenDDSharp.OpenDDS.DCPS;

namespace OpenDDSharp.Helpers;

internal sealed class TransportInstManager
{
    #region Fields
    private static readonly object _lock = new object();
    private static TransportInstManager _instance;
    private readonly ConcurrentDictionary<IntPtr, TransportInst> _insts = new ();
    #endregion

    #region Singleton
    public static TransportInstManager Instance
    {
        get
        {
            lock (_lock)
            {
                return _instance ??= new TransportInstManager();
            }
        }
    }
    #endregion

    #region Methods
    public void Add(IntPtr ptr, TransportInst inst)
    {
        _insts.AddOrUpdate(ptr, inst, (_, _) => inst);
    }

    public void Remove(IntPtr ptr)
    {
        _insts.TryRemove(ptr, out _);
    }

    public TransportInst Find(IntPtr ptr)
    {
        return _insts.TryGetValue(ptr, out var found) ? found : null;
    }
    #endregion
}