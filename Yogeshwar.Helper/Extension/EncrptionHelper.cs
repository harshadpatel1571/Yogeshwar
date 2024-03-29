﻿namespace Yogeshwar.Helper.Extension;

internal static class ServiceExtension
{
    private const string Key = "657148c456ac4415be3ef076ab1132f0";

    public static string Encrypt(string value)
    {
        var toEncryptArray = Encoding.UTF8.GetBytes(value);

        using var md = MD5.Create();
        var keyArray = md.ComputeHash(Encoding.UTF8.GetBytes(Key));
        md.Clear();

        using var aes = Aes.Create();
        aes.Key = keyArray;
        aes.Mode = CipherMode.ECB;
        aes.Padding = PaddingMode.PKCS7;

        using var cTransform = aes.CreateEncryptor();
        var resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        aes.Clear();

        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }

    public static string Decrypt(string value)
    {
        var toEncryptArray = Convert.FromBase64String(value);

        using var md = MD5.Create();
        var keyArray = md.ComputeHash(Encoding.UTF8.GetBytes(Key));
        md.Clear();

        using var aes = Aes.Create();
        aes.Key = keyArray;
        aes.Mode = CipherMode.ECB;
        aes.Padding = PaddingMode.PKCS7;

        using var cTransform = aes.CreateDecryptor();
        var resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        aes.Clear();
        return Encoding.UTF8.GetString(resultArray);
    }
}
