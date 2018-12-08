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
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/

namespace OpenDDSharp
{
    internal class Constants
    {
#if DEBUG
        internal const string API_DLL_X64 = @"OpenDDSWrapperd.x64.dll";
        internal const string API_DLL_X86 = @"OpenDDSWrapperd.x86.dll";
#else
        internal const string API_DLL_X64 = @"OpenDDSWrapper.x64.dll";
        internal const string API_DLL_X86 = @"OpenDDSWrapper.x86.dll";
#endif
    }
}
