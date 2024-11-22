namespace sims_iff.Enums;

public enum StrFormat : short
{
    Zero = 0,
    Ffff = -1,
    Feff = -257,
    Fdff = -513,
    Fcff = -769,
}

public static class StrFormatExtensions
{
    public static void Write(this StrFormat format, Stream stream)
    {
        stream.WriteInt16((short)format, Endianness.Big);
    }
}
