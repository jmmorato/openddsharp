using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace OpenDDSharp.Marshaller.Cdr;

/// <summary>
/// CDR writer.
/// </summary>
public class CdrWriter
{
    private readonly MemoryStream _stream;
    private readonly BinaryWriter _writer;

    /// <summary>
    /// Initializes a new instance of the <see cref="CdrWriter"/> class.
    /// </summary>
    public CdrWriter() : this(new MemoryStream())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CdrWriter"/> class.
    /// </summary>
    /// <param name="buffer">The buffer to write the bytes.</param>
    public CdrWriter(byte[] buffer) : this(new MemoryStream(buffer)) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="CdrWriter"/> class.
    /// </summary>
    /// <param name="stream">The stream used to write the bytes.</param>
    private CdrWriter(MemoryStream stream)
    {
        _stream = stream;
        _writer = new BinaryWriter(_stream);
    }

    /// <summary>
    /// Gets the buffer.
    /// </summary>
    /// <returns>The bytes span.</returns>
    public ReadOnlySpan<byte> GetBuffer()
    {
        return new ReadOnlySpan<byte>(_stream.GetBuffer(), 0, (int)_stream.Length);
    }

    /// <summary>
    /// Writes an unsigned byte to the stream.
    /// </summary>
    /// <param name="b">The unsigned byte to be written.</param>
    public void WriteByte(byte b) => _writer.Write(b);

    /// <summary>
    /// Writes a signed byte to the stream.
    /// </summary>
    /// <param name="b">The signed byte to be written.</param>
    public void WriteSByte(sbyte b) => _writer.Write(b);

    /// <summary>
    /// Writes an array of unsigned bytes to the stream.
    /// </summary>
    /// <param name="buf">The unsigned bytes to be written.</param>
    public void WriteBytes(ReadOnlySpan<byte> buf)
    {
#if NET6_0_OR_GREATER
        _writer.Write(buf);
#else
        _writer.Write(buf.ToArray());
#endif
    }

    /// <summary>
    /// Write a boolean value to the stream.
    /// </summary>
    /// <param name="b">The boolean value to be written.</param>
    public void WriteBool(bool b) => WriteByte(b ? (byte)0x01 : (byte)0x00);

    /// <summary>
    /// Writes a signed short to the stream.
    /// </summary>
    /// <param name="s">The short value to be written.</param>
    public void WriteInt16(short s)
    {
        Align(2);
        _writer.Write(s);
    }

    /// <summary>
    /// Writes an unsigned short to the stream.
    /// </summary>
    /// <param name="s">The unsigned short value to be written.</param>
    public void WriteUInt16(ushort s)
    {
        Align(2);
        _writer.Write(s);
    }

    /// <summary>
    /// Writes a signed integer to the stream.
    /// </summary>
    /// <param name="i">The signed integer to be written.</param>
    public void WriteInt32(int i)
    {
        Align(4);
        _writer.Write(i);
    }

    /// <summary>
    /// Writes an unsigned integer to the stream.
    /// </summary>
    /// <param name="i">The unsigned integer value to be written.</param>
    public void WriteUInt32(uint i)
    {
        Align(4);
        _writer.Write(i);
    }

    /// <summary>
    /// Writes a signed long to the stream.
    /// </summary>
    /// <param name="l">The signed long to be written.</param>
    public void WriteInt64(long l)
    {
        Align(8);
        _writer.Write(l);
    }

    /// <summary>
    /// Writes an unsigned long to the stream.
    /// </summary>
    /// <param name="l">The unsigned long value to be written.</param>
    public void WriteUInt64(ulong l)
    {
        Align(8);
        _writer.Write(l);
    }

    /// <summary>
    /// Writes a float to the stream.
    /// </summary>
    /// <param name="f">The float value to be written.</param>
    public void WriteSingle(float f)
    {
        Align(4);
        _writer.Write(f);
    }

    /// <summary>
    /// Writes a double to the stream.
    /// </summary>
    /// <param name="d">The double value to be written.</param>
    public void WriteDouble(double d)
    {
        Align(8);
        _writer.Write(d);
    }

