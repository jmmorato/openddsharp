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
    }

    public class BasicTestStruct
    {
        #region Fields
        private IList<int> _longSequence;
        private IList<string> _stringSequence;
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
        #endregion

        #region Constructors
        public BasicTestStruct()
        {
            _longSequence = new List<int>();
            _stringSequence = new List<string>();
        }
        #endregion

        #region Methods
        internal BasicTestStructWrapper ToNative(List<IntPtr> toFree)
        {
            BasicTestStructWrapper wrapper = new BasicTestStructWrapper();

            wrapper.Id = Id;

            if (Message != null)
            {
                wrapper.Message = Marshal.StringToHGlobalAnsi(Message);
                toFree.Add(wrapper.Message);
            }

            if (WMessage != null)
            {
                wrapper.WMessage = Marshal.StringToHGlobalUni(WMessage);
                toFree.Add(wrapper.WMessage);
            }

            Helper.UnboundedSequenceToPtr(LongSequence, ref wrapper.LongSequence);
            toFree.Add(wrapper.LongSequence);

            toFree.AddRange(Helper.UnboundedBasicStringSequenceToPtr(StringSequence, ref wrapper.StringSequence));
            toFree.Add(wrapper.StringSequence);

            return wrapper;
        }

        internal void FromNative(BasicTestStructWrapper wrapper)
        {
            Id = wrapper.Id;
            Message = Marshal.PtrToStringAnsi(wrapper.Message);
            WMessage = Marshal.PtrToStringUni(wrapper.WMessage);

            Helper.PtrToUnboundedSequence(wrapper.LongSequence, ref _longSequence);
            Helper.PtrToUnboundedBasicStringSequence(wrapper.StringSequence, ref _stringSequence);
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
            List<IntPtr> toFree = new List<IntPtr>();
            BasicTestStructWrapper wrapper = data.ToNative(toFree);
            if (Environment.Is64BitProcess)
            {                                
                ret = (ReturnCode)Write64(_native, ref wrapper, 0);
            }
            else
            {
                ret = (ReturnCode)Write86(_native, ref wrapper, 0);
            }

            // Always free the unmanaged memory.
            foreach(IntPtr ptr in toFree)
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
