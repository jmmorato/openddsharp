using System;
using System.Linq;
using System.Security;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OpenDDSharp.DDS;

namespace Test
{
    internal static class Helper
    {
#if DEBUG
        internal const string API_DLL_X64 = @"TestPInvokeIdlWrapperd.x64.dll";
        internal const string API_DLL_X86 = @"TestPInvokeIdlWrapperd.x86.dll";
#else
        internal const string API_DLL_X64 = @"TestPInvokeIdlWrapper.x64.dll";
        internal const string API_DLL_X86 = @"TestPInvokeIdlWrapper.x86.dll";
#endif 

        public static void PtrToSequence<T>(IntPtr ptr, ref IList<T> sequence, int capacity = 0)
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

        public static void SequenceToPtr<T>(IList<T> sequence, ref IntPtr ptr)
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

        public static void PtrToEnumSequence<T>(IntPtr ptr, ref IList<T> sequence, int capacity = 0) where T : Enum
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

        public static void EnumSequenceToPtr<T>(IList<T> sequence, ref IntPtr ptr) where T : Enum
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

        public static void PtrToBooleanSequence(IntPtr ptr, ref IList<bool> sequence, int capacity = 0)
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

        public static void BooleanSequenceToPtr(IList<bool> sequence, ref IntPtr ptr)
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

        public static void PtrToStringSequence(IntPtr ptr, ref IList<string> sequence, bool isUnicode, int capacity = 0)
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

        public static List<IntPtr> StringSequenceToPtr(IList<string> sequence, ref IntPtr ptr, bool isUnicode)
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

        public static void PtrToMultiArray<T>(IntPtr ptr, Array array)
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

        public static void MultiArrayToPtr<T>(Array array, ref IntPtr ptr)
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

        public static void PtrToEnumMultiArray<T>(IntPtr ptr, Array array) where T : Enum
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

        public static void EnumMultiArrayToPtr<T>(Array array, ref IntPtr ptr) where T : Enum
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

        public static void PtrToBooleanMultiArray(IntPtr ptr, Array array)
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