    /// <summary>
    /// Writes a char to the stream.
    /// </summary>
    /// <param name="c">The char value to be written.</param>
    public void WriteChar(char c) => _writer.Write(Convert.ToByte(c));

    /// <summary>
    /// Writes a wide char to the stream.
    /// </summary>
    /// <param name="c">The wide char value to be written.</param>
    public void WriteWChar(char c)
    {
        Align(2);
        var bytes = Encoding.Unicode.GetBytes(new[] { c });
        WriteBytes(bytes);
    }

    /// <summary>
    /// Writes a string to the stream.
    /// </summary>
    /// <param name="s">The string to be written.</param>
    public void WriteString(string s)
    {
        var bytes = Encoding.UTF8.GetBytes(s);
        WriteUInt32((uint)bytes.Length + 1);
        WriteBytes(bytes);
        WriteByte(0x00);
    }

    /// <summary>
    /// Writes a string to the stream.
    /// </summary>
    /// <param name="s">The string to be written.</param>
    public void WriteWString(string s)
    {
        var bytes = Encoding.Unicode.GetBytes(s);
        WriteUInt32((uint)bytes.Length + 2);
        WriteBytes(bytes);
        WriteByte(0x00);
        WriteByte(0x00);
    }

    /// <summary>
    /// Write an enumeration value to the stream.
    /// </summary>
    /// <param name="enumVal">The enumeration value to be written.</param>
    public void WriteEnum(uint enumVal) => WriteUInt32(enumVal);

    /// <summary>
    /// Write a array of booleans to the stream.
    /// </summary>
    /// <param name="sequence">The array of booleans to be written.</param>
    public void WriteBoolSequence(IList<bool> sequence)
    {
        if (sequence == null)
        {
            WriteSequenceLength(0);
            return;
        }

        var bytes = sequence.Select(b => b ? (byte)0x01 : (byte)0x00).ToArray();
        WriteSequenceLength((uint)bytes.Length);
        WriteBytes(bytes);
    }

    /// <summary>
    /// Write a array of booleans to the stream.
    /// </summary>
    /// <param name="array">The array of booleans to be written.</param>
    /// <param name="len">The length of the array.</param>
    public void WriteBoolArray(bool[] array, int len)
    {
        if (array == null)
        {
            for (var i = 0; i < len; i++)
            {
                WriteBool(false);
            }

            return;
        }

        var bytes = array.Select(b => b ? (byte)0x01 : (byte)0x00).ToArray();
        WriteBytes(bytes);
    }

    /// <summary>
    /// Write a sequence of unsigned bytes to the stream.
    /// </summary>
    /// <param name="sequence">The sequence of unsigned bytes to be written.</param>
    public void WriteByteSequence(IList<byte> sequence)
    {
        if (sequence == null)
        {
            WriteSequenceLength(0);
            return;
        }

        WriteSequenceLength((uint)sequence.Count);
        WriteBytes(sequence.ToArray());
    }

    /// <summary>
    /// Write a sequence of signed bytes to the stream.
    /// </summary>
    /// <param name="sequence">The sequence of signed bytes to be written.</param>
    public void WriteSByteSequence(IList<sbyte> sequence)
    {
        if (sequence == null)
        {
            WriteSequenceLength(0);
            return;
        }

        WriteSequenceLength((uint)sequence.Count);
        WriteBytes((byte[])(Array)sequence.ToArray());
    }

    /// <summary>
    /// Write an array of bytes to the stream.
    /// </summary>
    /// <param name="array">The array of bytes to be written.</param>
    /// <param name="len">The length of the array.</param>
    public void WriteByteArray(byte[] array, int len)
    {
        if (array == null)
        {
            for (var i = 0; i < len; i++)
            {
                WriteByte(0);
            }

            return;
        }

        WriteBytes(array);
    }

    /// <summary>
    /// Write an array of signed bytes to the stream.
    /// </summary>
    /// <param name="array">The array of signed bytes to be written.</param>
    /// <param name="len">The length of the array.</param>
    public void WriteSByteArray(sbyte[] array, int len)
    {
        if (array == null)
        {
            for (var i = 0; i < len; i++)
            {
                WriteByte(0);
            }

            return;
        }

        WriteBytes((byte[])(Array)array);
    }

