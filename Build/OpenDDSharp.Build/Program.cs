/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS.
Copyright (C) 2018 - 2021 Jose Morato

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
using Cake.Frosting;

namespace OpenDDSharp.Build
{
    /// <summary>
    /// Represents the program to run.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Main entry point.
        /// </summary>
        /// <param name="args">The program given arguments.</param>
        /// <returns>The integer result of the program.</returns>
        public static int Main(string[] args)
        {
            return new CakeHost()
                .UseContext<BuildContext>()
                .Run(args);
        }
    }
}
