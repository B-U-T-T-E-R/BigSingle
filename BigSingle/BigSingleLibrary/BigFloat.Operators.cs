using System.Numerics;

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

            if (left.Scale < right.Scale)
            {
                leftNum *= BigInteger.Pow(10, maxScale - minScale);
            }
            else if (left.Scale > right.Scale)
            {
                rightNum *= BigInteger.Pow(10, maxScale - minScale);
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

            if(left.Scale < right.Scale)
            {
                leftNum *= BigInteger.Pow(10, maxScale - minScale);
            }
            else if (left.Scale > right.Scale)
            {
                rightNum *= BigInteger.Pow(10, maxScale - minScale);
            }

            return leftNum < rightNum;
        }
        public static bool operator >(BigFloat left, BigFloat right)
        {
            BigInteger leftNum = BigInteger.Parse(left.DecodeDPD());
            BigInteger rightNum = BigInteger.Parse(right.DecodeDPD());

            int minScale = int.Min(left.scale, right.scale);
            int maxScale = int.Max(left.scale, right.scale);

            if (left.Scale < right.Scale)
            {
                leftNum *= BigInteger.Pow(10, maxScale - minScale);
            }
            else if (left.Scale > right.Scale)
            {
                rightNum *= BigInteger.Pow(10, maxScale - minScale);
            }

            return leftNum > rightNum;
        }
        public static BigFloat operator +(BigFloat left, BigFloat right)
        {
            BigInteger leftNum = BigInteger.Parse(left.DecodeDPD());
            BigInteger rightNum = BigInteger.Parse(right.DecodeDPD());

            if (left.mDPD[0] == true)
                leftNum *= -1;
            if (right.mDPD[0] == true)
                rightNum *= -1;

            int minScale = int.Min(left.scale, right.scale);
            int maxScale = int.Max(left.scale, right.scale);

            if (left.Scale < right.Scale)
            {
                leftNum *= BigInteger.Pow(10, maxScale - minScale);
            }
            else if (left.Scale > right.Scale)
            {
                rightNum *= BigInteger.Pow(10, maxScale - minScale);
            }

            leftNum += rightNum;

            string result = leftNum.ToString();

            string intPart = result.Length <= maxScale ? "0" : result.Substring(0, result.Length - maxScale);
            string fracPart = result.Length <= maxScale ? result.Substring(0) : result.Substring(result.Length - maxScale);

            return new BigFloat($"{(result[0] == '-' ? "-" : "")}{intPart},{fracPart}");
        }
        public static BigFloat operator -(BigFloat left, BigFloat right)
        {
            BigInteger leftNum = BigInteger.Parse(left.DecodeDPD());
            BigInteger rightNum = BigInteger.Parse(right.DecodeDPD());

            if (left.mDPD[0] == true)
                leftNum *= -1;
            if (right.mDPD[0] == true)
                rightNum *= -1;

            int minScale = int.Min(left.scale, right.scale);
            int maxScale = int.Max(left.scale, right.scale);

            if (left.Scale < right.Scale)
            {
                leftNum *= BigInteger.Pow(10, maxScale - minScale);
            }
            else if (left.Scale > right.Scale)
            {
                rightNum *= BigInteger.Pow(10, maxScale - minScale);
            }

            leftNum -= rightNum;

            string result = leftNum.ToString();

            return new BigFloat($"{(result[0] == '-' ? "-" : "")}{result.Substring(0, result.Length - maxScale)},{result.Substring(result.Length - maxScale)}");
        }
        public static BigFloat operator *(BigFloat left, BigFloat right)
        {
            BigInteger leftNum = BigInteger.Parse(left.DecodeDPD());
            BigInteger rightNum = BigInteger.Parse(right.DecodeDPD());

            if (left.mDPD[0] == true)
                leftNum *= -1;
            if (right.mDPD[0] == true)
                rightNum *= -1;

            int minScale = int.Min(left.scale, right.scale);
            int maxScale = int.Max(left.scale, right.scale);

            if (left.Scale < right.Scale)
            {
                leftNum *= BigInteger.Pow(10, maxScale - minScale);
            }
            else if (left.Scale > right.Scale)
            {
                rightNum *= BigInteger.Pow(10, maxScale - minScale);
            }

            leftNum *= rightNum;

            string result = leftNum.ToString();

            return new BigFloat($"{(result[0] == '-' ? "-" : "")}{result.Substring(0, result.Length - (maxScale + minScale))},{result.Substring(result.Length - (maxScale + minScale))}");
        }
        public static BigFloat operator *(BigFloat left, int right)
        {
            BigInteger leftNum = BigInteger.Parse(left.DecodeDPD());
            BigInteger rightNum = BigInteger.Parse(right.ToString());

            if (left.mDPD[0] == true)
                leftNum *= -1;

            int maxScale = left.scale;

            leftNum *= rightNum;

            string result = leftNum.ToString();

            return new BigFloat($"{(result[0] == '-' ? "-" : "")}{result.Substring(0, result.Length - (maxScale))},{result.Substring(result.Length - (maxScale))}");
        }
        public static BigFloat operator /(BigFloat left, BigFloat right)
        {
            BigInteger leftNum = BigInteger.Parse(left.DecodeDPD());
            BigInteger rightNum = BigInteger.Parse(right.DecodeDPD());

            if (left.mDPD[0] == true)
                leftNum *= -1;
            if (right.mDPD[0] == true)
                rightNum *= -1;

            int overallAccuracy = left.Accuracy < right.Accuracy ? right.Accuracy : left.Accuracy;

            int minScale = int.Min(left.scale, right.scale);
            int maxScale = int.Max(left.scale, right.scale);

            if (left.scale < right.scale)
            {
                leftNum *= BigInteger.Pow(10, maxScale - minScale);
            }
            else if (left.scale > right.scale)
            {
                rightNum *= BigInteger.Pow(10, maxScale - minScale);
            }

            BigInteger correct = leftNum / rightNum;

            leftNum *= BigInteger.Pow(10, overallAccuracy);

            correct *= BigInteger.Pow(10, overallAccuracy);

            leftNum /= rightNum;

            int lenZero = right.DecodeDPD().Length - left.DecodeDPD().Length;

            string fracRez = new string('0', lenZero < 0 ? 0 : lenZero) + (BigInteger.Abs(leftNum) - BigInteger.Abs(correct)).ToString();

            string intRez = correct.ToString();

            int index = 0;

            for (int i = 0; i < intRez.Length; i++)
            {
                if (intRez[i] != '0')
                    index = i;
            }

            intRez = intRez[..(index + 1)];

            string result = $"{(leftNum < 0 ? "-" : "")}{intRez},{fracRez}";

            int rightRemoveZero = result.Length - 1;
            for (; rightRemoveZero >= result.IndexOf(','); rightRemoveZero--)
            {
                if (result[rightRemoveZero] != '0')
                    break;
                else if (result[rightRemoveZero] == ',')
                {
                    rightRemoveZero = result.Length - 1;
                    break;
                }
            }

            int leftRemoveZero = 0;
            for (; leftRemoveZero < result.IndexOf(',') - 1; leftRemoveZero++)
            {
                if (result[rightRemoveZero] != '0')
                    break;
            }

            return new BigFloat($"{((left.mDPD[0] || right.mDPD[0]) ? "-" : "")}{intRez},{fracRez}");
        }
    }
}
