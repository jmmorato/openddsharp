﻿/*********************************************************************
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
using System;
using System.Collections.Generic;
#if NETCOREAPP3_1_OR_GREATER
using System.Runtime.CompilerServices;
#endif
using System.Runtime.InteropServices;
using System.Security;

namespace OpenDDSharp.Helpers
{
    internal static class MarshalHelper
    {
        #region Constants
        internal const string API_DLL = "OpenDDSWrapper";
        #endregion

        #region Methods
        public static unsafe void PtrToSequence<T>(this IntPtr ptr, ref ICollection<T> sequence, int capacity = 0)
        {
            // Ensure a not null empty list to populate
            if (sequence == null)
            {
                if (capacity > 0)
                {
                    sequence = new List<T>(capacity);
                }
                else
                {
                    sequence = new List<T>();
                }
            }
            else
            {
                sequence.Clear();
            }

            if (ptr == IntPtr.Zero)
            {
                return;
            }

            // Start by reading the size of the array
            var length = Marshal.ReadInt32(ptr);

#if NETCOREAPP3_1_OR_GREATER
            const int sizeInt = sizeof(int);
            if (!RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            {
                var span = new ReadOnlySpan<T>((ptr + sizeInt).ToPointer(), length);

                // Populate the list
                for (var i = 0; i < length; i++)
                {
                    sequence.Add(span[i]);
                }
            }
            else
            {
                // For efficiency, only compute the element size once
                var elSiz = Marshal.SizeOf<T>();

                // Populate the list
                for (var i = 0; i < length; i++)
                {
                    sequence.Add(Marshal.PtrToStructure<T>(ptr + sizeof(int) + (elSiz * i)));
                }
            }
#else
            // For efficiency, only compute the element size once
            var elSiz = Marshal.SizeOf<T>();

            // Populate the list
            for (var i = 0; i < length; i++)
            {
                sequence.Add(Marshal.PtrToStructure<T>(ptr + sizeof(int) + (elSiz * i)));
            }
#endif
        }

        public static unsafe void PtrToSequence<T>(this IntPtr ptr, ref IList<T> sequence, int capacity = 0)
        {
            // Ensure a not null empty list to populate
            if (sequence == null)
            {
                if (capacity > 0)
                {
                    sequence = new List<T>(capacity);
                }
                else
                {
                    sequence = new List<T>();
                }
            }
            else
            {
                sequence.Clear();
            }

            if (ptr == IntPtr.Zero)
            {
                return;
            }

            // Start by reading the size of the array
            var length = Marshal.ReadInt32(ptr);

#if NETCOREAPP3_1_OR_GREATER
            if (!RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            {
                const int sizeInt = sizeof(int);
                var span = new ReadOnlySpan<T>((ptr + sizeInt).ToPointer(), length);

                // Populate the list
                for (var i = 0; i < length; i++)
                {
                    sequence.Add(span[i]);
                }
            }
            else
            {
                // For efficiency, only compute the element size once
                var elSiz = Marshal.SizeOf<T>();

                // Populate the list
                for (var i = 0; i < length; i++)
                {
                    sequence.Add(Marshal.PtrToStructure<T>(ptr + sizeof(int) + (elSiz * i)));
                }
            }
#else
            // For efficiency, only compute the element size once
            var elSiz = Marshal.SizeOf<T>();

            // Populate the list
            for (var i = 0; i < length; i++)
            {
                sequence.Add(Marshal.PtrToStructure<T>(ptr + sizeof(int) + (elSiz * i)));
            }
#endif
        }

        public static void SequenceToPtr<T>(this ICollection<T> sequence, ref IntPtr ptr)
        {
            if (sequence == null || sequence.Count == 0)
            {
                // No structures in the list. Write 0 and return
                ptr = Marshal.AllocHGlobal(sizeof(int));
                Marshal.WriteInt32(ptr, 0);
                return;
            }

            var elSiz = Marshal.SizeOf<T>();

            // Get the total size of unmanaged memory that is needed (length + elements)
            var size = sizeof(int) + (elSiz * sequence.Count);

            // Allocate unmanaged space.
            ptr = Marshal.AllocHGlobal(size);

            // Write the "Length" field first
            Marshal.WriteInt32(ptr, sequence.Count);

            // Write the list data
            var i = 0;
            foreach (var element in sequence)
            {
                // Newly-allocated space has no existing object, so the last param is false
                Marshal.StructureToPtr(element, ptr + sizeof(int) + (elSiz * i), false);
                i++;
            }
        }

        public static void SequenceToPtr<T>(this IList<T> sequence, ref IntPtr ptr)
        {
            if (sequence == null || sequence.Count == 0)
            {
                // No structures in the list. Write 0 and return
                ptr = Marshal.AllocHGlobal(sizeof(int));
                Marshal.WriteInt32(ptr, 0);
                return;
            }

            int elSiz = Marshal.SizeOf<T>();

            // Get the total size of unmanaged memory that is needed (length + elements)
            int size = sizeof(int) + (elSiz * sequence.Count);

            // Allocate unmanaged space.
            ptr = Marshal.AllocHGlobal(size);

            // Write the "Length" field first
            Marshal.WriteInt32(ptr, sequence.Count);

            // Write the list data
            for (int i = 0; i < sequence.Count; i++)
            {
                // Newly-allocated space has no existing object, so the last param is false
                Marshal.StructureToPtr(sequence[i], ptr + sizeof(int) + (elSiz * i), false);
            }
        }

        public static unsafe void PtrToStringSequence(this IntPtr ptr, ref IList<string> sequence, bool isUnicode, int capacity = 0)
        {
            // Ensure a not null empty list to populate
            if (sequence == null)
            {
                if (capacity > 0)
                {
                    sequence = new List<string>(capacity);
                }
                else
                {
                    sequence = new List<string>();
                }
            }
            else
            {
                sequence.Clear();
            }

            if (ptr == IntPtr.Zero)
            {
                return;
            }

            // Start by reading the size of the array
            var length = Marshal.ReadInt32(ptr);

            const int sizeInt = sizeof(int);
            var span = new ReadOnlySpan<IntPtr>((ptr + sizeInt).ToPointer(), length);

            // Populate the array
            for (var i = 0; i < length; i++)
            {
                // Get the unmanaged pointer
                var pointer = span[i];

                // Convert the pointer in a string
                if (isUnicode)
                {
                    sequence.Add(Marshal.PtrToStringUni(pointer));
                }
                else
                {
                    sequence.Add(Marshal.PtrToStringAnsi(pointer));
                }
            }
        }

        public static List<IntPtr> StringSequenceToPtr(this IList<string> sequence, ref IntPtr ptr, bool isUnicode)
        {
            List<IntPtr> toRelease = new List<IntPtr>();

            if (sequence == null || sequence.Count == 0)
            {
                // No string in the list. Write 0 and return
                ptr = Marshal.AllocHGlobal(sizeof(int));
                Marshal.WriteInt32(ptr, 0);
                return toRelease;
            }

            // Get the total size of unmanaged memory that is needed (length + elements)
            int size = sizeof(int) + (IntPtr.Size * sequence.Count);

            // Allocate unmanaged space.
            ptr = Marshal.AllocHGlobal(size);

            // Write the "Length" field first
            Marshal.WriteInt32(ptr, sequence.Count);

            // Write the pointers to the string data
            for (int i = 0; i < sequence.Count; i++)
            {
                // Create a pointer to the string in unmanaged memory
                IntPtr sPtr;
                if (isUnicode)
                {
                    sPtr = Marshal.StringToHGlobalUni(sequence[i]);
                }
                else
                {
                    sPtr = Marshal.StringToHGlobalAnsi(sequence[i]);
                }

                // Add to pointer to the list of pointers to release
                toRelease.Add(sPtr);

                // Write the pointer location in ptr
                Marshal.StructureToPtr(sPtr, ptr + sizeof(int) + (i * IntPtr.Size), false);
            }

            return toRelease;
        }

        public static void ReleaseNativePointer(this IntPtr ptr)
        {
            UnsafeNativeMethods.ReleasePointer(ptr);
        }
        #endregion

        #region UnsafeNativeMethods
        /// <summary>
        /// This class suppresses stack walks for unmanaged code permission. (System.Security.SuppressUnmanagedCodeSecurityAttribute is applied to this class.)
        /// This class is for methods that are potentially dangerous. Any caller of these methods must perform a full security review to make sure that the usage
        /// is secure because no stack walk will be performed.
        /// </summary>
        [SuppressUnmanagedCodeSecurity]
        private static class UnsafeNativeMethods
        {
            [SuppressUnmanagedCodeSecurity]
            [DllImport(API_DLL, EntryPoint = "release_native_ptr", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void ReleasePointer(IntPtr ptr);
        }
        #endregion
    }
}
