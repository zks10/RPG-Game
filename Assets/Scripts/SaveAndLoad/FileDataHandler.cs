using UnityEngine;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class FileDataHandler 
{
    private readonly string dataDirPath = "";
    private readonly string dataFileName = "";
    private readonly bool encryptData;

    // Change this to something unique for your game!
    private static readonly string password = "kevindev-secure-password";

    // AES / PBKDF2 settings
    private const int KeySize = 32;            // 256-bit keys
    private const int Iterations = 10000;      // PBKDF2 iterations
    private const int SaltSize = 16;           // 128-bit salt

    public FileDataHandler(string _dataDirPath, string _dataFileName, bool _encryptData)
    {
        dataDirPath = _dataDirPath;
        dataFileName = _dataFileName;
        encryptData = _encryptData;
    }

    public void Save(GameData _data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string json = JsonUtility.ToJson(_data, true);

            if (encryptData)
                json = Encrypt(json);

            File.WriteAllText(fullPath, json);
        }
        catch(Exception e)
        {
            Debug.LogError("Error saving data to file: " + fullPath + "\n" + e);
        }
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        if (!File.Exists(fullPath))
        {
            Debug.Log("Save file not found. Creating new data.");
            return null;
        }

        try
        {
            string dataToLoad = File.ReadAllText(fullPath);

            if (encryptData)
                dataToLoad = Decrypt(dataToLoad);

            return JsonUtility.FromJson<GameData>(dataToLoad);
        }
        catch (Exception e)
        {
            Debug.LogError("Error loading data: " + e.Message);
            return null;
        }
    }

    public void DeleteData()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        if (File.Exists(fullPath))
            File.Delete(fullPath);
    }


    #region  AES-256 ENCRYPTION

    private string Encrypt(string plainText)
    {
        using (Aes aes = Aes.Create())
        {
            // Generate salt
            byte[] salt = new byte[SaltSize];
            RandomNumberGenerator.Fill(salt);

            // Derive key from the password + salt
            using var key = new Rfc2898DeriveBytes(password, salt, Iterations);
            aes.Key = key.GetBytes(KeySize);

            aes.GenerateIV();

            using var ms = new MemoryStream();

            // Write salt + IV first
            ms.Write(salt, 0, salt.Length);
            ms.Write(aes.IV, 0, aes.IV.Length);

            // Encrypt
            using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(plainText);
            }

            return Convert.ToBase64String(ms.ToArray());
        }
    }

    private string Decrypt(string cipherText)
    {
        byte[] data = Convert.FromBase64String(cipherText);

        using (Aes aes = Aes.Create())
        {
            int ivSize = aes.BlockSize / 8;

            // Extract salt + IV
            byte[] salt = new byte[SaltSize];
            Array.Copy(data, 0, salt, 0, SaltSize);

            byte[] iv = new byte[ivSize];
            Array.Copy(data, SaltSize, iv, 0, ivSize);

            using var key = new Rfc2898DeriveBytes(password, salt, Iterations);
            aes.Key = key.GetBytes(KeySize);
            aes.IV = iv;

            int dataStartIndex = SaltSize + ivSize;
            int dataLength = data.Length - dataStartIndex;

            using var ms = new MemoryStream(data, dataStartIndex, dataLength);
            using var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);

            return sr.ReadToEnd();
        }
    }
    #endregion
}
