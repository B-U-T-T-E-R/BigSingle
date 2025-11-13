using System.Collections;
using System.Numerics;

namespace BigSingle.BigSingleLibrary
{
    public partial class BigFloat
    {
        public static BigFloat Abs(BigFloat value)
        {
            value.mDPD.Set(0, false);

            return value;
        }
        public static BigFloat Clamp(BigFloat value, BigFloat min, BigFloat max) => value > max ? max : value < min ? min : value;
        public static int Compare(BigFloat left, BigFloat right) => left < right ? -1 : left > right ? 1 : 0;
        public static BigFloat CopySign(BigFloat value, BigFloat sign)
        {
            value.mDPD[0] = sign.mDPD[0];

            return value;
        }

        private void InitializeFromInteger(BigInteger value)
        {
            this.mDPD = new BitArray(11, false);
            this.mDPD = EncodeDPD(value.ToString());
            this.mDPD.Set(0, value < 0);
        }

        private static BitArray EncodeDPD(string value)
        {
            int len = (value.Length % 3) - 1;
            len += (len < 0 ? int.Abs(len) : 0);

            value = new string('0', len) + value;
            BitArray rez = new(10 * (value.Length / 3) + 1, false);

            string[] n = new string[value.Length / 3];

            for (int i = 0, j = 0; i < value.Length / 3; i++, j++)
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

            BitArray value = a.mDPD;
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
                        if (flags[3..] == "00")
                        {
                            r = (8 + (value[2 + 10 * j + correct] ? 1 : 0)).ToString();
                            r += (8 + (value[5 + 10 * j + correct] ? 1 : 0)).ToString();
                            r += ((4 * (value[0 + 10 * j + correct] ? 1 : 0)) + (2 * (value[1 + 10 * j + correct] ? 1 : 0)) + (value[9 + 10 * j + correct] ? 1 : 0)).ToString();
                        }
                        else if (flags[3..] == "01")
                        {
                            r = (8 + (value[2 + 10 * j + correct] ? 1 : 0)).ToString();
                            r += ((4 * (value[0 + 10 * j + correct] ? 1 : 0)) + (2 * (value[1 + 10 * j + correct] ? 1 : 0)) + (value[5 + 10 * j + correct] ? 1 : 0)).ToString();
                            r += (8 + (value[9 + 10 * j + correct] ? 1 : 0)).ToString();
                        }
                        else if (flags[3..] == "10")
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

            int leftZeroRem = 0;

            for (; leftZeroRem < rez.Length; leftZeroRem++)
            {
                if (rez[leftZeroRem] != '0')
                    break;
            }

            int rightZeroRem = rez.Length - 1;
            
            for(;  rightZeroRem >= 0; rightZeroRem--)
            {
                if (rez[rightZeroRem] != '0')
                    break;
            }

            rightZeroRem += 1;

            return rez[leftZeroRem..rightZeroRem];
        }
    }
}
