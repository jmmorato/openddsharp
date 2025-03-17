using System;
using System.Buffers.Binary;
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
    /// Reads a byte from the stream.
    /// </summary>
    /// <returns>The byte value.</returns>
    public byte ReadByte()
    {
        return _buf[_position++];
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
    /// Read an enumeration value from the stream.
    /// </summary>
    /// <returns>The unsigned integer that represents the enumeration.</returns>
    public uint ReadEnum() => ReadUInt32();

    /// <summary>
    /// Reads a sequence length from the stream.
    /// </summary>
    /// <returns>The sequence length value.</returns>
    public uint ReadSequenceLength() => ReadUInt32();

    private void Align(int alignment)
    {
        var modulo = _position % alignment;
        if (modulo > 0)
        {
            ReadBytes(alignment - modulo);
        }
    }

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