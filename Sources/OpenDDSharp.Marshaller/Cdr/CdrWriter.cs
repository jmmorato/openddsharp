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
    /// Writes a byte to the stream.
    /// </summary>
    /// <param name="b">The byte to be written.</param>
    public void WriteByte(byte b) => _writer.Write(b);

    /// <summary>
    /// Writes a sequence of bytes to the stream.
    /// </summary>
    /// <param name="buf">The bytes to be written.</param>
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
    /// Writes a string to the stream.
    /// </summary>
    /// <param name="s">The string to be written.</param>
    public void WriteString(string s)
    {
        WriteUInt32((uint)s.Length + 1);
        WriteBytes(Encoding.UTF8.GetBytes(s));
        WriteByte(0x00);
    }

    /// <summary>
    /// Write an enumeration value to the stream.
    /// </summary>
    /// <param name="enumVal">The enumeration value to be written.</param>
    public void WriteEnum(uint enumVal) => WriteUInt32(enumVal);

    /// <summary>
    /// Write a sequence of booleans to the stream.
    /// </summary>
    /// <param name="sequence">The sequence of booleans to be written.</param>
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
    /// Write a sequence of bytes to the stream.
    /// </summary>
    /// <param name="sequence">The sequence of bytes to be written.</param>
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

        var bytes = Encoding.UTF8.GetBytes(string.Join(string.Empty, sequence));
        WriteSequenceLength((uint)bytes.Length);
        WriteBytes(bytes);
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