[![Continuous Integration](https://github.com/jmmorato/openddsharp/actions/workflows/ci_standard.yaml/badge.svg?branch=develop)](https://github.com/jmmorato/openddsharp/actions/workflows/ci_standard.yaml)
[![Coverage Status](https://coveralls.io/repos/github/jmmorato/openddsharp/badge.svg?branch=develop)](https://coveralls.io/github/jmmorato/openddsharp?branch=develop)
![GitHub Sponsor](https://img.shields.io/github/sponsors/jmmorato?label=Sponsor&logo=GitHub)

# OpenDDSharp

OpenDDS wrapper for .NET languages.

OpenDDS is an open source implementation of the Object Management Group
(OMG) Data Distribution Service (DDS), developed and copyrighted by
Object Computing Incorporated (OCI). The OMG DDS specification is intended
to be suitable for systems whose requirements include real-time, high
volume, robustness, failure tolerant data distribution utilizing a
publish and subscribe model.

OpenDDS Website: [https://www.opendds.org](https://www.opendds.org)  
OpenDDS Repository: [https://github.com/OpenDDS/OpenDDS](https://github.com/OpenDDS/OpenDDS)  
OpenDDS Documentation: [https://opendds.readthedocs.io](https://opendds.readthedocs.io/en/latest/)  

OpenDDSharp has been compiled with OpenDDS v3.31.0

| Package                                                  | NuGet                                                            |
|----------------------------------------------------------|------------------------------------------------------------------|
| [OpenDDSharp][OpenDDSharpNuget]                          | [![OpenDDSharpShield]][OpenDDSharpNuget]                         |
| [OpenDDSharp.IdlGenerator][OpenDDSharpIdlGeneratorNuget] | [![OpenDDSharpIdlGeneratorShield]][OpenDDSharpIdlGeneratorNuget] |
| [OpenDDSharp.Native][OpenDDSharpNativeNuget]             | [![OpenDDSharpNativeShield]][OpenDDSharpNativeNuget]             |
| [OpenDDSharp.Templates][OpenDDSharpTemplatesNuget]       | [![OpenDDSharpTemplatesShield]][OpenDDSharpTemplatesNuget]       |

[OpenDDSharpNuget]: https://www.nuget.org/packages/OpenDDSharp/
[OpenDDSharpShield]: https://img.shields.io/nuget/v/OpenDDSharp.svg
[OpenDDSharpIdlGeneratorNuget]: https://www.nuget.org/packages/OpenDDSharp.IdlGenerator/
[OpenDDSharpIdlGeneratorShield]: https://img.shields.io/nuget/v/OpenDDSharp.IdlGenerator.svg
[OpenDDSharpNativeNuget]: https://www.nuget.org/packages/OpenDDSharp.Native/
[OpenDDSharpNativeShield]: https://img.shields.io/nuget/v/OpenDDSharp.Native.svg
[OpenDDSharpTemplatesNuget]: https://www.nuget.org/packages/OpenDDSharp.Templates/
[OpenDDSharpTemplatesShield]: https://img.shields.io/nuget/v/OpenDDSharp.Templates.svg

# Getting Started

The quickest way to start using OpenDDSharp is to follow the [OpenDDSharp Getting Started](https://www.openddsharp.com/articles/getting_started.html) article.
It will give you the first hints on how to work with the API and how to organize your dotnet projects.

A detailed [API Documentation](https://www.openddsharp.com/api/index.html) is available online.
Navigate through the namespaces to get information about the implemented OpenDDSharp APIs.

As OpenDDSharp is a wrapper of [OpenDDS](https://opendds.org), the [OpenDDS Developer's Guide](https://opendds.readthedocs.io/en/latest/)
is also a valuable resource that you can use but keep in mind that not everything is implemented yet. 

# Roadmap Overview
OpenDDSharp started as a proof of concept during a distributed system technology selection based on the following
OpenDDS articles:

* [Using TAO and OpenDDS with .NET](https://objectcomputing.com/resources/publications/mnb/2009/01/15/using-tao-and-opendds-net-part-i)
* [Code Generation with OpenDDS](https://objectcomputing.com/resources/publications/mnb/2010/06/02/code-generation-opendds-part-i)

The first versions of OpenDDSharp were using a similar C++/CLI wrapper than the articles explain but that solution
ties the API to .NET Framework and Windows operating systems.

In order to avoid the restrictions, the wrapper has been re-implemented using PInvoke and compiled for NET Standard 2.0.

The Data-Centric Publish-Subscribe (DCPS) model described in the main [DDS Specification v1.4](https://www.omg.org/spec/DDS/1.4)
is already implemented and ready to be used.

The IDL code generator is based on the [IDL4 to C# Language Mapping Specification](https://www.omg.org/spec/IDL4-CSHARP).
Visit https://www.openddsharp.com/articles/idl.html to check the current status of the implementation.

As OpenDDSharp is based on native OpenDDS libraries, it needs a different compilation for each Framework/Runtime that
support.

The following table shows the Target Frameworks that are already implemented and tested ( :white_check_mark: ) and
the ones that are planned for next versions ( :x: ):

| Target Framework  | Status             |
|-------------------|--------------------|
| net462 (or above) | :white_check_mark: |
| net6.0            | :white_check_mark: |
| net6.0-android    | :x:                |
| net6.0-ios        | :x:                |
| net7.0            | :white_check_mark: |
| net7.0-android    | :x:                |
| net7.0-ios        | :x:                |
| net8.0            | :white_check_mark: |
| net8.0-android    | :x:                |
| net8.0-ios        | :x:                |

The following table shows the Runtimes Identifiers that are already implemented and tested ( :white_check_mark: ) and
the ones that are planned for next versions ( :x: ):

| Runtime ID  | Status             |
|-------------|--------------------|
| win-x86     | :white_check_mark: |
| win-x64     | :white_check_mark: |
| win-arm64   | :x:                |
| linux-x64   | :white_check_mark: |
| linux-arm64 | :white_check_mark: |
| osx-x64     | :white_check_mark: |
| osx-arm64   | :white_check_mark: |

Once the core API is stable and working for all the planned Frameworks/Runtimes,
the development will be focus to provide other advanced DDS features

* [DDS Security](https://www.omg.org/spec/DDS-SECURITY)
* [Extensible and Dynamic Topic Types for DDS](https://www.omg.org/spec/DDS-XTypes)
* [RPC Over DDS](https://www.omg.org/spec/DDS-RPC)
* QoS XML Configuration
* ...

# How to Collaborate
OpenDDSharp is developed and maintained as a hobby during my free time but as (almost) all human been I have kids,
wife, family, friends or other hobbies that I like to enjoy with.

You should understand that I'm not 100% dedicated to the project and the only way to make it grow is collaborating
together.

## Use it, test it and report it
Just using OpenDDSharp you are already collaborating with the project.

Share your thoughts with the community by creating new [Discussions](https://github.com/jmmorato/openddsharp/discussions) and
report bugs or improvement requests on the [Issues](https://github.com/jmmorato/openddsharp/issues) section.

In addition, the online documentation contains one advertisement per page. I'm sorry for that but...
please consider to whitelist www.openddsharp.com in your favorite adsblock software if you are using one.

## Implement it yourself
As in almost all open sources projects, pull request are welcome.
If you find bugs or your project requires a not implemented feature, feel free to code it yourself and create a
pull request.

Try to structure your code nicely and follow the OpenDDSharp coding guidelines (OpenDDSharp.ruleset).
All pull requests will be reviewed and merged when approved. 

## Sponsor it
If you are using OpenDDSharp successfully in your projects or just want an open source alternative for your DDS
systems in C#, you should consider to sponsor it to ensure the future development of the project.

Check the monthly and one-time tiers in the GitHub [Sponsor](https://github.com/sponsors/jmmorato) section for more information.
