using BigSingle.BigSingleLibrary;
using System.Diagnostics;

namespace BigSingle
{
    class Program
    {
        static void Main()
        {
            Stopwatch sw = Stopwatch.StartNew();
            Console.WriteLine(BigMath.Pow(2, 3.14, 1000));
            sw.Stop();
            Console.WriteLine($"Прошло: {sw.Elapsed}");
        }
    }
}
