using System;

namespace _1_3_1_HashTable
{
    class Program
    {
        static void Main(string[] args)
        {
            var t = new HashTable<string, Object>();

            t.Add("1", 23);
            t.Add("2", "23");
            t.Add("3", "sdsd");
            t.Add("4", 213);
            t.Add("5", 9898);


            var rr = t.Contains("4");
            t.Remove("sadsad");
            t.Remove("5");
            // yukhimoe: had to comment out - not compiled
            //t.Remove(4);
            //t.Remove(3);
            //t.Remove(2);
            t.Add("2", "23");
            t.Add("3", "sdsd");
            t.Add("4", 213);
            t.Add("5", 9898);
            Console.ReadKey();
        }
    }
}
