# Byte reader

Simple byte reader distributed as an assembly. **Test** shows an example usage.

Several functions are available, namely:

###### `public static void PrintBytes(string file, int amount = -1)` 
Prints bytes in HEX up to *amount* (if within zero and *file length*) or *file length*, whichever comes first.
``` cs
string filename = "C:\Windows\System32\notepad.exe"
ByteReader.ByteReader.PrintBytes(filename, 10);

// Output: 4D 5A 90 00 03 00 00 00 04 00
```

###### `public static bool CheckFileSignature(string file, string signature)` 
Return true if the signature provided appears exactly at the beginning of the file; false otherwise.
``` cs
string filename = "C:\Windows\System32\notepad.exe"
string signature = "4D 5A"  // Signature of an exe
if (ByteReader.ByteReader.CheckFileSignature(filename, signature))
    Console.WriteLine($"Signature {signature} was found");

// Output: Signature "4D 5A" was found
```

###### `public static byte[] GetHexFromFile(string file)` 
Return an array containing the bytes of the file.
``` cs
string filename = "C:\Windows\System32\notepad.exe"
byte[] ba = ByteReader.ByteReader.GetHexFromFile(filename);
```

###### `public static string ByteArrayToString(byte[] ba, string separator = " ")` 
Parse a `byte[]` to a string separating the elements with the specified string.
``` cs
string filename = "C:\Windows\System32\notepad.exe"
byte[] ba = ByteReader.ByteReader.GetHexFromFile(filename);
Console.WriteLine(ByteReader.ByteReader.ByteArrayToString(ba, "-"));

// Output: 4D-5A-90-00-03-00-00-00-04-00-...
```

###### `public static byte[] StringToByteArray(string hex)`
Convert a string into a `byte[]`.
``` cs
string byteString = "4D 5A 90 00 03 00 00 00 04 00";
byte[] ba = ByteReader.ByteReader.StringToByteArray(byteString);
```

###### `public static void ByteArrayToFile(string fileName, byte[] byteArray)`
Write a `byte[]` to a file of a given name.
``` cs
string filename = "test.txt";
string byteString = "4D 5A 90 00 03 00 00 00 04 00";
ByteReader.ByteReader.ByteArrayToFile(filename, ByteReader.ByteReader.StringToByteArray(byteString));
ByteReader.ByteReader.PrintBytes(filename);

// Output: 4D 5A 90 00 03 00 00 00 04 00
```