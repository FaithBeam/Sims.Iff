using sims_iff.Enums;
using sims_iff.Interfaces;
using sims_iff.Models.ResourceContent.Str.Format;

namespace sims_iff.Models.ResourceContent.Str;

public static class Str
{
    public static IStr Read(Stream stream)
    {
        var strFormat = (StrFormat)stream.ReadInt16(endianness: Endianness.Big);
        switch (strFormat)
        {
            case StrFormat.Fdff:
                return Fdff.Load(strFormat, stream);
            case StrFormat.Zero:
            case StrFormat.Ffff:
            case StrFormat.Feff:
            case StrFormat.Fcff:
            default:
                throw new Exception($"Unknown Str Format exception {strFormat}");
        }
    }
}
