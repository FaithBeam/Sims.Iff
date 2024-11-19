using SharpBitStream;
using sims_iff.Interfaces;

namespace sims_iff.Models.ResourceContent.CARR;

public class Carr(
    int field1,
    int field2,
    string rrac,
    CareerInfo careerInfo,
    List<JobInfo> jobInfos
) : IResourceContent
{
    public int Field1 { get; set; } = field1;
    public int Field2 { get; set; } = field2;
    public string Rrac { get; set; } = rrac;
    public CareerInfo CareerInfo { get; set; } = careerInfo;
    public List<JobInfo> JobInfos { get; set; } = jobInfos;

    public static Carr Read(Stream stream)
    {
        var startingPos = stream.Position;
        var bs = new BitStream(stream);
        var field1 = stream.ReadInt32();
        var field2 = stream.ReadInt32();
        var rrac = stream.ReadString(4);
        var careerInfo = CareerInfo.Read(startingPos, stream, bs);
        var jobInfos = new List<JobInfo>();

        for (var i = 0; i < careerInfo.NumberJobLevels.Value; i++)
        {
            jobInfos.Add(JobInfo.Read(startingPos, stream, bs));
        }

        return new Carr(field1, field2, rrac, careerInfo, jobInfos);
    }

    public void Write(Stream stream)
    {
        var startingPos = stream.Position;
        stream.WriteInt32(Field1);
        stream.WriteInt32(Field2);
        stream.WriteString(Rrac);
        var bs = new BitStream(stream);
        CareerInfo.Write(startingPos, stream, bs);
        foreach (var ji in JobInfos)
        {
            ji.Write(startingPos, stream, bs);
        }
    }

    /// <summary>
    /// Align the stream to a 2 byte boundary
    /// Only should be ran after reading a string in CARR
    /// </summary>
    public static void AlignByteBoundary(long startingPos, Stream stream)
    {
        var padding = GetPadding(startingPos, stream);
        if (padding != 0)
        {
            stream.Position += padding;
        }
    }

    public static long GetPadding(long startingPos, Stream stream)
    {
        const int align = 2;
        var offset = Math.Abs(startingPos - stream.Position);
        return (align - offset % align) % align;
    }
}
