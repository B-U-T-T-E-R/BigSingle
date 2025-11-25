using System.Collections;
using System.Numerics;

namespace BigSingle.BigSingleLibrary
{
    public enum BigFloatState
    {
        Finite,
        PositiveInfinity,
        NegativeInfinity,
        NaN
    }
    public partial class BigFloat
    {
        public BigInteger value = 0;
        public int scale = 0;
        private int _accuracy = 32;
        private BigFloatState _state = BigFloatState.Finite;

        public int Accuracy
        {
            get
            {
                return _accuracy;
            }

            set
            {
                if (value >= 0)
                    _accuracy = value;
                else if (value < 0)
                    throw new Exception("Precision cannot be negative");
            }
        }

        public static readonly BigFloat Zero = new ("0");
        public static readonly BigFloat One = new ("1");
        public int Length => value.ToString().Length;
        public int LengthFractionalPart => scale;
        public int LengthIntegerPart => value.ToString().Length - scale < 0 ? 0 : value.ToString().Length - scale;

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
            switch (_state)
            {
                case BigFloatState.NaN:
                    return "NaN";
                case BigFloatState.PositiveInfinity:
                    return "Infinity";
                case BigFloatState.NegativeInfinity:
                    return "-Infinity";
            }

            string intPart = value.ToString()[..LengthIntegerPart];
            string fracPart = value.ToString()[LengthIntegerPart..];

            if (intPart.Length == 0)
                intPart = "0";

            int scaleZero = scale - fracPart.Length;

            scaleZero = scaleZero < 0 ? 0 : scaleZero;

            fracPart = new string('0', scaleZero) + fracPart;

            if(fracPart.Length == 0)
                fracPart = "0";

            fracPart = fracPart.TrimEnd('0');

            if (value == 0)
                return "0";
            if (fracPart == "")
                return intPart;

            return $"{intPart},{fracPart}";
        }
    }
}
