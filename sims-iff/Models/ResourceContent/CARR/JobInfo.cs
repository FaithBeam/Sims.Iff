using SharpBitStream;
using sims_iff.Models.ResourceContent.Str;

namespace sims_iff.Models.ResourceContent.CARR;

public class JobInfo(
    Field friendsRequired,
    Field cookingSkillRequired,
    Field mechanicalSkillRequired,
    Field charismaRequired,
    Field bodySkillRequired,
    Field logicSkillRequired,
    Field creativitySkillRequired,
    Field unknown1,
    Field unknown2,
    Field unknown3,
    Field hungerDecay,
    Field comfortDecay,
    Field hygieneDecay,
    Field bladderDecay,
    Field energyDecay,
    Field funDecay,
    Field socialDecay,
    Field salary,
    Field startTime,
    Field endTime,
    CarType carType,
    string jobName,
    string maleUniformMesh,
    string femaleUniformMesh,
    string uniformSkin,
    string unknown4
)
{
    public static JobInfo Read(long startingPosition, Stream stream, BitStream bs)
    {
        var friendsRequired = Field.Read(bs);
        var cookingSkillRequired = Field.Read(bs);
        var mechanicalSkillRequired = Field.Read(bs);
        var charismaRequired = Field.Read(bs);
        var bodySkillRequired = Field.Read(bs);
        var logicSkillRequired = Field.Read(bs);
        var creativitySkillRequired = Field.Read(bs);
        var unknown1 = Field.Read(bs);
        var unknown2 = Field.Read(bs);
        var unknown3 = Field.Read(bs);
        var hungerDecay = Field.Read(bs);
        var comfortDecay = Field.Read(bs);
        var hygieneDecay = Field.Read(bs);
        var bladderDecay = Field.Read(bs);
        var energyDecay = Field.Read(bs);
        var funDecay = Field.Read(bs);
        var socialDecay = Field.Read(bs);
        var salary = Field.Read(bs);
        var startTime = Field.Read(bs);
        var endTime = Field.Read(bs);
        var carType = (CarType)Field.Read(bs);
        // skip junk bits before next byte
        if (bs.GetPosition().BitPosition != 0)
        {
            stream.Position++;
            bs.SetPosition(new Position(stream.Position, 0));
        }

        var jobName = stream.ReadStringUntilValue(0);
        Carr.AlignByteBoundary(startingPosition, stream);
        var maleUniformMesh = stream.ReadStringUntilValue(0);
        Carr.AlignByteBoundary(startingPosition, stream);
        var femaleUniformMesh = stream.ReadStringUntilValue(0);
        Carr.AlignByteBoundary(startingPosition, stream);
        var uniformSkin = stream.ReadStringUntilValue(0);
        Carr.AlignByteBoundary(startingPosition, stream);
        var unknown4 = stream.ReadStringUntilValue(0);
        Carr.AlignByteBoundary(startingPosition, stream);

        // move the bitstream to catch up to stream
        bs.SetPosition(stream.Position, 0);
        return new JobInfo(
            friendsRequired,
            cookingSkillRequired,
            mechanicalSkillRequired,
            charismaRequired,
            bodySkillRequired,
            logicSkillRequired,
            creativitySkillRequired,
            unknown1,
            unknown2,
            unknown3,
            hungerDecay,
            comfortDecay,
            hygieneDecay,
            bladderDecay,
            energyDecay,
            funDecay,
            socialDecay,
            salary,
            startTime,
            endTime,
            carType,
            jobName,
            maleUniformMesh,
            femaleUniformMesh,
            uniformSkin,
            unknown4
        );
    }

    public Field FriendsRequired { get; set; } = friendsRequired;
    public Field CookingSkillRequired { get; set; } = cookingSkillRequired;
    public Field MechanicalSkillRequired { get; set; } = mechanicalSkillRequired;
    public Field CharismaRequired { get; set; } = charismaRequired;
    public Field BodySkillRequired { get; set; } = bodySkillRequired;
    public Field LogicSkillRequired { get; set; } = logicSkillRequired;
    public Field CreativitySkillRequired { get; set; } = creativitySkillRequired;
    public Field Unknown1 { get; set; } = unknown1;
    public Field Unknown2 { get; set; } = unknown2;
    public Field Unknown3 { get; set; } = unknown3;
    public Field HungerDecay { get; set; } = hungerDecay;
    public Field ComfortDecay { get; set; } = comfortDecay;
    public Field HygieneDecay { get; set; } = hygieneDecay;
    public Field BladderDecay { get; set; } = bladderDecay;
    public Field EnergyDecay { get; set; } = energyDecay;
    public Field FunDecay { get; set; } = funDecay;
    public Field SocialDecay { get; set; } = socialDecay;
    public Field Salary { get; set; } = salary;
    public Field StartTime { get; set; } = startTime;
    public Field EndTime { get; set; } = endTime;
    public CarType CarType { get; set; } = carType;
    public string JobName { get; set; } = jobName;
    public string MaleUniformMesh { get; set; } = maleUniformMesh;
    public string FemaleUniformMesh { get; set; } = femaleUniformMesh;
    public string UniformSkin { get; set; } = uniformSkin;
    public string Unknown4 { get; set; } = unknown4;

    public void Write(long startPosition, Stream stream, BitStream bs)
    {
        FriendsRequired.Write(bs);
        CookingSkillRequired.Write(bs);
        MechanicalSkillRequired.Write(bs);
        CharismaRequired.Write(bs);
        BodySkillRequired.Write(bs);
        LogicSkillRequired.Write(bs);
        CreativitySkillRequired.Write(bs);
        Unknown1.Write(bs);
        Unknown2.Write(bs);
        Unknown3.Write(bs);
        HungerDecay.Write(bs);
        ComfortDecay.Write(bs);
        HygieneDecay.Write(bs);
        BladderDecay.Write(bs);
        EnergyDecay.Write(bs);
        FunDecay.Write(bs);
        SocialDecay.Write(bs);
        Salary.Write(bs);
        StartTime.Write(bs);
        EndTime.Write(bs);
        new Field((int)CarType).Write(bs);

        stream.WriteString(JobName + char.MinValue);
        WritePadding(startPosition, stream);

        stream.WriteString(MaleUniformMesh + char.MinValue);
        WritePadding(startPosition, stream);

        stream.WriteString(FemaleUniformMesh + char.MinValue);
        WritePadding(startPosition, stream);

        stream.WriteString(UniformSkin + char.MinValue);
        WritePadding(startPosition, stream);

        stream.WriteString(Unknown4 + char.MinValue);
        WritePadding(startPosition, stream);

        bs.SetPosition(new Position(stream.Position, 0));
    }

    private void WritePadding(long startPos, Stream stream)
    {
        var padding = Carr.GetPadding(startPos, stream);
        for (var i = 0; i < padding; i++)
        {
            stream.WriteByte(0xA3);
        }
    }
}
