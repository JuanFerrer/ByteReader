using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Diagnostics;

namespace ByteReaderTests
{
    [TestClass]
    public class ByteReaderTest
    {
        #region PrintBytes

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void PrintBytesEmptyFile()
        {
            string file = @"";
            ByteReader.ByteReader.PrintBytes(file);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void PrintBytesNoFile()
        {
            string file = @"..\..\Test00.null";
            ByteReader.ByteReader.PrintBytes(file);
        }

        [TestMethod]
        public void PrintBytesWithFile()
        {
            string file = @"..\..\Test03.txt";
            ByteReader.ByteReader.PrintBytes(file);
            ByteReader.ByteReader.PrintBytes(file, -5);
            ByteReader.ByteReader.PrintBytes(file, -1);
            ByteReader.ByteReader.PrintBytes(file, 0);
            ByteReader.ByteReader.PrintBytes(file, 3);
            ByteReader.ByteReader.PrintBytes(file, 200);
        }

        #endregion

        #region CheckFileSignature

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void CheckFileSignatureEmptyFile()
        {
            string file = @"";
            string signature = "FF FF";
            ByteReader.ByteReader.CheckFileSignature(file, signature);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void CheckFileSignatureNoFile()
        {
            string file = @"..\..\Test00.null";
            string signature = "FF FF";
            ByteReader.ByteReader.CheckFileSignature(file, signature);
        }

        [TestMethod]
        public void CheckFileSignatureNoSignature()
        {
            string file = @"..\..\Test01.jpg";
            string signature = "";
            bool result = ByteReader.ByteReader.CheckFileSignature(file, signature);
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void CheckFileSignatureWithFile()
        {
            string file = @"..\..\Test01.jpg";
            string signature = "FF D8";
            bool result = ByteReader.ByteReader.CheckFileSignature(file, signature);
            Assert.AreEqual(true, result, $"Testing {file} with {signature}");

            file = @"..\..\Test02.kes";
            signature = "FF D8";
            result = ByteReader.ByteReader.CheckFileSignature(file, signature);
            Assert.AreEqual(false, result, $"Testing {file} with {signature}");

            file = @"..\..\Test02.kes";
            signature = "D0 CF 11 E0 A1 B1 1A E1";
            result = ByteReader.ByteReader.CheckFileSignature(file, signature);
            Assert.AreEqual(true, result, $"Testing {file} with {signature}");

            file = @"..\..\Test03.txt";
            signature = "4C 6F 72 65 6D 20 69 70 73 75 6D";
            result = ByteReader.ByteReader.CheckFileSignature(file, signature);
            Assert.AreEqual(true, result, "Signature is full file");
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void CheckFileSignatureLongSignature()
        {
            string file = @"..\..\Test03.txt";
            string signature = "4C 6F 72 65 6D 20 69 70 73 75 6D AA";
            bool result = ByteReader.ByteReader.CheckFileSignature(file, signature);
        }

        #endregion

        #region ByteArrayToAndFro

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void FileToByteArrayEmptyFile()
        {
            string file = @"";
            byte[] baf = ByteReader.ByteReader.FileToByteArray(file);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void FileToByteArrayNoFile()
        {
            string file = @"..\..\Test00.null";
            byte[] baf = ByteReader.ByteReader.FileToByteArray(file);
        }

        [TestMethod]
        public void FileToByteArrayFile()
        {
            string file = @"..\..\Test03.txt";
            byte[] bae = ByteReader.ByteReader.StringToByteArray("4C 6F 72 65 6D 20 69 70 73 75 6D");
            byte[] bar = ByteReader.ByteReader.FileToByteArray(file);
            bool result = bar.SequenceEqual(bae);
            Assert.AreEqual(true, result, "Comparing byte arrays");

            byte[] bane = ByteReader.ByteReader.StringToByteArray("FF D8");
            bar = ByteReader.ByteReader.FileToByteArray(file);
            result = bar.SequenceEqual(bane);
            Assert.AreEqual(false, result, "Comparing different byte arrays");
        }

        [TestMethod]
        public void ByteArrayToStringTest()
        {
            byte[] ba = { 0xFF, 0xA2, 0x2F, 0x00 };
            string expected = "FF A2 2F 00";
            string result = ByteReader.ByteReader.ByteArrayToString(ba);
            Assert.AreEqual(expected, result);

            byte[] bane = { 0xA2, 0x00, 0xFF, 0x2F };
            result = ByteReader.ByteReader.ByteArrayToString(bane);
            Assert.AreNotEqual(expected, result);
        }

        [TestMethod]
        public void StringToByteArrayTest()
        {
            string byteString = "A2 FF 00 2F";
            byte[] bae = { 0xA2, 0xFF, 0x00, 0x2F };
            byte[] bar = ByteReader.ByteReader.StringToByteArray(byteString);
            bool result = bar.SequenceEqual(bae);
            Assert.AreEqual(true, result);

            byteString = "";
            bar = ByteReader.ByteReader.StringToByteArray(byteString);
            result = bar.SequenceEqual(bae);
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void ByteArrayToFileTest()
        {
            string str = "Lorem ipsum";
            string filename = @"..\..\Test04.txt";
            byte[] ba = System.Text.Encoding.ASCII.GetBytes(str);
            ByteReader.ByteReader.ByteArrayToFile(filename, ba);
            string result = File.ReadAllLines(filename)[0];
            Assert.AreEqual(str, result);
        }

        #endregion

        public struct Result
        {
            public string Name { get; set; }
            public int Value { get; set; }
            public Result(string n, int v)
            {
                Name = n;
                Value = v;
            }
        }

        #region Performance
        [TestMethod]
        public void CheckFileSignaturePerformanceTest()
        {
            string[] resultsArray = File.ReadAllLines(@"..\..\PerformanceTestsResults.txt");
            Result[] results = new Result[3];
            for (int i = 0; i < resultsArray.Length; ++i)
            {
                string[] subStrings = resultsArray[i].Split(':');
                results[i] = new Result(subStrings[0], int.Parse(subStrings[1]));
            }
            string file = @"..\..\Test01.jpg";
            string signature = "FF D8";
            TimeSpan time = Time(() =>
            {
                if (ByteReader.ByteReader.CheckFileSignature(file, signature)) { }

            });
            Assert.IsTrue(time.Milliseconds.CompareTo(results[0].Value) < 0);
            if (Math.Abs(time.Milliseconds - results[0].Value) > 10)    // Over 10 ms difference
            {
                resultsArray[0] = "CheckFileSignature:" + time.Milliseconds;
                File.WriteAllLines(@"..\..\PerformanceTestsResults.txt", resultsArray);
            }
        }
        #endregion

        public static TimeSpan Time(Action action)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            action();
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }
    }
}
