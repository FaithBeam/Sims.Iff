using sims_iff.Enums;
using sims_iff.Interfaces;

namespace sims_iff.Models.ResourceContent.Str.Format;

public class Fdff(
    StrFormat format,
    short numberEntries,
    List<CodeStringPair> codeStringPairs,
    int[] extraData
) : IStr
{
    public StrFormat Format { get; set; } = format;
    public short NumberEntries { get; set; } = numberEntries;
    public List<CodeStringPair> CodeStringPairs { get; set; } = codeStringPairs;
    public int[] ExtraData { get; set; } = extraData;

    public static Fdff Load(StrFormat strFormat, Stream stream)
    {
        var numberEntries = stream.ReadInt16();
        var codeStringPairs = ReadCodeStringPairs(stream, numberEntries);
        var extraData = ReadExtraData(stream);
        return new Fdff(strFormat, numberEntries, codeStringPairs, extraData);
    }

    public void Write(Stream stream)
    {
        Format.Write(stream);
        stream.WriteInt16(NumberEntries);
        foreach (var csp in CodeStringPairs)
        {
            csp.Write(stream);
        }

        if (ExtraData is not null)
        {
            foreach (var b in ExtraData)
            {
                stream.WriteByte((byte)b);
            }
        }
    }

    private static int[] ReadExtraData(Stream stream)
    {
        var bytes = new List<int>();
        while (stream.Peek() is 0xA3 or 0x00 or 0x14)
        {
            bytes.AddRange(stream.ReadUntilValueChanges());
        }

        return bytes.ToArray();
    }

    private static List<CodeStringPair> ReadCodeStringPairs(Stream stream, int numberEntries)
    {
        var l = new List<CodeStringPair>();
        for (var i = 0; i < numberEntries; i++)
        {
            l.Add(CodeStringPair.Read(stream));
        }

        return l;
    }
}
