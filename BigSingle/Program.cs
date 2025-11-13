using BigSingle.BigSingleLibrary;

namespace BigSingle
{
    class Program
    {
        static void Main()
        {
            BigFloat a = "3,14";

            BigFloat b = BigMath.Pow(a, 2);

            Console.WriteLine(b);
        }
    }
}
