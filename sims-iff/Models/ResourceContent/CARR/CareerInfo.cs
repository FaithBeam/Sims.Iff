using SharpBitStream;

namespace sims_iff.Models.ResourceContent.CARR;

public class CareerInfo(byte compressionCode, string careerName, Field numberJobLevels)
{
    private const int Align = 2;

    public byte CompressionCode { get; set; } = compressionCode;
    public string CareerName { get; set; } = careerName;
    public Field NumberJobLevels { get; set; } = numberJobLevels;

    public static CareerInfo Read(long startingPos, Stream stream, BitStream bs)
    {
        var compressionCode = (byte)stream.ReadByte();
        var careerName = stream.ReadStringUntilValue(0);
        var offset = Math.Abs(startingPos - stream.Position);
        var padding = (Align - offset % Align) % Align;
        if (padding != 0)
        {
            stream.Position += padding;
        }
        // move bitstream to current stream position
        bs.SetPosition(new Position(stream.Position, 0));
        var numberJobLevels = Field.Read(bs);
        return new CareerInfo(compressionCode, careerName, numberJobLevels);
    }

    public void Write(long startingPos, Stream stream, BitStream bs)
    {
        stream.WriteByte(CompressionCode);
        stream.WriteString(CareerName + char.MinValue);
        var offset = Math.Abs(startingPos - stream.Position);
        var padding = (Align - offset % Align) % Align;
        for (var i = 0; i < padding; i++)
        {
            stream.WriteByte(0xA3);
        }
        bs.SetPosition(new Position(stream.Position, 0));
        NumberJobLevels.Write(bs);
    }
}
