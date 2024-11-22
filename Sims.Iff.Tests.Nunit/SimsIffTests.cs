using System.Buffers.Binary;
using System.Collections;
using sims_iff.Models;
using sims_iff.Models.ResourceContent.CARR;

namespace sims_iff.Tests;

public class SimsIffTests
{
    [Test]
    public void TestRead()
    {
        var path = "work.iff";
        var iff = Iff.Read(path);
        ;
    }

    [Test]
    public void TestWrite()
    {
        var path = "work.iff";
        var iff = Iff.Read(path);
        var carrs = iff
            .Resources.Where(r => r.TypeCode.Value == "CARR")
            .ToList()
            .Select(r => (Carr)r.Content)
            .First(x => x.JobInfos.Any(y => y.JobName == "Ringmaster"));
        carrs.JobInfos.Last().EnergyDecay.Value = 0;
        iff.Write("test.iff");
    }
}
