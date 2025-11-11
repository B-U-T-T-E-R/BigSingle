using System.Numerics;
using System.Text;

namespace BigSingle.BigSingleLibrary
{
    public partial class BigFloat
    {
        /*public static bool operator ==(BigFloat left, BigFloat right) => left.Equals(right);
        public static bool operator !=(BigFloat left, BigFloat right) => !left.Equals(right);
        public static bool operator <=(BigFloat left, BigFloat right) => left < right || left == right;
        public static bool operator >=(BigFloat left, BigFloat right) => left > right || left == right;
        public static bool operator <(BigFloat left, BigFloat right)
        {
            if (left.IntegerPart != right.IntegerPart)
                return left.IntegerPart < right.IntegerPart;

            BigInteger fracL = BigInteger.Parse(left.value.ToString());
            BigInteger fracR = BigInteger.Parse(right.value.ToString());

            int scaleL = left.value.ToString().IndexOf('0');
            int scaleR = right.value.ToString().IndexOf('0');

            scaleL = scaleL < 0 ? 0 : scaleL;
            scaleR = scaleR < 0 ? 0 : scaleR;

            if (scaleL < scaleR)
            {
                if (fracL < fracR)
                    return true;
                else if (fracL > fracR)
                    return false;
            }
            else
            {
                if (fracL < fracR)
                    return true;
                else if (fracL > fracR)
                    return false;
            }



            return false;
        }
        public static bool operator >(BigFloat left, BigFloat right)
        {
            if (left.IntegerPart != right.IntegerPart)
                return left.IntegerPart > right.IntegerPart;

            BigInteger fracL = BigInteger.Parse(left.value.ToString());
            BigInteger fracR = BigInteger.Parse(right.value.ToString());

            int scaleL = left.value.ToString().IndexOf('0');
            int scaleR = right.value.ToString().IndexOf('0');

            scaleL = scaleL < 0 ? 0 : scaleL;
            scaleR = scaleR < 0 ? 0 : scaleR;

            if (scaleL > scaleR)
            {
                if (fracL > fracR)
                    return true;
                else if (fracL < fracR)
                    return false;
            }
            else
            {
                if (fracL > fracR)
                    return true;
                else if (fracL < fracR)
                    return false;
            }

            return false;
        }
        public static BigFloat operator &(BigFloat left, BigFloat right)
        {
            BigInteger leftAnd = left.IntegerPart & right.IntegerPart;

            int scale = left.value.ToString().IndexOf('0') > right.value.ToString().IndexOf('0') ? right.value.ToString().IndexOf('0') : left.value.ToString().IndexOf('0');

            return $"{leftAnd},{new string('0', scale)}{BigInteger.Parse(left.value.ToString()) & BigInteger.Parse(left.value.ToString())}";
        }
        public static BigFloat operator |(BigFloat left, BigFloat right)
        {
            BigInteger leftAnd = left.IntegerPart | right.IntegerPart;

            int scale = left.value.ToString().IndexOf('0') > right.value.ToString().IndexOf('0') ? right.value.ToString().IndexOf('0') : left.value.ToString().IndexOf('0');

            return $"{leftAnd},{new string('0', scale)}{BigInteger.Parse(left.value.ToString()) | BigInteger.Parse(left.value.ToString())}";
        }
        public static BigFloat operator ^(BigFloat left, BigFloat right)
        {
            BigInteger leftAnd = left.IntegerPart ^ right.IntegerPart;

            int scale = left.value.ToString().IndexOf('0') > right.value.ToString().IndexOf('0') ? right.value.ToString().IndexOf('0') : left.value.ToString().IndexOf('0');

            return $"{leftAnd},{new string('0', scale)}{BigInteger.Parse(left.value.ToString()) ^ BigInteger.Parse(left.value.ToString())}";
        }

        //TODO реализовать +, -, *, /
        public static BigFloat operator +(BigFloat a, BigFloat b)
        {
            return null;
        }
        public static BigFloat operator -(BigFloat a, BigFloat b)
        {
            return null;
        }
        public static BigFloat operator *(BigFloat a, BigFloat b)
        {
            return null;
        }
        public static BigFloat operator -(BigFloat a)
        {
            a.value.Set(0, !a.value[0]);

            return a;
        }*/
    }
}
