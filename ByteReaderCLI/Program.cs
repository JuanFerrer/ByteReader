using System;
using System.IO;


namespace ByteReaderCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            int cursorPos = Console.CursorTop;
            if (args.Length > 0)
            {
                string funct = args[0].ToLowerInvariant();
                string file = args[1].ToLowerInvariant();
                if (File.Exists(file))
                {
                    switch (funct)
                    {
                        case "-s":
                            if (ByteReader.ByteReader.CheckFileSignature(file, args[2]))
                            {
                                Console.WriteLine("Signature " + args[2] + " was found.");
                            }
                            else
                                Console.WriteLine("Signature " + args[2] + " not found.");
                            break;
                        case "-p":
                            int amount = -1;
                            if (args.Length >= 2)
                                int.TryParse(args[2], out amount);
                            ByteReader.ByteReader.PrintBytes(file, amount);
                            break;
                        default:
                            break;
                        case "-w":
                            string toWrite = "";
                            if (args.Length > 2)
                                toWrite = args[2];
                            ByteReader.ByteReader.ByteArrayToFile(file, ByteReader.ByteReader.StringToByteArray(toWrite));
                            break;
                    }
                }

            }
            else
            {
                Console.WriteLine("Byte reader command-line 2017\nJuan Ferrer\n\n");
                int rowOffset = 4;
                cursorPos += rowOffset;
                Console.SetCursorPosition(0, cursorPos);
                for (int i = 0; i < commandText.Length; ++i)
                {
                    Console.Write(commandText[i]);
                    Console.SetCursorPosition(30, i + cursorPos);
                    Console.Write(helpText[i]);
                    Console.SetCursorPosition(0, i + cursorPos + 1);
                }
            }
        }

        static string[] commandText =
            {"-p <file> [<amount>]",
             "-s <file> <signature> ",
             "-w <file> <text>"};
        static string[] helpText =
            { "Print bytes until amount or end of file is reached.",
              "Check if the signature appears at the beginning of the file.",
              "Write byte string to file"};
    }
}