    /// <summary>
    /// Write a sequence of shorts to the stream.
    /// </summary>
    /// <param name="sequence">The sequence of shorts to be written.</param>
    public void WriteInt16Sequence(IList<short> sequence)
    {
        if (sequence == null)
        {
            WriteSequenceLength(0);
            return;
        }

        WriteSequenceLength((uint)sequence.Count);
        foreach (var item in sequence)
        {
            WriteInt16(item);
        }
    }

    /// <summary>
    /// Write an array of shorts to the stream.
    /// </summary>
    /// <param name="array">The array of shorts to be written.</param>
    /// <param name="len">The length of the array.</param>
    public void WriteInt16Array(short[] array, int len)
    {
        if (array == null)
        {
            for (var i = 0; i < len; i++)
            {
                WriteInt16(0);
            }

            return;
        }

        foreach (var item in array)
        {
            WriteInt16(item);
        }
    }

    /// <summary>
    /// Write a sequence of integers to the stream.
    /// </summary>
    /// <param name="sequence">The sequence of integers to be written.</param>
    public void WriteInt32Sequence(IList<int> sequence)
    {
        if (sequence == null)
        {
            WriteSequenceLength(0);
            return;
        }

        WriteSequenceLength((uint)sequence.Count);
        foreach (var item in sequence)
        {
            WriteInt32(item);
        }
    }

    /// <summary>
    /// Write an array of integers to the stream.
    /// </summary>
    /// <param name="array">The array of integers to be written.</param>
    /// <param name="len">The length of the array.</param>
    public void WriteInt32Array(int[] array, int len)
    {
        if (array == null)
        {
            for (var i = 0; i < len; i++)
            {
                WriteInt32(0);
            }

            return;
        }

        foreach (var item in array)
        {
            WriteInt32(item);
        }
    }

    /// <summary>
    /// Write a sequence of longs to the stream.
    /// </summary>
    /// <param name="sequence">The sequence of longs to be written.</param>
    public void WriteInt64Sequence(IList<long> sequence)
    {
        if (sequence == null)
        {
            WriteSequenceLength(0);
            return;
        }

        WriteSequenceLength((uint)sequence.Count);
        foreach (var item in sequence)
        {
            WriteInt64(item);
        }
    }

    /// <summary>
    /// Write an array of longs to the stream.
    /// </summary>
    /// <param name="array">The array of longs to be written.</param>
    /// <param name="len">The length of the array.</param>
    public void WriteInt64Array(long[] array, int len)
    {
        if (array == null)
        {
            for (var i = 0; i < len; i++)
            {
                WriteInt64(0);
            }

            return;
        }

        foreach (var item in array)
        {
            WriteInt64(item);
        }
    }

    /// <summary>
    /// Write a sequence of unsigned shorts to the stream.
    /// </summary>
    /// <param name="sequence">The sequence of unsigned shorts to be written.</param>
    public void WriteUInt16Sequence(IList<ushort> sequence)
    {
        if (sequence == null)
        {
            WriteSequenceLength(0);
            return;
        }

        WriteSequenceLength((uint)sequence.Count);
        foreach (var item in sequence)
        {
            WriteUInt16(item);
        }
    }

    /// <summary>
    /// Write an array of unsigned shorts to the stream.
    /// </summary>
    /// <param name="array">The array of unsigned shorts to be written.</param>
    /// <param name="len">The length of the array.</param>
    public void WriteUInt16Array(ushort[] array, int len)
    {
        if (array == null)
        {
            for (var i = 0; i < len; i++)
            {
                WriteUInt16(0);
            }

            return;
        }

        foreach (var item in array)
        {
            WriteUInt16(item);
        }
    }

    /// <summary>
    /// Write a sequence of unsigned integers to the stream.
    /// </summary>
    /// <param name="sequence">The sequence of unsigned integers to be written.</param>
    public void WriteUInt32Sequence(IList<uint> sequence)
    {
        if (sequence == null)
        {
            WriteSequenceLength(0);
            return;
        }

        WriteSequenceLength((uint)sequence.Count);
        foreach (var item in sequence)
        {
            WriteUInt32(item);
        }
    }

