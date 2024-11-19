using System.Text;
using sims_iff.Enums;

namespace sims_iff.Models.ResourceContent;

public class TypeCode
{
    public string Value { get; init; }
    public Endianness Endianness { get; init; }

    private TypeCode(string value, Endianness endianness = Endianness.Little)
    {
        Value = value;
        Endianness = endianness;
    }

    public static TypeCode Read(Stream stream)
    {
        var bytes = new byte[4];
        if (stream.Read(bytes, 0, 4) != 4)
        {
            throw new Exception("Reading type code failed");
        }

        var typeCodeStr = Encoding.UTF8.GetString(bytes);
        return typeCodeStr switch
        {
            "rsmp" => RsmpBe,
            "pmsr" => RsmpLe,
            "CARR" => CarrBe,
            "RRAC" => CarrLe,
            "STR#" => StrBe,
            "#RTS" => StrLe,
            _ => throw new ArgumentOutOfRangeException(nameof(typeCodeStr), typeCodeStr, null),
        };
    }

    public void Write(Stream stream, Endianness endianness = Endianness.Little)
    {
        var bytes = Encoding.UTF8.GetBytes(Value);
        if (endianness == Endianness.Big)
        {
            Array.Reverse(bytes);
        }
        stream.Write(bytes);
    }

    private static TypeCode RsmpLe => new("rsmp");
    private static TypeCode RsmpBe => new("rsmp", Endianness.Big);
    private static TypeCode CarrLe => new("CARR");
    private static TypeCode CarrBe => new("CARR", Endianness.Big);
    private static TypeCode StrLe => new("STR#");
    private static TypeCode StrBe => new("STR#", Endianness.Big);
}
