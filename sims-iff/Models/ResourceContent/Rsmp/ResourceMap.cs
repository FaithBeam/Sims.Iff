using sims_iff.Interfaces;

namespace sims_iff.Models.ResourceContent.Rsmp;

/// <summary>
/// Resource map
/// All numeric values are little-endian
/// </summary>
public class ResourceMap(
    int field1,
    int version,
    int pmsr,
    int size,
    int numberOfTypes,
    List<TypeList> typeLists
) : IResourceContent
{
    public int Field1 { get; set; } = field1;
    public int Version { get; set; } = version;
    public int Pmsr { get; set; } = pmsr;
    public int Size { get; set; } = size;
    public int NumberOfTypes { get; set; } = numberOfTypes;
    public List<TypeList> TypeLists { get; set; } = typeLists;

    public static ResourceMap Load(Stream stream)
    {
        var field1 = stream.ReadInt32();
        var version = stream.ReadInt32();
        var pmsr = stream.ReadInt32();
        var size = stream.ReadInt32();
        var numberOfTypes = stream.ReadInt32();
        var typeLists = new List<TypeList>();

        for (var i = 0; i < numberOfTypes; i++)
        {
            typeLists.Add(TypeList.Read(stream, version));
        }

        return new ResourceMap(field1, version, pmsr, size, numberOfTypes, typeLists);
    }

    public void Write(Stream stream)
    {
        var startingPos = stream.Position;
        stream.WriteInt32(Field1);
        stream.WriteInt32(Version);
        stream.WriteInt32(Pmsr);
        stream.Position += 4;
        stream.WriteInt32(NumberOfTypes);
        foreach (var typeList in TypeLists)
        {
            typeList.Write(stream, startingPos);
        }
    }
}