    /// <summary>
    /// Write an array of unsigned integers to the stream.
    /// </summary>
    /// <param name="array">The array of unsigned integers to be written.</param>
    /// <param name="len">The length of the array.</param>
    public void WriteUInt32Array(uint[] array, int len)
    {
        if (array == null)
        {
            for (var i = 0; i < len; i++)
            {
                WriteUInt32(0);
            }

            return;
        }

        foreach (var item in array)
        {
            WriteUInt32(item);
        }
    }

    /// <summary>
    /// Write a sequence of unsigned longs to the stream.
    /// </summary>
    /// <param name="sequence">The sequence of unsigned longs to be written.</param>
    public void WriteUInt64Sequence(IList<ulong> sequence)
    {
        if (sequence == null)
        {
            WriteSequenceLength(0);
            return;
        }

        WriteSequenceLength((uint)sequence.Count);
        foreach (var item in sequence)
        {
            WriteUInt64(item);
        }
    }

    /// <summary>
    /// Write an array of unsigned longs to the stream.
    /// </summary>
    /// <param name="array">The array of unsigned longs to be written.</param>
    /// <param name="len">The length of the array.</param>
    public void WriteUInt64Array(ulong[] array, int len)
    {
        if (array == null)
        {
            for (var i = 0; i < len; i++)
            {
                WriteUInt64(0);
            }

            return;
        }

        foreach (var item in array)
        {
            WriteUInt64(item);
        }
    }

    /// <summary>
    /// Write a sequence of floats to the stream.
    /// </summary>
    /// <param name="sequence">The sequence of floats to be written.</param>
    public void WriteSingleSequence(IList<float> sequence)
    {
        if (sequence == null)
        {
            WriteSequenceLength(0);
            return;
        }

        WriteSequenceLength((uint)sequence.Count);
        foreach (var item in sequence)
        {
            WriteSingle(item);
        }
    }

    /// <summary>
    /// Write an array of floats to the stream.
    /// </summary>
    /// <param name="array">The array of floats to be written.</param>
    /// <param name="len">The length of the array.</param>
    public void WriteSingleArray(float[] array, int len)
    {
        if (array == null)
        {
            for (var i = 0; i < len; i++)
            {
                WriteSingle(0);
            }

            return;
        }

        foreach (var item in array)
        {
            WriteSingle(item);
        }
    }

    /// <summary>
    /// Write a sequence of doubles to the stream.
    /// </summary>
    /// <param name="sequence">The sequence of doubles to be written.</param>
    public void WriteDoubleSequence(IList<double> sequence)
    {
        if (sequence == null)
        {
            WriteSequenceLength(0);
            return;
        }

        WriteSequenceLength((uint)sequence.Count);
        foreach (var item in sequence)
        {
            WriteDouble(item);
        }
    }

    /// <summary>
    /// Write an array of doubles to the stream.
    /// </summary>
    /// <param name="array">The array of doubles to be written.</param>
    /// <param name="len">The length of the array.</param>
    public void WriteDoubleArray(double[] array, int len)
    {
        if (array == null)
        {
            for (var i = 0; i < len; i++)
            {
                WriteDouble(0);
            }

            return;
        }

        foreach (var item in array)
        {
            WriteDouble(item);
        }
    }

    /// <summary>
    /// Write a sequence of chars to the stream.
    /// </summary>
    /// <param name="sequence">The sequence of chars to be written.</param>
    public void WriteCharSequence(IList<char> sequence)
    {
        if (sequence == null)
        {
            WriteSequenceLength(0);
            return;
        }

        var bytes = Encoding.UTF8.GetBytes(sequence.ToArray());
        WriteSequenceLength((uint)bytes.Length);
        WriteBytes(bytes);
    }

    /// <summary>
    /// Write a sequence of wide chars to the stream.
    /// </summary>
    /// <param name="sequence">The sequence of wide chars to be written.</param>
    public void WriteWCharSequence(IList<char> sequence)
    {
        if (sequence == null)
        {
            WriteSequenceLength(0);
            return;
        }

        var bytes = Encoding.Unicode.GetBytes(sequence.ToArray());
        WriteSequenceLength((uint)sequence.Count);
        WriteBytes(bytes);
    }

