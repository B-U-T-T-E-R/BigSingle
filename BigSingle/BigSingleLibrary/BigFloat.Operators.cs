using System.Numerics;

namespace BigSingle.BigSingleLibrary
{
    public partial class BigFloat
    {
        public static bool operator ==(BigFloat left, BigFloat right) => left.value == right.value && left.scale == right.scale;
        public static bool operator !=(BigFloat left, BigFloat right)
        {
            return !(left == right);
        }
        public static bool operator <=(BigFloat left, BigFloat right) => left < right || left == right;
        public static bool operator >=(BigFloat left, BigFloat right) => left > right || left == right;
        public static bool operator <(BigFloat left, BigFloat right)
        {
            BigInteger leftNum = left.value;
            BigInteger rightNum = right.value;

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

            return leftNum < rightNum;
        }
        public static bool operator >(BigFloat left, BigFloat right)
        {
            BigInteger leftNum = left.value;
            BigInteger rightNum = right.value;

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

            return leftNum > rightNum;
        }
        public static BigFloat operator +(BigFloat left, BigFloat right)
        {
            BigInteger leftNum = left.value;
            BigInteger rightNum = right.value;

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

            leftNum += rightNum;


            string result = leftNum.ToString();
            bool isNegative = false;
            if (result[0] == '-')
            {
                isNegative = true;
                result = result.Substring(1);
            }

            while (result.Length <= maxScale)
            {
                result = "0" + result;
            }

            string intPart = result.Substring(0, result.Length - maxScale);
            string fracPart = result.Substring(result.Length - maxScale);

            return new BigFloat($"{(isNegative ? "-" : "")}{intPart},{fracPart}");
        }
        public static BigFloat operator -(BigFloat left, BigFloat right)
        {
            BigInteger leftNum = left.value;
            BigInteger rightNum = right.value;

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

            leftNum -= rightNum;


            string result = leftNum.ToString();
            bool isNegative = false;
            if (result[0] == '-')
            {
                isNegative = true;
                result = result.Substring(1);
            }

            while (result.Length <= maxScale)
            {
                result = "0" + result;
            }

            string intPart = result.Substring(0, result.Length - maxScale);
            string fracPart = result.Substring(result.Length - maxScale);

            return new BigFloat($"{(isNegative ? "-" : "")}{intPart},{fracPart}");
        }
        public static BigFloat operator *(BigFloat left, BigFloat right)
        {
            BigInteger leftNum = left.value;
            BigInteger rightNum = right.value;

            BigInteger result = leftNum * rightNum;

            int newscale = left.scale + right.scale;

            string resultStr = result.ToString();
            bool isNegative = false;
            if (resultStr[0] == '-')
            {
                isNegative = true;
                resultStr = resultStr.Substring(1);
            }

            while(resultStr.Length <= newscale)
            {
                resultStr = "0" + resultStr;
            }

            string finalStr = resultStr.Insert(resultStr.Length - newscale, ",");

            return new BigFloat($"{(isNegative ? "-" : "")}{finalStr}");
        }
        public static BigFloat operator /(BigFloat left, BigFloat right)
        {
            BigInteger leftNum = left.value;
            BigInteger rightNum = right.value;

            if (rightNum == 0)
            {
                throw new DivideByZeroException();
            }

            int precision = Math.Max(left.Accuracy, right.Accuracy);

            leftNum *= BigInteger.Pow(10, precision + right.scale);

            BigInteger resultNum = leftNum / rightNum;

            int newScale = left.scale + precision;

            if (newScale < 0)
            {
                resultNum *= BigInteger.Pow(10, Math.Abs(newScale));
                newScale = 0; 
            }

            string resultStr = resultNum.ToString();

            while (resultStr.Length <= newScale)
            {
                resultStr = "0" + resultStr;
            }

            string finalStr = resultStr.Insert(resultStr.Length - newScale, ",");
            finalStr = finalStr.TrimEnd('0');

            return new BigFloat((resultStr[0] == '-' ? "-" : "") + finalStr);
        }
    }
}
