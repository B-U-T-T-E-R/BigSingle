using System.Collections;
using System.Numerics;
using System.Text;

namespace BigSingle.BigSingleLibrary
{
    public partial class BigFloat
    {
        //Операторы приведения типов
        //Целочисленные знаковые типы данных
        public static implicit operator BigFloat(sbyte value) => new(value);
        public static implicit operator BigFloat(short value) => new(value);
        public static implicit operator BigFloat(int value) => new(value);
        public static implicit operator BigFloat(long value) => new(value);
        public static implicit operator BigFloat(Int128 value) => new(value);
        public static explicit operator sbyte(BigFloat bigFloat) => Clamp(bigFloat, sbyte.MinValue, sbyte.MaxValue);
        public static explicit operator short(BigFloat bigFloat) => Clamp(bigFloat, short.MinValue, short.MaxValue);
        public static explicit operator int(BigFloat bigFloat) => Clamp(bigFloat, int.MinValue, int.MaxValue);
        public static explicit operator long(BigFloat bigFloat) => Clamp(bigFloat, long.MinValue, long.MaxValue);
        public static explicit operator Int128(BigFloat bigFloat) => Clamp(bigFloat, Int128.MinValue, Int128.MaxValue);

        //Целочисленные беззнаковые типы данных
        public static implicit operator BigFloat(byte value) => new(value);
        public static implicit operator BigFloat(ushort value) => new(value);
        public static implicit operator BigFloat(uint value) => new(value);
        public static implicit operator BigFloat(ulong value) => new(value);
        public static implicit operator BigFloat(UInt128 value) => new(value);
        public static explicit operator byte(BigFloat bigFloat) => Clamp(bigFloat, byte.MinValue, byte.MaxValue);
        public static explicit operator ushort(BigFloat bigFloat) => Clamp(bigFloat, ushort.MinValue, ushort.MaxValue);
        public static explicit operator uint(BigFloat bigFloat) => Clamp(bigFloat, uint.MinValue, uint.MaxValue);
        public static explicit operator ulong(BigFloat bigFloat) => Clamp(bigFloat, ulong.MinValue, ulong.MaxValue);
        public static explicit operator UInt128(BigFloat bigFloat) => Clamp(bigFloat, UInt128.MinValue, UInt128.MaxValue);

        //Числа с плавающей точкой
        public static implicit operator BigFloat(float value) => new(value);
        public static implicit operator BigFloat(double value) => new(value);
        public static implicit operator BigFloat(decimal value) => new(value);
        public static explicit operator float(BigFloat bigFloat) => Clamp(bigFloat, float.MinValue, float.MaxValue);
        public static explicit operator double(BigFloat bigFloat) => Clamp(bigFloat, double.MinValue, double.MaxValue);
        public static explicit operator decimal(BigFloat bigFloat) => Clamp(bigFloat, decimal.MinValue, decimal.MaxValue);

        //Общие приведения типов
        public static implicit operator BigFloat(string value) => new(value);
        public static explicit operator string(BigFloat bigFloat) => bigFloat.ToString();


        //Конструкторы класса
        //Целочисленные знаковые типы данных
        public BigFloat(sbyte value) => InitializeFormInteger(value);
        public BigFloat(short value) => InitializeFormInteger(value);
        public BigFloat(int value) => InitializeFormInteger(value);
        public BigFloat(long value) => InitializeFormInteger(value);
        public BigFloat(Int128 value) => InitializeFormInteger(value);

        //Целочисленные беззнаковые типы данных
        public BigFloat(byte value) => InitializeFormInteger(value);
        public BigFloat(ushort value) => InitializeFormInteger(value);
        public BigFloat(uint value) => InitializeFormInteger(value);
        public BigFloat(ulong value) => InitializeFormInteger(value);
        public BigFloat(UInt128 value) => InitializeFormInteger(value);


        //Числа с плавающей точкой
        public BigFloat(float value)
        {

            var valueStr = Convert.ToDecimal(value).ToString(System.Globalization.CultureInfo.InvariantCulture);
            var partsNum = valueStr.Split(',', '.');
            IntegerPart = BigInteger.Parse(partsNum[0]);
            FractionalPart = new StringBuilder(partsNum[1]);
            Sign = value < 0;
        }
        public BigFloat(double value)
        {
            int valIntPart = (int)value;
            int valFracPart = int.Parse(value.ToString()[(valIntPart.ToString().Length + 1)..]);

            FractionalPart2 = Parse(valFracPart);
        }
        public BigFloat(decimal value)
        {
            var valueStr = value.ToString(System.Globalization.CultureInfo.InvariantCulture);
            var partsNum = valueStr.Split(',', '.');
            IntegerPart = BigInteger.Parse(partsNum[0]);
            FractionalPart = new StringBuilder(partsNum[1]);
            Sign = value < 0;
        }

