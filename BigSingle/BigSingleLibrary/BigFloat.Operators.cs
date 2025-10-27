using System.Numerics;
using System.Text;

namespace BigSingle.BigSingleLibrary
{
    public partial class BigFloat
    {
        public static bool operator ==(BigFloat left, BigFloat right) => left.Equals(right);
        public static bool operator !=(BigFloat left, BigFloat right) => !left.Equals(right);
        public static bool operator <=(BigFloat left, BigFloat right) => left < right || left == right;
        public static bool operator >=(BigFloat left, BigFloat right) => left > right || left == right;
        public static bool operator <(BigFloat left, BigFloat right)
        {
            if (left.IntegerPart != right.IntegerPart)
                return left.IntegerPart < right.IntegerPart;

            BigInteger fracL = BigInteger.Parse(left.FractionalPart.ToString());
            BigInteger fracR = BigInteger.Parse(right.FractionalPart.ToString());

            int scaleL = left.FractionalPart.ToString().IndexOf('0');
            int scaleR = right.FractionalPart.ToString().IndexOf('0');

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

            BigInteger fracL = BigInteger.Parse(left.FractionalPart.ToString());
            BigInteger fracR = BigInteger.Parse(right.FractionalPart.ToString());

            int scaleL = left.FractionalPart.ToString().IndexOf('0');
            int scaleR = right.FractionalPart.ToString().IndexOf('0');

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

            int scale = left.FractionalPart.ToString().IndexOf('0') > right.FractionalPart.ToString().IndexOf('0') ? right.FractionalPart.ToString().IndexOf('0') : left.FractionalPart.ToString().IndexOf('0');

            return $"{leftAnd},{new string('0', scale)}{BigInteger.Parse(left.FractionalPart.ToString()) & BigInteger.Parse(left.FractionalPart.ToString())}";
        }
        public static BigFloat operator |(BigFloat left, BigFloat right)
        {
            BigInteger leftAnd = left.IntegerPart | right.IntegerPart;

            int scale = left.FractionalPart.ToString().IndexOf('0') > right.FractionalPart.ToString().IndexOf('0') ? right.FractionalPart.ToString().IndexOf('0') : left.FractionalPart.ToString().IndexOf('0');

            return $"{leftAnd},{new string('0', scale)}{BigInteger.Parse(left.FractionalPart.ToString()) | BigInteger.Parse(left.FractionalPart.ToString())}";
        }
        public static BigFloat operator ^(BigFloat left, BigFloat right)
        {
            BigInteger leftAnd = left.IntegerPart ^ right.IntegerPart;

            int scale = left.FractionalPart.ToString().IndexOf('0') > right.FractionalPart.ToString().IndexOf('0') ? right.FractionalPart.ToString().IndexOf('0') : left.FractionalPart.ToString().IndexOf('0');

            return $"{leftAnd},{new string('0', scale)}{BigInteger.Parse(left.FractionalPart.ToString()) ^ BigInteger.Parse(left.FractionalPart.ToString())}";
        }

