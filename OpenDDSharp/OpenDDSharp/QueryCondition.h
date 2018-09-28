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
#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsSubscriptionC.h>
#pragma managed

#include "DataReader.h"
#include "ReadCondition.h"
#include "SampleStateMask.h"
#include "ViewStateMask.h"
#include "InstanceStateMask.h"
#include "ReturnCode.h"

#include <vcclr.h>
#include <msclr/marshal.h>

namespace OpenDDSharp {
	namespace DDS {

		/// <summary>
		/// QueryCondition objects are specialized <see cref="ReadCondition" /> objects that allow the application to also specify a filter on the locally available data.
		/// </summary>
		/// <remarks>
		/// The query is similar to an SQL WHERE clause and can be parameterized by arguments that are dynamically changeable by the <see cref="SetQueryParameters" /> operation.		
		/// </remarks>
		public ref class QueryCondition : public ReadCondition {

		internal:
			::DDS::QueryCondition_ptr impl_entity;

        public:
            !QueryCondition();

		public:
			/// <summary>
			/// Gets the queryexpression associated with the <see cref="QueryCondition" />. 
			/// That is, the expression specified when the <see cref="QueryCondition" /> was created.
			/// </summary>
			property System::String^ QueryExpression {
				System::String^ get();
			}

		internal:
			QueryCondition(::DDS::QueryCondition_ptr query_condition, OpenDDSharp::DDS::DataReader^ reader);

		public:
			/// <summary>
			/// Gets the query parameters associated with the <see cref="QueryCondition" />. That is, the parameters specified on the last
			///	successful call to <see cref="SetQueryParameters" />, or if <see cref="SetQueryParameters" /> was never called, the arguments specified when the
			///	<see cref="QueryCondition" /> was created.
			/// </summary>
			/// <param name="queryParameters">The query parameters list to be filled up.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode GetQueryParameters(IList<System::String^>^ queryParameters);

			/// <summary>
			/// Changes the query parameters associated with the <see cref="QueryCondition" />.
			/// </summary>
			/// <param name="queryParameters">The query parameters values to be set.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode SetQueryParameters(... array<System::String^>^ queryParameters);

		private:
			System::String^ GetQueryExpression();

		};
	};
};
