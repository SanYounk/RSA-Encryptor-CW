using CourseWorkIT;
using System;
using System.Numerics;

class Program
{
    static void Main(string[] args)
    {
        RSAAlgorithm rsa = new RSAAlgorithm();
        FileManager fileManager = new FileManager();
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new MainForm()); // Запуск главной формы

        if (args.Length == 0)
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("  generate-keys <publicKeyPath> <privateKeyPath>");
            Console.WriteLine("  encrypt <publicKeyPath> <inputFile> <outputFile>");
            Console.WriteLine("  decrypt <privateKeyPath> <inputFile> <outputFile>");
            return;
        }

        try
        {
            switch (args[0])
            {
                case "generate-keys":
                    rsa.GenerateKeys();
                    fileManager.SaveKeysToFile(
                        args[1],          // publicKeyPath
                        args[2],          // privateKeyPath
                        rsa.PublicKey,    // publicKey
                        rsa.PrivateKey,   // privateKey
                        rsa.Modulus       // modulus
                    );
                    Console.WriteLine("Keys have been successfully generated and saved.");
                    break;

                case "encrypt":
                    fileManager.LoadKeysFromFile(args[1], args[2], out BigInteger publicKey, out _, out BigInteger modulus);
                    rsa.PublicKey = publicKey;
                    rsa.Modulus = modulus;
                    fileManager.EncryptFile(args[3], args[4], rsa);
                    Console.WriteLine("The file has been encrypted successfully.");
                    break;

                case "decrypt":
                    fileManager.LoadKeysFromFile(args[1], args[2], out _, out BigInteger privateKey, out BigInteger modul);
                    rsa.PrivateKey = privateKey;
                    rsa.Modulus = modul;
                    fileManager.DecryptFile(args[3], args[4], rsa);
                    Console.WriteLine("The file has been successfully decrypted.");
                    break;

                default:
                    Console.WriteLine("Unknown order.");
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
