using System;
using System.Buffers.Binary;
using System.Collections.Generic;
#if NET6_0_OR_GREATER
#else
using System.Runtime.InteropServices;
#endif
using System.Text;

namespace OpenDDSharp.Marshaller.Cdr;

/// <summary>
/// CDR reader.
/// </summary>
public class CdrReader
{
    private readonly ReadOnlyMemory<byte> _buf;
    private int _position;

    /// <summary>
    /// Initializes a new instance of the <see cref="CdrReader"/> class.
    /// </summary>
    /// <param name="buf">The buffer to read from.</param>
    public CdrReader(byte[] buf) : this(new ReadOnlyMemory<byte>(buf))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CdrReader"/> class.
    /// </summary>
    /// <param name="buf">The buffer to read from.</param>
    public CdrReader(ReadOnlyMemory<byte> buf)
    {
        _buf = buf;
        _position = 0;
    }

    public CdrReader()
    {
        _position = 0;
    }

    /// <summary>
    /// Reads an unsigned byte from the stream.
    /// </summary>
    /// <returns>The byte value.</returns>
    public byte ReadByte()
    {
        return _buf.Span[_position++];
    }

    public byte ReadByte(Span<byte> span)
    {
        return span[_position++];
    }

    /// <summary>
    /// Reads a signed byte from the stream.
    /// </summary>
    /// <returns>The byte value.</returns>
    public sbyte ReadSByte()
    {
        return (sbyte)_buf.Span[_position++];
    }

    public sbyte ReadSByte(Span<byte> span)
    {
        return (sbyte)span[_position++];
    }

    /// <summary>
    /// Reads a sequence of bytes from the stream.
    /// </summary>
    /// <param name="count">The number of bytes to read.</param>
    /// <returns>The bytes from the stream.</returns>
    public ReadOnlySpan<byte> ReadBytes(int count)
    {
        var result = _buf.Slice(_position, count).Span;
        _position += count;
        return result;
    }
    public Span<byte> ReadBytes(Span<byte> span, int count)
    {
        var result = span.Slice(_position, count);
        _position += count;
        return result;
    }

    /// <summary>
    /// Read a boolean value from the stream.
    /// </summary>
    /// <returns>The boolean value.</returns>
    public bool ReadBool() => ReadByte() != 0x00;

    public bool ReadBool(Span<byte> span) => ReadByte(span) != 0x00;

    /// <summary>
    /// Reads a signed short from the stream.
    /// </summary>
    /// <returns>The signed short value.</returns>
    public short ReadInt16()
    {
        Align(2);
        return BinaryPrimitives.ReadInt16LittleEndian(ReadBytes(2));
    }

