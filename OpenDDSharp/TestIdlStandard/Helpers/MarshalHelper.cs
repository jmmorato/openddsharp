﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public static class MarshalHelper
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
                sequence.Add(Marshal.PtrToStringUni(pointer));
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
            IntPtr sPtr = IntPtr.Zero;
            if (isUnicode)
                sPtr = Marshal.StringToHGlobalUni(sequence[i]);
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
        int[] dIndex = new int[array.Rank];
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
        int[] dIndex = new int[array.Rank];
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
            array.SetValue(aux == 1 ? true : false, dimensions);
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
            IntPtr sPtr = IntPtr.Zero;
            if (isUnicode)
                sPtr = Marshal.StringToHGlobalUni((string)enumerator.Current);
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
        int[] dIndex = new int[array.Rank];
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
                array.SetValue(Marshal.PtrToStringUni(pointer), dimensions);
            else
                array.SetValue(Marshal.PtrToStringAnsi(pointer), dimensions);
        }
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