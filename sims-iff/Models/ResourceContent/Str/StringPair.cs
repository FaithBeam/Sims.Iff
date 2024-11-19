using System.Diagnostics;

namespace sims_iff.Models.ResourceContent.Str;

[DebuggerDisplay("Data = {Data}; Notes = {Notes}")]
public class StringPair(string? data, string? notes)
{
    public static StringPair Read(Stream stream)
    {
        var data = stream.ReadStringUntilValue(0);
        string? notes = null;
        if (HasComment(stream))
        {
            notes = stream.ReadStringUntilValue(0);
        }
        else
        {
            stream.Position++;
        }
        return new StringPair(data, notes);
    }

    public void Write(Stream stream)
    {
        stream.WriteString(Data + char.MinValue);
        stream.WriteString(Notes + char.MinValue);
    }

    private static bool HasComment(Stream stream) => stream.Peek() != 0;

    public string? Data { get; set; } = data;
    public string? Notes { get; set; } = notes;
}
