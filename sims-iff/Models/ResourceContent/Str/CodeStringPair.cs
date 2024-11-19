using System.Diagnostics;
using sims_iff.Enums;

namespace sims_iff.Models.ResourceContent.Str;

[DebuggerDisplay("Language Code = {LanguageCode}, StringPair = {StringPair}")]
public class CodeStringPair(LanguageCode languageCode, StringPair stringPair)
{
    public LanguageCode LanguageCode { get; set; } = languageCode;
    public StringPair StringPair { get; set; } = stringPair;

    public static CodeStringPair Read(Stream stream)
    {
        return new CodeStringPair((LanguageCode)stream.ReadByte(), StringPair.Read(stream));
    }

    public void Write(Stream stream)
    {
        stream.WriteByte((byte)LanguageCode);
        StringPair.Write(stream);
    }
}
