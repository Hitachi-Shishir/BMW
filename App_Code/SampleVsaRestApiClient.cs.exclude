using System;
using System.Security.Cryptography;
using System.Text;


public class Hash
{
    public string RandomNumber { get; protected set; }
    public string RawSHA256Hash { get; protected set; }
    public string CoveredSHA256Hash { get; protected set; }
    public string RawSHA1Hash { get; protected set; }
    public string CoveredSHA1Hash { get; protected set; }
    public Hash(string UserName, string Password)
    {
        RandomNumber = GenerateRandomNumber(8);
        RawSHA256Hash = CalculateHash(Password, "SHA-256");
        CoveredSHA256Hash = CalculateHash(Password, UserName, "SHA-256");
        CoveredSHA256Hash = CalculateHash(CoveredSHA256Hash, RandomNumber, "SHA-256");
        RawSHA1Hash = CalculateHash(Password, "SHA-1");
        CoveredSHA1Hash = CalculateHash(Password, UserName, "SHA-1");
        CoveredSHA1Hash = CalculateHash(CoveredSHA1Hash, RandomNumber, "SHA-1"
       );
    }
    private string CalculateHash(string Value1, string Value2, string HashingAlgorithm)
    {
        return CalculateHash(Value1 + Value2, HashingAlgorithm);
    }
    private string CalculateHash(string Value, string HashingAlgorithm)
    {
        byte[] arrByte;
        if (HashingAlgorithm == "SHA-1")
        {
            SHA1Managed hash = new SHA1Managed();
            arrByte = hash.ComputeHash(ASCIIEncoding.ASCII.GetBytes(Value));
        }
        else if (HashingAlgorithm == "SHA-256")
        {
            SHA256Managed hash = new SHA256Managed();
            arrByte = hash.ComputeHash(ASCIIEncoding.ASCII.GetBytes(Value));
        }
        else
        {

            throw new ApplicationException(string.Format("Unknow hashing algorithm: { 0 }", HashingAlgorithm));
        }
        string s = "";
        for (int i = 0; i < arrByte.Length; i++)
        {
            s += arrByte[i].ToString("x2");
        }
        return s;
    }
    private string GenerateRandomNumber(int numberOfDigits)
    {
        System.Security.Cryptography.RNGCryptoServiceProvider rng = new
        System.Security.Cryptography.RNGCryptoServiceProvider();
        byte[] numbers = new byte[numberOfDigits * 2];
        rng.GetNonZeroBytes(numbers);
        string result = "";
        for (int i = 0; i < numberOfDigits; i++)
        {
            result += numbers[i].ToString();
        }
        result = result.Replace("0", "");
        return result.Substring(1, numberOfDigits);
    }
}




