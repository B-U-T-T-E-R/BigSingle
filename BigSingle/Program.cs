using BigSingle.BigSingleLibrary;
using System.Diagnostics;

namespace BigSingle
{
    class Program
    {
        static void Main()
        {
            BigFloat a = "0." + new string('0', 100000) + "2";
            BigFloat b = "0." + new string('0', 100000) + "2";

            Stopwatch sw = Stopwatch.StartNew();
            Console.WriteLine(a + b);
            Console.WriteLine(a - b);
            Console.WriteLine(a * b);
            Console.WriteLine(a / b);
            sw.Stop();

            Console.WriteLine(sw.ElapsedMilliseconds + "ms"); // 4369
        }
    }
}
