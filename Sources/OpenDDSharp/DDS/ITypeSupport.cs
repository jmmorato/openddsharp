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

namespace OpenDDSharp.DDS;

/// <summary>
/// Base interface that provides support operations for IDL-generated types.
/// </summary>
public interface ITypeSupport
{
    /// <summary>
    /// Gets the default name for the data-type represented by the TypeSupport.
    /// </summary>
    /// <returns>The type default name.</returns>
    string GetTypeName();

    /// <summary>
    /// Register a data-type.
    /// This operation allows an application to communicate to the Service the existence of a data type.
    /// </summary>
    /// <remarks>
    /// It is a pre-condition error to use the same typeName to register two different TypeSupport with the same
    /// DomainParticipant. If an application attempts this, the operation will fail and return PRECONDITION_NOT_MET.
    /// However, it is allowed to register the same TypeSupport multiple times with a DomainParticipant using the same
    /// or different values for the type_name. If register_type is called multiple times on the same TypeSupport with
    /// the same DomainParticipant and type_name the second (and subsequent) registrations are ignored but the operation
    /// returns OK.
    /// </remarks>
    /// <param name="dp">The <see cref="DomainParticipant"/> which the type is registered.</param>
    /// <param name="typeName">The type name used for the registration</param>
    /// <returns>The <see cref="ReturnCode"/> of the register operation.</returns>
    ReturnCode RegisterType(DomainParticipant dp, string typeName);

    /// <summary>
    /// Unregister a data-type.
    /// </summary>
    /// <param name="dp">The <see cref="DomainParticipant"/> which the type is un-registered.</param>
    /// <param name="typeName">The type name used for the registration.</param>
    /// <returns>The <see cref="ReturnCode"/> of the unregister operation.</returns>
    ReturnCode UnregisterType(DomainParticipant dp, string typeName);
}

/// <summary>
/// Specialized type interface that provides support operations for IDL-generated types.
/// </summary>
/// <typeparam name="T">The IDL type.</typeparam>
public interface ITypeSupport<T> : ITypeSupport
{
    /// <summary>
    /// Encodes a sample into a string using JSON format.
    /// </summary>
    /// <param name="sample">The sample to be encoded.</param>
    /// <returns>The JSON sample representation.</returns>
    string EncodeToString(T sample);

    /// <summary>
    /// Decodes a JSON string into a sample.
    /// </summary>
    /// <param name="encoded">The sample JSON representation.</param>
    /// <returns>The decoded sample.</returns>
    T DecodeFromString(string encoded);

    /// <summary>
    /// Encodes a sample into a byte array using CDR format.
    /// </summary>
    /// <param name="sample">The sample to be encoded.</param>
    /// <returns>The CDR sample representation.</returns>
    byte[] EncodeToBytes(T sample);

    /// <summary>
    /// Decodes a CDR byte array into a sample.
    /// </summary>
    /// <param name="encoded">The sample CDR representation.</param>
    /// <returns>The decoded sample.</returns>
    T DecodeFromBytes(byte[] encoded);
}