using System.Numerics;

namespace BigSingle.BigSingleLibrary
{
    public partial class BigFloat
    {
        public static BigFloat Abs(BigFloat value)
        {
            value.value.Set(0, false);

            return value;
        }
        //public static BigFloat Clamp(BigFloat value, BigFloat min, BigFloat max) => value > max ? max : value < min ? min : value;
        //public static int Compare(BigFloat left, BigFloat right) => left < right ? -1 : left > right ? 1 : 0;
        //public static BigFloat CopySign(BigFloat value, BigFloat sign) => value.value[0] == sign.value[0] ? value : new BigFloat($"{(sign.value[0] ? "-" : "")}{BigInteger.Abs(value.IntegerPart)},{value.value}");
    }
}
