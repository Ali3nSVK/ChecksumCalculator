using System;
using System.IO;
using System.Security.Cryptography;

namespace ChecksumCalculator
{
    public enum HashType
    {
        Crc32,
        Md5,
        Sha1,
        Sha256,
        Sha512
    }

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
                case HashType.Crc32:
                    return new Crc32();
                case HashType.Md5:
                    return new MD5CryptoServiceProvider();
                case HashType.Sha1:
                    return new SHA1CryptoServiceProvider();
                case HashType.Sha256:
                    return new SHA256CryptoServiceProvider();
                case HashType.Sha512:
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
