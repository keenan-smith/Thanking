using System;
using System.Linq;
using System.Security.Cryptography;

namespace Thanking.Utilities
{
    public class HashUtilities
    {
        public static String GetSHA2HashString(Byte[] Bytes) =>
            String.Join("", GetSHA2Hash(Bytes).Select(a => a.ToString("x2")).ToArray()).ToUpper();
        
        public static Byte[] GetSHA2Hash(Byte[] Bytes)
        {
            using (SHA256 SHA256 = SHA256Managed.Create())
                return SHA256.ComputeHash(Bytes);
        }
    }
}