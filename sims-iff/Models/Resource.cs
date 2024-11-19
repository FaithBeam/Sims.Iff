using System.Diagnostics;
using System.Resources;
using System.Text;
using sims_iff.Enums;
using sims_iff.Interfaces;
using sims_iff.Models.ResourceContent.CARR;
using sims_iff.Models.ResourceContent.Rsmp;
using sims_iff.Models.ResourceContent.Str;

namespace sims_iff.Models;

[DebuggerDisplay("{TypeCode.Value}: {Name}")]
public class Resource(
    ResourceContent.TypeCode typeCode,
    int size,
    ushort id,
    ushort flags,
    string name,
    IResourceContent content
)
{
    public static Resource Load(Stream stream)
    {
        var typeCode = ResourceContent.TypeCode.Read(stream);
        var size = stream.ReadInt32(endianness: Endianness.Big);
        var id = stream.ReadUInt16(endianness: Endianness.Big);
        var flags = stream.ReadUInt16(endianness: Endianness.Big);
        var name = stream.ReadString(64).Replace("\0", "");
        var content = ReadResourceContent(stream, typeCode);
        return new Resource(typeCode, size, id, flags, name, content);
    }

    [DebuggerDisplay("{TypeCode.Value}")]
    public Models.ResourceContent.TypeCode TypeCode { get; set; } = typeCode;

    public int Size { get; set; } = size;
    public ushort Id { get; set; } = id;
    public ushort Flags { get; set; } = flags;
    public string Name { get; set; } = name;
    public IResourceContent Content { get; set; } = content;

    public void Write(Stream stream)
    {
        TypeCode.Write(stream);
        stream.WriteInt32(Size, endianness: Endianness.Big);
        stream.WriteUInt16(Id, Endianness.Big);
        stream.WriteUInt16(Flags, Endianness.Big);
        var nameBytes = Encoding.UTF8.GetBytes(Name);
        var neededNullTerminations = 64 - nameBytes.Length;
        if (neededNullTerminations > 0)
        {
            var newNameBytes = new byte[neededNullTerminations + nameBytes.Length];
            nameBytes.CopyTo(newNameBytes, 0);
            stream.Write(newNameBytes);
        }
        else
        {
            stream.WriteString(Name);
        }
        Content.Write(stream);
    }

    private static IResourceContent ReadResourceContent(
        Stream stream,
        Models.ResourceContent.TypeCode tc
    ) =>
        tc.Value switch
        {
            "rsmp" => ResourceMap.Load(stream),
            "CARR" => Carr.Read(stream),
            "STR#" => Str.Read(stream),
            _ => throw new ArgumentOutOfRangeException(nameof(tc), tc, null),
        };
}
