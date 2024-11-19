using System.Diagnostics;
using sims_iff.Models.ResourceContent.CARR;

namespace sims_iff.Models.ResourceContent.Rsmp;

[DebuggerDisplay("{Name}")]
public class ListEntry(
    int offset,
    ushort id,
    int version,
    ushort? unknownField,
    ushort flags,
    string name
)
{
    public int Offset { get; set; } = offset;
    public ushort Id { get; set; } = id;
    public int Version { get; set; } = version;
    public ushort? UnknownField { get; set; } = unknownField;
    public ushort Flags { get; set; } = flags;
    public string Name { get; set; } = name;

    public static ListEntry Load(Stream stream, int version)
    {
        var offset = stream.ReadInt32();
        var id = stream.ReadUInt16();
        ushort? unknownField = null;
        if (version == 1)
        {
            unknownField = BitConverter.ToUInt16(stream.ReadBytes(2));
        }

        var flags = stream.ReadUInt16();
        var name = stream.ReadStringUntilValue(0);
        return new ListEntry(offset, id, version, unknownField, flags, name);
    }

    public void Write(Stream stream, long startingPos)
    {
        stream.WriteInt32(Offset);
        stream.WriteUInt16(Id);
        if (Version == 1)
        {
            stream.WriteUInt16((ushort)UnknownField!);
        }
        stream.WriteUInt16(Flags);
        // may need to pad to fit even boundary
        stream.WriteString(Name + char.MinValue);
        var padding = Carr.GetPadding(startingPos, stream);
        for (var i = 0; i < padding; i++)
        {
            stream.WriteByte(0xA3);
        }
    }
}
