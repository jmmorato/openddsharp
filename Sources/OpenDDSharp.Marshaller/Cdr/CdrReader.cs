using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace OpenDDSharp.Marshaller.Cdr;

/// <summary>
/// CDR reader.
/// </summary>
public class CdrReader
{
    private readonly byte[] _buf;
    private int _position;

    /// <summary>
    /// Initializes a new instance of the <see cref="CdrReader"/> class.
    /// </summary>
    /// <param name="buf">The buffer to read from.</param>
    public CdrReader(byte[] buf)
    {
        _buf = buf;
        _position = 0;
    }

    /// <summary>
    /// Reads a unsigned byte from the stream.
    /// </summary>
    /// <returns>The byte value.</returns>
    public byte ReadByte()
    {
        return _buf[_position++];
    }

    /// <summary>
    /// Reads a signed byte from the stream.
    /// </summary>
    /// <returns>The byte value.</returns>
    public sbyte ReadSByte()
    {
        return (sbyte)_buf[_position++];
    }

    /// <summary>
    /// Reads a sequence of bytes from the stream.
    /// </summary>
    /// <param name="count">The number of bytes to read.</param>
    /// <returns>The bytes from the stream.</returns>
    public ReadOnlySpan<byte> ReadBytes(int count)
    {
        var result = new ReadOnlySpan<byte>(_buf, _position, count);
        _position += count;
        return result;
    }

    /// <summary>
    /// Read a boolean value from the stream.
    /// </summary>
    /// <returns>The boolean value.</returns>
    public bool ReadBool() => ReadByte() != 0x00;

    /// <summary>
    /// Reads a signed short from the stream.
    /// </summary>
    /// <returns>The signed short value.</returns>
    public short ReadInt16()
    {
        Align(2);
        return BinaryPrimitives.ReadInt16LittleEndian(ReadBytes(2));
    }

    /// <summary>
    /// Reads an unsigned short from the stream.
    /// </summary>
    /// <returns>The unsigned short value.</returns>
    public ushort ReadUInt16()
    {
        Align(2);
        return BinaryPrimitives.ReadUInt16LittleEndian(ReadBytes(2));
    }

    /// <summary>
    /// Reads a signed integer from the stream.
    /// </summary>
    /// <returns>The signed integer value.</returns>
    public int ReadInt32()
    {
        Align(4);
        return BinaryPrimitives.ReadInt32LittleEndian(ReadBytes(4));
    }

    /// <summary>
    /// Reads an unsigned integer from the stream.
    /// </summary>
    /// <returns>The unsigned integer value.</returns>
    public uint ReadUInt32()
    {
        Align(4);
        return BinaryPrimitives.ReadUInt32LittleEndian(ReadBytes(4));
    }

    /// <summary>
    /// Reads a signed long from the stream.
    /// </summary>
    /// <returns>The signed long value.</returns>
    public long ReadInt64()
    {
        Align(8);
        return BinaryPrimitives.ReadInt64LittleEndian(ReadBytes(8));
    }

    /// <summary>
    /// Reads an unsigned long from the stream.
    /// </summary>
    /// <returns>The unsigned long value.</returns>
    public ulong ReadUInt64()
    {
        Align(8);
        return BinaryPrimitives.ReadUInt64LittleEndian(ReadBytes(8));
    }

    /// <summary>
    /// Reads a float from the stream.
    /// </summary>
    /// <returns>The float value.</returns>
    public float ReadSingle()
    {
        Align(4);
#if NET6_0_OR_GREATER
        return BinaryPrimitives.ReadSingleLittleEndian(ReadBytes(4));
#else
        return !BitConverter.IsLittleEndian
                ? Int32BitsToSingle(BinaryPrimitives.ReverseEndianness(ReadInt32()))
                : MemoryMarshal.Read<float>(ReadBytes(4));
#endif
    }

    /// <summary>
    /// Reads a double from the stream.
    /// </summary>
    /// <returns>The double value.</returns>
    public double ReadDouble()
    {
        Align(8);

#if NET6_0_OR_GREATER
        return BinaryPrimitives.ReadDoubleLittleEndian(ReadBytes(8));
#else
        return !BitConverter.IsLittleEndian ?
                Int64BitsToDouble(BinaryPrimitives.ReverseEndianness(ReadInt64())) :
                MemoryMarshal.Read<double>(ReadBytes(8));
#endif
    }

