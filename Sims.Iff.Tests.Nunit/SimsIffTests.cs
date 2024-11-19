using System.Buffers.Binary;
using System.Collections;
using sims_iff.Models;

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
        iff.Write("test.iff");
    }
}
