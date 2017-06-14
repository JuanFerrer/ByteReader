using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ByteReaderTests
{
    [TestClass]
    public class ByteReaderTest
    {
        //////////////////////////////////////////////////////////////////
        // PrintBytes
        //////////////////////////////////////////////////////////////////

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
            string file = @"..\..\Test01.jpg";
            ByteReader.ByteReader.PrintBytes(file);
        }

        //////////////////////////////////////////////////////////////////
        // CheckFileSignature
        //////////////////////////////////////////////////////////////////

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

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void GetHexFromFileEmptyFile()
        {
            string file = @"";
            byte[] baf = ByteReader.ByteReader.GetHexFromFile(file);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void GetHexFromFileNoFile()
        {
            string file = @"..\..\Test00.null";
            byte[] baf = ByteReader.ByteReader.GetHexFromFile(file);
        }

        [TestMethod]
        public void GetHexFromFile()
        {
            string file = @"..\..\Test03.txt";
            byte[] bae = ByteReader.ByteReader.StringToByteArray("4C 6F 72 65 6D 20 69 70 73 75 6D");
            byte[] bar = ByteReader.ByteReader.GetHexFromFile(file);
            bool result = bar.SequenceEqual(bae);
            Assert.AreEqual(true, result, "Comparing byte arrays");

            byte[] bane = ByteReader.ByteReader.StringToByteArray("FF D8");
            bar = ByteReader.ByteReader.GetHexFromFile(file);
            result = bar.SequenceEqual(bane);
            Assert.AreEqual(false, result, "Comparing different byte arrays");
        }
    }
}