    /// <summary>
    /// Reads a character from the stream.
    /// </summary>
    /// <returns>The character value.</returns>
    public char ReadChar() => Convert.ToChar(ReadByte());

    /// <summary>
    /// Reads a wide character from the stream.
    /// </summary>
    /// <returns>The wide character value.</returns>
    public char ReadWChar()
    {
        Align(2);
        var c = ReadBytes(2);
        return Encoding.Unicode.GetString(c.ToArray())[0];
    }

    /// <summary>
    /// Reads an string from the stream.
    /// </summary>
    /// <returns>The string value.</returns>
    public string ReadString()
    {
        var len = ReadUInt32();

        var strBuf = ReadBytes((int)len - 1);
        ReadByte();

#if NET6_0_OR_GREATER
        return Encoding.UTF8.GetString(strBuf);
#else
        return Encoding.UTF8.GetString(strBuf.ToArray());
#endif
    }

    /// <summary>
    /// Reads a string from the stream.
    /// </summary>
    /// <returns>The string value.</returns>
    public string ReadWString()
    {
        var len = ReadUInt32();
        var strBuf = ReadBytes((int)len);

#if NET6_0_OR_GREATER
        return Encoding.Unicode.GetString(strBuf);
#else
        return Encoding.Unicode.GetString(strBuf.ToArray());
#endif
    }

    /// <summary>
    /// Read an enumeration value from the stream.
    /// </summary>
    /// <returns>The unsigned integer that represents the enumeration.</returns>
    public uint ReadEnum() => ReadUInt32();

    /// <summary>
    /// Reads a sequence of boolean values from the stream.
    /// </summary>
    /// <returns>The sequence of booleans from the stream.</returns>
    public IList<bool> ReadBoolSequence()
    {
        var len = ReadSequenceLength();
        var result = new bool[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadBool();
        }

        return result;
    }

    /// <summary>
    /// Reads an array of boolean values from the stream.
    /// </summary>
    /// <param name="len">The length of the array.</param>
    /// <returns>The array of booleans from the stream.</returns>
    public bool[] ReadBoolArray(int len)
    {
        var result = new bool[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadBool();
        }

        return result;
    }

    /// <summary>
    /// Reads a sequence of bytes from the stream.
    /// </summary>
    /// <returns>The sequence of bytes from the stream.</returns>
    public IList<byte> ReadByteSequence()
    {
        var len = ReadSequenceLength();
        return ReadBytes((int)len).ToArray();
    }

    /// <summary>
    /// Reads a sequence of bytes from the stream.
    /// </summary>
    /// <returns>The sequence of bytes from the stream.</returns>
    public IList<sbyte> ReadSByteSequence()
    {
        var len = ReadSequenceLength();
        return (sbyte[])(Array)ReadBytes((int)len).ToArray();
    }

    /// <summary>
    /// Reads an array of bytes from the stream.
    /// </summary>
    /// <param name="len">The length of the array.</param>
    /// <returns>The array of bytes from the stream.</returns>
    public byte[] ReadByteArray(int len)
    {
        return ReadBytes(len).ToArray();
    }

    /// <summary>
    /// Reads an array of signed bytes from the stream.
    /// </summary>
    /// <param name="len">The length of the array.</param>
    /// <returns>The array of signed bytes from the stream.</returns>
    public sbyte[] ReadSByteArray(int len)
    {
        return (sbyte[])(Array)ReadBytes(len).ToArray();
    }


    /// <summary>
    /// Reads a sequence of signed short values from the stream.
    /// </summary>
    /// <returns>The sequence of signed short values from the stream.</returns>
    public IList<short> ReadInt16Sequence()
    {
        var len = ReadSequenceLength();
        var result = new short[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadInt16();
        }

        return result;
    }

    /// <summary>
    /// Reads an array of signed short values from the stream.
    /// </summary>
    /// <param name="len">The length of the array.</param>
    /// <returns>The array of signed short values from the stream.</returns>
    public short[] ReadInt16Array(int len)
    {
        var result = new short[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadInt16();
        }

        return result;
    }

