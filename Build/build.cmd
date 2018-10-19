@echo off

REM Delete ext folder if exist
IF EXIST "..\ext" (
 rmdir ..\ext /s /q
)

REM Checkout OpenDDS to the version 3.13
mkdir ..\ext
cd ..\ext
git clone git://github.com/objectcomputing/OpenDDS.git
cd OpenDDS
git fetch && git fetch --tags
git checkout tags/DDS-3.13

REM Open Visual Studio command prompt
call "%VSINSTALLDIR%\VC\Auxiliary\Build\vcvarsall.bat" amd64

REM Download ACE/TAO and unzip
PowerShell -ExecutionPolicy Bypass -Command "& { Invoke-WebRequest -Uri 'http://download.ociweb.com/TAO-2.2a/ACE+TAO-2.2a_with_latest_patches_NO_makefiles.zip' -OutFile ACE+TAO-2.2a_with_latest_patches_NO_makefiles.zip }"
7z x ACE+TAO-2.2a_with_latest_patches_NO_makefiles.zip

REM Prepare environment variables
set MPC_ROOT=%CD%\ACE_wrappers\MPC
set ACE_ROOT=%CD%\ACE_wrappers
set TAO_ROOT=%CD%\ACE_wrappers\TAO
set DDS_ROOT=%CD%
set PATH=%PATH%;%ACE_ROOT%\bin;%DDS_ROOT%\bin;%ACE_ROOT%\lib;%DDS_ROOT%\lib

REM Call OpenDDS configure script
call configure --no-tests
xcopy %CD%\..\..\Build\ACE.vcxproj %ACE_ROOT%\ace /y

REM Apply the needed OpenDDS patches
git apply %CD%\..\..\Patches\ConditionImpl.h.patch
git apply %CD%\..\..\Patches\DataReaderImpl.cpp.patch
git apply %CD%\..\..\Patches\DataReaderImpl.h.patch
git apply %CD%\..\..\Patches\MultiTopicDataReaderBase.cpp.patch
git apply %CD%\..\..\Patches\unique_ptr.h.patch
git apply %CD%\..\..\Patches\SubscriberImpl.cpp.patch
git apply %CD%\..\..\Patches\DataWriterImpl.cpp.patch

REM Build OpenDDS for x64 platforms and create the bin_x64 and lib_x64 folders
msbuild DDS_TAOv2.sln /m /p:Configuration=Debug;Platform=x64
msbuild DDS_TAOv2.sln /m /p:Configuration=Release;Platform=x64
xcopy %CD%\ACE_wrappers\lib\* %CD%\ACE_wrappers\lib_x64 /s /i
xcopy %CD%\ACE_wrappers\bin\* %CD%\ACE_wrappers\bin_x64 /s /i
xcopy %CD%\lib\* %CD%\lib_x64 /s /i
xcopy %CD%\bin\* %CD%\bin_x64 /s /i

REM Build OpenDDS for Win32 platforms and create the bin_x86 and lib_x86 folders
msbuild DDS_TAOv2.sln /m /p:Configuration=Debug;Platform=Win32
msbuild DDS_TAOv2.sln /m /p:Configuration=Release;Platform=Win32
xcopy %CD%\ACE_wrappers\lib\* %CD%\ACE_wrappers\lib_x86 /s /i
xcopy %CD%\ACE_wrappers\bin\* %CD%\ACE_wrappers\bin_x86 /s /i
xcopy %CD%\lib\* %CD%\lib_x86 /s /i
xcopy %CD%\bin\* %CD%\bin_x86 /s /i

REM Change folder to OpenDDSharp
cd ..\..\OpenDDSharp

REM Copy the OpenDDSharp code generator templates
xcopy %CD%\OpenDDSharp.IdlGenerator\HeaderTemplate.txt %DDS_ROOT%\dds\idl
xcopy %CD%\OpenDDSharp.IdlGenerator\ImplTemplate.txt %DDS_ROOT%\dds\idl

REM Restore nuget packages for the solution
nuget restore OpenDDSharp.sln

REM Build the ExportFileGenerator and copy it to the ACE_ROOT\bin folder
msbuild OpenDDSharp.sln /t:OpenDDSharp_ExportFileGenerator /p:Configuration="Release" /p:Platform="Any CPU" /p:BuildProjectReferences=false
xcopy %CD%\OpenDDSharp.ExportFileGenerator\bin\Release\* %ACE_ROOT%\bin

REM Build OpenDDSharp for x64 platforms
msbuild OpenDDSharp.sln /p:Configuration=Debug;Platform=x64
msbuild OpenDDSharp.sln /p:Configuration=Release;Platform=x64

REM Prepare bin and lib folders for x86 compilation
rmdir %DDS_ROOT%\bin /s /q
rmdir %DDS_ROOT%\lib /s /q
rmdir %ACE_ROOT%\bin /s /q
rmdir %ACE_ROOT%\lib /s /q
xcopy %DDS_ROOT%\bin_x86\* %DDS_ROOT%\bin /s /i
xcopy %DDS_ROOT%\lib_x86\* %DDS_ROOT%\lib /s /i
xcopy %ACE_ROOT%\bin_x86\* %ACE_ROOT%\bin /s /i
xcopy %ACE_ROOT%\lib_x86\* %ACE_ROOT%\lib /s /i

REM Build the ExportFileGenerator again and copy it to the ACE_ROOT\bin folder
msbuild OpenDDSharp.sln /t:OpenDDSharp_ExportFileGenerator /p:Configuration="Release" /p:Platform="Any CPU" /p:BuildProjectReferences=false
xcopy %CD%\OpenDDSharp.ExportFileGenerator\bin\Release\* %ACE_ROOT%\bin

REM Build OpenDDSharp for x86 platforms
msbuild OpenDDSharp.sln /p:Configuration=Debug;Platform=x86
msbuild OpenDDSharp.sln /p:Configuration=Release;Platform=x86

REM Come back to build folder
cd ..\Build

REM Create NuGet packages
nuget pack "%CD%\..\OpenDDSharp\OpenDDSharp\OpenDDSharp.nuspec"
nuget pack "%CD%\..\OpenDDSharp\OpenDDSharp.IdlGenerator\OpenDDSharp.IdlGenerator.nuspec"
