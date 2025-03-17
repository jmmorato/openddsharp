using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace OpenDDSharp.Marshaller
{
    /// <summary>
    /// Helper class for marshalling.
    /// </summary>
    public static class MarshalHelper
    {
        private const string API_DLL = "OpenDDSWrapper";

        /// <summary>
        /// Convert a pointer to a sequence.
        /// </summary>
        /// <param name="ptr">The pointer to be converted.</param>
        /// <param name="sequence">The destination sequence.</param>
        /// <param name="capacity">The sequence bound.</param>
        /// <typeparam name="T">The type of the sequence elements.</typeparam>
        public static unsafe void PtrToSequence<T>(this IntPtr ptr, ref IList<T> sequence, int capacity = 0)
        {
            // Ensure a not null empty list to populate
            if (sequence == null)
            {
                sequence = capacity > 0 ? new List<T>(capacity) : new List<T>();
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
            var span = new ReadOnlySpan<T>((ptr + sizeInt).ToPointer(), length);

            // Populate the list
            for (var i = 0; i < length; i++)
            {
                sequence.Add(span[i]);
            }
        }

        /// <summary>
        /// Convert a pointer to a sequence of UTF8 strings.
        /// </summary>
        /// <param name="ptr">The pointer to be converted.</param>
        /// <param name="sequence">The destination sequence.</param>
        public static unsafe void PtrToUTF8StringSequence(this IntPtr ptr, ref IList<string> sequence)
        {
            // Ensure a not null empty list to populate
            if (sequence == null)
            {
                sequence = new List<string>();
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
                sequence.Add(StringFromNativeUtf8(pointer));
            }
        }

        /// <summary>
        /// Release a native pointer.
        /// </summary>
        /// <param name="ptr">The pointer to be released.</param>
        public static void ReleaseNativePointer(this IntPtr ptr)
        {
            UnsafeNativeMethods.ReleasePointer(ptr);
        }

        /// <summary>
        /// Release a native string pointer.
        /// </summary>
        /// <param name="ptr">The pointer to be released.</param>
        public static void ReleaseNativeStringPointer(this IntPtr ptr)
        {
            UnsafeNativeMethods.ReleaseStringPointer(ptr);
        }

        /// <summary>
        /// Release a native string sequence.
        /// </summary>
        /// <param name="ptr">The pointer to be released.</param>
        public static void ReleaseNativeStringSequence(this IntPtr ptr)
        {
            UnsafeNativeMethods.ReleaseStringSequence(ref ptr);
        }

        /// <summary>
        /// Convert a managed string to a native pointer.
        /// </summary>
        /// <param name="managedString">The string to be converted.</param>
        /// <returns>The converted pointer.</returns>
        public static IntPtr NativeUtf8FromString(string managedString)
        {
            if (managedString == null)
            {
                return IntPtr.Zero;
            }

#if NETCOREAPP3_1_OR_GREATER
            return Marshal.StringToCoTaskMemUTF8(managedString);
#else
            var len = Encoding.UTF8.GetByteCount(managedString);

            var buffer = new byte[len + 1];
            Encoding.UTF8.GetBytes(managedString, 0, managedString.Length, buffer, 0);

            var nativeUtf8 = Marshal.AllocHGlobal(buffer.Length);
            Marshal.Copy(buffer, 0, nativeUtf8, buffer.Length);

            return nativeUtf8;
#endif
        }

        /// <summary>
        /// Convert a native pointer to a string.
        /// </summary>
        /// <param name="nativeUtf8">The native pointer.</param>
        /// <returns>The converted string.</returns>
        public static unsafe string StringFromNativeUtf8(IntPtr nativeUtf8)
        {
            if (nativeUtf8 == IntPtr.Zero)
            {
                return null;
            }
#if NETCOREAPP3_1_OR_GREATER
            return Marshal.PtrToStringUTF8(nativeUtf8);
#else
            var len = 0;
            while (Marshal.ReadByte(nativeUtf8, len) != 0)
            {
                ++len;
            }

            var span = new ReadOnlySpan<byte>(nativeUtf8.ToPointer(), len);

            return Encoding.UTF8.GetString(span.ToArray());
#endif
        }

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

            [SuppressUnmanagedCodeSecurity]
            [DllImport(API_DLL, EntryPoint = "release_basic_string_ptr", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void ReleaseStringPointer(IntPtr ptr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(API_DLL, EntryPoint = "release_basic_string_sequence_ptr",
                CallingConvention = CallingConvention.Cdecl)]
            internal static extern void ReleaseStringSequence([In, Out] ref IntPtr ptr);
        }
        #endregion
    }
}
