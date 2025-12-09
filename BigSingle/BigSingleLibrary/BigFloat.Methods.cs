using System.Collections;
using System.Numerics;

namespace BigSingle.BigSingleLibrary
{
    public partial class BigFloat
    {
        public static BigFloat Abs(BigFloat value)
        {
            value *= (value < 0 ? -1 : 1);

            return value;
        }
        public static BigFloat Clamp(BigFloat value, BigFloat min, BigFloat max) => value > max ? max : value < min ? min : value;
        public static int Compare(BigFloat left, BigFloat right) => left < right ? -1 : left > right ? 1 : 0;
        public static BigFloat CopySign(BigFloat value, BigFloat sign)
        {
            if(sign < 0)
            {
                value *= (value < 0 ? 1 : -1);
            }
            else
            {
                value *= (value < 0 ? -1 : 1);
            }

            return value;
        }

        private void InitializeFromInteger(BigInteger value)
        {
            this.value = value;
            scale = 0;
        }

        public static BigFloat[] SetAccuracy(int acc, params BigFloat[] value)
        {
            for(int i = 0; i < value.Length; i++)
            {
                value[i].Accuracy = acc;
            }

            return value;
        }

        public static BigFloat SetPrecision(BigFloat value, int precision)
        {
            if(precision < 0)
            {
                return new BigFloat("-1");
            }

            if(precision >= value.scale)
            {
                return value;
            }

            return value.ToString()[..precision];
        }
    }
}
