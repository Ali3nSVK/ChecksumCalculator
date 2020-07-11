using System;

namespace Checksum_Calculator
{
    public class HashType
    {
        private HashType(string value, string reference)
        {
            Value = value;
            Reference = reference;
        }

        public string Value { get; set; }
        public string Reference { get; set; }

        public static HashType CRC32
        {
            get
            {
                return new HashType("CRC32", "Checksum_Calculator.Crc32");
            }
        }

        public static HashType MD5
        {
            get
            {
                return new HashType("MD5", "System.Security.Cryptography.MD5CryptoServiceProvider");
            }
        }
        
        public static HashType SHA1
        {
            get
            {
                return new HashType("SHA-1", "System.Security.Cryptography.SHA1CryptoServiceProvider");
            }
        }

        public static HashType SHA256
        {
            get
            {
                return new HashType("SHA-256", "System.Security.Cryptography.SHA256Managed");
            }
        }

        public static HashType SHA512
        {
            get
            {
                return new HashType("SHA-512", "System.Security.Cryptography.SHA512Managed");
            }
        }
    }
}
