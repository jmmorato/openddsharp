@echo off

REM Prepare ext folder
TITLE Prepare ext folder
REM setlocal enabledelayedexpansion
IF EXIST "..\ext" (
    rmdir ..\ext /s /q    
)

REM Unset enviroment Variables
TITLE Unset enviroment variables
set MPC_ROOT=
set ACE_ROOT=
set TAO_ROOT=
set DDS_ROOT=

REM If not already set in your build environment, set VSINSTALLDIR to your Visual Studio installation path.
REM TITLE Set VSINSTALLDIR
REM set VSINSTALLDIR=<SET TO YOUR VS INSTALLATION PATH>

REM Checkout OpenDDS to the version 3.14
TITLE Checkout OpenDDS to the version 3.14
mkdir ..\ext
cd ..\ext
git clone git://github.com/objectcomputing/OpenDDS.git
cd OpenDDS
git fetch && git fetch --tags
git checkout tags/DDS-3.14

REM Apply the needed OpenDDS patches
REM TITLE Apply the needed OpenDDS patches
REM git apply %CD%\..\..\Patches\ConditionImpl.h.patch
REM git apply %CD%\..\..\Patches\DataReaderImpl.cpp.patch
REM git apply %CD%\..\..\Patches\DataReaderImpl.h.patch
    
REM Download ACE/TAO and unzip
REM TITLE Download ACE/TAO and unzip
REM PowerShell -Command "& { Invoke-WebRequest -Uri 'http://download.ociweb.com/TAO-2.2a/ACE+TAO-2.2a_with_latest_patches_NO_makefiles.zip' -OutFile ACE+TAO-2.2a_with_latest_patches_NO_makefiles.zip }"
REM 7z x ACE+TAO-2.2a_with_latest_patches_NO_makefiles.zip

REM Open Visual Studio command prompt
TITLE Open Visual Studio command prompt amd64
call "%VSINSTALLDIR%\VC\Auxiliary\Build\vcvarsall.bat" amd64

REM Call OpenDDS configure script
TITLE OpenDDS configure script
call configure -v --ace-github-latest --no-tests
    
REM Create a copy of the original bin/lib folders
TITLE Create a copy of the original bin/lib folders
xcopy .\bin\* .\original_bin /s /i /Y
xcopy .\lib\* .\original_lib /s /i /Y
xcopy .\ACE_TAO\ACE\bin\* .\ACE_TAO\ACE\original_bin /s /i /Y
xcopy .\ACE_TAO\ACE\lib\* .\ACE_TAO\ACE\original_lib /s /i /Y

cd ..\..\Build

REM Prepare environment variables
TITLE Prepare environment variables
cd ..\ext\OpenDDS
set MPC_ROOT=%CD%\ACE_TAO\ACE\MPC
set ACE_ROOT=%CD%\ACE_TAO\ACE
set TAO_ROOT=%CD%\ACE_TAO\TAO
set DDS_ROOT=%CD%
set PATH=%ACE_ROOT%\bin;%DDS_ROOT%\bin;%ACE_ROOT%\lib;%DDS_ROOT%\lib;%PATH%

REM Restore the original bin/lib folders
TITLE Restore the original bin/lib folders
rmdir %DDS_ROOT%\bin /s /q
rmdir %DDS_ROOT%\lib /s /q
rmdir %ACE_ROOT%\bin /s /q
rmdir %ACE_ROOT%\lib /s /q
xcopy %DDS_ROOT%\original_bin\* %DDS_ROOT%\bin /s /i /Y
xcopy %DDS_ROOT%\original_lib\* %DDS_ROOT%\lib /s /i /Y
xcopy %ACE_ROOT%\original_bin\* %ACE_ROOT%\bin /s /i /Y
xcopy %ACE_ROOT%\original_lib\* %ACE_ROOT%\lib /s /i /Y

REM Build OpenDDS for x64 platforms
TITLE DEBUG x64: Build OpenDDS
msbuild DDS_TAOv2.sln /m /p:Configuration=Debug;Platform=x64
TITLE RELEASE x64: Build OpenDDS
msbuild DDS_TAOv2.sln /m /p:Configuration=Release;Platform=x64

REM Create bin/lib_x64 folders
TITLE Create bin/lib_x64 folders
xcopy %CD%\ACE_TAO\ACE\lib\* %CD%\ACE_TAO\ACE\lib_x64 /s /i /Y
xcopy %CD%\ACE_TAO\ACE\bin\* %CD%\ACE_TAO\ACE\bin_x64 /s /i /Y
xcopy %CD%\lib\* %CD%\lib_x64 /s /i /Y
xcopy %CD%\bin\* %CD%\bin_x64 /s /i /Y

REM Restore the original bin/lib folders
TITLE Restore the original bin/lib folders
rmdir %DDS_ROOT%\bin /s /q
rmdir %DDS_ROOT%\lib /s /q
rmdir %ACE_ROOT%\bin /s /q
rmdir %ACE_ROOT%\lib /s /q
xcopy %DDS_ROOT%\original_bin\* %DDS_ROOT%\bin /s /i /Y
xcopy %DDS_ROOT%\original_lib\* %DDS_ROOT%\lib /s /i /Y
xcopy %ACE_ROOT%\original_bin\* %ACE_ROOT%\bin /s /i /Y
xcopy %ACE_ROOT%\original_lib\* %ACE_ROOT%\lib /s /i /Y

