namespace sims_iff.Models;

public static class StringExtensions
{
    public static byte[] GetBytes(this string str) => str.Select(x => (byte)x).ToArray();
}
