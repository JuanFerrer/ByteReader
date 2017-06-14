using System;
using System.IO;


namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string funct = args[0];
            string file = args[1];
            if (File.Exists(file))
            {
                switch (funct)
                {
                    case "-s":
                        if (ByteReader.ByteReader.CheckFileSignature(file, args[2]))
                        {
                            Console.WriteLine("Signature " + args[2] + " was found.");
                        }
                        break;
                    case "-l":
                        ByteReader.ByteReader.PrintBytes(file);
                        break;
                    default:
                        break;
                }
            }

            Console.ReadKey();
        }
    }
}