        //Общий конструктор
        public BigFloat(string value)
        {
            var valueStr = value.ToString(System.Globalization.CultureInfo.InvariantCulture);

            if (value.Contains('-'))
            {
                Sign = true;
                valueStr = valueStr.Replace("-", "");
            }

            if (!valueStr.Contains('.') && !valueStr.Contains(','))
            {
                IntegerPart = BigInteger.Parse(valueStr);
                FractionalPart = new StringBuilder('0');
                return;
            }



            var splitNum = valueStr.Split('.', ',');

            if (splitNum[0].Length < 1)
                IntegerPart = 0;
            else
                IntegerPart = BigInteger.Parse(splitNum[0]);


            FractionalPart = new StringBuilder(splitNum[1]);
        }

        public BigFloat()
        {
            IntegerPart = BigInteger.Zero;
            FractionalPart = new StringBuilder("0");
            Sign = false;
        }

        //Дополнительные функции для приведения целочисленных типов
        private void InitializeFormInteger(BigInteger value)
        {
            IntegerPart = value;
            FractionalPart = new StringBuilder("0");
            Sign = value < 0;
        }

        private void InitializeFromInteger(BigInteger value)
        {
            IntegerPart = BigInteger.Abs(value);
            FractionalPart2 = new BitArray(8, false);
            FractionalPart2.Set(4, value < 0);
        }

        //Метод для ограничивания BigFloat для привидения типа
        private static T Clamp<T>(BigFloat value, T min, T max) where T : struct, IComparable
        {
            //Получаем тип приведения
            Type typeOfT = typeof(T);

            //Ограничиваем BigFloat, если он превышает лимиты, возвращаем максимум или минимум
            if (typeOfT != typeof(float) && typeOfT != typeof(double) && typeOfT != typeof(Int128) && typeOfT != typeof(UInt128))
            {
                if (value.IntegerPart > new BigInteger(Convert.ToDecimal(max))) return max;
                if (value.IntegerPart < new BigInteger(Convert.ToDecimal(min))) return min;
            }
            else if (typeOfT != typeof(Int128) && typeOfT != typeof(UInt128))
            {
                if ((value.IntegerPart + 1) > new BigInteger(Convert.ToDouble(max))) return max;
                if ((value.IntegerPart + 1) < new BigInteger(Convert.ToDouble(min))) return min;
            }
            else if (typeOfT == typeof(Int128))
            {
                if ((value.IntegerPart + 1) > BigInteger.Parse(Int128.Parse(max.ToString()).ToString())) return max;
                if ((value.IntegerPart + 1) < BigInteger.Parse(Int128.Parse(min.ToString()).ToString())) return min;
            }
            else if (typeOfT == typeof(UInt128))
            {
                if ((value.IntegerPart + 1) > BigInteger.Parse(UInt128.Parse(max.ToString()).ToString())) return max;
                if ((value.IntegerPart + 1) < BigInteger.Parse(UInt128.Parse(min.ToString()).ToString())) return min;
            }


            //Приведение типов для чисел с плавающей и фиксированных точек
            if (typeOfT == typeof(float))
                return (T)(object)float.Parse((value.Sign ? "-" : "") + value.ToString());
            if (typeOfT == typeof(double))
                return (T)(object)double.Parse((value.Sign ? "-" : "") + value.ToString());
            if (typeOfT == typeof(decimal))
                return (T)(object)decimal.Parse((value.Sign ? "-" : "") + value.ToString());
            if (typeOfT == typeof(Int128))
                return (T)(object)Int128.Parse((value.Sign ? "-" : "") + value.IntegerPart.ToString());
            if (typeOfT == typeof(UInt128))
                return (T)(object)UInt128.Parse(value.IntegerPart.ToString());

            long a = long.Parse(value.IntegerPart.ToString());


            return (T)Convert.ChangeType(a, typeOfT);
        }

        private BitArray Parse(int value)
        {
            string num = "";
            bool[] bools = new bool[value.ToString().Length * 4 + 4];

            while (value > 0)
            {
                num += (value % 10).ToString("B4");
                value /= 10;
            }

            StringBuilder numBCD = new ();

            for(int i = 0; i < num.Length; i += 4)
            {
                numBCD.Insert(0, num[i..(i + 4)]);
            }

            

            for(int i = 0; i < numBCD.Length; i++)
            {
                bools[i + 4] = numBCD[i] - '0' != 0;
            }

            BitArray bits = new (bools);

            return bits;
        }

        private void ShowBitArr(BitArray bitArr)
        {
            for(int i = 0; i < bitArr.Length / 4; i++)
            {
                for(int j = i * 4; j < i * 4 + 4; j++)
                {
                    Console.Write(bitArr[j] ? 1 : 0);
                }
                Console.Write(" ");
            }
        }
    }
}
