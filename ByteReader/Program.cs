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
        /// <param name="file">Full file path</param>
        /// <param name="amount">Number of bytes to be read</param>
        public static void PrintBytes(string file, int amount = -1)
        {
            byte[] bytes = new byte[1];
            if (File.Exists(file))
                bytes = FileToByteArray(file);
            else
                throw new FileNotFoundException("\"" + file + "\"" + " was not found");
            int maxI = amount >= 0 && amount <= bytes.Length ? amount : bytes.Length;
            for (int i = 0; i < maxI; ++i)
                Console.Write(bytes[i].ToString("X2") + " ");
        }

        /// <summary>
        /// Find whether signature appears at the beginning of the file
        /// </summary>
        /// <param name="file">Full file path</param>
        /// <param name="signature">Bytes that are expected to appear at the start of the file</param>
        /// <returns></returns>
        public static bool CheckFileSignature(string file, string signature)
        {
            if (File.Exists(file))
            {
                if (signature == "")
                    return false;

                byte[] fileBytes = FileToByteArray(file);
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
        /// <param name="file">Full file path</param>
        /// <returns></returns>
        public static byte[] FileToByteArray(string file)
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

        /// <summary>
        /// Write byte array to a file.
        /// </summary>
        /// <param name="file">Full file path</param>
        /// <param name="ba">Array of bytes to be written to file</param>
        public static void ByteArrayToFile(string file, byte[] ba)
        {
            using (var fs = new FileStream(file, FileMode.Create, FileAccess.Write))
            {
                fs.Write(ba, 0, ba.Length);
            }
        }

        /// <summary>
        /// Parse byte array into a string
        /// </summary>
        /// <param name="ba">Byte array to be converted</param>
        /// <param name="separator">Character to separate bytes on string representation</param>
        /// <returns></returns>
        public static string ByteArrayToString(byte[] ba, string separator = " ")
        {
            var shb = new System.Runtime.Remoting.Metadata.W3cXsd2001.SoapHexBinary(ba);
            return System.Text.RegularExpressions.Regex.Replace(shb.ToString(), @"\w{2}(?!$)", "$0" + separator);
        }

        /// <summary>
        /// Parse a string into a byte array
        /// </summary>
        /// <param name="hex">String for of bytes in hex format</param>
        /// <returns></returns>
        public static byte[] StringToByteArray(string hex)
        {
            var shb = System.Runtime.Remoting.Metadata.W3cXsd2001.SoapHexBinary.Parse(hex);
            return shb.Value;
        }


        ////////////////////////////////////////////////////////////
        /// Helper functions
        ////////////////////////////////////////////////////////////

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

        private static List<string> SplitIntoParts(this string s, int partLength)
        {
            var list = new List<string>();

            if (!string.IsNullOrEmpty(s) && partLength > 0)
            {
                for (var i = 0; i < s.Length; i += partLength)
                {
                    list.Add(s.Substring(i, Math.Min(partLength, s.Length - i)));
                }
            }

            return list;
        }
    }
}