        //TODO реализовать +, -, *, /
        public static BigFloat operator +(BigFloat a, BigFloat b)
        {
            int scale = a.FractionalPart.ToString().Length - b.FractionalPart.ToString().Length;
            BigInteger remainder = new();

            BigInteger[] aNum;
            BigInteger[] bNum;

            if (scale < 0)
            {
                aNum = [a.IntegerPart, BigInteger.Parse(a.FractionalPart.ToString())];
                bNum = [b.IntegerPart, BigInteger.Parse(b.FractionalPart.ToString()[Math.Abs(scale)..])];
                remainder = BigInteger.Parse(b.FractionalPart.ToString()[..Math.Abs(scale)]);
            }
            else if (scale > 0)
            {
                aNum = [a.IntegerPart, BigInteger.Parse(a.FractionalPart.ToString()[Math.Abs(scale)..])];
                bNum = [b.IntegerPart, BigInteger.Parse(b.FractionalPart.ToString())];
                remainder = BigInteger.Parse(a.FractionalPart.ToString()[..Math.Abs(scale)]);
            }
            else
            {
                aNum = [a.IntegerPart, BigInteger.Parse(a.FractionalPart.ToString())];
                bNum = [b.IntegerPart, BigInteger.Parse(b.FractionalPart.ToString())];
            }

            int lenFractionalPart = aNum[1].ToString().Length;
            for (int i = 0; i < aNum.Length; i++)
            {
                aNum[i] += (new BigInteger[2])[i];
            }

            if ((remainder * BigInteger.Parse(Math.Pow(10, aNum[1].ToString().Length - 1).ToString()) + aNum[1]).ToString().Length > aNum.Length)
            {
                aNum[0] += 1;
                return new BigFloat($"{aNum[0]},{aNum[1].ToString()[1..]}");
            }
            else if (aNum[1].ToString().Length > lenFractionalPart)
            {
                aNum[0] += 1;
                return new BigFloat($"{aNum[0]},{aNum[1].ToString()[1..]}");
            }

            return new BigFloat($"{aNum[0]},{aNum[1]}");
        }
        public static BigFloat operator -(BigFloat a, BigFloat b)
        {
            BigInteger exponent = a.IntegerPart - b.IntegerPart;
            StringBuilder mantissa = new StringBuilder().Append("");

            int min = a.FractionalPart.Length > b.FractionalPart.Length ? b.FractionalPart.Length : a.FractionalPart.Length;

            for (int i = 0; i < min; i++)
            {
                mantissa = mantissa.Append((a.FractionalPart[i] - '0') - (b.FractionalPart[i] - '0'));
            }

            if (a.FractionalPart.Length < b.FractionalPart.Length)
            {
                var rangeFractionalPart = b.FractionalPart.ToString()[min..];
                mantissa = mantissa.Append(rangeFractionalPart);
            }

            if (BigInteger.Parse("1" + a.FractionalPart) < BigInteger.Parse("1" + b.FractionalPart))
                exponent--;

            int len = b.FractionalPart.Length - a.FractionalPart.Length;
            len = Math.Abs(len);
            //len = a.FractionalPart.Length == b.FractionalPart.Length ? len : len - 1;

            if (mantissa[0] == '-')
                mantissa = mantissa.Replace("-", "");

            return new BigFloat($"{exponent},{new String('0', len)}{mantissa}");
        }
        public static BigFloat operator *(BigFloat a, BigFloat b)
        {
            BigInteger aBI = BigInteger.Parse($"{a.IntegerPart}{a.FractionalPart}");
            BigInteger bBI = BigInteger.Parse($"{b.IntegerPart}{b.FractionalPart}");

            BigInteger result = aBI * bBI;

            int lenFractionalPart = (a.FractionalPart.ToString().Length + b.FractionalPart.ToString().Length - 3);

            int indexOfTheEndOfTheFractionalPart = result.ToString().IndexOf('0', lenFractionalPart + 2);
            int indexOfTheStartOfTheFractionalPart = (result.ToString().Length - lenFractionalPart - 3);

            indexOfTheStartOfTheFractionalPart = indexOfTheEndOfTheFractionalPart < 0 ? 0 : indexOfTheStartOfTheFractionalPart;
            indexOfTheEndOfTheFractionalPart = indexOfTheEndOfTheFractionalPart < 0 ? result.ToString().Length : indexOfTheEndOfTheFractionalPart;

            string rightNum = result.ToString()[(indexOfTheStartOfTheFractionalPart)..indexOfTheEndOfTheFractionalPart];
            string leftNum = result.ToString()[..indexOfTheStartOfTheFractionalPart];

            return new BigFloat($"{leftNum},{rightNum}");
        }
        public static BigFloat operator -(BigFloat a) => new ($"{-a.IntegerPart},{a.FractionalPart}");
    }
}
