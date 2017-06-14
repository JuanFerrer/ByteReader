using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace ByteReader
{
    public static class ByteReader
    {
        /// <summary>
        /// Print bytes until either amount or end of file is reached. -1 goes until the end of the file
        /// </summary>
        /// <param name="file"></param>
        /// <param name="amount"></param>
        public static void PrintBytes(string file, int amount = -1)
        {
            byte[] bytes = new byte[1];
            if (File.Exists(file))
                bytes = GetHexFromFile(file);
            else
                throw new FileNotFoundException("\"" + file + "\"" + " was not found");
            int maxI = amount >= 0 && amount <= bytes.Length? amount : bytes.Length;
            for (int i = 0; i < bytes.Length; ++i)
                Console.Write(bytes[i].ToString("X2") + " ");
        }

        /// <summary>
        /// Find whether signature appears at the beginning of the file
        /// </summary>
        /// <param name="file"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        public static bool CheckFileSignature(string file, string signature)
        {
            if (File.Exists(file))
            {
                if (signature == "")
                    return false;

                // List<byte> signBytes = new List<byte>(System.Text.Encoding.ASCII.GetBytes(signature));
                // List<byte> fileBytes = new List<byte>(GetHexFromFile(file));
                // Check whether signBytes is a subset of fileBytes
                // return !signBytes.Except(fileBytes).Any();

                byte[] fileBytes = GetHexFromFile(file);
                byte[] signBytes = StringToByteArray(signature);

                for (int i = 0; i < signBytes.Length; ++i)
                {
                    if (signBytes[i] != fileBytes[i])
                        return false;
                }
                return true;
            }
            else
                throw new FileNotFoundException("\"" + file + "\"" + " was not found");
        }

        /// <summary>
        /// Get byte array from file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static byte[] GetHexFromFile(string file)
        {
            if (File.Exists(file))
            {
                BinaryReader reader = new BinaryReader(new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.None));
                reader.BaseStream.Position = 0x0;     // The offset you are reading the data from
                byte[] data = ReadAllBytes(reader);
                reader.Close();
                return data;
            }
            else
                throw new FileNotFoundException("\"" + file + "\"" + " was not found");
        }


        private static byte[] ReadAllBytes(BinaryReader reader)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ba"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string ByteArrayToString(byte[] ba, string separator = " ")
        {
            var shb = new System.Runtime.Remoting.Metadata.W3cXsd2001.SoapHexBinary(ba);
            return System.Text.RegularExpressions.Regex.Replace(shb.ToString(), ".{2}", separator);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static byte[] StringToByteArray(string hex)
        {
            var shb = System.Runtime.Remoting.Metadata.W3cXsd2001.SoapHexBinary.Parse(hex);
            return shb.Value;

        }

        /// <summary>
        /// Write byte array to a file.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="byteArray"></param>
        public static void ByteArrayToFile(string fileName, byte[] byteArray)
        {
            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                fs.Write(byteArray, 0, byteArray.Length);
            }
        }
    }
}
