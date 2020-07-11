using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;

namespace Checksum_Calculator
{
    public class Hasher
    {
        #region Properties

        private readonly FileStream InputStream;

        public Hasher(FileStream inputStream)
        {
            InputStream = inputStream;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns checksum based on hash algorithm specified as argument.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetChecksumByType(HashType type)
        {
            byte[] computedHash = ComputeDynamicHash(type, InputStream);
            return BitConverter.ToString(computedHash).Replace("-", "‌​").ToLower();
        }

        /// <summary>
        /// Instantiates proper cryptography class and computes hash based on input stream.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="inputStream"></param>
        /// <returns></returns>
        private byte[] ComputeDynamicHash(HashType type, FileStream inputStream)
        {
            Type hashType = Type.GetType(type.Reference, true);
            var cryptoInstance = Activator.CreateInstance(hashType) as HashAlgorithm;

            object[] MethodArgs = new object[] { inputStream };
            MethodInfo method = hashType.GetMethod("ComputeHash", new[] { typeof(Stream) });
            return (byte[])method.Invoke(cryptoInstance, MethodArgs);
        }
        
        #endregion
    }
}
