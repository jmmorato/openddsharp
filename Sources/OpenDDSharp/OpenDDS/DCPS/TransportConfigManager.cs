/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
using System;
using System.Collections.Concurrent;

namespace OpenDDSharp.OpenDDS.DCPS;

internal class TransportConfigManager
{
    #region Fields
    private static readonly object _lock = new ();
    private static TransportConfigManager _instance;
    private readonly ConcurrentDictionary<IntPtr, TransportConfig> _configs = new ();
    #endregion

    #region Singleton
    public static TransportConfigManager Instance
    {
        get
        {
            lock (_lock)
            {
                return _instance ??= new TransportConfigManager();
            }
        }
    }
    #endregion

    #region Methods
    public void Add(IntPtr ptr, TransportConfig config)
    {
        _configs.AddOrUpdate(ptr, config, (_, _) => config);
    }

    public void Remove(IntPtr ptr)
    {
        _configs.TryRemove(ptr, out _);
    }

    public TransportConfig Find(IntPtr ptr)
    {
        return _configs.TryGetValue(ptr, out var found) ? found : null;
    }

    public void Clear()
    {
        _configs.Clear();
    }
    #endregion
}