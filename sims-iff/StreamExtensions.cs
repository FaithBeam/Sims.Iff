using System.Text;
using sims_iff.Enums;

namespace sims_iff;

public static class StreamExtensions
{
    /// <summary>
    /// Read 4 bytes from the stream and convert it to int32
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="offset"></param>
    /// <param name="endianness"></param>
    /// <returns></returns>
    public static int ReadInt32(
        this Stream stream,
        int offset = 0,
        Endianness endianness = Endianness.Little
    ) => BitConverter.ToInt32(ReadBytes(stream, 4, offset, endianness));

    /// <summary>
    /// Write int32 to the stream
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="int"></param>
    /// <param name="endianness"></param>
    public static void WriteInt32(
        this Stream stream,
        int @int,
        Endianness endianness = Endianness.Little
    )
    {
        var bytes = BitConverter.GetBytes(@int);
        if (endianness == Endianness.Big)
        {
            Array.Reverse(bytes);
        }
        stream.Write(bytes);
    }

    /// <summary>
    /// Read 2 bytes from the stream and convert it to ushort
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="offset"></param>
    /// <param name="endianness"></param>
    /// <returns></returns>
    public static ushort ReadUInt16(
        this Stream stream,
        int offset = 0,
        Endianness endianness = Endianness.Little
    ) => BitConverter.ToUInt16(ReadBytes(stream, 2, offset, endianness));

    public static void WriteUInt16(
        this Stream stream,
        ushort @ushort,
        Endianness endianness = Endianness.Little
    )
    {
        var bytes = BitConverter.GetBytes(@ushort);
        if (endianness == Endianness.Big)
        {
            Array.Reverse(bytes);
        }
        stream.Write(bytes);
    }

    /// <summary>
    /// Read 2 bytes from the stream and convert it to short
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="offset"></param>
    /// <param name="endianness"></param>
    /// <returns></returns>
    public static short ReadInt16(
        this Stream stream,
        int offset = 0,
        Endianness endianness = Endianness.Little
    ) => BitConverter.ToInt16(ReadBytes(stream, 2, offset, endianness));

    public static void WriteInt16(
        this Stream stream,
        short @short,
        Endianness endianness = Endianness.Little
    )
    {
        var bytes = BitConverter.GetBytes(@short);
        if (endianness == Endianness.Big)
        {
            Array.Reverse(bytes);
        }
        stream.Write(bytes);
    }

    /// <summary>
    /// Read number of bytes from the stream and convert it to a UTF8 string
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="numBytes"></param>
    /// <returns></returns>
    public static string ReadString(this Stream stream, int numBytes) =>
        Encoding.UTF8.GetString(ReadBytes(stream, numBytes));

    /// <summary>
    /// Read bytes from the stream until the provided value is hit
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static byte[] ReadUntilValue(this Stream stream, int value)
    {
        var bytes = new List<byte>();
        while (true)
        {
            var b = stream.ReadByte();
            if (b == value || b == -1)
            {
                break;
            }

            bytes.Add((byte)b);
        }

        return bytes.ToArray();
    }

    /// <summary>
    /// Read an arbitrary amount of bytes until the value of byte no longer matches the initial read byte
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static int[] ReadUntilValueChanges(this Stream stream)
    {
        var bytes = new List<int>();
        var initialByte = stream.ReadByte();
        if (initialByte == -1)
        {
            throw new Exception("End of stream read exception");
        }
        var curByte = initialByte;
        bytes.Add(initialByte);
        while (curByte == initialByte)
        {
            curByte = stream.ReadByte();
            if (curByte == -1)
            {
                break;
            }

            if (curByte != initialByte)
            {
                stream.Position--;
                break;
            }
            bytes.Add(curByte);
        }

        return bytes.ToArray();
    }

    /// <summary>
    /// Read bytes from the stream until the provided value is hit and convert it to a string
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ReadStringUntilValue(this Stream stream, int value)
    {
        var val = ReadUntilValue(stream, value);
        return string.Concat(val.Select(x => (char)x));
    }

    /// <summary>
    /// Read x number of bytes and optionally provide endianness
    /// </summary>
    /// <returns></returns>
    public static byte[] ReadBytes(
        this Stream stream,
        int numBytes,
        int offset = 0,
        Endianness endianness = Endianness.Little
    )
    {
        var bytes = new byte[numBytes];
        if (stream.Read(bytes, offset, numBytes) != numBytes)
        {
            throw new Exception($"Tried to read more bytes than was left in stream");
        }

        if (endianness == Endianness.Big)
        {
            Array.Reverse(bytes);
        }

        return bytes;
    }

    /// <summary>
    /// Get the next byte in the stream without moving the stream position
    /// </summary>
    /// <param name="stream"></param>
    /// <returns>The value of the next byte. -1 if the end of the stream</returns>
    public static int Peek(this Stream stream)
    {
        var b = stream.ReadByte();
        if (b != -1)
        {
            stream.Position--;
        }

        return b;
    }

    /// <summary>
    /// Write a ASCII string to a stream
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="str"></param>
    /// <param name="endianness"></param>
    public static void WriteString(
        this Stream stream,
        string str,
        Endianness endianness = Endianness.Little
    )
    {
        var b = str.Select(x => (byte)x).ToArray();
        if (endianness == Endianness.Big)
        {
            // var bytes = Encoding.ASCII.GetBytes(str);
            Array.Reverse(b);
        }
        stream.Write(b);
    }
}
