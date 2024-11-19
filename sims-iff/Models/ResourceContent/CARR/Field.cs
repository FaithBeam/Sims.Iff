using System.Collections;
using System.Diagnostics;
using SharpBitStream;
using sims_iff.Models.ResourceContent.Str;

namespace sims_iff.Models.ResourceContent.CARR;

[DebuggerDisplay("{Value}")]
public class Field(int value)
{
    public static Field Read(BitStream bs)
    {
        if (bs.ReadUnsigned(1) == 0)
        {
            return new Field(0);
        }

        var code = bs.ReadUnsigned(2);
        var width = Widths[code];
        return new Field((int)bs.ReadSigned(width + 1));
    }

    public void Write(BitStream bs)
    {
        var fieldBits = GetFieldBits(Value);
        foreach (var bit in fieldBits)
        {
            bs.WriteUnsigned(1, (ulong)bit);
        }
    }

    public int Value { get; set; } = value;

    public static implicit operator CarType(Field f) => (CarType)f.Value;

    private static readonly int[] Widths = [5, 10, 20, 31];
    private static readonly int[][] WidthBits =
    [
        [1, 0, 0],
        [1, 0, 1],
        [1, 1, 0],
        [1, 1, 1],
    ];

    private static int[] GetFieldBits(int value)
    {
        if (value == 0)
        {
            return [0];
        }

        var isNegative = value < 0;
        if (isNegative)
        {
            value = -value;
        }
        var ba = new BitArray([value]).Cast<bool>().Select(x => x ? 1 : 0).ToList();
        var valBitWidth = 0;
        for (var i = 0; i < ba.Count; i++)
        {
            if (ba[i] == 1)
            {
                valBitWidth = i;
            }
        }

        var bitWidth = Widths.First(x => x > valBitWidth);

        // perform two's complement
        if (isNegative)
        {
            // invert bits
            ba = ba.Select(x => x == 0 ? 1 : 0).ToList();

            // add 1 to least significant bit
            int carry;
            var idx = 0;
            do
            {
                var result = ba[idx] + 1;
                ba[idx] = result % 2;
                carry = result == 2 ? 1 : 0;
                idx++;
            } while (carry != 0);
        }

        var prefix = WidthBits[Array.IndexOf(Widths, bitWidth)];
        // prefix + sign + bitwidth
        var newArr = new int[3 + 1 + bitWidth];
        // set prefix
        newArr[0] = prefix[0];
        newArr[1] = prefix[1];
        newArr[2] = prefix[2];
        // set sign bit
        newArr[3] = isNegative ? 1 : 0;

        // reverse and copy values to new array
        for (var i = 0; i < bitWidth; i++)
        {
            var idx = i + 4;
            var other = bitWidth - i - 1;
            newArr[idx] = ba[other];
        }
        return newArr;
    }
}
