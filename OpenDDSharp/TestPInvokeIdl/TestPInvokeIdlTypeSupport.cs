using System;
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

        public static void PtrToUnboundedSequence<T>(IntPtr ptr, ref IList<T> sequence)
        {
            // Ensure a not null empty list to populate
            if (sequence == null)
                sequence = new List<T>();
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

        public static void UnboundedSequenceToPtr<T>(IList<T> sequence, ref IntPtr ptr)
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

        public static void PtrToUnboundedBasicStringSequence(IntPtr ptr, ref IList<string> sequence)
        {
            // Ensure a not null empty list to populate
            if (sequence == null)
                sequence = new List<string>();
            else
                sequence.Clear();

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
                sequence.Add(Marshal.PtrToStringAnsi(pointer));
            }
        }

        public static List<IntPtr> UnboundedBasicStringSequenceToPtr(IList<string> sequence, ref IntPtr ptr)
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
                IntPtr sPtr = Marshal.StringToHGlobalAnsi(sequence[i]);
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
                Marshal.StructureToPtr((T)enumerator.Current, ptr + (elSiz * i), false);
                i++;
            }
        }
    }

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
        private IList<NestedTestStruct> _structSequence;
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

        public int[] LongArray { get; set; }

        public string[] StringArray { get; set; }

        public string[] WStringArray { get; set; }

        public NestedTestStruct StructTest { get; set; }

        public IList<NestedTestStruct> StructSequence
        {
            get { return _structSequence; }
            set { _structSequence = value; }
        }

        public NestedTestStruct[] StructArray { get; set; }

        public int[,,] LongMultiArray { get; set; }
        #endregion

        #region Constructors
        public BasicTestStruct()
        {
            _longSequence = new List<int>();
            _stringSequence = new List<string>();
            LongArray = new int[5];
            StringArray = new string[10];
            WStringArray = new string[4];
            StructTest = new NestedTestStruct();
            _structSequence = new List<NestedTestStruct>();
            StructArray = new NestedTestStruct[5];
            LongMultiArray = new int[3, 4, 2];
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

            Helper.UnboundedSequenceToPtr(LongSequence, ref wrapper.LongSequence);
            toRelease.Add(wrapper.LongSequence);

            toRelease.AddRange(Helper.UnboundedBasicStringSequenceToPtr(StringSequence, ref wrapper.StringSequence));
            toRelease.Add(wrapper.StringSequence);

            wrapper.LongArray = LongArray;

            wrapper.StringArray = new IntPtr[10];
            for (int i = 0; i < 10; i++)
            {
                if (StringArray[i] != null)
                {
                    wrapper.StringArray[i] = Marshal.StringToHGlobalAnsi(StringArray[i]);
                    toRelease.Add(wrapper.StringArray[i]);
                }
            }

            wrapper.WStringArray = new IntPtr[4];
            for (int i = 0; i < 4; i++)
            {
                if (WStringArray[i] != null)
                {
                    wrapper.WStringArray[i] = Marshal.StringToHGlobalUni(WStringArray[i]);
                    toRelease.Add(wrapper.WStringArray[i]);
                }
            }

            wrapper.StructTest = StructTest.ToNative(toRelease);

            // We need to use the wrapper struct to marshal the pointer
            // In the generated code, the aux variable will be suffixed with the field name
            List<NestedTestStructWrapper> aux = new List<NestedTestStructWrapper>();
            foreach(NestedTestStruct s in StructSequence)
            {
                aux.Add(s.ToNative(toRelease));
            }
            Helper.UnboundedSequenceToPtr(aux, ref wrapper.StructSequence);
            toRelease.Add(wrapper.StructSequence);

            wrapper.StructArray = new NestedTestStructWrapper[5];
            for (int i = 0; i < 5; i++)
            {
                if (StructArray[i] != null)
                {
                    wrapper.StructArray[i] = StructArray[i].ToNative(toRelease);
                }
            }

            Helper.MultiArrayToPtr<int>(LongMultiArray, ref wrapper.LongMultiArray);
            toRelease.Add(wrapper.LongMultiArray);

            return wrapper;
        }

        internal void FromNative(BasicTestStructWrapper wrapper)
        {
            Id = wrapper.Id;
            Message = Marshal.PtrToStringAnsi(wrapper.Message);
            WMessage = Marshal.PtrToStringUni(wrapper.WMessage);

            Helper.PtrToUnboundedSequence(wrapper.LongSequence, ref _longSequence);
            Helper.PtrToUnboundedBasicStringSequence(wrapper.StringSequence, ref _stringSequence);

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
            IList<NestedTestStructWrapper> aux = new List<NestedTestStructWrapper>();
            Helper.PtrToUnboundedSequence(wrapper.StructSequence, ref aux);
            foreach(NestedTestStructWrapper native in aux)
            {
                NestedTestStruct s = new NestedTestStruct();
                s.FromNative(native);
                StructSequence.Add(s);
            }

            for (int i = 0; i < 5; i++)
            {
                StructArray[i] = new NestedTestStruct();
                StructArray[i].FromNative(wrapper.StructArray[i]);                
            }

            if (LongMultiArray == null)
            {
                LongMultiArray = new int[3, 4, 2];
            }
            Helper.PtrToMultiArray<int>(wrapper.LongMultiArray, LongMultiArray);
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

        //[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I4, SizeConst = 12)]
        public IntPtr LongMultiArray;
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
