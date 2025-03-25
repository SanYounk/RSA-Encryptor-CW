using System;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

public class RSAAlgorithm
{
    public BigInteger PublicKey { get; set; }
    public BigInteger PrivateKey { get; set; }
    public BigInteger Modulus { get; set; }

    public void GenerateKeys(int keySize = 512, BigInteger e = default)
    {
        if (e == default)
            e = new BigInteger(65537); // Значение по умолчанию для e

        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            BigInteger p, q;
            do
            {
                p = GenerateLargePrime(keySize / 2, rng);
                q = GenerateLargePrime(keySize / 2, rng);
            } while (p == q);

            Modulus = p * q;
            BigInteger phi = (p - 1) * (q - 1);

            if (BigInteger.GreatestCommonDivisor(e, phi) != 1)
                throw new InvalidOperationException("e and φ(n) must be relatively prime.");

            PublicKey = e;
            PrivateKey = ModInverse(e, phi);
        }
    }

    public BigInteger Encrypt(BigInteger message)
    {
        if (Modulus == 0)
            throw new InvalidOperationException("The module cannot be zero.");
        if (message >= Modulus)
            throw new ArgumentException("The message must be smaller than the module.");

        return BigInteger.ModPow(message, PublicKey, Modulus);
    }

    public BigInteger Decrypt(BigInteger encryptedMessage)
    {
        if (Modulus == 0 || PrivateKey == 0)
            throw new InvalidOperationException("Модуль или закрытый ключ не могут быть нулем.");

        return BigInteger.ModPow(encryptedMessage, PrivateKey, Modulus);
    }

    public string EncryptString(string message)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(message);
        BigInteger messageInt = new BigInteger(bytes);
        BigInteger encryptedInt = Encrypt(messageInt);
        return Convert.ToBase64String(encryptedInt.ToByteArray());
    }

    public string DecryptString(string encryptedMessage)
    {
        byte[] encryptedBytes = Convert.FromBase64String(encryptedMessage);
        BigInteger encryptedInt = new BigInteger(encryptedBytes);
        BigInteger decryptedInt = Decrypt(encryptedInt);
        byte[] decryptedBytes = decryptedInt.ToByteArray();
        return Encoding.UTF8.GetString(decryptedBytes);
    }

    private BigInteger GenerateLargePrime(int bits, RNGCryptoServiceProvider rng)
    {
        while (true)
        {
            byte[] bytes = new byte[bits / 8];
            rng.GetBytes(bytes);
            BigInteger num = new BigInteger(bytes);
            num = BigInteger.Abs(num);

            if (num < BigInteger.Pow(2, bits - 1))
                num += BigInteger.Pow(2, bits - 1);

            if (IsPrime(num, rng))
                return num;
        }
    }

    private bool IsPrime(BigInteger n, RNGCryptoServiceProvider rng, int k = 5)
    {
        if (n < 2) return false;
        if (n % 2 == 0) return n == 2;

        BigInteger d = n - 1;
        int s = 0;
        while (d % 2 == 0) { d /= 2; s++; }

        for (int i = 0; i < k; i++)
        {
            BigInteger a;
            do
            {
                byte[] buffer = new byte[n.ToByteArray().Length];
                rng.GetBytes(buffer);
                a = new BigInteger(buffer);
            } while (a < 2 || a >= n - 2);

            BigInteger x = BigInteger.ModPow(a, d, n);
            if (x == 1 || x == n - 1) continue;

            for (int j = 0; j < s - 1; j++)
            {
                x = BigInteger.ModPow(x, 2, n);
                if (x == 1) return false;
                if (x == n - 1) break;
            }
            if (x != n - 1) return false;
        }
        return true;
    }

    private BigInteger ModInverse(BigInteger a, BigInteger m)
    {
        if (m == 1)
            return 0;
        if (a == 0)
            throw new ArgumentException("The parameter 'a' cannot be null.");

        BigInteger m0 = m;
        BigInteger y = 0, x = 1;

        while (a > 1)
        {
            if (m == 0)
                throw new DivideByZeroException("Attempted division by zero.");

            BigInteger q = a / m;
            BigInteger t = m;

            m = a % m;
            a = t;
            t = y;

            y = x - q * y;
            x = t;
        }

        if (x < 0)
            x += m0;

        return x;
    }
}