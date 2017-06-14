using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace ByteReader
{
    public static class ByteReader
    {
        public static void PrintBytes(string file)
        {
            byte[] bytes = new byte[1];
            if (File.Exists(file))
                bytes = GetHexFromFile(file);
            else
                Console.WriteLine("File not found");
            for (int i = 0; i < bytes.Length; ++i)
                Console.Write(bytes[i].ToString("X2") + " ");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        public static bool CheckFileSignature(string file, string signature)
        {
            List<byte> signBytes = new List<byte>(System.Text.Encoding.ASCII.GetBytes(signature));
            List<byte> fileBytes = new List<byte>(GetHexFromFile(file));

            // Check whether signBytes is a subset of fileBytes
            return !signBytes.Except(fileBytes).Any();
        }

        public static byte[] GetHexFromFile(string file)
        {
            BinaryReader reader = new BinaryReader(new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.None));
            reader.BaseStream.Position = 0x0;     // The offset you are reading the data from
            byte[] data = ReadAllBytes(reader);
            reader.Close();
            return data;
        }


        public static byte[] ReadAllBytes(BinaryReader reader)
        {
            const int bufferSize = 4096;
            using (var ms = new MemoryStream())
            {
                byte[] buffer = new byte[bufferSize];
                int count;
                while ((count = reader.Read(buffer, 0, buffer.Length)) != 0)
                    ms.Write(buffer, 0, count);
                return ms.ToArray();
            }
        }

        public static void ByteArrayToFile(string fileName, byte[] byteArray)
        {
            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                fs.Write(byteArray, 0, byteArray.Length);
            }
        }
    }
}
