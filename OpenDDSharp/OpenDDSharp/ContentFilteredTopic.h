/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

OpenDDSharp is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsTopicC.h>
#pragma managed

#include "TopicDescription.h"

#include <vcclr.h>
#include <msclr/marshal.h>

#using <System.Core.dll>
#using <System.Linq.dll>

using namespace System::Linq;
using namespace System::Collections::Generic;

namespace OpenDDSharp {
	namespace DDS {

		/// <summary>
		/// ContentFilteredTopic is an implementation of <see cref="ITopicDescription" /> that allows for content-based subscriptions.
		/// ContentFilteredTopic describes a more sophisticated subscription that indicates the subscriber does not want to necessarily see
		/// all values of each instance published under the <see cref="Topic" />. Rather, it wants to see only the values whose contents satisfy certain
		///	criteria. This class therefore can be used to request content-based subscriptions.
		/// </summary>
		/// <remarks>
		/// The selection of the content is done using the filter expression with the expression parameters.
		/// <list type="bullet">
		///		<item><description>The filter expression is a string that specifies the criteria to select the data samples of interest. It is similar to the WHERE part of an SQL clause.</description></item>
		///		<item><description>The expression parameters are a collection of strings that give values to the 'parameters' ("%n" tokens) in the filter expression. The number of supplied parameters must fit with the requested values in the filter expression</description></item>
		///	</list>
		/// </remarks>
		public ref class ContentFilteredTopic : public OpenDDSharp::DDS::TopicDescription {

		internal:
			::DDS::ContentFilteredTopic_ptr impl_entity;

		public:
			/// <summary>
			/// Gets the filter expression associated with the <see cref="ContentFilteredTopic" />. That is, the expression specified when the
			/// <see cref="ContentFilteredTopic" /> was created.
			/// </summary>
			property System::String^ FilterExpression {
				System::String^ get();
			}

			/// <summary>
			/// Gets the <see cref="Topic" /> associated with the <see cref="ContentFilteredTopic" />. That is, the <see cref="Topic" /> specified when the
			/// <see cref="ContentFilteredTopic" /> was created.
			/// </summary>
			property OpenDDSharp::DDS::Topic^ RelatedTopic {
				OpenDDSharp::DDS::Topic^ get();
			}

		internal:
			ContentFilteredTopic(::DDS::ContentFilteredTopic_ptr native);

		public:
			/// <summary>
			/// Gets the expression parameters associated with the <see cref="ContentFilteredTopic" />. That is, the parameters specified
			/// on the last successful call to <see cref="ContentFilteredTopic::SetExpressionParameters" />, or if it was never called, the parameters
			///	specified when the <see cref="ContentFilteredTopic" /> was created.
			/// </summary>
			/// <param name="params">The expression parameters list to be filled up.</param>
			/// <returns>The <see cref="OpenDDSharp::DDS::ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode GetExpressionParameters(IList<System::String^>^ params);

			/// <summary>
			/// Changes the expression parameters associated with the <see cref="ContentFilteredTopic" />.
			/// </summary>
			/// <param name="params">The expression parameters values to be set.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode SetExpressionParameters(... array<System::String^>^ params);

		private:
			System::String^ GetFilterExpression();
			OpenDDSharp::DDS::Topic^ GetRelatedTopic();
		};
	};
};
