using System.Collections;
using System.Numerics;
using System.Text;

namespace BigSingle.BigSingleLibrary
{
    public partial class BigFloat
    {
        public BigInteger IntegerPart = 0;
        public StringBuilder FractionalPart = new ('0');
        public BitArray FractionalPart2 = new (8, false);
        public bool Sign;

        public static readonly BigFloat Zero = new (0.0);
        public static readonly BigFloat One = new (1.0);
        public int Length => IntegerPart.ToString().Length + FractionalPart.Length;
        public int LengthFractionalPart => FractionalPart.Length;
        public int LengthIntegerPart => IntegerPart.ToString().Length;

        private static int GetBCDNibbleValue(BitArray arr, int startIndex)
        {
            int bcdDigit = 0;

            bcdDigit += arr[startIndex] ? 8 : 0;
            bcdDigit += arr[startIndex + 1] ? 4 : 0;
            bcdDigit += arr[startIndex + 2] ? 2 : 0;
            bcdDigit += arr[startIndex + 3] ? 1 : 0;

            return bcdDigit;
        }

        private static string BcdArrayToDigitsStringDirect(BitArray bcdArray, int startIndex, bool isIntegerPart)
        {
            int length = bcdArray.Length - startIndex;
            if (length % 4 != 0)
            {
                throw new ArgumentException("Длина массива BCD-данных, начиная с указанного индекса, должна быть кратна 4.");
            }

            if (length == 0 && isIntegerPart)
            {
                return "0"; 
            }

            StringBuilder sb = new ();

            for (int i = startIndex; i < bcdArray.Length; i += 4)
            {
                int digit = GetBCDNibbleValue(bcdArray, i);

                if (digit > 9)
                {
                    throw new ArgumentException($"Обнаружен недопустимый BCD-код (значение {digit}) в позиции {i}.");
                }

                sb.Append(digit);
            }

            if (sb.Length > 1 && sb[0] == '0' && isIntegerPart)
            {
                return sb.ToString().TrimStart('0');
            }

            return sb.ToString();
        }

        public static string ConvertBcdBitArraysToStringDirect(BitArray integerPartBcd, BitArray fractionalPartBcd)
        {
            if (fractionalPartBcd.Length < 4)
            {
                throw new ArgumentException("BitArray дробной части должен содержать как минимум 4 бита для знака.");
            }

            int signCode = GetBCDNibbleValue(fractionalPartBcd, 0);
            string sign = "";

            if (signCode == 13 || signCode == 11)
            {
                sign = "-";
            }

            string integerDigits = BcdArrayToDigitsStringDirect(integerPartBcd, 0, true);

            string fractionalDigits = BcdArrayToDigitsStringDirect(fractionalPartBcd, 4, false);

            if (string.IsNullOrEmpty(fractionalDigits) || fractionalDigits.All(c => c == '0'))
            {
                return $"{sign}{integerDigits}";
            }
            else
            {
                return $"{sign}{integerDigits}.{fractionalDigits}";
            }
        }

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
            return ConvertBcdBitArraysToStringDirect(new BitArray(4), FractionalPart2);
        }
    }
}
