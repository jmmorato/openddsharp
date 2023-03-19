using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

internal static class MarshalHelper
{
    private static readonly UTF32Encoding _encoding = new UTF32Encoding(!BitConverter.IsLittleEndian, false);

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

    public static void SequenceToPtr<T>(this IList<T> sequence, ref IntPtr ptr)
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

    public static void LongDoubleSequenceToPtr(this IList<decimal> sequence, ref IntPtr ptr)
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

    public static char PtrToWChar(this IntPtr ptr)
    {
        if (ptr == IntPtr.Zero)
        {
            return '\0';
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return Marshal.PtrToStructure<char>(ptr);

        }

        var bytes = new byte[4];
        Marshal.Copy(ptr, bytes, 0, 4);
        return _encoding.GetString(bytes).FirstOrDefault();
    }
    
    public static IntPtr WCharToPtr(this char c)
    {
        if (c == default(char))
        {
            return IntPtr.Zero;
        }

        var elSiz = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? 2 : 4;
        
        // Allocate unmanaged space.
        var ptr = Marshal.AllocHGlobal(elSiz);

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Marshal.StructureToPtr(c, ptr, false);
        }
        else
        {
            var bytes = _encoding.GetBytes(new[] { c });
            Marshal.Copy(bytes, 0, ptr, bytes.Length);
        }

        return ptr;
    }
    
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
        for (var i = 0; i < length; i++)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                sequence.Add(Marshal.PtrToStructure<char>(ptr + sizeof(int) + (elSiz * i)));
            }
            else
            {
                var bytes = new byte[4];
                Marshal.Copy(ptr + sizeof(int) + (elSiz * i), bytes, 0, 4);
                var character =  _encoding.GetString(bytes).FirstOrDefault();
                sequence.Add(character);
                
                // var utf32 = Marshal.PtrToStructure<int>(ptr + sizeof(int) + (elSiz * i));
                // sequence.Add(ConvertFromUtf32(utf32));
            }
        }
    }

    public static void WCharSequenceToPtr(this IList<char> sequence, ref IntPtr ptr)
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

        // Write the list data
        for (var i = 0; i < sequence.Count; i++)
        {
            // Newly-allocated space has no existing object, so the last param is false
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Marshal.StructureToPtr(sequence[i], ptr + sizeof(int) + (elSiz * i), false);
            }
            else
            {
                var bytes = _encoding.GetBytes(new[] { sequence[i] });
                Marshal.Copy(bytes, 0, ptr + sizeof(int) + (elSiz * i), bytes.Length);
                // Marshal.StructureToPtr(char.ConvertToUtf32(sequence[i].ToString(), 0), ptr + sizeof(int) + (elSiz * i), false);
            }
        }
    }

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

    public static void EnumSequenceToPtr<T>(this IList<T> sequence, ref IntPtr ptr) where T : Enum
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

    public static void BooleanSequenceToPtr(this IList<bool> sequence, ref IntPtr ptr)
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
            Marshal.StructureToPtr(aux, ptr + sizeof(int) +  i, false);
        }
    }

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

    public static void PtrToStringSequence(this IntPtr ptr, ref IList<string> sequence, bool isUnicode, int capacity = 0)
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

    public static List<IntPtr> StringSequenceToPtr(this IList<string> sequence, ref IntPtr ptr, bool isUnicode)
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
            var aux = (bool)enumerator.Current ? (byte)1 : (byte)0;
            Marshal.StructureToPtr(aux, ptr + i, false);
            i++;
        }
    }

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
                value = _encoding.GetString(bytes).FirstOrDefault();
                
                // var aux = Marshal.PtrToStructure<int>(ptr + (elSiz * i));
                // value = ConvertFromUtf32(aux);
            }

            array.SetValue(value, dimensions);
        }
    }

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

    public static decimal ToDecimal(this double d)
    {
        if (double.IsNaN(d))
        {
            return decimal.MinValue;
        }

        Console.WriteLine("ToDecimal: " + d);
        if (d <= (double)decimal.MinValue)
        {
            return decimal.MinValue;
        }
        else if (d >= (double)decimal.MaxValue)
        {
            return decimal.MaxValue;
        }
        else
        {
            return (decimal)d;
        }
    }

    // public static char ConvertFromUtf32(int codepoint)
    // {
    //     var isValidUnicodeCharacter = (codepoint >= 0x00000 && codepoint <= 0x10FFFF)
    //                                   && (codepoint < 0xD800 || codepoint > 0xDFFF);
    //
    //     return !isValidUnicodeCharacter ? '\0' : char.ConvertFromUtf32(codepoint).FirstOrDefault();
    // }

    internal static void UpdateDimensionsArray(this Array array, int[] dimensions)
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
}