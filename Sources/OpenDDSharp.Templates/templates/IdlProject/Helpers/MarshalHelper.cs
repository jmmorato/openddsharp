using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

internal static class MarshalHelper
{
    public static void PtrToSequence<T>(this IntPtr ptr, ref IList<T> sequence, int capacity = 0)
    {
        // Ensure a not null empty list to populate
        if (sequence == null)
        {
            if (capacity > 0)
                sequence = new List<T>(capacity);
            else
                sequence = new List<T>();
        }
        else
            sequence.Clear();

        if (ptr == IntPtr.Zero)
            return;

        // Start by reading the size of the array
        int length = Marshal.ReadInt32(ptr);
        // For efficiency, only compute the element size once
        int elSiz = Marshal.SizeOf<T>();
        // Populate the list
        for (int i = 0; i < length; i++)
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

    public static void PtrToLongDoubleSequence(this IntPtr ptr, ref IList<decimal> sequence, int capacity = 0)
    {
        // Ensure a not null empty list to populate
        if (sequence == null)
        {
            if (capacity > 0)
                sequence = new List<decimal>(capacity);
            else
                sequence = new List<decimal>();
        }
        else
        {
            sequence.Clear();
        }

        if (ptr == IntPtr.Zero)
            return;

        // Start by reading the size of the array
        int length = Marshal.ReadInt32(ptr);
        // For efficiency, only compute the element size once
        int elSiz = Marshal.SizeOf<double>();
        // Populate the list
        for (int i = 0; i < length; i++)
        {
            sequence.Add(Convert.ToDecimal(Marshal.PtrToStructure<double>(ptr + sizeof(int) + (elSiz * i))));
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

        int elSiz = Marshal.SizeOf<double>();
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
            Marshal.StructureToPtr(Convert.ToDouble(sequence[i]), ptr + sizeof(int) + (elSiz * i), false);
        }
    }

    public static void PtrToWCharSequence(this IntPtr ptr, ref IList<char> sequence, int capacity = 0)
    {
        // Ensure a not null empty list to populate
        if (sequence == null)
        {
            if (capacity > 0)
            {
                sequence = new List<char>(capacity);
            }
            else
            {
                sequence = new List<char>();
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
        int length = Marshal.ReadInt32(ptr);

        // For efficiency, only compute the element size once
#if Windows
        int elSiz = 2;
#else
        int elSiz = 4;
#endif

        // Populate the list
        for (int i = 0; i < length; i++)
        {
#if Windows
            sequence.Add(Marshal.PtrToStructure<char>(ptr + sizeof(int) + (elSiz * i)));            
#else
            int utf32 = Marshal.PtrToStructure<int>(ptr + sizeof(int) + (elSiz * i));
            sequence.Add(ConvertFromUtf32(utf32));
#endif
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

#if Windows
        int elSiz = 2;
#else
        int elSiz = 4;
#endif
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
#if Windows
            Marshal.StructureToPtr(sequence[i], ptr + sizeof(int) + (elSiz * i), false);
#else
            Marshal.StructureToPtr(char.ConvertToUtf32(sequence[i].ToString(), 0), ptr + sizeof(int) + (elSiz * i), false);
#endif
        }
    }

    public static void PtrToEnumSequence<T>(this IntPtr ptr, ref IList<T> sequence, int capacity = 0) where T : Enum
    {
        // Ensure a not null empty list to populate
        if (sequence == null)
        {
            if (capacity > 0)
                sequence = new List<T>(capacity);
            else
                sequence = new List<T>();
        }
        else
        {
            sequence.Clear();
        }

        if (ptr == IntPtr.Zero) return;

        // Start by reading the size of the array
        int length = Marshal.ReadInt32(ptr);
        // For efficiency, only compute the element size once
        int elSiz = Marshal.SizeOf(Enum.GetUnderlyingType(typeof(T)));
        // Populate the list
        for (int i = 0; i < length; i++)
        {
            int aux = Marshal.PtrToStructure<int>(ptr + sizeof(int) + (elSiz * i));
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

        int elSiz = Marshal.SizeOf(Enum.GetUnderlyingType(typeof(T)));
        // Get the total size of unmanaged memory that is needed (length + elements)
        int size = sizeof(int) + (elSiz * sequence.Count);
        // Allocate unmanaged space.
        ptr = Marshal.AllocHGlobal(size);
        // Write the "Length" field first
        Marshal.WriteInt32(ptr, sequence.Count);
        // Write the list data
        //Marshal.Copy(sequence.ToArray(), 0, ptr + sizeof(int), sequence.Count);
        for (int i = 0; i < sequence.Count; i++)
        {
            // IDL only accept integer enumerations
            int value = Convert.ToInt32(sequence[i]);
            // Newly-allocated space has no existing object, so the last param is false 
            Marshal.StructureToPtr(value, ptr + sizeof(int) + (elSiz * i), false);

        }
    }

    public static void PtrToBooleanSequence(this IntPtr ptr, ref IList<bool> sequence, int capacity = 0)
    {
        // Ensure a not null empty list to populate
        if (sequence == null)
        {
            if (capacity > 0)
                sequence = new List<bool>(capacity);
            else
                sequence = new List<bool>();
        }
        else
        {
            sequence.Clear();
        }

        if (ptr == IntPtr.Zero)
            return;

        // Start by reading the size of the array
        int length = Marshal.ReadInt32(ptr);

        // Structure size is one byte for C++ bool type
        int elSiz = 1;

        // Populate the list
        for (int i = 0; i < length; i++)
        {
            byte aux = Marshal.PtrToStructure<byte>(ptr + sizeof(int) + (elSiz * i));
            sequence.Add(aux == 1 ? true : false);
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

        // Structure size is one byte for C++ bool type
        int elSiz = 1;
        // Get the total size of unmanaged memory that is needed (length + elements)
        int size = sizeof(int) + (elSiz * sequence.Count);
        // Allocate unmanaged space.
        ptr = Marshal.AllocHGlobal(size);
        // Write the "Length" field first
        Marshal.WriteInt32(ptr, sequence.Count);
        // Write the list data
        for (int i = 0; i < sequence.Count; i++)
        {
            byte aux = Convert.ToBoolean(sequence[i]) ? (byte)0x01 : (byte)0x00;

            // Newly-allocated space has no existing object, so the last param is false 
            Marshal.StructureToPtr(aux, ptr + sizeof(int) + (elSiz * i), false);
        }
    }

    public static string PtrToWideString(this IntPtr ptr)
    {
#if Windows
        int length = 0;
        while (Marshal.ReadInt16(ptr, length) != 0)
        {
            length += 2;
        }
        byte[] buffer = new byte[length];
        Marshal.Copy(ptr, buffer, 0, buffer.Length);

        return Encoding.Unicode.GetString(buffer);        
#else
        int length = 0;
        while (Marshal.ReadInt32(ptr, length) != 0)
        {
            length += 4;
        }
        byte[] buffer = new byte[length];
        Marshal.Copy(ptr, buffer, 0, buffer.Length);

        return Encoding.UTF32.GetString(buffer);
#endif
    }

    public static IntPtr WideStringToPtr(this string str)
    {
#if Windows
        var utfBytes = Encoding.Unicode.GetBytes(str);

        byte[] bytes = new byte[utfBytes.Length + 2];
        Array.Copy(utfBytes, bytes, utfBytes.Length);  
#else
        var utfBytes = Encoding.UTF32.GetBytes(str);

        byte[] bytes = new byte[utfBytes.Length + 4];
        Array.Copy(utfBytes, bytes, utfBytes.Length);
#endif

        IntPtr unmanagedPointer = Marshal.AllocHGlobal(bytes.Length);
        Marshal.Copy(bytes, 0, unmanagedPointer, bytes.Length);

        return unmanagedPointer;
    }

    public static void PtrToStringSequence(this IntPtr ptr, ref IList<string> sequence, bool isUnicode, int capacity = 0)
    {
        // Ensure a not null empty list to populate
        if (sequence == null)
        {
            if (capacity > 0)
                sequence = new List<string>(capacity);
            else
                sequence = new List<string>();
        }
        else
        {
            sequence.Clear();
        }

        if (ptr == IntPtr.Zero)
            return;

        // Start by reading the size of the array
        int length = Marshal.ReadInt32(ptr);

        // Populate the array
        for (int i = 0; i < length; i++)
        {
            // Get the unmanaged pointer
            IntPtr pointer = Marshal.PtrToStructure<IntPtr>(ptr + sizeof(int) + (IntPtr.Size * i));

            // Convert the pointer in a string
            if (isUnicode)
                sequence.Add(PtrToWideString(pointer));
            else
                sequence.Add(Marshal.PtrToStringAnsi(pointer));
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
                sPtr = WideStringToPtr(sequence[i]);
            else
                sPtr = Marshal.StringToHGlobalAnsi(sequence[i]);

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
            return;

        int elSiz = Marshal.SizeOf<T>();
        int length = 1;
        for (int i = 0; i < array.Rank; i++)
        {
            length *= array.GetLength(i);
        }

        int[] dimensions = new int[array.Rank];
        //int[] dIndex = new int[array.Rank];
        for (int i = 0; i < length; i++)
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

        int elSiz = Marshal.SizeOf<T>();
        // Get the total size of unmanaged memory that is needed
        int size = elSiz * array.Length;
        // Allocate unmanaged space.
        ptr = Marshal.AllocHGlobal(size);

        System.Collections.IEnumerator enumerator = array.GetEnumerator();
        int i = 0;
        while (enumerator.MoveNext())
        {
            if (enumerator.Current is char && elSiz == 1)
            {
                byte aux = Convert.ToByte(enumerator.Current);
                Marshal.StructureToPtr(aux, ptr + (elSiz * i), false);
            }
            else if (enumerator.Current is char && elSiz == 4)
            {
                int aux = char.ConvertToUtf32(enumerator.Current.ToString(), 0);
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
            return;

        int elSiz = Marshal.SizeOf(Enum.GetUnderlyingType(typeof(T)));
        int length = 1;
        for (int i = 0; i < array.Rank; i++)
        {
            length *= array.GetLength(i);
        }

        int[] dimensions = new int[array.Rank];
        for (int i = 0; i < length; i++)
        {
            if (i > 0)
            {
                UpdateDimensionsArray(array, dimensions);
            }

            int aux = Marshal.PtrToStructure<int>(ptr + (elSiz * i));
            array.SetValue((T)Enum.ToObject(typeof(T), aux), dimensions);
        }
    }

    public static void EnumMultiArrayToPtr<T>(this Array array, ref IntPtr ptr) where T : Enum
    {
        if (array == null || array.Length == 0)
        {
            return;
        }

        int elSiz = Marshal.SizeOf(Enum.GetUnderlyingType(typeof(T)));
        // Get the total size of unmanaged memory that is needed
        int size = elSiz * array.Length;
        // Allocate unmanaged space.
        ptr = Marshal.AllocHGlobal(size);

        System.Collections.IEnumerator enumerator = array.GetEnumerator();
        int i = 0;
        while (enumerator.MoveNext())
        {
            // IDL only accept integer enumerations
            int value = Convert.ToInt32(enumerator.Current);
            // Newly-allocated space has no existing object, so the last param is false 
            Marshal.StructureToPtr(value, ptr + (elSiz * i), false);

            i++;
        }
    }

    public static void PtrToBooleanMultiArray(this IntPtr ptr, Array array)
    {
        // We need to ensure that the array is not null before the call 
        if (array == null)
            return;

        int length = 1;
        for (int i = 0; i < array.Rank; i++)
        {
            length *= array.GetLength(i);
        }

        int[] dimensions = new int[array.Rank];
        int[] dIndex = new int[array.Rank];
        for (int i = 0; i < length; i++)
        {
            if (i > 0)
            {
                UpdateDimensionsArray(array, dimensions);
            }

            byte aux = Marshal.PtrToStructure<byte>(ptr + i);
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
        int size = array.Length;
        // Allocate unmanaged space.
        ptr = Marshal.AllocHGlobal(size);

        System.Collections.IEnumerator enumerator = array.GetEnumerator();
        int i = 0;
        while (enumerator.MoveNext())
        {
            byte aux = (bool)enumerator.Current ? (byte)1 : (byte)0;
            Marshal.StructureToPtr(aux, ptr + i, false);
            i++;
        }
    }

    public static void PtrToWCharMultiArray(this IntPtr ptr, Array array)
    {
        // We need to ensure that the array is not null before the call 
        if (array == null)
            return;

        int length = 1;
        for (int i = 0; i < array.Rank; i++)
        {
            length *= array.GetLength(i);
        }

#if Windows
        int elSiz = Marshal.SizeOf<char>();
#else
        int elSiz = Marshal.SizeOf<int>();
#endif

        int[] dimensions = new int[array.Rank];
        for (int i = 0; i < length; i++)
        {
            if (i > 0)
            {
                UpdateDimensionsArray(array, dimensions);
            }

#if Windows
            char value = Marshal.PtrToStructure<char>(ptr + (elSiz * i));
#else
            int aux = Marshal.PtrToStructure<int>(ptr + (elSiz * i));
            char value = ConvertFromUtf32(aux);
#endif
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

        int elSiz = IntPtr.Size;
        // Get the total size of unmanaged memory that is needed
        int size = elSiz * array.Length;
        // Allocate unmanaged space.
        ptr = Marshal.AllocHGlobal(size);

        System.Collections.IEnumerator enumerator = array.GetEnumerator();
        int i = 0;
        while (enumerator.MoveNext())
        {
            // Create a pointer to the string in unmanaged memory
            IntPtr sPtr;
            if (isUnicode)
                sPtr = WideStringToPtr((string)enumerator.Current);
            else
                sPtr = Marshal.StringToHGlobalAnsi((string)enumerator.Current);

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
            return;

        int elSiz = IntPtr.Size;
        int length = 1;
        for (int i = 0; i < array.Rank; i++)
        {
            length *= array.GetLength(i);
        }

        int[] dimensions = new int[array.Rank];
        for (int i = 0; i < length; i++)
        {
            if (i > 0)
            {
                UpdateDimensionsArray(array, dimensions);
            }

            // Get the unmanaged pointer
            IntPtr pointer = Marshal.PtrToStructure<IntPtr>(ptr + (IntPtr.Size * i));
            // Convert the pointer in a string
            if (isUnicode)
                array.SetValue(PtrToWideString(pointer), dimensions);
            else
                array.SetValue(Marshal.PtrToStringAnsi(pointer), dimensions);
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

    public static char ConvertFromUtf32(int codepoint)
    {
        bool isValidUnicodeCharacter = (codepoint >= 0x00000 && codepoint <= 0x10FFFF) &&
                    (codepoint < 0xD800 || codepoint > 0xDFFF);

        if (!isValidUnicodeCharacter)
        {
            return '\0';
        }

        return char.ConvertFromUtf32(codepoint).FirstOrDefault();
    }

    internal static void UpdateDimensionsArray(this Array array, int[] dimensions)
    {
        dimensions[array.Rank - 1]++;
        if (dimensions[array.Rank - 1] >= array.GetLength(array.Rank - 1))
        {
            for (int j = array.Rank - 1; j > 0; j--)
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