    /// <summary>
    /// Reads a sequence of unsigned short values from the stream.
    /// </summary>
    /// <returns>The sequence of unsigned short values from the stream.</returns>
    public IList<ushort> ReadUInt16Sequence()
    {
        var len = ReadSequenceLength();
        var result = new ushort[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadUInt16();
        }

        return result;
    }

    /// <summary>
    /// Reads an array of unsigned short values from the stream.
    /// </summary>
    /// <param name="len">The length of the array.</param>
    /// <returns>The array of unsigned short values from the stream.</returns>
    public ushort[] ReadUInt16Array(int len)
    {
        var result = new ushort[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadUInt16();
        }

        return result;
    }

    /// <summary>
    /// Reads a sequence of signed integer values from the stream.
    /// </summary>
    /// <returns>The sequence of signed integer values from the stream.</returns>
    public IList<int> ReadInt32Sequence()
    {
        var len = ReadSequenceLength();
        var result = new int[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadInt32();
        }

        return result;
    }

    /// <summary>
    /// Reads an array of signed integer values from the stream.
    /// </summary>
    /// <param name="len">The length of the array.</param>
    /// <returns>The array of signed integer values from the stream.</returns>
    public int[] ReadInt32Array(int len)
    {
        var result = new int[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadInt32();
        }

        return result;
    }

    /// <summary>
    /// Reads a sequence of unsigned integer values from the stream.
    /// </summary>
    /// <returns>The sequence of unsigned integer values from the stream.</returns>
    public IList<uint> ReadUInt32Sequence()
    {
        var len = ReadSequenceLength();
        var result = new uint[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadUInt32();
        }

        return result;
    }

    /// <summary>
    /// Reads an array of unsigned integer values from the stream.
    /// </summary>
    /// <param name="len">The length of the array.</param>
    /// <returns>The array of unsigned integer values from the stream.</returns>
    public uint[] ReadUInt32Array(int len)
    {
        var result = new uint[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadUInt32();
        }

        return result;
    }

    /// <summary>
    /// Reads a sequence of signed long values from the stream.
    /// </summary>
    /// <returns>The sequence of signed long values from the stream.</returns>
    public IList<long> ReadInt64Sequence()
    {
        var len = ReadSequenceLength();
        var result = new long[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadInt64();
        }

        return result;
    }

    /// <summary>
    /// Reads an array of signed long values from the stream.
    /// </summary>
    /// <param name="len">The length of the array.</param>
    /// <returns>The array of signed long values from the stream.</returns>
    public long[] ReadInt64Array(int len)
    {
        var result = new long[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadInt64();
        }

        return result;
    }

    /// <summary>
    /// Reads a sequence of unsigned long values from the stream.
    /// </summary>
    /// <returns>The sequence of unsigned long values from the stream.</returns>
    public IList<ulong> ReadUInt64Sequence()
    {
        var len = ReadSequenceLength();
        var result = new ulong[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadUInt64();
        }

        return result;
    }

    /// <summary>
    /// Reads an array of unsigned long values from the stream.
    /// </summary>
    /// <param name="len">The length of the array.</param>
    /// <returns>The array of unsigned long values from the stream.</returns>
    public ulong[] ReadUInt64Array(int len)
    {
        var result = new ulong[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadUInt64();
        }

        return result;
    }

    /// <summary>
    /// Reads a sequence of float values from the stream.
    /// </summary>
    /// <returns>The sequence of float values from the stream.</returns>
    public IList<float> ReadSingleSequence()
    {
        var len = ReadSequenceLength();
        var result = new float[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadSingle();
        }

        return result;
    }

    /// <summary>
    /// Reads an array of float values from the stream.
    /// </summary>
    /// <param name="len">The length of the array.</param>
    /// <returns>The array of float values from the stream.</returns>
    public float[] ReadSingleArray(int len)
    {
        var result = new float[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadSingle();
        }

        return result;
    }

    /// <summary>
    /// Reads a sequence of double values from the stream.
    /// </summary>
    /// <returns>The sequence of double values from the stream.</returns>
    public IList<double> ReadDoubleSequence()
    {
        var len = ReadSequenceLength();
        var result = new double[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadDouble();
        }

        return result;
    }

