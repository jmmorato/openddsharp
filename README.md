# OpenDDSharp
OpenDDS wrapper for .NET languages.

OpenDDS is an open source implementation of the Object Management Group
(OMG) Data Distribution Service (DDS), developed and copyrighted by
Object Computing Incorporated (OCI). The OMG DDS specification is intended
to be suitable for systems whose requirements include real-time, high
volume, robustness, failure tolerant data distribution utilizing a
publish and subscribe model.

OpenDDS Website: [http://www.opendds.org](http://www.opendds.org)  
OpenDDS Repository: [https://github.com/objectcomputing/OpenDDS](https://github.com/objectcomputing/OpenDDS)

OpenDDSharp has been compiled with OpenDDS v3.16

| Package | NuGet |
|---------|-------|
| [OpenDDSharp][OpenDDSharpNuget] | [![OpenDDSharpShield]][OpenDDSharpNuget] |
| [OpenDDSharp.IdlGenerator][OpenDDSharpIdlGeneratorNuget] | [![OpenDDSharpIdlGeneratorShield]][OpenDDSharpIdlGeneratorNuget] |

[OpenDDSharpNuget]: https://www.nuget.org/packages/OpenDDSharp/
[OpenDDSharpShield]: https://img.shields.io/nuget/v/OpenDDSharp.svg
[OpenDDSharpIdlGeneratorNuget]: https://www.nuget.org/packages/OpenDDSharp.IdlGenerator/
[OpenDDSharpIdlGeneratorShield]: https://img.shields.io/nuget/v/OpenDDSharp.IdlGenerator.svg

# How to Build

In order to ease the build process of OpenDDSharp, a Powershell script is provided in the repository `Build` folder. You must call it from the same directory where is placed, and ensure that the OpenDDS environment variables (DDS_ROOT, ACE_ROOT and TAO_ROOT) are not set in your system.

The script will download and compile OpenDDS in a new `ext` folder, then compiles OpenDDSharp and execute the unit tests for the project.

Some parameters can be provided to configure the compilation process:

**BuildPlatform**: Defines the platform to build OpenDDSharp. The accepted values are `x64` and `x86`. If this parameter is not provided, the default value is `x64`. 

**BuildConfiguration**: Defines the configuration to build OpenDDSharp. The accepted values are `Release` and `Debug`. If this parameter is not provided, the default value is `Release` 

**VisualStudioVersion**: Defines the Visual Studio version that will be used to compile OpenDDSharp. Obviously, the Visual Studio version must be installed in the system before call the script. The accepted values are `VS2017` and `VS2019`. If this parameter is not provided, the default value is `VS2019`.

**OpenDdsVersion**: Defines the OpenDDS version that will be downloaded and compiled. If this parameter is not provided, the default value is `3.16`. Changing the version of OpenDDS could require code adaptations on the OpenDDSharp layer.

**IgnoreThirdPartySetup**: You can ignore the OpenDDS compilation with this parameter if it was already compiled by a previous call to the script.

**PerlPath**: Defines the Perl path to be used during the OpenDDS compilation. Perl is used for the OpenDDS configure script to generate the Visual Studio project files. It is recommended to use [Straweberry Perl](https://strawberryperl.com/). By default the `C:/Strawberry/perl/bin` will be used to find the perl executable, but you can use this parameter to point the installation path where your Perl software is installed.

Example:

```ps
OpenDDSharp.Build.CppCli.ps1 --BuildConfiguration=x64 --BuildPlatform=Release --OpenDdsVersion=3.16 --IgnoreThirdPartySetup=False --VisualStudioVersion=VS2019
```