/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
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
#pragma once

#include "ace/Init_ACE.h"

namespace OpenDDSharp {

	///<summary>
	/// The ACE library static class used for initialization and finalization
	///</summary>
	public ref class Ace {

	public:
		/// <summary>
		/// This method initializes the ACE library services and initializes
		/// ACE's internal resources. Applications should not instantiate
		/// ACE classes or call methods on objects of these classes until a
		/// ACE.Init() returns successfully.
		/// </summary>
		/// <returns>
		/// Returns 0 on success, -1 on failure, and 1 if it had already been called.
		/// </returns>
		static int Init();

		/// <summary>
		/// Finalize the ACE library services and releases ACE's internal
		/// resources. In general, do not instantiate ACE classes or call
		/// methods on objects of these classes after a Ace.Fini() has been
		/// called.
		/// </summary>
		/// <returns>
		/// Returns 0 on success, -1 on failure, and 1 if it had already been called.
		/// </returns>
		static int Fini();
	};

}