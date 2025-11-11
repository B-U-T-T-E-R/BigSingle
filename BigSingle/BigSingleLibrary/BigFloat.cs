using System.Collections;
using System.Numerics;
using System.Text;

namespace BigSingle.BigSingleLibrary
{
    public partial class BigFloat
    {
        public BitArray value = new (1, false);
        public int scale = 0;

        public static readonly BigFloat Zero = new (0.0);
        public static readonly BigFloat One = new (1.0);
        public int Length => value.Length;
        public int LengthDPD => value.Length / 3;
        public int LengthFractionalPart => scale;
        public int LengthIntegerPart => value.Length / 3 - scale;

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj is null)
            {
                return false;
            }

            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
        public override string ToString()
        {
            string num = DecodeDPD(this);
            string integerPart = num.Substring(0, num.Length - scale);
            string fracPart = num.Substring(num.Length - scale);

            return $"{integerPart},{fracPart}";
        }
    }
}
