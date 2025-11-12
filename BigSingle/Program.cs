using BigSingle.BigSingleLibrary;

namespace BigSingle
{
    class Program
    {
        static void Main()
        {
            BigFloat a = "3.14";
            BigFloat b = "2.1";

            Console.WriteLine(a / b);

            //1,49523889523889523889523889523889
            //1,4952380952380952380952380952381
        }
    }
}
