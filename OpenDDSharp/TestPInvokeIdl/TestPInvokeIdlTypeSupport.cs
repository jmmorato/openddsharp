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

        public static void PtrToUnboundedSequence<T>(IntPtr ptr, ref IList<T> sequence)
        {
            if (sequence == null)
                sequence = new List<T>();
            else
                sequence.Clear();

            if (ptr == IntPtr.Zero)
                return;            

            // Start by reading the size of the array
            int length = Marshal.ReadInt32(ptr);
            // Create the managed array that will be added to the list
            T[] array = new T[length];
            // For efficiency, only compute the element size once
            int elSiz = Marshal.SizeOf<T>();
            // Populate the array
            for (int i = 0; i < length; i++)
            {
                array[i] = Marshal.PtrToStructure<T>(ptr + sizeof(int) + (elSiz * i));
            }

            ((List<T>)sequence).AddRange(array);
        }

        public static void UnboundedSequenceToPtr<T>(IList<T> sequence, ref IntPtr ptr)
        {
            if (sequence == null)
            {
                Marshal.WriteInt32(ptr, 0);
                return;
            }

            T[] array = sequence.ToArray();
            int elSiz = Marshal.SizeOf<T>();
            // Get the total size of unmanaged memory that is needed (length + elements)
            int size = sizeof(int) + (elSiz * array.Length);
            // Allocate unmanaged space.
            ptr = Marshal.AllocHGlobal(size);
            // Write the "Length" field first
            Marshal.WriteInt32(ptr, array.Length);
            // Write the array data
            for (int i = 0; i < array.Length; i++)
            {   
                // Newly-allocated space has no existing object, so the last param is false
                Marshal.StructureToPtr(array[i], ptr + sizeof(int) + (elSiz * i), false);
            }
        }
    }

    public class BasicTestStruct
    {
        #region Fields
        private IList<int> _longSequence;
        #endregion

        #region Properties
        public int Id { get; set; }
        
        public string Message { get; set; }

        public IList<int> LongSequence
        {
            get { return _longSequence; }
            set { _longSequence = value; }
        }
        #endregion

        #region Constructors
        public BasicTestStruct()
        {
            LongSequence = new List<int>();
        }
        #endregion

        #region Methods
        internal BasicTestStructWrapper ToNative()
        {
            BasicTestStructWrapper wrapper = new BasicTestStructWrapper();
            wrapper.Id = Id;
            wrapper.Message = Marshal.StringToHGlobalAnsi(Message);
            Helper.UnboundedSequenceToPtr(LongSequence, ref wrapper.LongSequence);

            return wrapper;
        }

        internal void FromNative(BasicTestStructWrapper wrapper)
        {
            Id = wrapper.Id;
            Message = Marshal.PtrToStringAnsi(wrapper.Message);

            Helper.PtrToUnboundedSequence(wrapper.LongSequence, ref _longSequence);            
        }
        #endregion
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct BasicTestStructWrapper
    {
        public int Id;

        // Strings need to be treated as a IntPtr to avoid memory relase issues
        public IntPtr Message;

        // Sequences need to be treated with a custom marshaler             
        public IntPtr LongSequence;
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
            BasicTestStructWrapper wrapper = data.ToNative();
            if (Environment.Is64BitProcess)
            {                                
                ret = (ReturnCode)Write64(_native, ref wrapper, 0);
            }
            else
            {
                ret = (ReturnCode)Write86(_native, ref wrapper, 0);
            }

            // Always free the unmanaged memory.
            Marshal.FreeHGlobal(wrapper.Message);
            Marshal.FreeHGlobal(wrapper.LongSequence);

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
