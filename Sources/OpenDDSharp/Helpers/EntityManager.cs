/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
using System;
using System.Collections.Concurrent;
using OpenDDSharp.DDS;

namespace OpenDDSharp.Helpers;

internal sealed class EntityManager
{
    #region Fields
    private static readonly object _lock = new object();
    private static EntityManager _instance;
    private readonly ConcurrentDictionary<IntPtr, Entity> _insts = new ConcurrentDictionary<IntPtr, Entity>();
    #endregion

    #region Singleton
    public static EntityManager Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new EntityManager();
                }

                return _instance;
            }
        }
    }
    #endregion

    #region Methods
    public void Add(IntPtr ptr, Entity inst)
    {
        _insts.AddOrUpdate(ptr, inst, (p, t) => inst);
    }

    public void Remove(IntPtr ptr)
    {
        _insts.TryRemove(ptr, out _);
    }

    public Entity Find(IntPtr ptr)
    {
        if (_insts.TryGetValue(ptr, out var found))
        {
            return found;
        }

        return null;
    }
    #endregion
}