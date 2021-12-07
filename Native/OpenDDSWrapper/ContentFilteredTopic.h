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
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
#pragma once

#include "Utils.h"
#include "QosPolicies.h"
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
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
#pragma once

#include "Utils.h"

EXTERN_METHOD_EXPORT
::DDS::TopicDescription_ptr ContentFilteredTopic_NarrowTopicDescription(::DDS::ContentFilteredTopic_ptr t);

EXTERN_METHOD_EXPORT
char* ContentFilteredTopic_GetTypeName(::DDS::ContentFilteredTopic_ptr t);

EXTERN_METHOD_EXPORT
char* ContentFilteredTopic_GetName(::DDS::ContentFilteredTopic_ptr t);

EXTERN_METHOD_EXPORT
::DDS::DomainParticipant_ptr ContentFilteredTopic_GetParticipant(::DDS::ContentFilteredTopic_ptr t);

EXTERN_METHOD_EXPORT
char* ContentFilteredTopic_GetFilterExpression(::DDS::ContentFilteredTopic_ptr t);

EXTERN_METHOD_EXPORT
::DDS::Topic_ptr ContentFilteredTopic_GetRelatedTopic(::DDS::ContentFilteredTopic_ptr t);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t ContentFilteredTopic_GetExpressionParameters(::DDS::ContentFilteredTopic_ptr t, void*& seq);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t ContentFilteredTopic_SetExpressionParameters(::DDS::ContentFilteredTopic_ptr t, void* seq);