        public static void BooleanMultiArrayToPtr(Array array, ref IntPtr ptr)
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
                Marshal.StructureToPtr(aux, ptr +  i, false);
                i++;
            }
        }

        public static List<IntPtr> StringMultiArrayToPtr(Array array, ref IntPtr ptr, bool isUnicode)
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

        public static void PtrToStringMultiArray(IntPtr ptr, Array array, bool isUnicode)
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

        internal static void UpdateDimensionsArray(Array array, int[] dimensions)
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

    public enum PrimitiveEnum
    {
        ENUM1,
        ENUM2,
        ENUM3,
        ENUM4,
        ENUM5,
        ENUM6,
        ENUM7,
        ENUM8,
        ENUM9,
        ENUM10,
        ENUM11,
        ENUM12
    };

    public class NestedTestStruct
    {
        public int Id { get; set; }

        public string Message { get; set; }

        internal NestedTestStructWrapper ToNative(List<IntPtr> toRelease)
        {
            NestedTestStructWrapper wrapper = new NestedTestStructWrapper();

            wrapper.Id = Id;

            if (Message != null)
            {
                wrapper.Message = Marshal.StringToHGlobalAnsi(Message);
                toRelease.Add(wrapper.Message);
            }

            return wrapper;
        }

        internal void FromNative(NestedTestStructWrapper wrapper)
        {
            Id = wrapper.Id;
            Message = Marshal.PtrToStringAnsi(wrapper.Message);            
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct NestedTestStructWrapper
    {        
        public int Id;
               
        public IntPtr Message;
    }

    public class BasicTestStruct
    {
        #region Fields
        private IList<int> _longSequence;
        private IList<string> _stringSequence;
        private IList<string> _wstringSequence;
        private IList<float> _floatSequence;
        private IList<double> _doubleSequence;
        private IList<double> _longDoubleSequence;
        private IList<char> _charSequence;
        private IList<char> _wcharSequence;
        private IList<short> _shortSequence;
        private IList<long> _longlongSequence;
        private IList<ushort> _ushortSequence;
        private IList<uint> _ulongSequence;
        private IList<ulong> _ulonglongSequence;
        private IList<bool> _booleanSequence;
        private IList<byte> _octetSequence;
        private IList<PrimitiveEnum> _enumSequence;
        private IList<int> _longBoundedSequence;
        private IList<string> _stringBoundedSequence;
        private IList<string> _wstringBoundedSequence;
        private IList<double> _longDoubleBoundedSequence;
        private IList<bool> _booleanBoundedSequence;
        private IList<PrimitiveEnum> _enumBoundedSequence;
        #endregion

        #region Properties
        public int Id { get; set; }
        
        public string Message { get; set; }

        public string WMessage { get; set; }

        public IList<int> LongSequence
        {
            get { return _longSequence; }
            set { _longSequence = value; }
        }

        public IList<string> StringSequence
        {
            get { return _stringSequence; }
            set { _stringSequence = value; }
        }

        public IList<string> WStringSequence
        {
            get { return _wstringSequence; }
            set { _wstringSequence = value; }
        }

        public int[] LongArray { get; set; }

        public string[] StringArray { get; set; }

        public string[] WStringArray { get; set; }

        public NestedTestStruct StructTest { get; set; }

        public IList<NestedTestStruct> StructSequence { get; set; }

        public NestedTestStruct[] StructArray { get; set; }

        public int[,,] LongMultiArray { get; set; }

        public string[,,] StringMultiArray { get; set; }

        public string[,,] WStringMultiArray { get; set; }

        public NestedTestStruct[,,] StructMultiArray { get; set; }

        public float FloatType { get; set; }

        public double DoubleType { get; set; }

        public double LongDoubleType { get; set; }

        public float[] FloatArray { get; set; }

        public double[] DoubleArray { get; set; }

        public double[] LongDoubleArray { get; set; }

        public IList<float> FloatSequence
        {
            get { return _floatSequence; }
            set { _floatSequence = value; }
        }

        public IList<double> DoubleSequence
        {
            get { return _doubleSequence; }
            set { _doubleSequence = value; }
        }

        public IList<double> LongDoubleSequence
        {
            get { return _longDoubleSequence; }
            set { _longDoubleSequence = value; }
        }

        public float[,,] FloatMultiArray { get; set; }

        public double[,,] DoubleMultiArray { get; set; }

        public double[,,] LongDoubleMultiArray { get; set; }

        public char CharType { get; set; }

        public char WCharType { get; set; }

        public char[] CharArray { get; set; }

        public char[] WCharArray { get; set; }

        public IList<char> CharSequence
        {
            get { return _charSequence; }
            set { _charSequence = value; }
        }

        public IList<char> WCharSequence
        {
            get { return _wcharSequence; }
            set { _wcharSequence = value; }
        }

        public char[,,] CharMultiArray { get; set; }

        public char[,,] WCharMultiArray { get; set; }

        public short ShortType { get; set; }

        public long LongLongType { get; set; }

        public ushort UnsignedShortType { get; set; }

        public uint UnsignedLongType { get; set; }

        public ulong UnsignedLongLongType { get; set; }

        public short[] ShortArray { get; set; }

        public long[] LongLongArray { get; set; }

        public ushort[] UnsignedShortArray { get; set; }

        public uint[] UnsignedLongArray { get; set; }

        public ulong[] UnsignedLongLongArray { get; set; }

        public IList<short> ShortSequence
        {
            get { return _shortSequence; }
            set { _shortSequence = value; }
        }

        public IList<long> LongLongSequence
        {
            get { return _longlongSequence; }
            set { _longlongSequence = value; }
        }

        public IList<ushort> UnsignedShortSequence
        {
            get { return _ushortSequence; }
            set { _ushortSequence = value; }
        }

        public IList<uint> UnsignedLongSequence
        {
            get { return _ulongSequence; }
            set { _ulongSequence = value; }
        }

        public IList<ulong> UnsignedLongLongSequence
        {
            get { return _ulonglongSequence; }
            set { _ulonglongSequence = value; }
        }

        public short[,,] ShortMultiArray { get; set; }

        public long[,,] LongLongMultiArray { get; set; }

        public ushort[,,] UnsignedShortMultiArray { get; set; }

        public uint[,,] UnsignedLongMultiArray { get; set; }

        public ulong[,,] UnsignedLongLongMultiArray { get; set; }

        public bool BooleanType { get; set; }

        public byte OctetType { get; set; }

        public bool[] BooleanArray { get; set; }

        public byte[] OctetArray { get; set; }

        public IList<bool> BooleanSequence
        {
            get { return _booleanSequence; }
            set { _booleanSequence = value; }
        }

        public IList<byte> OctetSequence
        {
            get { return _octetSequence; }
            set { _octetSequence = value; }
        }

        public bool[,,] BooleanMultiArray { get; set; }

        public byte[,,] OctetMultiArray { get; set; }

        public PrimitiveEnum TestEnum { get; set; }

        public PrimitiveEnum[] EnumArray { get; set; }

        public IList<PrimitiveEnum> EnumSequence
        {
            get { return _enumSequence; }
            set { _enumSequence = value; }
        }

        public PrimitiveEnum[,,] EnumMultiArray { get; set; }

        public IList<int> LongBoundedSequence
        {
            get { return _longBoundedSequence; }
            set { _longBoundedSequence = value; }
        }

        public IList<string> StringBoundedSequence
        {
            get { return _stringBoundedSequence; }
            set { _stringBoundedSequence = value; }
        }

        public IList<string> WStringBoundedSequence
        {
            get { return _wstringBoundedSequence; }
            set { _wstringBoundedSequence = value; }
        }

        public IList<NestedTestStruct> StructBoundedSequence { get; set; }

        public IList<double> LongDoubleBoundedSequence
        {
            get { return _longDoubleBoundedSequence; }
            set { _longDoubleBoundedSequence = value; }
        }

        public IList<bool> BooleanBoundedSequence
        {
            get { return _booleanBoundedSequence; }
            set { _booleanBoundedSequence = value; }
        }

        public IList<PrimitiveEnum> EnumBoundedSequence
        {
            get { return _enumBoundedSequence; }
            set { _enumBoundedSequence = value; }
        }
        #endregion

        #region Constructors
        public BasicTestStruct()
        {
            _longSequence = new List<int>();
            _stringSequence = new List<string>();
            _wstringSequence = new List<string>();
            LongArray = new int[5];
            StringArray = new string[10];
            WStringArray = new string[4];
            StructTest = new NestedTestStruct();
            StructSequence = new List<NestedTestStruct>();
            StructArray = new NestedTestStruct[5];
            LongMultiArray = new int[3, 4, 2];
            StringMultiArray = new string[3, 4, 2];
            StructMultiArray = new NestedTestStruct[3, 4, 2];
            FloatArray = new float[5];
            DoubleArray = new double[5];
            LongDoubleArray = new double[5];
            _floatSequence = new List<float>();
            _doubleSequence = new List<double>();
            _longDoubleSequence = new List<double>();
            FloatMultiArray = new float[3, 4, 2];
            DoubleMultiArray = new double[3, 4, 2];
            LongDoubleMultiArray = new double[3, 4, 2];
            CharArray = new char[5];
            WCharArray = new char[5];
            CharSequence = new List<char>();
            WCharSequence = new List<char>();
            CharMultiArray = new char[3, 4, 2];
            WCharMultiArray = new char[3, 4, 2];
            ShortArray = new short[5];
            LongLongArray = new long[5];
            UnsignedShortArray = new ushort[5];
            UnsignedLongArray = new uint[5];
            UnsignedLongLongArray = new ulong[5];
            _shortSequence = new List<short>();
            _longlongSequence = new List<long>();
            _ushortSequence = new List<ushort>();
            _ulongSequence = new List<uint>();
            _ulonglongSequence = new List<ulong>();
            ShortMultiArray = new short[3, 4, 2];
            LongLongMultiArray = new long[3, 4, 2];
            UnsignedShortMultiArray = new ushort[3, 4, 2];
            UnsignedLongMultiArray = new uint[3, 4, 2];
            UnsignedLongLongMultiArray = new ulong[3, 4, 2];
            BooleanArray = new bool[5];
            OctetArray = new byte[5];
            _booleanSequence = new List<bool>();
            _octetSequence = new List<byte>();
            BooleanMultiArray = new bool[3, 4, 2];
            OctetMultiArray = new byte[3, 4, 2];
            EnumArray = new PrimitiveEnum[5];
            _enumSequence = new List<PrimitiveEnum>();
            EnumMultiArray = new PrimitiveEnum[3, 4, 2];
            _longBoundedSequence = new List<int>(5);
            _stringBoundedSequence = new List<string>(5);
            _wstringBoundedSequence = new List<string>(5);
            StructBoundedSequence = new List<NestedTestStruct>(5);
            _longDoubleBoundedSequence = new List<double>(5);
            _booleanBoundedSequence = new List<bool>(5);
            _enumBoundedSequence = new List<PrimitiveEnum>(5);
        }
        #endregion

        #region Methods
        internal BasicTestStructWrapper ToNative(List<IntPtr> toRelease)
        {
            BasicTestStructWrapper wrapper = new BasicTestStructWrapper();

            wrapper.Id = Id;

            if (Message != null)
            {
                wrapper.Message = Marshal.StringToHGlobalAnsi(Message);
                toRelease.Add(wrapper.Message);
            }

            if (WMessage != null)
            {
                wrapper.WMessage = Marshal.StringToHGlobalUni(WMessage);
                toRelease.Add(wrapper.WMessage);
            }

            Helper.SequenceToPtr(LongSequence, ref wrapper.LongSequence);
            toRelease.Add(wrapper.LongSequence);

            toRelease.AddRange(Helper.StringSequenceToPtr(StringSequence, ref wrapper.StringSequence, false));
            toRelease.Add(wrapper.StringSequence);

            toRelease.AddRange(Helper.StringSequenceToPtr(WStringSequence, ref wrapper.WStringSequence, true));
            toRelease.Add(wrapper.WStringSequence);

            wrapper.LongArray = LongArray;

            if (StringArray != null)
            {
                wrapper.StringArray = new IntPtr[10];
                for (int i = 0; i < 10; i++)
                {
                    if (StringArray[i] != null)
                    {
                        wrapper.StringArray[i] = Marshal.StringToHGlobalAnsi(StringArray[i]);
                        toRelease.Add(wrapper.StringArray[i]);
                    }
                }
            }

            if (WStringArray != null)
            {
                wrapper.WStringArray = new IntPtr[4];
                for (int i = 0; i < 4; i++)
                {
                    if (WStringArray[i] != null)
                    {
                        wrapper.WStringArray[i] = Marshal.StringToHGlobalUni(WStringArray[i]);
                        toRelease.Add(wrapper.WStringArray[i]);
                    }
                }
            }

            if (StructTest != null)
            {
                wrapper.StructTest = StructTest.ToNative(toRelease);
            }

            // We need to use the wrapper struct to marshal the pointer
            if (StructSequence != null)
            {
                List<NestedTestStructWrapper> aux = new List<NestedTestStructWrapper>();
                foreach (NestedTestStruct s in StructSequence)
                {
                    aux.Add(s.ToNative(toRelease));
                }
                Helper.SequenceToPtr(aux, ref wrapper.StructSequence);
                toRelease.Add(wrapper.StructSequence);
            }

            if (StructArray != null)
            {
                wrapper.StructArray = new NestedTestStructWrapper[5];
                for (int i = 0; i < 5; i++)
                {
                    if (StructArray[i] != null)
                    {
                        wrapper.StructArray[i] = StructArray[i].ToNative(toRelease);
                    }
                }
            }

            if (LongMultiArray != null)
            {
                Helper.MultiArrayToPtr<int>(LongMultiArray, ref wrapper.LongMultiArray);
                toRelease.Add(wrapper.LongMultiArray);
            }

            // Multi-dimensional array of strings
            if (StringMultiArray != null)
            {
                toRelease.AddRange(Helper.StringMultiArrayToPtr(StringMultiArray, ref wrapper.StringMultiArray, false));
                toRelease.Add(wrapper.StringMultiArray);
            }

            // Multi-dimensional array of wstrings
            if (WStringMultiArray != null)
            {
                toRelease.AddRange(Helper.StringMultiArrayToPtr(WStringMultiArray, ref wrapper.WStringMultiArray, true));
                toRelease.Add(wrapper.WStringMultiArray);
            }

            // Multi-dimensional array of structs
            if (StructMultiArray != null)
            {
                NestedTestStructWrapper[] aux = new NestedTestStructWrapper[24];
                int i = 0;
                foreach (NestedTestStruct s in StructMultiArray)
                {
                    if (s != null)
                        aux[i] = s.ToNative(toRelease);

                    i++;
                }

                Helper.MultiArrayToPtr<NestedTestStructWrapper>(aux, ref wrapper.StructMultiArray);
                toRelease.Add(wrapper.StructMultiArray);
            }

            wrapper.FloatType = FloatType;
            wrapper.DoubleType = DoubleType;
            wrapper.LongDoubleType = LongDoubleType;

            wrapper.FloatArray = FloatArray;
            wrapper.DoubleArray = DoubleArray;
            wrapper.LongDoubleArray = LongDoubleArray;

            Helper.SequenceToPtr(FloatSequence, ref wrapper.FloatSequence);
            toRelease.Add(wrapper.FloatSequence);

            Helper.SequenceToPtr(DoubleSequence, ref wrapper.DoubleSequence);
            toRelease.Add(wrapper.DoubleSequence);

            Helper.SequenceToPtr(LongDoubleSequence, ref wrapper.LongDoubleSequence);
            toRelease.Add(wrapper.LongDoubleSequence);

            if (FloatMultiArray != null)
            {
                Helper.MultiArrayToPtr<float>(FloatMultiArray, ref wrapper.FloatMultiArray);
                toRelease.Add(wrapper.FloatMultiArray);
            }

            if (DoubleMultiArray != null)
            {
                Helper.MultiArrayToPtr<double>(DoubleMultiArray, ref wrapper.DoubleMultiArray);
                toRelease.Add(wrapper.DoubleMultiArray);
            }

            if (LongDoubleMultiArray != null)
            {
                Helper.MultiArrayToPtr<double>(LongDoubleMultiArray, ref wrapper.LongDoubleMultiArray);
                toRelease.Add(wrapper.LongDoubleMultiArray);
            }

            // Char types
            wrapper.CharType = CharType;
            wrapper.WCharType = WCharType;

            wrapper.CharArray = CharArray;
            wrapper.WCharArray = WCharArray;

            if (CharSequence != null)
            {
                IList<byte> aux = System.Text.Encoding.ASCII.GetBytes(CharSequence.ToArray()).ToList();
                Helper.SequenceToPtr(aux, ref wrapper.CharSequence);
                toRelease.Add(wrapper.CharSequence);
            }

            if (WCharSequence != null)
            {
                Helper.SequenceToPtr(WCharSequence, ref wrapper.WCharSequence);
                toRelease.Add(wrapper.WCharSequence);
            }

            if (CharMultiArray != null)
            {
                Helper.MultiArrayToPtr<byte>(CharMultiArray, ref wrapper.CharMultiArray);
                toRelease.Add(wrapper.CharMultiArray);
            }

            if (WCharMultiArray != null)
            {
                Helper.MultiArrayToPtr<char>(WCharMultiArray, ref wrapper.WCharMultiArray);
                toRelease.Add(wrapper.WCharMultiArray);
            }

            // Integer types
            wrapper.ShortType = ShortType;
            wrapper.LongLongType = LongLongType;
            wrapper.UnsignedShortType = UnsignedShortType;
            wrapper.UnsignedLongType = UnsignedLongType;
            wrapper.UnsignedLongLongType = UnsignedLongLongType;

            wrapper.ShortArray = ShortArray;
            wrapper.LongLongArray = LongLongArray;
            wrapper.UnsignedShortArray = UnsignedShortArray;
            wrapper.UnsignedLongArray = UnsignedLongArray;
            wrapper.UnsignedLongLongArray = UnsignedLongLongArray;

            Helper.SequenceToPtr(ShortSequence, ref wrapper.ShortSequence);
            toRelease.Add(wrapper.ShortSequence);

            Helper.SequenceToPtr(LongLongSequence, ref wrapper.LongLongSequence);
            toRelease.Add(wrapper.LongLongSequence);

            Helper.SequenceToPtr(UnsignedShortSequence, ref wrapper.UnsignedShortSequence);
            toRelease.Add(wrapper.UnsignedShortSequence);

            Helper.SequenceToPtr(UnsignedLongSequence, ref wrapper.UnsignedLongSequence);
            toRelease.Add(wrapper.UnsignedLongSequence);

            Helper.SequenceToPtr(UnsignedLongLongSequence, ref wrapper.UnsignedLongLongSequence);
            toRelease.Add(wrapper.UnsignedLongLongSequence);

            if (ShortMultiArray != null)
            {
                Helper.MultiArrayToPtr<short>(ShortMultiArray, ref wrapper.ShortMultiArray);
                toRelease.Add(wrapper.ShortMultiArray);
            }

            if (LongLongMultiArray != null)
            {
                Helper.MultiArrayToPtr<long>(LongLongMultiArray, ref wrapper.LongLongMultiArray);
                toRelease.Add(wrapper.LongLongMultiArray);
            }

            if (UnsignedShortMultiArray != null)
            {
                Helper.MultiArrayToPtr<ushort>(UnsignedShortMultiArray, ref wrapper.UnsignedShortMultiArray);
                toRelease.Add(wrapper.UnsignedShortMultiArray);
            }

            if (UnsignedLongMultiArray != null)
            {
                Helper.MultiArrayToPtr<uint>(UnsignedLongMultiArray, ref wrapper.UnsignedLongMultiArray);
                toRelease.Add(wrapper.UnsignedLongMultiArray);
            }

            if (UnsignedLongLongMultiArray != null)
            {
                Helper.MultiArrayToPtr<ulong>(UnsignedLongLongMultiArray, ref wrapper.UnsignedLongLongMultiArray);
                toRelease.Add(wrapper.UnsignedLongLongMultiArray);
            }

            // Other primitive types
            wrapper.BooleanType = BooleanType;
            wrapper.OctetType = OctetType;

            wrapper.BooleanArray = BooleanArray;
            wrapper.OctetArray = OctetArray;

            Helper.BooleanSequenceToPtr(BooleanSequence, ref wrapper.BooleanSequence);
            toRelease.Add(wrapper.BooleanSequence);

            Helper.SequenceToPtr(OctetSequence, ref wrapper.OctetSequence);
            toRelease.Add(wrapper.OctetSequence);

            if (BooleanMultiArray != null)
            {
                Helper.BooleanMultiArrayToPtr(BooleanMultiArray, ref wrapper.BooleanMultiArray);
                toRelease.Add(wrapper.BooleanMultiArray);
            }

            if (OctetMultiArray != null)
            {
                Helper.MultiArrayToPtr<byte>(OctetMultiArray, ref wrapper.OctetMultiArray);
                toRelease.Add(wrapper.OctetMultiArray);
            }

            // Enumerations
            wrapper.TestEnum = TestEnum;
            wrapper.EnumArray = EnumArray;

            Helper.EnumSequenceToPtr(EnumSequence, ref wrapper.EnumSequence);
            toRelease.Add(wrapper.EnumSequence);

            if (EnumMultiArray != null)
            {
                Helper.EnumMultiArrayToPtr<PrimitiveEnum>(EnumMultiArray, ref wrapper.EnumMultiArray);
                toRelease.Add(wrapper.EnumMultiArray);
            }

            // Unbounded sequences
            Helper.SequenceToPtr(LongBoundedSequence, ref wrapper.LongBoundedSequence);
            toRelease.Add(wrapper.LongBoundedSequence);

            toRelease.AddRange(Helper.StringSequenceToPtr(StringBoundedSequence, ref wrapper.StringBoundedSequence, false));
            toRelease.Add(wrapper.StringBoundedSequence);

            toRelease.AddRange(Helper.StringSequenceToPtr(WStringBoundedSequence, ref wrapper.WStringBoundedSequence, true));
            toRelease.Add(wrapper.WStringBoundedSequence);

            if (StructBoundedSequence != null)
            {
                List<NestedTestStructWrapper> aux = new List<NestedTestStructWrapper>(5);
                foreach (NestedTestStruct s in StructBoundedSequence)
                {
                    aux.Add(s.ToNative(toRelease));
                }
                Helper.SequenceToPtr(aux, ref wrapper.StructBoundedSequence);
                toRelease.Add(wrapper.StructBoundedSequence);
            }

            Helper.SequenceToPtr(LongDoubleBoundedSequence, ref wrapper.LongDoubleBoundedSequence);
            toRelease.Add(wrapper.LongDoubleBoundedSequence);

            Helper.BooleanSequenceToPtr(BooleanBoundedSequence, ref wrapper.BooleanBoundedSequence);
            toRelease.Add(wrapper.BooleanBoundedSequence);

            Helper.EnumSequenceToPtr(EnumBoundedSequence, ref wrapper.EnumBoundedSequence);
            toRelease.Add(wrapper.EnumBoundedSequence);

            return wrapper;
        }

        internal void FromNative(BasicTestStructWrapper wrapper)
        {
            Id = wrapper.Id;

            if (wrapper.Message != IntPtr.Zero)
            {
                Message = Marshal.PtrToStringAnsi(wrapper.Message);
            }
            else
            {
                Message = null;
            }

            if (wrapper.WMessage != IntPtr.Zero)
            {
                WMessage = Marshal.PtrToStringUni(wrapper.WMessage);
            }
            else
            {
                WMessage = null;
            }

            Helper.PtrToSequence(wrapper.LongSequence, ref _longSequence);
            Helper.PtrToStringSequence(wrapper.StringSequence, ref _stringSequence, false);
            Helper.PtrToStringSequence(wrapper.WStringSequence, ref _wstringSequence, true);

            LongArray = wrapper.LongArray;

            for (int i = 0; i < 10; i++)
            {
                if (wrapper.StringArray[i] != null)
                {
                    StringArray[i] = Marshal.PtrToStringAnsi(wrapper.StringArray[i]);
                }
            }

            for (int i = 0; i < 4; i++)
            {
                if (wrapper.WStringArray[i] != null)
                {
                    WStringArray[i] = Marshal.PtrToStringUni(wrapper.WStringArray[i]);
                }
            }

            StructTest.FromNative(wrapper.StructTest);

            // We need to use the wrapper struct to receive the structures
            // In the generated code, the aux variable will be suffixed with the field name
            if (wrapper.StructSequence != IntPtr.Zero)
            {
                IList<NestedTestStructWrapper> aux = new List<NestedTestStructWrapper>();
                Helper.PtrToSequence(wrapper.StructSequence, ref aux);
                foreach (NestedTestStructWrapper native in aux)
                {
                    NestedTestStruct s = new NestedTestStruct();
                    s.FromNative(native);
                    StructSequence.Add(s);
                }
            }

            for (int i = 0; i < 5; i++)
            {
                StructArray[i] = new NestedTestStruct();
                StructArray[i].FromNative(wrapper.StructArray[i]);                
            }

            // Multi-dimensional array of primitives
            if (LongMultiArray == null)
            {
                LongMultiArray = new int[3, 4, 2];
            }
            Helper.PtrToMultiArray<int>(wrapper.LongMultiArray, LongMultiArray);

            // Multi-dimensional array of strings
            if (StringMultiArray == null)
            {
                StringMultiArray = new string[3, 4, 2];
            }
            Helper.PtrToStringMultiArray(wrapper.StringMultiArray, StringMultiArray, false);

            // Multi-dimensional array of wstrings
            if (WStringMultiArray == null)
            {
                WStringMultiArray = new string[3, 4, 2];
            }
            Helper.PtrToStringMultiArray(wrapper.WStringMultiArray, WStringMultiArray, true);

            // Multi-dimensional array of structs
            if (wrapper.StructMultiArray != null)
            {
                if (StructMultiArray == null)
                {
                    StructMultiArray = new NestedTestStruct[3, 4, 2];
                }

                NestedTestStructWrapper[] aux_StructMultiArray = new NestedTestStructWrapper[24];
                Helper.PtrToMultiArray<NestedTestStructWrapper>(wrapper.StructMultiArray, aux_StructMultiArray);
                int[] dimensions = new int[StructMultiArray.Rank];
                for (int i = 0; i < 24; i++)
                {
                    if (i > 0)
                    {
                        Helper.UpdateDimensionsArray(StructMultiArray, dimensions);
                    }

                    NestedTestStruct aux = new NestedTestStruct();
                    aux.FromNative(aux_StructMultiArray[i]);
                    StructMultiArray.SetValue(aux, dimensions);
                }
            }

            FloatType = wrapper.FloatType;
            DoubleType = wrapper.DoubleType;
            LongDoubleType = wrapper.LongDoubleType;

            FloatArray = wrapper.FloatArray;
            DoubleArray = wrapper.DoubleArray;
            LongDoubleArray = wrapper.LongDoubleArray;

            Helper.PtrToSequence(wrapper.FloatSequence, ref _floatSequence);
            Helper.PtrToSequence(wrapper.DoubleSequence, ref _doubleSequence);
            Helper.PtrToSequence(wrapper.LongDoubleSequence, ref _longDoubleSequence);

            if (FloatMultiArray == null)
            {
                FloatMultiArray = new float[3, 4, 2];
            }
            Helper.PtrToMultiArray<float>(wrapper.FloatMultiArray, FloatMultiArray);

            if (DoubleMultiArray == null)
            {
                DoubleMultiArray = new double[3, 4, 2];
            }
            Helper.PtrToMultiArray<double>(wrapper.DoubleMultiArray, DoubleMultiArray);

            if (LongDoubleMultiArray == null)
            {
                LongDoubleMultiArray = new double[3, 4, 2];
            }
            Helper.PtrToMultiArray<double>(wrapper.LongDoubleMultiArray, LongDoubleMultiArray);

            // Char types
            CharType = wrapper.CharType;
            WCharType = wrapper.WCharType;

            CharArray = wrapper.CharArray;
            WCharArray = wrapper.WCharArray;

            Helper.PtrToSequence(wrapper.CharSequence, ref _charSequence);
            Helper.PtrToSequence(wrapper.WCharSequence, ref _wcharSequence);

            if (CharMultiArray == null)
            {
                CharMultiArray = new char[3, 4, 2];
            }
            Helper.PtrToMultiArray<byte>(wrapper.CharMultiArray, CharMultiArray);

            if (WCharMultiArray == null)
            {
                WCharMultiArray = new char[3, 4, 2];
            }
            Helper.PtrToMultiArray<char>(wrapper.WCharMultiArray, WCharMultiArray);

            // Integer types
            ShortType = wrapper.ShortType;
            LongLongType = wrapper.LongLongType;
            UnsignedShortType = wrapper.UnsignedShortType;
            UnsignedLongType = wrapper.UnsignedLongType;
            UnsignedLongLongType = wrapper.UnsignedLongLongType;

            ShortArray = wrapper.ShortArray;
            LongLongArray = wrapper.LongLongArray;
            UnsignedShortArray = wrapper.UnsignedShortArray;
            UnsignedLongArray = wrapper.UnsignedLongArray;
            UnsignedLongLongArray = wrapper.UnsignedLongLongArray;

            Helper.PtrToSequence(wrapper.ShortSequence, ref _shortSequence);
            Helper.PtrToSequence(wrapper.LongLongSequence, ref _longlongSequence);
            Helper.PtrToSequence(wrapper.UnsignedShortSequence, ref _ushortSequence);
            Helper.PtrToSequence(wrapper.UnsignedLongSequence, ref _ulongSequence);
            Helper.PtrToSequence(wrapper.UnsignedLongLongSequence, ref _ulonglongSequence);

            if (ShortMultiArray == null)
            {
                ShortMultiArray = new short[3, 4, 2];
            }
            Helper.PtrToMultiArray<short>(wrapper.ShortMultiArray, ShortMultiArray);

            if (LongLongMultiArray == null)
            {
                LongLongMultiArray = new long[3, 4, 2];
            }
            Helper.PtrToMultiArray<long>(wrapper.LongLongMultiArray, LongLongMultiArray);

            if (UnsignedShortMultiArray == null)
            {
                UnsignedShortMultiArray = new ushort[3, 4, 2];
            }
            Helper.PtrToMultiArray<ushort>(wrapper.UnsignedShortMultiArray, UnsignedShortMultiArray);

            if (UnsignedLongMultiArray == null)
            {
                UnsignedLongMultiArray = new uint[3, 4, 2];
            }
            Helper.PtrToMultiArray<uint>(wrapper.UnsignedLongMultiArray, UnsignedLongMultiArray);

            if (UnsignedLongLongMultiArray == null)
            {
                UnsignedLongLongMultiArray = new ulong[3, 4, 2];
            }
            Helper.PtrToMultiArray<ulong>(wrapper.UnsignedLongLongMultiArray, UnsignedLongLongMultiArray);

            // Other primitives types
            BooleanType = wrapper.BooleanType;
            OctetType = wrapper.OctetType;

            BooleanArray = wrapper.BooleanArray;
            OctetArray = wrapper.OctetArray;

            Helper.PtrToBooleanSequence(wrapper.BooleanSequence, ref _booleanSequence);
            Helper.PtrToSequence(wrapper.OctetSequence, ref _octetSequence);

            if (BooleanMultiArray == null)
            {
                BooleanMultiArray = new bool[3, 4, 2];
            }
            Helper.PtrToBooleanMultiArray(wrapper.BooleanMultiArray, BooleanMultiArray);

            if (OctetMultiArray == null)
            {
                OctetMultiArray = new byte[3, 4, 2];
            }
            Helper.PtrToMultiArray<byte>(wrapper.OctetMultiArray, OctetMultiArray);

            // Enumerations
            TestEnum = wrapper.TestEnum;
            EnumArray = wrapper.EnumArray;
            Helper.PtrToEnumSequence(wrapper.EnumSequence, ref _enumSequence);

            if (EnumMultiArray == null)
            {
                EnumMultiArray = new PrimitiveEnum[3, 4, 2];
            }
            Helper.PtrToEnumMultiArray<PrimitiveEnum>(wrapper.EnumMultiArray, EnumMultiArray);

            // Bounded sequences
            Helper.PtrToSequence(wrapper.LongBoundedSequence, ref _longBoundedSequence, 5);

            Helper.PtrToStringSequence(wrapper.StringBoundedSequence, ref _stringBoundedSequence, false, 5);
            Helper.PtrToStringSequence(wrapper.WStringBoundedSequence, ref _wstringBoundedSequence, true, 5);

            if (wrapper.StructBoundedSequence != IntPtr.Zero)
            {
                IList<NestedTestStructWrapper> aux = new List<NestedTestStructWrapper>(5);
                Helper.PtrToSequence(wrapper.StructBoundedSequence, ref aux, 5);
                foreach (NestedTestStructWrapper native in aux)
                {
                    NestedTestStruct s = new NestedTestStruct();
                    s.FromNative(native);
                    StructBoundedSequence.Add(s);
                }
            }

            Helper.PtrToSequence(wrapper.LongDoubleBoundedSequence, ref _longDoubleBoundedSequence, 5);

            Helper.PtrToBooleanSequence(wrapper.BooleanBoundedSequence, ref _booleanBoundedSequence, 5);

            Helper.PtrToEnumSequence(wrapper.EnumBoundedSequence, ref _enumBoundedSequence, 5);
        }
        #endregion
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct BasicTestStructWrapper
    {
        public int Id;

        // Strings need to be treated as a IntPtr to avoid memory relase issues
        public IntPtr Message;

        // WStrings need to be treated as a IntPtr to avoid memory relase issues
        public IntPtr WMessage;

        // Sequences need to be treated with a custom marshaler             
        public IntPtr LongSequence;

        // Sequences need to be treated with a custom marshaler             
        public IntPtr StringSequence;

        // Sequences need to be treated with a custom marshaler             
        public IntPtr WStringSequence;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I4, SizeConst = 5)]
        public int[] LongArray;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.SysInt, SizeConst = 10)]
        public IntPtr[] StringArray;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.SysInt, SizeConst = 4)]
        public IntPtr[] WStringArray;

        [MarshalAs(UnmanagedType.Struct)]
        public NestedTestStructWrapper StructTest;

        public IntPtr StructSequence;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 5)]
        public NestedTestStructWrapper[] StructArray;

        public IntPtr LongMultiArray;

        public IntPtr StringMultiArray;

        public IntPtr WStringMultiArray;

        public IntPtr StructMultiArray;

        public float FloatType;

        public double DoubleType;

        public double LongDoubleType;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 5)]
        public float[] FloatArray;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R8, SizeConst = 5)]
        public double[] DoubleArray;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R8, SizeConst = 5)]
        public double[] LongDoubleArray;

        public IntPtr FloatSequence;

        public IntPtr DoubleSequence;

        public IntPtr LongDoubleSequence;

        public IntPtr FloatMultiArray;

        public IntPtr DoubleMultiArray;

        public IntPtr LongDoubleMultiArray;
                
        [MarshalAs(UnmanagedType.I1)]
        public char CharType;

        [MarshalAs(UnmanagedType.I2)]
        public char WCharType;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 5)]
        public char[] CharArray;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I2, SizeConst = 5)]
        public char[] WCharArray;

        public IntPtr CharSequence;

        public IntPtr WCharSequence;

        public IntPtr CharMultiArray;

        public IntPtr WCharMultiArray;

        public short ShortType;

        public long LongLongType;

        public ushort UnsignedShortType;

        public uint UnsignedLongType;

        public ulong UnsignedLongLongType;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I2, SizeConst = 5)]
        public short[] ShortArray;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I8, SizeConst = 5)]
        public long[] LongLongArray;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U2, SizeConst = 5)]
        public ushort[] UnsignedShortArray;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U4, SizeConst = 5)]
        public uint[] UnsignedLongArray;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U8, SizeConst = 5)]
        public ulong[] UnsignedLongLongArray;

        public IntPtr ShortSequence;

        public IntPtr LongLongSequence;

        public IntPtr UnsignedShortSequence;

        public IntPtr UnsignedLongSequence;

        public IntPtr UnsignedLongLongSequence;

        public IntPtr ShortMultiArray;

        public IntPtr LongLongMultiArray;

        public IntPtr UnsignedShortMultiArray;

        public IntPtr UnsignedLongMultiArray;

        public IntPtr UnsignedLongLongMultiArray;

        // The default marshalling for a bool will expand it to 4 bytes. 
        // It need to be adapted to C++ bool than expand to 1 byte.
        [MarshalAs(UnmanagedType.I1)]
        public bool BooleanType;

        [MarshalAs(UnmanagedType.U1)]
        public byte OctetType;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 5)]
        public bool[] BooleanArray;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 5)]
        public byte[] OctetArray;

        public IntPtr BooleanSequence;

        public IntPtr OctetSequence;

        public IntPtr BooleanMultiArray;

        public IntPtr OctetMultiArray;

        public PrimitiveEnum TestEnum;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I4, SizeConst = 5)]
        public PrimitiveEnum[] EnumArray;

        public IntPtr EnumSequence;

        public IntPtr EnumMultiArray;

        public IntPtr LongBoundedSequence;

        public IntPtr StringBoundedSequence;
        
        public IntPtr WStringBoundedSequence;

        public IntPtr StructBoundedSequence;

        public IntPtr LongDoubleBoundedSequence;

        public IntPtr BooleanBoundedSequence;

        public IntPtr EnumBoundedSequence;
    }

    public class BasicTestStructTypeSupport
    {
        #region Field
        private IntPtr _native;
        #endregion

        #region Constructors
        public BasicTestStructTypeSupport()
        {
            if (Environment.Is64BitProcess)
            {
                _native = BasicTestStructTypeSupportNew64();
            }
            else
            {
                _native = BasicTestStructTypeSupportNew86();
            }
        }
        #endregion

        #region Methods
        public string GetTypeName()
        {
            if (Environment.Is64BitProcess)
            {
                return Marshal.PtrToStringAnsi(GetTypeName64(_native));                
            }
            else
            {
                return Marshal.PtrToStringAnsi(GetTypeName86(_native));                              
            }
        }

        public ReturnCode RegisterType(DomainParticipant dp, string typeName)
        {
            if (Environment.Is64BitProcess)
            {
                return (ReturnCode)RegisterType64(_native, dp.ToNative(), typeName);
            }
            else
            {
                return (ReturnCode)RegisterType86(_native, dp.ToNative(), typeName);
            }
        }

        public ReturnCode UnregisterType(DomainParticipant dp, string typeName)
        {            
            if (Environment.Is64BitProcess)
            {
                return (ReturnCode)UnregisterType64(_native, dp.ToNative(), typeName);
            }
            else
            {
                return (ReturnCode)UnregisterType86(_native, dp.ToNative(), typeName);
            }
        }
        #endregion

        #region PInvoke
        [SuppressUnmanagedCodeSecurity]
        [DllImport(Helper.API_DLL_X64, EntryPoint = "BasicTestStructTypeSupport_new", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr BasicTestStructTypeSupportNew64();

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Helper.API_DLL_X86, EntryPoint = "BasicTestStructTypeSupport_new", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr BasicTestStructTypeSupportNew86();

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Helper.API_DLL_X64, EntryPoint = "BasicTestStructTypeSupport_GetTypeName", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        private static extern IntPtr GetTypeName64(IntPtr native);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Helper.API_DLL_X86, EntryPoint = "BasicTestStructTypeSupport_GetTypeName", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        private static extern IntPtr GetTypeName86(IntPtr native);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Helper.API_DLL_X64, EntryPoint = "BasicTestStructTypeSupport_RegisterType", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        private static extern int RegisterType64(IntPtr native, IntPtr dp, string typeName);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Helper.API_DLL_X86, EntryPoint = "BasicTestStructTypeSupport_RegisterType", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        private static extern int RegisterType86(IntPtr native, IntPtr dp, string typeName);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Helper.API_DLL_X64, EntryPoint = "BasicTestStructTypeSupport_UnregisterType", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        private static extern int UnregisterType64(IntPtr native, IntPtr dp, string typeName);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Helper.API_DLL_X86, EntryPoint = "BasicTestStructTypeSupport_UnregisterType", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        private static extern int UnregisterType86(IntPtr native, IntPtr dp, string typeName);
        #endregion
    }

    public class BasicTestStructDataWriter : DataWriter
    {
        #region Fields
        private new readonly IntPtr _native;
        #endregion

        #region Constructors
        public BasicTestStructDataWriter(DataWriter dw) : base(dw.ToNative())
        {
            if (Environment.Is64BitProcess)
            {
                _native = Narrow64(base._native);
            }
            else
            {
                _native = Narrow86(base._native);
            }
        }
        #endregion

        #region Methods
        public ReturnCode Write(BasicTestStruct data)
        {
            if (data == null)
            {
                return ReturnCode.BadParameter;
            }

            ReturnCode ret = ReturnCode.Error;
            List<IntPtr> toRelease = new List<IntPtr>();

            BasicTestStructWrapper wrapper = data.ToNative(toRelease);
            if (Environment.Is64BitProcess)
            {                                
                ret = (ReturnCode)Write64(_native, ref wrapper, 0);
            }
            else
            {
                ret = (ReturnCode)Write86(_native, ref wrapper, 0);
            }

            // Always free the unmanaged memory.
            foreach(IntPtr ptr in toRelease)
            {
                Marshal.FreeHGlobal(ptr);
            }

            return ret;
        }
        #endregion

        #region PInvoke
        [SuppressUnmanagedCodeSecurity]
        [DllImport(Helper.API_DLL_X64, EntryPoint = "BasicTestStructDataWriter_Narrow", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Narrow64(IntPtr dw);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Helper.API_DLL_X86, EntryPoint = "BasicTestStructDataWriter_Narrow", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Narrow86(IntPtr dw);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Helper.API_DLL_X64, EntryPoint = "BasicTestStructDataWriter_Write", CallingConvention = CallingConvention.Cdecl)]
        private static extern int Write64(IntPtr dw, [MarshalAs(UnmanagedType.Struct), In] ref BasicTestStructWrapper data, int handle);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Helper.API_DLL_X86, EntryPoint = "BasicTestStructDataWriter_Write", CallingConvention = CallingConvention.Cdecl)]
        private static extern int Write86(IntPtr dw, [MarshalAs(UnmanagedType.Struct), In] ref BasicTestStructWrapper data, int handle);
        #endregion
    }

    public class BasicTestStructDataReader : DataReader
    {
        #region Fields
        private new readonly IntPtr _native;
        #endregion

        #region Constructors
        public BasicTestStructDataReader(DataReader dr) : base(dr.ToNative())
        {
            if (Environment.Is64BitProcess)
            {
                _native = Narrow64(base._native);
            }
            else
            {
                _native = Narrow86(base._native);
            }
        }
        #endregion

        #region Methods
        public ReturnCode Read()
        {
            if (Environment.Is64BitProcess)
            {
                ReturnCode ret = (ReturnCode)Read64(_native);
                if (ret == ReturnCode.Ok)
                {
                    // TODO
                }

                return ret;
            }
            else
            {
                ReturnCode ret = (ReturnCode)Read86(_native);
                if (ret == ReturnCode.Ok)
                {
                    // TODO
                }

                return ret;
            }
        }

        public ReturnCode ReadNextSample(BasicTestStruct data)
        {
            if (data == null)
            {
                return ReturnCode.BadParameter;
            }

            ReturnCode ret = ReturnCode.Error;
            BasicTestStructWrapper wrapper = new BasicTestStructWrapper();
            if (Environment.Is64BitProcess)
            {                
                ret = (ReturnCode)ReadNextSample64(_native, ref wrapper);
                if (ret == ReturnCode.Ok)
                {
                    data.FromNative(wrapper);

                    // Always free the unmanaged memory.
                    // As the unmanaged memory was reserved in C++ we need a method to release it from C++.
                    Release64(ref wrapper);
                }

                return ret;
            }
            else
            {
                ret = (ReturnCode)ReadNextSample86(_native, ref wrapper);
                if (ret == ReturnCode.Ok)
                {
                    data.FromNative(wrapper);

                    // Always free the unmanaged memory.
                    // As the unmanaged memory was reserved in C++ we need a method to release it from C++.                    
                    Release86(ref wrapper);
                }
            }

            return ret;
        }
        #endregion

        #region PInvoke
        [SuppressUnmanagedCodeSecurity]
        [DllImport(Helper.API_DLL_X64, EntryPoint = "BasicTestStructWrapper_release", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Release64([MarshalAs(UnmanagedType.Struct), In, Out] ref BasicTestStructWrapper data);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Helper.API_DLL_X86, EntryPoint = "BasicTestStructWrapper_release", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Release86([MarshalAs(UnmanagedType.Struct), In, Out] ref BasicTestStructWrapper data);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Helper.API_DLL_X64, EntryPoint = "BasicTestStructDataReader_Narrow", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Narrow64(IntPtr dw);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Helper.API_DLL_X86, EntryPoint = "BasicTestStructDataReader_Narrow", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Narrow86(IntPtr dw);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Helper.API_DLL_X64, EntryPoint = "BasicTestStructDataReader_Read", CallingConvention = CallingConvention.Cdecl)]
        private static extern int Read64(IntPtr dr);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Helper.API_DLL_X86, EntryPoint = "BasicTestStructDataReader_Read", CallingConvention = CallingConvention.Cdecl)]
        private static extern int Read86(IntPtr dr);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Helper.API_DLL_X64, EntryPoint = "BasicTestStructDataReader_ReadNextSample", CallingConvention = CallingConvention.Cdecl)]
        private static extern int ReadNextSample64(IntPtr dr, [MarshalAs(UnmanagedType.Struct), In, Out] ref BasicTestStructWrapper data);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Helper.API_DLL_X86, EntryPoint = "BasicTestStructDataReader_ReadNextSample", CallingConvention = CallingConvention.Cdecl)]
        private static extern int ReadNextSample86(IntPtr dr, [MarshalAs(UnmanagedType.Struct), In] ref BasicTestStructWrapper data);
        #endregion
    }
}
