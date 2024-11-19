using sims_iff.Enums;

namespace sims_iff.Models;

public class Iff(string signature, int offsetToResourceMap, List<Resource> resources)
{
    public string Signature { get; set; } = signature;
    public int OffsetToResourceMap { get; set; } = offsetToResourceMap;
    public List<Resource> Resources { get; set; } = resources;

    public void Write(string path)
    {
        using var fs = File.Create(path);
        fs.WriteString(Signature);
        fs.WriteInt32(OffsetToResourceMap, Endianness.Big);
        foreach (var resource in Resources)
        {
            resource.Write(stream: fs);
        }
    }

    /// <summary>
    /// Read an iff file and return an iff object
    /// </summary>
    /// <param name="pathToIff"></param>
    /// <returns></returns>
    public static Iff Read(string pathToIff)
    {
        using var stream = new FileStream(pathToIff, FileMode.Open, FileAccess.Read);
        var signature = stream.ReadString(60);
        var offsetToResourceMap = stream.ReadInt32(endianness: Endianness.Big);
        var resources = new List<Resource>();
        while (stream.Peek() != -1)
        {
            resources.Add(Resource.Load(stream));
        }
        var iff = new Iff(signature, offsetToResourceMap, resources);
        return iff;
    }
}