    /// <summary>
    /// Reads an array of double values from the stream.
    /// </summary>
    /// <param name="len">The length of the array.</param>
    /// <returns>The array of double values from the stream.</returns>
    public double[] ReadDoubleArray(int len)
    {
        var result = new double[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadDouble();
        }

        return result;
    }

    /// <summary>
    /// Reads a sequence of character values from the stream.
    /// </summary>
    /// <returns>The sequence of character values from the stream.</returns>
    public IList<char> ReadCharSequence()
    {
        var len = ReadSequenceLength();
        var result = new char[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadChar();
        }

        return result;
    }

    /// <summary>
    /// Reads a sequence of wide character values from the stream.
    /// </summary>
    /// <returns>The sequence of wide character values from the stream.</returns>
    public IList<char> ReadWCharSequence()
    {
        var len = ReadSequenceLength();
        var result = new char[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadWChar();
        }

        return result;
    }

    /// <summary>
    /// Reads an array of character values from the stream.
    /// </summary>
    /// <param name="len">The length of the array.</param>
    /// <returns>The array of character values from the stream.</returns>
    public char[] ReadCharArray(int len)
    {
        var result = new char[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadChar();
        }

        return result;
    }

    /// <summary>
    /// Reads an array of wide character values from the stream.
    /// </summary>
    /// <param name="len">The length of the array.</param>
    /// <returns>The array of wide character values from the stream.</returns>
    public char[] ReadWCharArray(int len)
    {
        var result = new char[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadWChar();
        }

        return result;
    }

    /// <summary>
    /// Reads a sequence of string values from the stream.
    /// </summary>
    /// <returns>The sequence of string values from the stream.</returns>
    public IList<string> ReadStringSequence()
    {
        var len = ReadSequenceLength();
        var result = new string[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadString();
        }

        return result;
    }

    /// <summary>
    /// Reads a sequence of wide string values from the stream.
    /// </summary>
    /// <returns>The sequence of string values from the stream.</returns>
    public IList<string> ReadWStringSequence()
    {
        var len = ReadSequenceLength();
        var result = new string[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadWString();
        }

        return result;
    }

    /// <summary>
    /// Reads a array of string values from the stream.
    /// </summary>
    /// <param name="len">The length of the array.</param>
    /// <returns>The array of string values from the stream.</returns>
    public string[] ReadStringArray(int len)
    {
        var result = new string[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadString();
        }

        return result;
    }

    /// <summary>
    /// Reads a array of string values from the stream.
    /// </summary>
    /// <param name="len">The length of the array.</param>
    /// <returns>The array of string values from the stream.</returns>
    public string[] ReadWStringArray(int len)
    {
        var result = new string[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadWString();
        }

        return result;
    }

    /// <summary>
    /// Reads a sequence of enumeration values from the stream.
    /// </summary>
    /// <returns>The sequence of enumeration values from the stream.</returns>
    public IList<uint> ReadEnumSequence()
    {
        var len = ReadSequenceLength();
        var result = new uint[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadEnum();
        }

        return result;
    }

    /// <summary>
    /// Reads an array of enumeration values from the stream.
    /// </summary>
    /// <param name="len">The length of the array.</param>
    /// <returns>The array of enumeration values from the stream.</returns>
    public uint[] ReadEnumArray(int len)
    {
        var result = new uint[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadEnum();
        }

        return result;
    }

    private void Align(int alignment)
    {
        var modulo = _position % alignment;
        if (modulo > 0)
        {
            ReadBytes(alignment - modulo);
        }
    }

    private uint ReadSequenceLength() => ReadUInt32();

    /// <summary>
    /// Converts the specified 32-bit signed integer to a single-precision floating point number.
    /// </summary>
    /// <param name="value">The number to convert.</param>
    /// <returns>A single-precision floating point number whose bits are identical to <paramref name="value"/>.</returns>
    private static unsafe float Int32BitsToSingle(int value) => *((float*)&value);

    /// <summary>
    /// Converts the specified 64-bit signed integer to a double-precision floating point number.
    /// </summary>
    /// <param name="value">The number to convert.</param>
    /// <returns>A double-precision floating point number whose bits are identical to <paramref name="value"/>.</returns>
    private static unsafe double Int64BitsToDouble(long value) => *((double*)&value);
}