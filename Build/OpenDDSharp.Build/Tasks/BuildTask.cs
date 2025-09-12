/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
using Cake.Frosting;

namespace OpenDDSharp.Build.Tasks
{
    /// <summary>
    /// Build task.
    /// </summary>
    [TaskName("Build")]
    [IsDependentOn(typeof(BuildOpenDDSharpTask))]
    public class BuildTask : FrostingTask
    {
    }
}
