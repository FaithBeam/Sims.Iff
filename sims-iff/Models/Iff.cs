using sims_iff.Enums;
using sims_iff.Models.ResourceContent.Rsmp;

namespace sims_iff.Models;

public class Iff(string signature, int offsetToResourceMap, List<Resource> resources)
{
    public string Signature { get; set; } = signature;
    public int OffsetToResourceMap { get; set; } = offsetToResourceMap;
    public List<Resource> Resources { get; set; } = resources;

    public void Write(string path)
    {
        var foundRsmp = Resources.FirstOrDefault(x => x.TypeCode.Value == "rsmp");
        if (foundRsmp is not null)
        {
            Resources.Remove(foundRsmp);
        }

        using var fs = File.Create(path);
        fs.WriteString(Signature);
        fs.Position += 4;
        foreach (var resource in Resources.Where(resource => resource.TypeCode.Value != "rsmp"))
        {
            resource.Write(stream: fs);
        }

        var types = Resources.Select(x => x.TypeCode.Value).Distinct().ToList();
        var rsmpContentNumberOfTypes = types.Count;
        var rsmpContentTypeList = (
            from type in types
            let listEntries = Resources
                .Where(x => x.TypeCode.Value == type)
                .Select(x => new ListEntry((int)x.WrittenOffset, x.Id, 0, null, x.Flags, x.Name))
                .ToList()
            select new TypeList(
                new Models.ResourceContent.TypeCode(type),
                listEntries.Count,
                listEntries
            )
        ).ToList();
        var rsmpContent = new ResourceMap(
            0,
            0,
            BitConverter.ToInt32("pmsr".GetBytes()),
            -1,
            rsmpContentNumberOfTypes,
            rsmpContentTypeList
        );

        var rsmpTypeCode = new Models.ResourceContent.TypeCode("rsmp");
        var rsmpId = (ushort)(Resources.Max(x => x.Id) + 1);
        var rsmpName = string.Concat(new byte[64].Select(x => (char)x));
        var rsmp = new Resource(rsmpTypeCode, -1, rsmpId, 0, rsmpName, rsmpContent);
        rsmp.Write(stream: fs);
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
