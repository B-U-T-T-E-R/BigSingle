using System.Numerics;
using System.Text;

namespace BigSingle.BigSingleLibrary
{
    public partial class BigFloat
    {
        public static bool operator ==(BigFloat left, BigFloat right)
        {
            BigInteger leftNum = BigInteger.Parse(left.DecodeDPD());
            BigInteger rightNum = BigInteger.Parse(right.DecodeDPD());

            int minScale = int.Min(left.scale, right.scale);
            int maxScale = int.Max(left.scale, right.scale);

            if (leftNum.ToString().Length < rightNum.ToString().Length)
            {
                leftNum *= BigInteger.Pow(10, maxScale - maxScale);
            }
            else if (leftNum.ToString().Length > rightNum.ToString().Length)
            {
                rightNum *= BigInteger.Pow(10, maxScale - maxScale);
            }

            return leftNum == rightNum;
        }
        public static bool operator !=(BigFloat left, BigFloat right)
        {
            return !(left == right);
        }
        public static bool operator <=(BigFloat left, BigFloat right) => left < right || left == right;
        public static bool operator >=(BigFloat left, BigFloat right) => left > right || left == right;
        public static bool operator <(BigFloat left, BigFloat right)
        {
            BigInteger leftNum = BigInteger.Parse(left.DecodeDPD());
            BigInteger rightNum = BigInteger.Parse(right.DecodeDPD());

            int minScale = int.Min(left.scale, right.scale);
            int maxScale = int.Max(left.scale, right.scale);

            if(leftNum.ToString().Length < rightNum.ToString().Length)
            {
                leftNum *= BigInteger.Pow(10, maxScale - maxScale);
            }
            else if (leftNum.ToString().Length > rightNum.ToString().Length)
            {
                rightNum *= BigInteger.Pow(10, maxScale - maxScale);
            }

            return leftNum < rightNum;
        }
        public static bool operator >(BigFloat left, BigFloat right)
        {
            BigInteger leftNum = BigInteger.Parse(left.DecodeDPD());
            BigInteger rightNum = BigInteger.Parse(right.DecodeDPD());

            int minScale = int.Min(left.scale, right.scale);
            int maxScale = int.Max(left.scale, right.scale);

            if (leftNum.ToString().Length < rightNum.ToString().Length)
            {
                leftNum *= BigInteger.Pow(10, maxScale - maxScale);
            }
            else if (leftNum.ToString().Length > rightNum.ToString().Length)
            {
                rightNum *= BigInteger.Pow(10, maxScale - maxScale);
            }

            return leftNum > rightNum;
        }
        public static BigFloat operator +(BigFloat left, BigFloat right)
        {
            BigInteger leftNum = BigInteger.Parse(left.DecodeDPD());
            BigInteger rightNum = BigInteger.Parse(right.DecodeDPD());

            if (left.value[0] == true)
                leftNum *= -1;
            if (right.value[0] == true)
                rightNum *= -1;

            int minScale = int.Min(left.scale, right.scale);
            int maxScale = int.Max(left.scale, right.scale);

            if (leftNum.ToString().Length < rightNum.ToString().Length)
            {
                leftNum *= BigInteger.Pow(10, maxScale - maxScale);
            }
            else if (leftNum.ToString().Length > rightNum.ToString().Length)
            {
                rightNum *= BigInteger.Pow(10, maxScale - maxScale);
            }

            leftNum += rightNum;

            string result = leftNum.ToString();

            return new BigFloat($"{(result[0] == '-' ? "-" : "")}{result.Substring(0, result.Length - maxScale)},{result.Substring(result.Length - maxScale)}");
        }
        public static BigFloat operator -(BigFloat left, BigFloat right)
        {
            BigInteger leftNum = BigInteger.Parse(left.DecodeDPD());
            BigInteger rightNum = BigInteger.Parse(right.DecodeDPD());

            if (left.value[0] == true)
                leftNum *= -1;
            if (right.value[0] == true)
                rightNum *= -1;

            int minScale = int.Min(left.scale, right.scale);
            int maxScale = int.Max(left.scale, right.scale);

            if (leftNum.ToString().Length < rightNum.ToString().Length)
            {
                leftNum *= BigInteger.Pow(10, maxScale - maxScale);
            }
            else if (leftNum.ToString().Length > rightNum.ToString().Length)
            {
                rightNum *= BigInteger.Pow(10, maxScale - maxScale);
            }

            leftNum -= rightNum;

            string result = leftNum.ToString();

            return new BigFloat($"{(result[0] == '-' ? "-" : "")}{result.Substring(0, result.Length - maxScale)},{result.Substring(result.Length - maxScale)}");
        }
        public static BigFloat operator *(BigFloat left, BigFloat right)
        {
            BigInteger leftNum = BigInteger.Parse(left.DecodeDPD());
            BigInteger rightNum = BigInteger.Parse(right.DecodeDPD());

            if (left.value[0] == true)
                leftNum *= -1;
            if (right.value[0] == true)
                rightNum *= -1;

            int minScale = int.Min(left.scale, right.scale);
            int maxScale = int.Max(left.scale, right.scale);

            if (leftNum.ToString().Length < rightNum.ToString().Length)
            {
                leftNum *= BigInteger.Pow(10, maxScale - maxScale);
            }
            else if (leftNum.ToString().Length > rightNum.ToString().Length)
            {
                rightNum *= BigInteger.Pow(10, maxScale - maxScale);
            }

            leftNum *= rightNum;

            string result = leftNum.ToString();

            return new BigFloat($"{(result[0] == '-' ? "-" : "")}{result.Substring(0, result.Length - (maxScale + minScale))},{result.Substring(result.Length - (maxScale + minScale))}");
        }
        public static BigFloat operator /(BigFloat left, BigFloat right)
        {
            BigInteger leftNum = BigInteger.Parse(left.DecodeDPD());
            BigInteger rightNum = BigInteger.Parse(right.DecodeDPD());

            int overallAccuracy = left.Accuracy > right.Accuracy ? left.Accuracy : right.Accuracy;

            if (left.value[0] == true)
                leftNum *= -1;
            if (right.value[0] == true)
                rightNum *= -1;

            int minScale = int.Min(left.scale, right.scale);
            int maxScale = int.Max(left.scale, right.scale);

            if (leftNum.ToString().Length < rightNum.ToString().Length)
            {
                leftNum *= BigInteger.Pow(10, maxScale - minScale);
            }
            else if (leftNum.ToString().Length > rightNum.ToString().Length)
            {
                rightNum *= BigInteger.Pow(10, maxScale - minScale);
            }

            leftNum *= BigInteger.Pow(10, overallAccuracy);

            leftNum /= rightNum;

            string result = leftNum.ToString();

            int lenFrac = maxScale - minScale;

            result = $"{(result[0] == '-' ? "-" : "")}{result.Substring(0, lenFrac)},{result.Substring(lenFrac)}";

            int i = result.Length - 1;
            for (; i >= 0; i--)
            {
                if (result[i] != '0')
                    break;
            }

            return result[..(i + 1)];
        }
    }
}
