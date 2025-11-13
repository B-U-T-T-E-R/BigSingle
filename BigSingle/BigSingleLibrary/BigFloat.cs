using System.Collections;

namespace BigSingle.BigSingleLibrary
{
    public partial class BigFloat
    {
        public BitArray mDPD = new (1, false);
        public int scale = 0;
        public int Accuracy = (int.MaxValue / 6760) - 1;
        private int _accuracy;

        public int Scale
        {
            get
            {
                return _accuracy;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("The scale cannot be negative.");
                }
                else if (value > (int.MaxValue / 6000 - 1))
                {
                    _accuracy = int.MaxValue / 6000 - 1;
                }
                else
                {
                    _accuracy = value;
                }
            }
        }

        public static readonly BigFloat Zero = new (0.0);
        public static readonly BigFloat One = new (1.0);
        public int Length => mDPD.Length;
        public int LengthDPD => mDPD.Length / 3;
        public int LengthFractionalPart => scale;
        public int LengthIntegerPart => mDPD.Length / 3 - scale;

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
            string num = DecodeDPD();

            if(scale > num.Length)
            {
                return $"{(mDPD[0] ? "-" : "")}{0},{new string('0', scale - num.Length) + num}";
            }

            string integerPart = num[..(num.Length - scale)];
            string fracPart = num[(num.Length - scale)..];



            return $"{(mDPD[0] ? "-" : "")}{integerPart},{fracPart}";
        }
    }
}
