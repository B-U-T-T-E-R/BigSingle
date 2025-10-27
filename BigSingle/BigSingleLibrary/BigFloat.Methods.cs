using System.Numerics;
using System.Security.AccessControl;
using System.Text;

namespace BigSingle.BigSingleLibrary
{
    public partial class BigFloat
    {
        public static BigFloat Abs(BigFloat value) => value.Sign ? new BigFloat($"{value.IntegerPart * -1},{value.FractionalPart}") : value;
        public static BigFloat Clamp(BigFloat value, BigFloat min, BigFloat max) => value > max ? max : value < min ? min : value;
        public static int Compare(BigFloat left, BigFloat right) => left < right ? -1 : left > right ? 1 : 0;
        public static BigFloat CopySign(BigFloat value, BigFloat sign) => value.Sign == sign.Sign ? value : new BigFloat($"{(sign.Sign ? "-" : "")}{BigInteger.Abs(value.IntegerPart)},{value.FractionalPart}");
    }
}
