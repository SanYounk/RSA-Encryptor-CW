using System;
using System.IO;
using System.Numerics;
using System.Text;

public class FileManager
{
    // Сохранение ключей в файл
    public void SaveKeysToFile(string publicKeyPath, string privateKeyPath, BigInteger publicKey, BigInteger privateKey, BigInteger modulus)
    {
        File.WriteAllText(publicKeyPath, $"{publicKey}\n{modulus}");
        File.WriteAllText(privateKeyPath, $"{privateKey}\n{modulus}");
    }

    // Загрузка ключей из файла
    public void LoadKeysFromFile(string publicKeyPath, string privateKeyPath, out BigInteger publicKey, out BigInteger privateKey, out BigInteger modulus)
    {
        string[] publicKeyData = File.ReadAllLines(publicKeyPath);
        string[] privateKeyData = File.ReadAllLines(privateKeyPath);

        publicKey = BigInteger.Parse(publicKeyData[0]);
        modulus = BigInteger.Parse(publicKeyData[1]);
        privateKey = BigInteger.Parse(privateKeyData[0]);
    }

    // Шифрование файла
    public void EncryptFile(string inputFilePath, string outputFilePath, RSAAlgorithm rsa)
    {
        byte[] fileBytes = File.ReadAllBytes(inputFilePath);
        BigInteger messageInt = new BigInteger(fileBytes);
        BigInteger encryptedInt = rsa.Encrypt(messageInt);
        byte[] encryptedBytes = encryptedInt.ToByteArray();
        File.WriteAllBytes(outputFilePath, encryptedBytes);

    }

    // Дешифрование файла
    public void DecryptFile(string inputFilePath, string outputFilePath, RSAAlgorithm rsa)
    {
        byte[] encryptedBytes = File.ReadAllBytes(inputFilePath);
        BigInteger encryptedInt = new BigInteger(encryptedBytes);
        BigInteger decryptedInt = rsa.Decrypt(encryptedInt);
        byte[] decryptedBytes = decryptedInt.ToByteArray();
        File.WriteAllBytes(outputFilePath, decryptedBytes);
    }

    public static string EncryptText(string message, BigInteger publicKey, BigInteger modulus)
    {
        byte[] messageBytes = Encoding.UTF8.GetBytes(message); // Текст -> байты
        BigInteger messageInt = new BigInteger(messageBytes);  // Байты -> число
        BigInteger encryptedInt = BigInteger.ModPow(messageInt, publicKey, modulus); // RSA-шифрование

        return Convert.ToBase64String(encryptedInt.ToByteArray()); // Кодируем в строку
    }

    public static string DecryptText(string encryptedMessage, BigInteger privateKey, BigInteger modulus)
    {
        BigInteger encryptedInt = new BigInteger(Convert.FromBase64String(encryptedMessage)); // Строка -> число
        BigInteger decryptedInt = BigInteger.ModPow(encryptedInt, privateKey, modulus); // Расшифровка

        return Encoding.UTF8.GetString(decryptedInt.ToByteArray()); // Число -> текст
    }
}