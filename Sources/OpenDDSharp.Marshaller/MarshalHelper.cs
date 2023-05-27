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
        private const string API_DLL = @"OpenDDSWrapper";

        private static readonly UTF32Encoding _utf32Encoding = new UTF32Encoding(!BitConverter.IsLittleEndian, false);
        private static readonly Encoding _utf16Encoding = Encoding.Unicode;

        /// <summary>
        /// Convert a pointer to a sequence.
        /// </summary>
        /// <param name="ptr">The pointer to be converted.</param>
        /// <param name="sequence">The destination sequence.</param>
        /// <param name="capacity">The sequence bound.</param>
        /// <typeparam name="T">The type of the sequence elements.</typeparam>
        public static void PtrToSequence<T>(this IntPtr ptr, ref IList<T> sequence, int capacity = 0)
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

            // For efficiency, only compute the element size once
            var elSiz = Marshal.SizeOf<T>();

            // Populate the list
            for (var i = 0; i < length; i++)
            {
                sequence.Add(Marshal.PtrToStructure<T>(ptr + sizeof(int) + (elSiz * i)));
            }
        }

        /// <summary>
        /// Convert a sequence to a pointer.
        /// </summary>
        /// <param name="sequence">The sequence to be converted.</param>
        /// <param name="ptr">The destination pointer.</param>
        /// <typeparam name="T">The type of the sequence elements.</typeparam>
        public static void SequenceToPtr<T>(this IList<T> sequence, out IntPtr ptr)
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
            for (var i = 0; i < sequence.Count; i++)
            {
                // Newly-allocated space has no existing object, so the last param is false 
                Marshal.StructureToPtr(sequence[i], ptr + sizeof(int) + (elSiz * i), false);
            }
        }

        /// <summary>
        /// Convert a pointer to a sequence of decimals.
        /// </summary>
        /// <param name="ptr">The pointer to be converted.</param>
        /// <param name="sequence">The destination sequence.</param>
        /// <param name="capacity">The sequence bound.</param>
        public static void PtrToLongDoubleSequence(this IntPtr ptr, ref IList<decimal> sequence, int capacity = 0)
        {
            // Ensure a not null empty list to populate
            if (sequence == null)
            {
                sequence = capacity > 0 ? new List<decimal>(capacity) : new List<decimal>();
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

            // For efficiency, only compute the element size once
            var elSiz = Marshal.SizeOf<decimal>();

            // Populate the list
            for (var i = 0; i < length; i++)
            {
                sequence.Add(Convert.ToDecimal(Marshal.PtrToStructure<decimal>(ptr + sizeof(int) + (elSiz * i))));
            }
        }

        /// <summary>
        /// Convert a sequence of decimals to a pointer.
        /// </summary>
        /// <param name="sequence">The sequence to be converted.</param>
        /// <param name="ptr">The destination pointer.</param>
        public static void LongDoubleSequenceToPtr(this IList<decimal> sequence, out IntPtr ptr)
        {
            if (sequence == null || sequence.Count == 0)
            {
                // No structures in the list. Write 0 and return
                ptr = Marshal.AllocHGlobal(sizeof(int));
                Marshal.WriteInt32(ptr, 0);
                return;
            }

            var elSiz = Marshal.SizeOf<decimal>();

            // Get the total size of unmanaged memory that is needed (length + elements)
            var size = sizeof(int) + (elSiz * sequence.Count);

            // Allocate unmanaged space.
            ptr = Marshal.AllocHGlobal(size);

            // Write the "Length" field first
            Marshal.WriteInt32(ptr, sequence.Count);

            // Write the list data
            for (var i = 0; i < sequence.Count; i++)
            {
                // Newly-allocated space has no existing object, so the last param is false 
                Marshal.StructureToPtr(Convert.ToDouble(sequence[i]), ptr + sizeof(int) + (elSiz * i), false);
            }
        }

        /// <summary>
        ///  Convert a pointer to a wchar character.
        /// </summary>
        /// <param name="ptr">The pointer to be converted.</param>
        /// <returns>The converted character.</returns>
        public static char PtrToWChar(this IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                return '\0';
            }

            var elSiz = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? 2 : 4;

            var bytes = new byte[elSiz];
            Marshal.Copy(ptr, bytes, 0, elSiz);

            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? _utf16Encoding.GetString(bytes).FirstOrDefault()
                : _utf32Encoding.GetString(bytes).FirstOrDefault();
        }

        /// <summary>
        /// Convert a wchar character to a pointer.
        /// </summary>
        /// <param name="c">The character to be converted.</param>
        /// <returns>The converted pointer.</returns>
        public static IntPtr WCharToPtr(this char c)
        {
            if (c == default(char))
            {
                return IntPtr.Zero;
            }

            var elSiz = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? 2 : 4;

            // Allocate unmanaged space.
            var ptr = Marshal.AllocHGlobal(elSiz);

            byte[] bytes;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                bytes = _utf16Encoding.GetBytes(new[] { c });
            }
            else
            {
                bytes = _utf32Encoding.GetBytes(new[] { c });
            }

            Marshal.Copy(bytes, 0, ptr, bytes.Length);

            return ptr;
        }

        /// <summary>
        /// Convert a pointer to a sequence of wchar characters.
        /// </summary>
        /// <param name="ptr">The pointer to be converted.</param>
        /// <param name="sequence">The destination sequence.</param>
        /// <param name="capacity">The sequence bound.</param>
        public static void PtrToWCharSequence(this IntPtr ptr, ref IList<char> sequence, int capacity = 0)
        {
            // Ensure a not null empty list to populate
            if (sequence == null)
            {
                sequence = capacity > 0 ? new List<char>(capacity) : new List<char>();
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

            // For efficiency, only compute the element size once
            var elSiz = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? 2 : 4;

            // Populate the list
            var bytes = new byte[elSiz * length];
            Marshal.Copy(ptr + sizeof(int), bytes, 0, elSiz * length);

            string str;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                str = _utf16Encoding.GetString(bytes);
            }
            else
            {
                str = _utf32Encoding.GetString(bytes);
            }

            if (sequence is List<char> asList)
            {
                asList.AddRange(str.ToCharArray());
            }
            else
            {
                foreach (var item in str)
                {
                    sequence.Add(item);
                }
            }
        }

        /// <summary>
        /// Convert a sequence of wchar characters to a pointer.
        /// </summary>
        /// <param name="sequence">The sequence to be converted.</param>
        /// <param name="ptr">The destination pointer.</param>
        public static void WCharSequenceToPtr(this IList<char> sequence, out IntPtr ptr)
        {
            if (sequence == null || sequence.Count == 0)
            {
                // No structures in the list. Write 0 and return
                ptr = Marshal.AllocHGlobal(sizeof(int));
                Marshal.WriteInt32(ptr, 0);
                return;
            }

            var elSiz = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? 2 : 4;

            // Get the total size of unmanaged memory that is needed (length + elements)
            var size = sizeof(int) + (elSiz * sequence.Count);

            // Allocate unmanaged space.
            ptr = Marshal.AllocHGlobal(size);

            // Write the "Length" field first
            Marshal.WriteInt32(ptr, sequence.Count);

            byte[] bytes;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                bytes = _utf16Encoding.GetBytes(sequence.ToArray());
            }
            else
            {
                bytes = _utf32Encoding.GetBytes(sequence.ToArray());
            }

            Marshal.Copy(bytes, 0, ptr + sizeof(int), bytes.Length);
        }

        /// <summary>
        /// Convert a pointer to a sequence of enums.
        /// </summary>
        /// <param name="ptr">The pointer to be converted.</param>
        /// <param name="sequence">The destination sequence.</param>
        /// <param name="capacity">The sequence bound.</param>
        /// <typeparam name="T">The enumeration type.</typeparam>
        public static void PtrToEnumSequence<T>(this IntPtr ptr, ref IList<T> sequence, int capacity = 0) where T : Enum
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

            // For efficiency, only compute the element size once
            var elSiz = Marshal.SizeOf(Enum.GetUnderlyingType(typeof(T)));

            // Populate the list
            for (var i = 0; i < length; i++)
            {
                var aux = Marshal.PtrToStructure<int>(ptr + sizeof(int) + (elSiz * i));
                sequence.Add((T)Enum.ToObject(typeof(T), aux));
            }
        }

        /// <summary>
        /// Convert a sequence of enums to a pointer.
        /// </summary>
        /// <param name="sequence">The sequence to be converted.</param>
        /// <param name="ptr">The destination pointer.</param>
        /// <typeparam name="T">The enumeration type.</typeparam>
        public static void EnumSequenceToPtr<T>(this IList<T> sequence, out IntPtr ptr) where T : Enum
        {
            if (sequence == null || sequence.Count == 0)
            {
                // No structures in the list. Write 0 and return
                ptr = Marshal.AllocHGlobal(sizeof(int));
                Marshal.WriteInt32(ptr, 0);
                return;
            }

            var elSiz = Marshal.SizeOf(Enum.GetUnderlyingType(typeof(T)));

            // Get the total size of unmanaged memory that is needed (length + elements)
            var size = sizeof(int) + (elSiz * sequence.Count);

            // Allocate unmanaged space.
            ptr = Marshal.AllocHGlobal(size);

            // Write the "Length" field first
            Marshal.WriteInt32(ptr, sequence.Count);

            // Write the list data
            for (var i = 0; i < sequence.Count; i++)
            {
                // IDL only accept integer enumerations
                var value = Convert.ToInt32(sequence[i]);

                // Newly-allocated space has no existing object, so the last param is false 
                Marshal.StructureToPtr(value, ptr + sizeof(int) + (elSiz * i), false);
            }
        }

        /// <summary>
        /// Converts a pointer to a sequence of <see cref="bool"/>.
        /// </summary>
        /// <param name="ptr">The pointer to be converted.</param>
        /// <param name="sequence">The destination sequence.</param>
        /// <param name="capacity">The sequence bound.</param>
        public static void PtrToBooleanSequence(this IntPtr ptr, ref IList<bool> sequence, int capacity = 0)
        {
            // Ensure a not null empty list to populate
            if (sequence == null)
            {
                sequence = capacity > 0 ? new List<bool>(capacity) : new List<bool>();
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

            // Structure size is one byte for C++ bool type
            const int elSiz = 1;

            // Populate the list
            for (var i = 0; i < length; i++)
            {
                var aux = Marshal.PtrToStructure<byte>(ptr + sizeof(int) + (elSiz * i));
                sequence.Add(aux == 1);
            }
        }

        /// <summary>
        /// Converts a list of boolean values to a pointer.
        /// </summary>
        /// <param name="sequence">The sequence to be converted.</param>
        /// <param name="ptr">The destination pointer.</param>
        public static void BooleanSequenceToPtr(this IList<bool> sequence, out IntPtr ptr)
        {
            if (sequence == null || sequence.Count == 0)
            {
                // No structures in the list. Write 0 and return
                ptr = Marshal.AllocHGlobal(sizeof(int));
                Marshal.WriteInt32(ptr, 0);
                return;
            }

            // Get the total size of unmanaged memory that is needed (length + elements)
            var size = sizeof(int) + sequence.Count;

            // Allocate unmanaged space.
            ptr = Marshal.AllocHGlobal(size);

            // Write the "Length" field first
            Marshal.WriteInt32(ptr, sequence.Count);

            // Write the list data
            for (var i = 0; i < sequence.Count; i++)
            {
                var aux = Convert.ToBoolean(sequence[i]) ? (byte)0x01 : (byte)0x00;

                // Newly-allocated space has no existing object, so the last param is false 
                Marshal.StructureToPtr(aux, ptr + sizeof(int) + i, false);
            }
        }

        /// <summary>
        /// Converts a pointer to a string.
        /// </summary>
        /// <param name="ptr">The pointer to be converted.</param>
        /// <returns>The converted string.</returns>
        public static string PtrToWideString(this IntPtr ptr)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                int length = 0;
                while (Marshal.ReadInt16(ptr, length) != 0)
                {
                    length += 2;
                }

                var buffer = new byte[length];
                Marshal.Copy(ptr, buffer, 0, buffer.Length);

                return Encoding.Unicode.GetString(buffer);
            }
            else
            {
                var length = 0;
                while (Marshal.ReadInt32(ptr, length) != 0)
                {
                    length += 4;
                }

                var buffer = new byte[length];
                Marshal.Copy(ptr, buffer, 0, buffer.Length);

                return Encoding.UTF32.GetString(buffer);
            }
        }

        /// <summary>
        /// Converts a string to a pointer to a sequence of wide characters.
        /// </summary>
        /// <param name="str">The string to be converted</param>
        /// <returns>The converted pointer.</returns>
        public static IntPtr WideStringToPtr(this string str)
        {
            byte[] bytes;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var utfBytes = Encoding.Unicode.GetBytes(str);

                bytes = new byte[utfBytes.Length + 2];
                Array.Copy(utfBytes, bytes, utfBytes.Length);
            }
            else
            {
                var utfBytes = Encoding.UTF32.GetBytes(str);

                bytes = new byte[utfBytes.Length + 4];
                Array.Copy(utfBytes, bytes, utfBytes.Length);
            }

            var unmanagedPointer = Marshal.AllocHGlobal(bytes.Length);
            Marshal.Copy(bytes, 0, unmanagedPointer, bytes.Length);

            return unmanagedPointer;
        }

        /// <summary>
        ///  Converts a pointer to a sequence of strings to a list of strings.
        /// </summary>
        /// <param name="ptr">The pointer to be converted.</param>
        /// <param name="sequence">The destination sequence.</param>
        /// <param name="isUnicode">Indicates whether the strings are unicode encoded.</param>
        /// <param name="capacity">The initial sequence bound.</param>
        public static void PtrToStringSequence(this IntPtr ptr, ref IList<string> sequence, bool isUnicode,
            int capacity = 0)
        {
            // Ensure a not null empty list to populate
            if (sequence == null)
            {
                sequence = capacity > 0 ? new List<string>(capacity) : new List<string>();
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

            // Populate the array
            for (var i = 0; i < length; i++)
            {
                // Get the unmanaged pointer
                var pointer = Marshal.PtrToStructure<IntPtr>(ptr + sizeof(int) + (IntPtr.Size * i));

                // Convert the pointer in a string
                if (isUnicode)
                {
                    sequence.Add(PtrToWideString(pointer));
                }
                else
                {
                    sequence.Add(Marshal.PtrToStringAnsi(pointer));
                }
            }
        }

        /// <summary>
        /// Convert a pointer to a sequence of UTF8 strings.
        /// </summary>
        /// <param name="ptr">The pointer to be converted.</param>
        /// <param name="sequence">The destination sequence.</param>
        public static void PtrToUTF8StringSequence(this IntPtr ptr, ref IList<string> sequence)
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

            // Populate the array
            for (var i = 0; i < length; i++)
            {
                // Get the unmanaged pointer
                var pointer = Marshal.PtrToStructure<IntPtr>(ptr + sizeof(int) + (IntPtr.Size * i));

                // Convert the pointer in a string
                sequence.Add(StringFromNativeUtf8(pointer));
            }
        }

        /// <summary>
        /// Converts a sequence of strings to a pointer.
        /// </summary>
        /// <param name="sequence">The sequence to be converted.</param>
        /// <param name="ptr">The destination pointer.</param>
        /// <param name="isUnicode">Indicates whether the strings are unicode encoded.</param>
        /// <returns>A list of pointers to be released.</returns>
        public static List<IntPtr> StringSequenceToPtr(this IList<string> sequence, out IntPtr ptr, bool isUnicode)
        {
            var toRelease = new List<IntPtr>();

            if (sequence == null || sequence.Count == 0)
            {
                // No string in the list. Write 0 and return
                ptr = Marshal.AllocHGlobal(sizeof(int));
                Marshal.WriteInt32(ptr, 0);
                return toRelease;
            }

            // Get the total size of unmanaged memory that is needed (length + elements)
            var size = sizeof(int) + (IntPtr.Size * sequence.Count);

            // Allocate unmanaged space.
            ptr = Marshal.AllocHGlobal(size);

            // Write the "Length" field first
            Marshal.WriteInt32(ptr, sequence.Count);

            // Write the pointers to the string data
            for (var i = 0; i < sequence.Count; i++)
            {
                // Create a pointer to the string in unmanaged memory
                var sPtr = isUnicode ? WideStringToPtr(sequence[i]) : Marshal.StringToHGlobalAnsi(sequence[i]);

                // Add to pointer to the list of pointers to release
                toRelease.Add(sPtr);

                // Write the pointer location in ptr
                Marshal.StructureToPtr(sPtr, ptr + sizeof(int) + (i * IntPtr.Size), false);
            }

            return toRelease;
        }

        /// <summary>
        /// Convert a pointer to a multi-dimensional array.
        /// </summary>
        /// <param name="ptr">The pointer to be converted.</param>
        /// <param name="array">The destination array.</param>
        /// <typeparam name="T">The type of the array elements.</typeparam>
        public static void PtrToMultiArray<T>(this IntPtr ptr, Array array)
        {
            // We need to ensure that the array is not null before the call 
            if (array == null)
            {
                return;
            }

            var elSiz = Marshal.SizeOf<T>();

            var length = 1;
            for (var i = 0; i < array.Rank; i++)
            {
                length *= array.GetLength(i);
            }

            var dimensions = new int[array.Rank];
            for (var i = 0; i < length; i++)
            {
                if (i > 0)
                {
                    UpdateDimensionsArray(array, dimensions);
                }

                array.SetValue(Marshal.PtrToStructure<T>(ptr + (elSiz * i)), dimensions);
            }
        }

        /// <summary>
        /// Convert a multi-dimensional array to a pointer.
        /// </summary>
        /// <param name="array">The array to be converted.</param>
        /// <param name="ptr">The destination pointer.</param>
        /// <typeparam name="T">The type of the array elements.</typeparam>
        public static void MultiArrayToPtr<T>(this Array array, ref IntPtr ptr)
        {
            if (array == null || array.Length == 0)
            {
                return;
            }

            var elSiz = Marshal.SizeOf<T>();

            // Get the total size of unmanaged memory that is needed
            var size = elSiz * array.Length;

            // Allocate unmanaged space.
            ptr = Marshal.AllocHGlobal(size);

            var enumerator = array.GetEnumerator();
            var i = 0;
            while (enumerator.MoveNext())
            {
                if (enumerator.Current is char && elSiz == 1)
                {
                    var aux = Convert.ToByte(enumerator.Current);
                    Marshal.StructureToPtr(aux, ptr + (elSiz * i), false);
                }
                else if (enumerator.Current is char && elSiz == 4)
                {
                    var aux = char.ConvertToUtf32(enumerator.Current.ToString(), 0);
                    Marshal.StructureToPtr(aux, ptr + (elSiz * i), false);
                }
                else
                {
                    Marshal.StructureToPtr((T)enumerator.Current, ptr + (elSiz * i), false);
                }

                i++;
            }
        }

        /// <summary>
        /// Converts a pointer to a multi-dimensional array of enums
        /// </summary>
        /// <param name="ptr">The pointer to be converted.</param>
        /// <param name="array">The destination array.</param>
        /// <typeparam name="T">The type of the array elements.</typeparam>
        public static void PtrToEnumMultiArray<T>(this IntPtr ptr, Array array) where T : Enum
        {
            // We need to ensure that the array is not null before the call 
            if (array == null)
            {
                return;
            }

            var elSiz = Marshal.SizeOf(Enum.GetUnderlyingType(typeof(T)));
            var length = 1;
            for (var i = 0; i < array.Rank; i++)
            {
                length *= array.GetLength(i);
            }

            var dimensions = new int[array.Rank];
            for (var i = 0; i < length; i++)
            {
                if (i > 0)
                {
                    UpdateDimensionsArray(array, dimensions);
                }

                var aux = Marshal.PtrToStructure<int>(ptr + (elSiz * i));
                array.SetValue((T)Enum.ToObject(typeof(T), aux), dimensions);
            }
        }

        /// <summary>
        /// Convert a multi-dimensional array to a pointer.
        /// </summary>
        /// <param name="array">The array to be converted.</param>
        /// <param name="ptr">The destination pointer.</param>
        /// <typeparam name="T">The type of the array elements.</typeparam>
        public static void EnumMultiArrayToPtr<T>(this Array array, ref IntPtr ptr) where T : Enum
        {
            if (array == null || array.Length == 0)
            {
                return;
            }

            var elSiz = Marshal.SizeOf(Enum.GetUnderlyingType(typeof(T)));

            // Get the total size of unmanaged memory that is needed
            var size = elSiz * array.Length;

            // Allocate unmanaged space.
            ptr = Marshal.AllocHGlobal(size);

            var enumerator = array.GetEnumerator();
            var i = 0;
            while (enumerator.MoveNext())
            {
                // IDL only accept integer enumerations
                var value = Convert.ToInt32(enumerator.Current);

                // Newly-allocated space has no existing object, so the last param is false 
                Marshal.StructureToPtr(value, ptr + (elSiz * i), false);

                i++;
            }
        }

        /// <summary>
        /// Converts a multi-dimensional array of boolean to a pointer
        /// </summary>
        /// <param name="ptr">The pointer to be converted.</param>
        /// <param name="array">The destination array.</param>
        public static void PtrToBooleanMultiArray(this IntPtr ptr, Array array)
        {
            // We need to ensure that the array is not null before the call 
            if (array == null)
            {
                return;
            }

            var length = 1;
            for (var i = 0; i < array.Rank; i++)
            {
                length *= array.GetLength(i);
            }

            var dimensions = new int[array.Rank];
            for (var i = 0; i < length; i++)
            {
                if (i > 0)
                {
                    UpdateDimensionsArray(array, dimensions);
                }

                var aux = Marshal.PtrToStructure<byte>(ptr + i);
                array.SetValue(aux == 1, dimensions);
            }
        }

        /// <summary>
        /// Converts a multi-dimensional array of boolean values to a pointer.
        /// </summary>
        /// <param name="array">The array to be converted.</param>
        /// <param name="ptr">The destination pointer.</param>
        public static void BooleanMultiArrayToPtr(this Array array, ref IntPtr ptr)
        {
            if (array == null || array.Length == 0)
            {
                return;
            }

            // Get the total size of unmanaged memory that is needed
            var size = array.Length;

            // Allocate unmanaged space.
            ptr = Marshal.AllocHGlobal(size);

            var enumerator = array.GetEnumerator();

            var i = 0;
            while (enumerator.MoveNext())
            {
                var aux = enumerator.Current != null && (bool)enumerator.Current ? (byte)1 : (byte)0;
                Marshal.StructureToPtr(aux, ptr + i, false);
                i++;
            }
        }

        /// <summary>
        /// Converts a pointer to a multi-dimensional array of wchar characters.
        /// </summary>
        /// <param name="ptr">The pointer to be converted.</param>
        /// <param name="array">The destination array.</param>
        public static void PtrToWCharMultiArray(this IntPtr ptr, Array array)
        {
            // We need to ensure that the array is not null before the call 
            if (array == null)
            {
                return;
            }

            var length = 1;
            for (var i = 0; i < array.Rank; i++)
            {
                length *= array.GetLength(i);
            }

            var elSiz = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? Marshal.SizeOf<char>()
                : Marshal.SizeOf<int>();

            var dimensions = new int[array.Rank];
            for (var i = 0; i < length; i++)
            {
                if (i > 0)
                {
                    UpdateDimensionsArray(array, dimensions);
                }

                char value;
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    value = Marshal.PtrToStructure<char>(ptr + (elSiz * i));
                }
                else
                {
                    var bytes = new byte[4];
                    Marshal.Copy(ptr + (elSiz * i), bytes, 0, 4);
                    value = _utf32Encoding.GetString(bytes).FirstOrDefault();
                }

                array.SetValue(value, dimensions);
            }
        }

        /// <summary>
        /// Converts a multi-dimensional array of strings to an array of pointers to strings.
        /// </summary>
        /// <param name="array">The array to be converted.</param>
        /// <param name="ptr">The destination pointer.</param>
        /// <param name="isUnicode">Indicates if the string are encoded in unicode.</param>
        /// <returns>A list of pointers to be released.</returns>
        public static List<IntPtr> StringMultiArrayToPtr(this Array array, ref IntPtr ptr, bool isUnicode)
        {
            List<IntPtr> toRelease = new List<IntPtr>();

            if (array == null || array.Length == 0)
            {
                return toRelease;
            }

            var elSiz = IntPtr.Size;

            // Get the total size of unmanaged memory that is needed
            var size = elSiz * array.Length;

            // Allocate unmanaged space.
            ptr = Marshal.AllocHGlobal(size);

            var enumerator = array.GetEnumerator();
            var i = 0;
            while (enumerator.MoveNext())
            {
                // Create a pointer to the string in unmanaged memory
                var sPtr = isUnicode
                    ? WideStringToPtr((string)enumerator.Current)
                    : Marshal.StringToHGlobalAnsi((string)enumerator.Current);

                // Add to pointer to the list of pointers to release
                toRelease.Add(sPtr);

                // Write the pointer location in ptr
                Marshal.StructureToPtr(sPtr, ptr + (i * IntPtr.Size), false);

                i++;
            }

            return toRelease;
        }

        /// <summary>
        /// Convert a pointer to a multi-dimensional array of strings.
        /// </summary>
        /// <param name="ptr">The pointer to be converted.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="isUnicode">Indicates if the string are encoded in unicode.</param>
        public static void PtrToStringMultiArray(this IntPtr ptr, Array array, bool isUnicode)
        {
            // We need to ensure that the array is not null before the call
            if (array == null)
            {
                return;
            }

            var length = 1;
            for (var i = 0; i < array.Rank; i++)
            {
                length *= array.GetLength(i);
            }

            var dimensions = new int[array.Rank];
            for (var i = 0; i < length; i++)
            {
                if (i > 0)
                {
                    UpdateDimensionsArray(array, dimensions);
                }

                // Get the unmanaged pointer
                var pointer = Marshal.PtrToStructure<IntPtr>(ptr + (IntPtr.Size * i));

                // Convert the pointer in a string
                if (isUnicode)
                {
                    array.SetValue(PtrToWideString(pointer), dimensions);
                }
                else
                {
                    array.SetValue(Marshal.PtrToStringAnsi(pointer), dimensions);
                }
            }
        }

        /// <summary>
        /// Update the dimensions array.
        /// </summary>
        /// <param name="array">The array to be updated.</param>
        /// <param name="dimensions">The new dimensions for the array.</param>
        public static void UpdateDimensionsArray(this Array array, int[] dimensions)
        {
            dimensions[array.Rank - 1]++;
            if (dimensions[array.Rank - 1] >= array.GetLength(array.Rank - 1))
            {
                for (var j = array.Rank - 1; j > 0; j--)
                {
                    dimensions[j - 1]++;
                    dimensions[j] = 0;

                    if (dimensions[j - 1] < array.GetLength(j - 1))
                    {
                        break;
                    }
                }
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
        /// Release a native wide string pointer.
        /// </summary>
        /// <param name="ptr">The pointer to be released.</param>
        public static void ReleaseNativeWideStringPointer(this IntPtr ptr)
        {
            UnsafeNativeMethods.ReleaseWideStringPointer(ptr);
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
        /// Release a native wide string sequence.
        /// </summary>
        /// <param name="ptr">The pointer to be released.</param>
        public static void ReleaseNativeWideStringSequence(this IntPtr ptr)
        {
            UnsafeNativeMethods.ReleaseWideStringSequence(ref ptr);
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

            var len = Encoding.UTF8.GetByteCount(managedString);

            var buffer = new byte[len + 1];
            Encoding.UTF8.GetBytes(managedString, 0, managedString.Length, buffer, 0);

            var nativeUtf8 = Marshal.AllocHGlobal(buffer.Length);
            Marshal.Copy(buffer, 0, nativeUtf8, buffer.Length);

            return nativeUtf8;
        }

        /// <summary>
        /// Convert a native pointer to a string.
        /// </summary>
        /// <param name="nativeUtf8">The native pointer.</param>
        /// <returns>The converted string.</returns>
        public static string StringFromNativeUtf8(IntPtr nativeUtf8)
        {
            if (nativeUtf8 == IntPtr.Zero)
            {
                return null;
            }

            var len = 0;
            while (Marshal.ReadByte(nativeUtf8, len) != 0)
            {
                ++len;
            }

            var buffer = new byte[len];
            Marshal.Copy(nativeUtf8, buffer, 0, buffer.Length);

            return Encoding.UTF8.GetString(buffer);
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
            [DllImport(API_DLL, EntryPoint = "release_wide_string_ptr", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void ReleaseWideStringPointer(IntPtr ptr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(API_DLL, EntryPoint = "release_basic_string_sequence_ptr",
                CallingConvention = CallingConvention.Cdecl)]
            internal static extern void ReleaseStringSequence([In, Out] ref IntPtr ptr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(API_DLL, EntryPoint = "release_wide_string_sequence_ptr",
                CallingConvention = CallingConvention.Cdecl)]
            internal static extern void ReleaseWideStringSequence([In, Out] ref IntPtr ptr);
        }

        #endregion
    }
}