REM Build OpenDDS for Win32 platforms
TITLE DEBUG Win32: Build OpenDDS
msbuild DDS_TAOv2.sln /m /p:Configuration=Debug;Platform=Win32
TITLE RELEASE WIN32: Build OpenDDS
msbuild DDS_TAOv2.sln /m /p:Configuration=Release;Platform=Win32

REM Create bin/lib_x86 folders
TITLE Create bin/lib_x86 folders
xcopy %CD%\ACE_TAO\ACE\lib\* %CD%\ACE_TAO\ACE\lib_x86 /s /i /Y
xcopy %CD%\ACE_TAO\ACE\bin\* %CD%\ACE_TAO\ACE\bin_x86 /s /i /Y
xcopy %CD%\lib\* %CD%\lib_x86 /s /i /Y
xcopy %CD%\bin\* %CD%\bin_x86 /s /i /Y

REM Change folder to OpenDDSharp
cd ..\..\OpenDDSharp

REM Copy the OpenDDSharp code generator templates
xcopy %CD%\OpenDDSharp.IdlGenerator\HeaderTemplate.txt %DDS_ROOT%\dds\idl /Y
xcopy %CD%\OpenDDSharp.IdlGenerator\ImplTemplate.txt %DDS_ROOT%\dds\idl /Y

REM Restore nuget packages for the solution
TITLE Restore nuget packages for the solution
nuget restore OpenDDSharp.sln

REM Prepare bin and lib folders for x64 compilation
rmdir %DDS_ROOT%\bin /s /q
rmdir %DDS_ROOT%\lib /s /q
rmdir %ACE_ROOT%\bin /s /q
rmdir %ACE_ROOT%\lib /s /q
xcopy %DDS_ROOT%\bin_x64\* %DDS_ROOT%\bin /s /i /Y
xcopy %DDS_ROOT%\lib_x64\* %DDS_ROOT%\lib /s /i /Y
xcopy %ACE_ROOT%\bin_x64\* %ACE_ROOT%\bin /s /i /Y
xcopy %ACE_ROOT%\lib_x64\* %ACE_ROOT%\lib /s /i /Y

REM Build the ExportFileGenerator and copy it to the ACE_ROOT\bin folder
TITLE Build the ExportFileGenerator and copy it to the ACE_ROOT\bin folder
msbuild OpenDDSharp.sln /t:OpenDDSharp_ExportFileGenerator /p:Configuration="Release" /p:Platform="Any CPU" /p:BuildProjectReferences=false
xcopy %CD%\OpenDDSharp.ExportFileGenerator\bin\Release\* %ACE_ROOT%\bin /Y

REM Build OpenDDSharp for x64 platforms
TITLE DEBUG x64: Build OpenDDSharp
msbuild OpenDDSharp.sln /p:Configuration=Debug;Platform=x64
TITLE RELEASE x64: Build OpenDDSharp
msbuild OpenDDSharp.sln /p:Configuration=Release;Platform=x64

REM Prepare bin and lib folders for x86 compilation
rmdir %DDS_ROOT%\bin /s /q
rmdir %DDS_ROOT%\lib /s /q
rmdir %ACE_ROOT%\bin /s /q
rmdir %ACE_ROOT%\lib /s /q
xcopy %DDS_ROOT%\bin_x86\* %DDS_ROOT%\bin /s /i /Y
xcopy %DDS_ROOT%\lib_x86\* %DDS_ROOT%\lib /s /i /Y
xcopy %ACE_ROOT%\bin_x86\* %ACE_ROOT%\bin /s /i /Y
xcopy %ACE_ROOT%\lib_x86\* %ACE_ROOT%\lib /s /i /Y

REM Build the ExportFileGenerator again and copy it to the ACE_ROOT\bin folder
TITLE Build the ExportFileGenerator again and copy it to the ACE_ROOT\bin folder
msbuild OpenDDSharp.sln /t:OpenDDSharp_ExportFileGenerator /p:Configuration="Release" /p:Platform="Any CPU" /p:BuildProjectReferences=false
xcopy %CD%\OpenDDSharp.ExportFileGenerator\bin\Release\* %ACE_ROOT%\bin\ /Y

REM Build OpenDDSharp for x86 platforms
TITLE DEBUG x86: Build OpenDDSharp
msbuild OpenDDSharp.sln /p:Configuration=Debug;Platform=x86
TITLE RELEASE x86: Build OpenDDSharp
msbuild OpenDDSharp.sln /p:Configuration=Release;Platform=x86

REM Come back to build folder
cd ..\Build

REM Create NuGet packages
TITLE Create NuGet packages
nuget pack "%CD%\..\OpenDDSharp\OpenDDSharp\OpenDDSharp.nuspec"
nuget pack "%CD%\..\OpenDDSharp\OpenDDSharp.IdlGenerator\OpenDDSharp.IdlGenerator.nuspec"