    public short ReadInt16(Span<byte> span)
    {
        Align(span, 2);
        return BinaryPrimitives.ReadInt16LittleEndian(ReadBytes(span, 2));
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

    public ushort ReadUInt16(Span<byte> span)
    {
        Align(span, 2);
        return BinaryPrimitives.ReadUInt16LittleEndian(ReadBytes(span, 2));
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

    public int ReadInt32(Span<byte> span)
    {
        Align(span, 4);
        return BinaryPrimitives.ReadInt32LittleEndian(ReadBytes(span, 4));
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

    public uint ReadUInt32(Span<byte> span)
    {
        Align(span, 4);
        return BinaryPrimitives.ReadUInt32LittleEndian(ReadBytes(span, 4));
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

    public long ReadInt64(Span<byte> span)
    {
        Align(span, 8);
        return BinaryPrimitives.ReadInt64LittleEndian(ReadBytes(span, 8));
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

    public ulong ReadUInt64(Span<byte> span)
    {
        Align(span, 8);
        return BinaryPrimitives.ReadUInt64LittleEndian(ReadBytes(span, 8));
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

    public float ReadSingle(Span<byte> span)
    {
        Align(span, 4);
#if NET6_0_OR_GREATER
        return BinaryPrimitives.ReadSingleLittleEndian(ReadBytes(span, 4));
#else
        return !BitConverter.IsLittleEndian
                ? Int32BitsToSingle(BinaryPrimitives.ReverseEndianness(ReadInt32(span)))
                : MemoryMarshal.Read<float>(ReadBytes(span, 4));
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

    public double ReadDouble(Span<byte> span)
    {
        Align(span, 8);
#if NET6_0_OR_GREATER
        return BinaryPrimitives.ReadDoubleLittleEndian(ReadBytes(span, 8));
#else
        return !BitConverter.IsLittleEndian ?
                Int64BitsToDouble(BinaryPrimitives.ReverseEndianness(ReadInt64(span))) :
                MemoryMarshal.Read<double>(ReadBytes(span, 8));
#endif
    }

    /// <summary>
    /// Reads a character from the stream.
    /// </summary>
    /// <returns>The character value.</returns>
    public char ReadChar() => Convert.ToChar(ReadByte());

    public char ReadChar(Span<byte> span) => Convert.ToChar(ReadByte(span));

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

    public char ReadWChar(Span<byte> span)
    {
        Align(span, 2);
        var c = ReadBytes(span, 2);
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

    public string ReadString(Span<byte> span)
    {
        var len = ReadUInt32(span);

        var strBuf = ReadBytes(span, (int)len - 1);
        ReadByte(span);
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

    public string ReadWString(Span<byte> span)
    {
        var len = ReadUInt32(span);
        var strBuf = ReadBytes(span, (int)len);

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

    public uint ReadEnum(Span<byte> span) => ReadUInt32(span);

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

    public IList<bool> ReadBoolSequence(Span<byte> span)
    {
        var len = ReadSequenceLength(span);
        var result = new bool[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadBool(span);
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

    public bool[] ReadBoolArray(Span<byte> span, int len)
    {
        var result = new bool[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadBool(span);
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

    public IList<byte> ReadByteSequence(Span<byte> span)
    {
        var len = ReadSequenceLength(span);
        return ReadBytes(span, (int)len).ToArray();
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

    public IList<sbyte> ReadSByteSequence(Span<byte> span)
    {
        var len = ReadSequenceLength(span);
        return (sbyte[])(Array)ReadBytes(span, (int)len).ToArray();
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

    public byte[] ReadByteArray(Span<byte> span, int len)
    {
        return ReadBytes(span, len).ToArray();
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

    public sbyte[] ReadSByteArray(Span<byte> span, int len)
    {
        return (sbyte[])(Array)ReadBytes(span, len).ToArray();
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

    public IList<short> ReadInt16Sequence(Span<byte> span)
    {
        var len = ReadSequenceLength(span);
        var result = new short[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadInt16(span);
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

    public short[] ReadInt16Array(Span<byte> span, int len)
    {
        var result = new short[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadInt16(span);
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

    public IList<ushort> ReadUInt16Sequence(Span<byte> span)
    {
        var len = ReadSequenceLength(span);
        var result = new ushort[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadUInt16(span);
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

    public ushort[] ReadUInt16Array(Span<byte> span, int len)
    {
        var result = new ushort[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadUInt16(span);
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

    public IList<int> ReadInt32Sequence(Span<byte> span)
    {
        var len = ReadSequenceLength(span);
        var result = new int[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadInt32(span);
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

    public int[] ReadInt32Array(Span<byte> span, int len)
    {
        var result = new int[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadInt32(span);
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

    public IList<uint> ReadUInt32Sequence(Span<byte> span)
    {
        var len = ReadSequenceLength(span);
        var result = new uint[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadUInt32(span);
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

    public uint[] ReadUInt32Array(Span<byte> span, int len)
    {
        var result = new uint[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadUInt32(span);
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

    public IList<long> ReadInt64Sequence(Span<byte> span)
    {
        var len = ReadSequenceLength(span);
        var result = new long[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadInt64(span);
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

    public long[] ReadInt64Array(Span<byte> span, int len)
    {
        var result = new long[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadInt64(span);
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

    public IList<ulong> ReadUInt64Sequence(Span<byte> span)
    {
        var len = ReadSequenceLength(span);
        var result = new ulong[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadUInt64(span);
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

    public ulong[] ReadUInt64Array(Span<byte> span, int len)
    {
        var result = new ulong[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadUInt64(span);
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

    public IList<float> ReadSingleSequence(Span<byte> span)
    {
        var len = ReadSequenceLength(span);
        var result = new float[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadSingle(span);
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

    public float[] ReadSingleArray(Span<byte> span, int len)
    {
        var result = new float[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadSingle(span);
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

    public IList<double> ReadDoubleSequence(Span<byte> span)
    {
        var len = ReadSequenceLength(span);
        var result = new double[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadDouble(span);
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

    public double[] ReadDoubleArray(Span<byte> span, int len)
    {
        var result = new double[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadDouble(span);
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

    public IList<char> ReadCharSequence(Span<byte> span)
    {
        var len = ReadSequenceLength(span);
        var result = new char[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadChar(span);
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

    public IList<char> ReadWCharSequence(Span<byte> span)
    {
        var len = ReadSequenceLength(span);
        var result = new char[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadWChar(span);
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

    public char[] ReadCharArray(Span<byte> span, int len)
    {
        var result = new char[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadChar(span);
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

    public char[] ReadWCharArray(Span<byte> span, int len)
    {
        var result = new char[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadWChar(span);
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

    public IList<string> ReadStringSequence(Span<byte> span)
    {
        var len = ReadSequenceLength(span);
        var result = new string[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadString(span);
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

    public IList<string> ReadWStringSequence(Span<byte> span)
    {
        var len = ReadSequenceLength(span);
        var result = new string[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadWString(span);
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

    public string[] ReadStringArray(Span<byte> span, int len)
    {
        var result = new string[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadString(span);
        }

        return result;
    }

    /// <summary>
    /// Reads an array of string values from the stream.
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

    public string[] ReadWStringArray(Span<byte> span, int len)
    {
        var result = new string[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadWString(span);
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

    public IList<uint> ReadEnumSequence(Span<byte> span)
    {
        var len = ReadSequenceLength(span);
        var result = new uint[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadEnum(span);
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

    public uint[] ReadEnumArray(Span<byte> span, int len)
    {
        var result = new uint[len];
        for (var i = 0; i < len; i++)
        {
            result[i] = ReadEnum(span);
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

    private void Align(Span<byte> span, int alignment)
    {
        var modulo = _position % alignment;
        if (modulo > 0)
        {
            _position += alignment - modulo;
            // ReadBytes(span, alignment - modulo);
        }
    }

    private uint ReadSequenceLength() => ReadUInt32();

    private uint ReadSequenceLength(Span<byte> span) => ReadUInt32(span);

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