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
        /*public static explicit operator sbyte(BigFloat bigFloat) => Clamp(bigFloat, sbyte.MinValue, sbyte.MaxValue);
        public static explicit operator short(BigFloat bigFloat) => Clamp(bigFloat, short.MinValue, short.MaxValue);
        public static explicit operator int(BigFloat bigFloat) => Clamp(bigFloat, int.MinValue, int.MaxValue);
        public static explicit operator long(BigFloat bigFloat) => Clamp(bigFloat, long.MinValue, long.MaxValue);
        public static explicit operator Int128(BigFloat bigFloat) => Clamp(bigFloat, Int128.MinValue, Int128.MaxValue);*/

        //Целочисленные беззнаковые типы данных
        public static implicit operator BigFloat(byte value) => new(value);
        public static implicit operator BigFloat(ushort value) => new(value);
        public static implicit operator BigFloat(uint value) => new(value);
        public static implicit operator BigFloat(ulong value) => new(value);
        public static implicit operator BigFloat(UInt128 value) => new(value);
        /*public static explicit operator byte(BigFloat bigFloat) => Clamp(bigFloat, byte.MinValue, byte.MaxValue);
        public static explicit operator ushort(BigFloat bigFloat) => Clamp(bigFloat, ushort.MinValue, ushort.MaxValue);
        public static explicit operator uint(BigFloat bigFloat) => Clamp(bigFloat, uint.MinValue, uint.MaxValue);
        public static explicit operator ulong(BigFloat bigFloat) => Clamp(bigFloat, ulong.MinValue, ulong.MaxValue);
        public static explicit operator UInt128(BigFloat bigFloat) => Clamp(bigFloat, UInt128.MinValue, UInt128.MaxValue);*/

        //Числа с плавающей точкой
        public static implicit operator BigFloat(float value) => new(value);
        public static implicit operator BigFloat(double value) => new(value);
        public static implicit operator BigFloat(decimal value) => new(value);
        /*public static explicit operator float(BigFloat bigFloat) => Clamp(bigFloat, float.MinValue, float.MaxValue);
        public static explicit operator double(BigFloat bigFloat) => Clamp(bigFloat, double.MinValue, double.MaxValue);
        public static explicit operator decimal(BigFloat bigFloat) => Clamp(bigFloat, decimal.MinValue, decimal.MaxValue);*/

        //Общие приведения типов
        public static implicit operator BigFloat(string value) => new(value);
        public static explicit operator string(BigFloat bigFloat) => bigFloat.ToString();


        //Конструкторы класса
        //Целочисленные знаковые типы данных
        public BigFloat(sbyte value) => InitializeFromInteger(value);
        public BigFloat(short value) => InitializeFromInteger(value);
        public BigFloat(int value) => InitializeFromInteger(value);
        public BigFloat(long value) => InitializeFromInteger(value);
        public BigFloat(Int128 value) => InitializeFromInteger(value);

        //Целочисленные беззнаковые типы данных
        public BigFloat(byte value) => InitializeFromInteger(value);
        public BigFloat(ushort value) => InitializeFromInteger(value);
        public BigFloat(uint value) => InitializeFromInteger(value);
        public BigFloat(ulong value) => InitializeFromInteger(value);
        public BigFloat(UInt128 value) => InitializeFromInteger(value);


        //Числа с плавающей точкой
        public BigFloat(float value)
        {
            string valueStr = Convert.ToDecimal(value).ToString(System.Globalization.CultureInfo.InvariantCulture);
            string[] partsNum = valueStr.Split(',', '.');
            this.value = EncodeDPD(partsNum[0] + partsNum[1]);
            scale = partsNum[1].Length;
        }
        public BigFloat(double value)
        {
            string valueStr = Convert.ToDecimal(value).ToString(System.Globalization.CultureInfo.InvariantCulture);
            string[] partsNum = valueStr.Split(',', '.');

            string intPart = partsNum[0];
            string fracPart = partsNum.Length > 1 ? partsNum[1] : "";

            this.value = EncodeDPD(intPart + fracPart);
            scale = fracPart.Length;
        }
        public BigFloat(decimal value)
        {
            var valueStr = value.ToString(System.Globalization.CultureInfo.InvariantCulture);
            var partsNum = valueStr.Split(',', '.');
            this.value = EncodeDPD(partsNum[0] + partsNum[1]);
            scale = partsNum[1].Length;
        }

        //Общий конструктор
        public BigFloat(string value)
        {
            if(String.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException("Value is null or empty");

            int dotPosition = value.IndexOfAny([',', '.']);


            string intValue;
            string fracValue;

            if(dotPosition > 0)
            {
                intValue = value.Substring(0, value.IndexOfAny([',', '.']));
                fracValue = value.Substring(value.IndexOfAny([',', '.']) + 1);
            }
            else
            {
                intValue = value;
                fracValue = "0";
            }

            this.value = EncodeDPD(intValue + fracValue);
            this.scale = fracValue.Length;
        }

        public BigFloat()
        {
            value = new BitArray(21, false);
        }

        //Дополнительные функции для приведения целочисленных типов
        private void InitializeFromInteger(BigInteger value)
        {
            this.value = new BitArray(11, false);
            this.value = EncodeDPD(value.ToString());
            this.value.Set(0, value < 0);
        }

        //Метод для ограничивания BigFloat для привидения типа
        /*private static T Clamp<T>(BigFloat value, T min, T max) where T : struct, IComparable
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
                return (T)(object)float.Parse((value.value[0] ? "-" : "") + value.ToString());
            if (typeOfT == typeof(double))
                return (T)(object)double.Parse((value.value[0] ? "-" : "") + value.ToString());
            if (typeOfT == typeof(decimal))
                return (T)(object)decimal.Parse((value.value[0] ? "-" : "") + value.ToString());
            if (typeOfT == typeof(Int128))
                return (T)(object)Int128.Parse((value.value[0] ? "-" : "") + value.IntegerPart.ToString());
            if (typeOfT == typeof(UInt128))
                return (T)(object)UInt128.Parse(value.IntegerPart.ToString());

            long a = long.Parse(value.IntegerPart.ToString());


            return (T)Convert.ChangeType(a, typeOfT);
        }*/

        private BitArray EncodeDPD(string value)
        {
            int len = (value.Length % 3) - 1;
            len += (len < 0 ? int.Abs(len) : 0);

            value = new string('0', len) + value;
            BitArray rez = new(10 * (value.Length / 3) + 1, false);

            string[] n = new string[value.Length / 3];

            for(int i = 0, j = 0; i < value.Length / 3; i++, j++)
            {
                n[j] += value[3 * i];
                n[j] += value[3 * i + 1];
                n[j] += value[3 * i + 2];
            }

            for (int j = 0; j < n.Length; j++)
            {
                bool[] flags = new bool[3];

                for (int i = 0; i < 3; i++)
                {
                    flags[i] = false;
                    if ((n[j][i] - '0') >= 8)
                        flags[i] = true;
                }

                if (!flags[0])
                {
                    if (!flags[1])
                    {
                        if (!flags[2])
                        {
                            rez.Set(0 + 10 * j + 1, (n[j][0] - '0').ToString("B4")[1] == '1');
                            rez.Set(1 + 10 * j + 1, (n[j][0] - '0').ToString("B4")[2] == '1');
                            rez.Set(2 + 10 * j + 1, (n[j][0] - '0').ToString("B4")[3] == '1');
                            rez.Set(3 + 10 * j + 1, (n[j][1] - '0').ToString("B4")[1] == '1');
                            rez.Set(4 + 10 * j + 1, (n[j][1] - '0').ToString("B4")[2] == '1');
                            rez.Set(5 + 10 * j + 1, (n[j][1] - '0').ToString("B4")[3] == '1');
                            rez.Set(6 + 10 * j + 1, false);
                            rez.Set(7 + 10 * j + 1, (n[j][2] - '0').ToString("B4")[1] == '1');
                            rez.Set(8 + 10 * j + 1, (n[j][2] - '0').ToString("B4")[2] == '1');
                            rez.Set(9 + 10 * j + 1, (n[j][2] - '0').ToString("B4")[3] == '1');
                        }                
                        else             
                        {                
                            rez.Set(0 + 10 * j + 1, (n[j][0] - '0').ToString("B4")[1] == '1');
                            rez.Set(1 + 10 * j + 1, (n[j][0] - '0').ToString("B4")[2] == '1');
                            rez.Set(2 + 10 * j + 1, (n[j][0] - '0').ToString("B4")[3] == '1');
                            rez.Set(3 + 10 * j + 1, (n[j][1] - '0').ToString("B4")[1] == '1');
                            rez.Set(4 + 10 * j + 1, (n[j][1] - '0').ToString("B4")[2] == '1');
                            rez.Set(5 + 10 * j + 1, (n[j][1] - '0').ToString("B4")[3] == '1');
                            rez.Set(6 + 10 * j + 1, true);
                            rez.Set(7 + 10 * j + 1, false);
                            rez.Set(8 + 10 * j + 1, false);
                            rez.Set(9 + 10 * j + 1, (n[j][2] - '0').ToString("B4")[3] == '1');
                        }
                    }
                    else
                    {
                        if (!flags[2])
                        {
                            rez.Set(0 + 10 * j + 1, (n[j][0] - '0').ToString("B4")[1] == '1');
                            rez.Set(1 + 10 * j + 1, (n[j][0] - '0').ToString("B4")[2] == '1');
                            rez.Set(2 + 10 * j + 1, (n[j][0] - '0').ToString("B4")[3] == '1');
                            rez.Set(3 + 10 * j + 1, (n[j][2] - '0').ToString("B4")[1] == '1');
                            rez.Set(4 + 10 * j + 1, (n[j][2] - '0').ToString("B4")[2] == '1');
                            rez.Set(5 + 10 * j + 1, (n[j][1] - '0').ToString("B4")[3] == '1');
                            rez.Set(6 + 10 * j + 1, true);
                            rez.Set(7 + 10 * j + 1, false);
                            rez.Set(8 + 10 * j + 1, true);
                            rez.Set(9 + 10 * j + 1, (n[j][2] - '0').ToString("B4")[3] == '1');
                        }
                        else
                        {
                            rez.Set(0 + 10 * j + 1, (n[j][0] - '0').ToString("B4")[1] == '1');
                            rez.Set(1 + 10 * j + 1, (n[j][0] - '0').ToString("B4")[2] == '1');
                            rez.Set(2 + 10 * j + 1, (n[j][0] - '0').ToString("B4")[3] == '1');
                            rez.Set(3 + 10 * j + 1, true);
                            rez.Set(4 + 10 * j + 1, false);
                            rez.Set(5 + 10 * j + 1, (n[j][1] - '0').ToString("B4")[3] == '1');
                            rez.Set(6 + 10 * j + 1, true);
                            rez.Set(7 + 10 * j + 1, true);
                            rez.Set(8 + 10 * j + 1, true);
                            rez.Set(9 + 10 * j + 1, (n[j][2] - '0').ToString("B4")[3] == '1');
                        }
                    }
                }
                else
                {
                    if (!flags[1])
                    {
                        if (!flags[2])
                        {
                            rez.Set(0 + 10 * j + 1, (n[j][2] - '0').ToString("B4")[1] == '1');
                            rez.Set(1 + 10 * j + 1, (n[j][2] - '0').ToString("B4")[2] == '1');
                            rez.Set(2 + 10 * j + 1, (n[j][0] - '0').ToString("B4")[3] == '1');
                            rez.Set(3 + 10 * j + 1, (n[j][1] - '0').ToString("B4")[1] == '1');
                            rez.Set(4 + 10 * j + 1, (n[j][1] - '0').ToString("B4")[2] == '1');
                            rez.Set(5 + 10 * j + 1, (n[j][1] - '0').ToString("B4")[3] == '1');
                            rez.Set(6 + 10 * j + 1, true);
                            rez.Set(7 + 10 * j + 1, true);
                            rez.Set(8 + 10 * j + 1, false);
                            rez.Set(9 + 10 * j + 1, (n[j][2] - '0').ToString("B4")[3] == '1');
                        }
                        else
                        {
                            rez.Set(0 + 10 * j + 1, (n[j][1] - '0').ToString("B4")[1] == '1');
                            rez.Set(1 + 10 * j + 1, (n[j][1] - '0').ToString("B4")[2] == '1');
                            rez.Set(2 + 10 * j + 1, (n[j][0] - '0').ToString("B4")[3] == '1');
                            rez.Set(3 + 10 * j + 1, false);
                            rez.Set(4 + 10 * j + 1, true);
                            rez.Set(5 + 10 * j + 1, (n[j][1] - '0').ToString("B4")[3] == '1');
                            rez.Set(6 + 10 * j + 1, true);
                            rez.Set(7 + 10 * j + 1, true);
                            rez.Set(8 + 10 * j + 1, true);
                            rez.Set(9 + 10 * j + 1, (n[j][2] - '0').ToString("B4")[3] == '1');
                        }
                    }
                    else
                    {
                        if (!flags[2])
                        {
                            rez.Set(0 + 10 * j + 1, (n[j][2] - '0').ToString("B4")[1] == '1');
                            rez.Set(1 + 10 * j + 1, (n[j][2] - '0').ToString("B4")[2] == '1');
                            rez.Set(2 + 10 * j + 1, (n[j][0] - '0').ToString("B4")[3] == '1');
                            rez.Set(3 + 10 * j + 1, false);
                            rez.Set(4 + 10 * j + 1, false);
                            rez.Set(5 + 10 * j + 1, (n[j][1] - '0').ToString("B4")[3] == '1');
                            rez.Set(6 + 10 * j + 1, true);
                            rez.Set(7 + 10 * j + 1, true);
                            rez.Set(8 + 10 * j + 1, true);
                            rez.Set(9 + 10 * j + 1, (n[j][2] - '0').ToString("B4")[3] == '1');
                        }
                        else
                        {
                            rez.Set(0 + 10 * j + 1, false);
                            rez.Set(1 + 10 * j + 1, false);
                            rez.Set(2 + 10 * j + 1, (n[j][0] - '0').ToString("B4")[3] == '1');
                            rez.Set(3 + 10 * j + 1, true);
                            rez.Set(4 + 10 * j + 1, true);
                            rez.Set(5 + 10 * j + 1, (n[j][1] - '0').ToString("B4")[3] == '1');
                            rez.Set(6 + 10 * j + 1, true);
                            rez.Set(7 + 10 * j + 1, true);
                            rez.Set(8 + 10 * j + 1, true);
                            rez.Set(9 + 10 * j + 1, (n[j][2] - '0').ToString("B4")[3] == '1');
                        }
                    }
                }
            }

            return rez;
        }

        public string DecodeDPD()
        {
            BigFloat a = this;
            int correct = 0;
            if (a.Length % 10 == 1)
                correct = 1;

            BitArray value = a.value;
            string rez = String.Empty;
            
            for (int j = 0; j < value.Length / 10; j++)
            {
                string flags = $"{(value[10 * j + 6 + correct] ? 1 : 0)}{(value[10 * j + 7 + correct] ? 1 : 0)}{(value[10 * j + 8 + correct] ? 1 : 0)}{(value[10 * j + 3 + correct] ? 1 : 0)}{(value[10 * j + 4 + correct] ? 1 : 0)}";

                string r;


                if (flags[0] == '0')
                {
                    r = ((4 * (value[0 + 10 * j + correct] ? 1 : 0)) + (2 * (value[1 + 10 * j + correct] ? 1 : 0)) + (value[2 + 10 * j + correct] ? 1 : 0)).ToString();
                    r += ((4 * (value[3 + 10 * j + correct] ? 1 : 0)) + (2 * (value[4 + 10 * j + correct] ? 1 : 0)) + (value[5 + 10 * j + correct] ? 1 : 0)).ToString();
                    r += ((4 * (value[7 + 10 * j + correct] ? 1 : 0)) + (2 * (value[8 + 10 * j + correct] ? 1 : 0)) + (value[9 + 10 * j + correct] ? 1 : 0)).ToString();
                }
                else
                {
                    if (flags[..3] == "100")
                    {
                        r = ((4 * (value[0 + 10 * j + correct] ? 1 : 0)) + (2 * (value[1 + 10 * j + correct] ? 1 : 0)) + (value[2 + 10 * j + correct] ? 1 : 0)).ToString();
                        r += ((4 * (value[3 + 10 * j + correct] ? 1 : 0)) + (2 * (value[4 + 10 * j + correct] ? 1 : 0)) + (value[5 + 10 * j + correct] ? 1 : 0)).ToString();
                        r += (8 + (value[9 + 10 * j + correct] ? 1 : 0)).ToString();
                    }
                    else if (flags[..3] == "101")
                    {
                        r = ((4 * (value[0 + 10 * j + correct] ? 1 : 0)) + (2 * (value[1 + 10 * j + correct] ? 1 : 0)) + (value[2 + 10 * j + correct] ? 1 : 0)).ToString();
                        r += (8 + (value[5 + 10 * j + correct] ? 1 : 0));
                        r += ((4 * (value[3 + 10 * j + correct] ? 1 : 0)) + (2 * (value[4 + 10 * j + correct] ? 1 : 0)) + (value[9 + 10 * j + correct] ? 1 : 0)).ToString();
                    }
                    else if (flags[..3] == "110")
                    {
                        r = (8 + (value[2 + 10 * j + correct] ? 1 : 0)).ToString();
                        r += ((4 * (value[3 + 10 * j + correct] ? 1 : 0)) + (2 * (value[4 + 10 * j + correct] ? 1 : 0)) + (value[5 + 10 * j + correct] ? 1 : 0)).ToString();
                        r += ((4 * (value[0 + 10 * j + correct] ? 1 : 0)) + (2 * (value[1 + 10 * j + correct] ? 1 : 0)) + (value[9 + 10 * j + correct] ? 1 : 0)).ToString();
                    }
                    else
                    {
                        if (flags[4..] == "00")
                        {
                            r = (8 + (value[2 + 10 * j + correct] ? 1 : 0)).ToString();
                            r += (8 + (value[5 + 10 * j + correct] ? 1 : 0)).ToString();
                            r += ((4 * (value[0 + 10 * j + correct] ? 1 : 0)) + (2 * (value[1 + 10 * j + correct] ? 1 : 0)) + (value[9 + 10 * j + correct] ? 1 : 0)).ToString();
                        }
                        else if (flags[4..] == "01")
                        {
                            r = (8 + (value[2 + 10 * j + correct] ? 1 : 0)).ToString();
                            r += ((4 * (value[0 + 10 * j + correct] ? 1 : 0)) + (2 * (value[1 + 10 * j + correct] ? 1 : 0)) + (value[5 + 10 * j + correct] ? 1 : 0)).ToString();
                            r += (8 + (value[9 + 10 * j + correct] ? 1 : 0)).ToString();
                        }
                        else if (flags[4..] == "10")
                        {
                            r = ((4 * (value[0 + 10 * j + correct] ? 1 : 0)) + (2 * (value[1 + 10 * j + correct] ? 1 : 0)) + (value[2 + 10 * j + correct] ? 1 : 0)).ToString();
                            r += (8 + (value[5 + 10 * j + correct] ? 1 : 0)).ToString();
                            r += (8 + (value[9 + 10 * j + correct] ? 1 : 0)).ToString();
                        }
                        else
                        {
                            r = (8 + (value[2 + 10 * j + correct] ? 1 : 0)).ToString();
                            r += (8 + (value[5 + 10 * j + correct] ? 1 : 0)).ToString();
                            r += (8 + (value[9 + 10 * j + correct] ? 1 : 0)).ToString();
                        }
                    }
                }

                rez += r;
            }

            int remZero = 0;

            for (; remZero < a.Length - a.scale; remZero++)
            {
                if (rez[remZero] != '0')
                {
                    break;
                }
            }

            return rez[remZero..];
        }
    }
}
