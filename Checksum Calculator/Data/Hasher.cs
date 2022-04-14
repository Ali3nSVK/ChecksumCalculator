using ChecksumCalculator.Data;
using System;
using System.IO;
using System.Security.Cryptography;

namespace ChecksumCalculator
{
    public class Hasher
    {
        private readonly string filename;

        public Hasher(string fname)
        {
            filename = fname;
        }

        private HashAlgorithm GetHashAlgorithm(HashType type)
        {
            switch(type)
            {
                case HashType.CRC32:
                    return new Crc32();
                case HashType.MD5:
                    return new MD5CryptoServiceProvider();
                case HashType.SHA1:
                    return new SHA1CryptoServiceProvider();
                case HashType.SHA256:
                    return new SHA256CryptoServiceProvider();
                case HashType.SHA384:
                    return new SHA384CryptoServiceProvider();
                case HashType.SHA512:
                    return new SHA512CryptoServiceProvider();
                default:
                    throw new NotSupportedException();
            }
        }

        public string ComputeHash(HashType type)
        {
            using (var stream = File.OpenRead(filename))
            {
                return BitConverter.ToString(GetHashAlgorithm(type).ComputeHash(stream)).
                    Replace("-", "‌​").
                    ToLower();
            }
        }
    }
}
