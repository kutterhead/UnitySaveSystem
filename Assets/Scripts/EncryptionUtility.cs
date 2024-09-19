using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class EncryptionUtility
{
    private static readonly string encryptionKey = "claveSecreta1234"; // Debe tener exactamente 16 caracteres para AES-128 o 32 para AES-256

    // Método para encriptar un texto plano a un formato encriptado usando AES
    public static string Encrypt(string plainText)
    {
        byte[] key = Encoding.UTF8.GetBytes(encryptionKey);
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.GenerateIV(); // Crea un IV único para este cifrado
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream ms = new MemoryStream())
            {
                // Primero escribimos el IV (vector de inicialización)
                ms.Write(aes.IV, 0, aes.IV.Length);
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }
                }
                return Convert.ToBase64String(ms.ToArray()); // Convertimos los bytes encriptados a una cadena Base64
            }
        }
    }

    // Método para desencriptar el texto encriptado de vuelta a texto plano
    public static string Decrypt(string cipherText)
    {
        byte[] fullCipher = Convert.FromBase64String(cipherText);
        byte[] key = Encoding.UTF8.GetBytes(encryptionKey);
        using (Aes aes = Aes.Create())
        {
            byte[] iv = new byte[aes.BlockSize / 8];
            byte[] cipher = new byte[fullCipher.Length - iv.Length];

            // Extraemos el IV del texto cifrado
            Array.Copy(fullCipher, iv, iv.Length);
            Array.Copy(fullCipher, iv.Length, cipher, 0, cipher.Length);

            aes.Key = key;
            aes.IV = iv;

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (MemoryStream ms = new MemoryStream(cipher))
            {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(cs))
                    {
                        return sr.ReadToEnd(); // Devolvemos el texto descifrado
                    }
                }
            }
        }
    }
}
