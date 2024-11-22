using System.Diagnostics;
using sims_iff.Enums;

namespace sims_iff.Models.ResourceContent.Rsmp;

[DebuggerDisplay("{TypeCode}")]
public class TypeList(TypeCode typeCode, int numberListEntries, List<ListEntry> listEntries)
{
    public TypeCode TypeCode { get; set; } = typeCode;
    public int NumberListEntries { get; set; } = numberListEntries;
    public List<ListEntry> ListEntries { get; set; } = listEntries;

    public static TypeList Read(Stream stream, int version)
    {
        var typeCode = TypeCode.Read(stream);
        var numberListEntries = stream.ReadInt32();
        var listEntries = new List<ListEntry>();

        for (var i = 0; i < numberListEntries; i++)
        {
            listEntries.Add(ListEntry.Load(stream, version));

            // TODO SUSPECT
            // version 0 fields are aligned on even boundaries. Move the stream position.
            if (version == 0 && stream.Position % 2 != 0)
            {
                stream.Position++;
            }
        }

        return new TypeList(typeCode, numberListEntries, listEntries);
    }

    public void Write(Stream stream, long startingPos)
    {
        TypeCode.Write(stream, Endianness.Big);
        stream.WriteInt32(NumberListEntries);
        foreach (var le in ListEntries)
        {
            le.Write(stream, startingPos);
        }
    }
}