    /// <summary>
    /// Write an array of chars to the stream.
    /// </summary>
    /// <param name="array">The array of chars to be written.</param>
    /// <param name="len">The length of the array.</param>
    public void WriteCharArray(char[] array, int len)
    {
        if (array == null)
        {
            for (var i = 0; i < len; i++)
            {
                WriteChar('\0');
            }

            return;
        }

        var bytes = Encoding.UTF8.GetBytes(array);
        WriteBytes(bytes);
    }

    /// <summary>
    /// Write an array of wide chars to the stream.
    /// </summary>
    /// <param name="array">The array of wide chars to be written.</param>
    /// <param name="len">The length of the array.</param>
    public void WriteWCharArray(char[] array, int len)
    {
        if (array == null)
        {
            for (var i = 0; i < len; i++)
            {
                WriteWChar('\0');
            }

            return;
        }

        foreach (var c in array)
        {
            WriteWChar(c);
        }
    }

    /// <summary>
    /// Write a sequence of strings to the stream.
    /// </summary>
    /// <param name="sequence">The sequence of strings to be written.</param>
    public void WriteStringSequence(IList<string> sequence)
    {
        if (sequence == null)
        {
            WriteSequenceLength(0);
            return;
        }

        WriteSequenceLength((uint)sequence.Count);
        foreach (var str in sequence)
        {
            WriteString(str);
        }
    }

    /// <summary>
    /// Write a sequence of wide strings to the stream.
    /// </summary>
    /// <param name="sequence">The sequence of strings to be written.</param>
    public void WriteWStringSequence(IList<string> sequence)
    {
        if (sequence == null)
        {
            WriteSequenceLength(0);
            return;
        }

        WriteSequenceLength((uint)sequence.Count);
        foreach (var str in sequence)
        {
            WriteWString(str);
        }
    }

    /// <summary>
    /// Write an array of strings to the stream.
    /// </summary>
    /// <param name="array">The array of strings to be written.</param>
    /// <param name="len">The length of the array.</param>
    public void WriteStringArray(string[] array, int len)
    {
        if (array == null)
        {
            for (var i = 0; i < len; i++)
            {
                WriteString(string.Empty);
            }

            return;
        }

        foreach (var str in array)
        {
            WriteString(str);
        }
    }

    /// <summary>
    /// Write an array of strings to the stream.
    /// </summary>
    /// <param name="array">The array of strings to be written.</param>
    /// <param name="len">The length of the array.</param>
    public void WriteWStringArray(string[] array, int len)
    {
        if (array == null)
        {
            for (var i = 0; i < len; i++)
            {
                WriteString(string.Empty);
            }

            return;
        }

        foreach (var str in array)
        {
            WriteWString(str);
        }
    }

    /// <summary>
    /// Write a sequence of enumerations to the stream.
    /// </summary>
    /// <param name="sequence">The sequence of enumerations to be written.</param>
    public void WriteEnumSequence(IList<uint> sequence)
    {
        if (sequence == null)
        {
            WriteSequenceLength(0);
            return;
        }

        WriteSequenceLength((uint)sequence.Count);
        foreach (var item in sequence)
        {
            WriteEnum(item);
        }
    }

    /// <summary>
    /// Write an array of enumerations to the stream.
    /// </summary>
    /// <param name="array">The array of enumerations to be written.</param>
    /// <param name="len">The length of the array.</param>
    public void WriteEnumArray(uint[] array, int len)
    {
        if (array == null)
        {
            for (var i = 0; i < len; i++)
            {
                WriteEnum(0);
            }

            return;
        }

        foreach (var item in array)
        {
            WriteEnum(item);
        }
    }

    /// <summary>
    /// Convert the stream to a string.
    /// </summary>
    /// <returns>The converted string.</returns>
    public override string ToString()
    {
        return Encoding.UTF8.GetString(GetBuffer().ToArray());
    }

    private void Align(int alignment)
    {
        var modulo = _writer.BaseStream.Position % alignment;
        if (modulo <= 0)
        {
            return;
        }

        for (var i = 0; i < alignment - modulo; i++)
        {
            WriteByte(0x00);
        }
    }

    private void WriteSequenceLength(uint length) => WriteUInt32(length);
}