using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Windows.Forms;

namespace RSA_Cipher
{
    public partial class window_application : Form
    {
        private RSA rsa;

        public window_application()
        {
            InitializeComponent();
            rsa = new RSA();
            rsa.GenerateKeys();
            DisplayKeys();
        }

        private void DisplayKeys()
        {
            public_key.Text = $"Public Key: (n = {rsa.PublicKeyN}, e = {rsa.PublicKeyE})";
            private_key.Text = $"Private Key: (n = {rsa.PublicKeyN}, d = {rsa.PrivateKeyD})";
        }

        private void encryption_button_Click(object sender, EventArgs e)
        {
            string message = encryption_field.Text.ToUpper(); // Zamiana na wielkie litery
            if (!IsValidMessage(message))
            {
                MessageBox.Show("Tekst może zawierać tylko alfabet polski z literami V,Q i X.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            List<BigInteger> cipherBlocks = rsa.Encrypt(messageBytes);
            decryption_field.Text = string.Join("|", cipherBlocks);
        }

        private void decryption_button_Click(object sender, EventArgs e)
        {
            string cipherText = decryption_field.Text;
            if (!cipherText.All(c => char.IsDigit(c) || c == '|'))
            {
                MessageBox.Show("Pole może zawierać tylko liczby oddzielone przecinkami.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string[] cipherStrings = cipherText.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            List<BigInteger> cipherBlocks = cipherStrings.Select(s => BigInteger.Parse(s)).ToList();
            byte[] decryptedBytes = rsa.Decrypt(cipherBlocks);
            string decryptedMessage = Encoding.UTF8.GetString(decryptedBytes).Trim('\0');
            encryption_field.Text = decryptedMessage;
        }
        private void los_button_Click(object sender, EventArgs e)
        {
            rsa.GenerateKeys();
            DisplayKeys();
        }
        private bool IsValidMessage(string message)
        {
            const string allowedChars = "AĄBCĆDEĘFGHIJKLŁMNŃOÓPRSŚTUWYZŹŻXQV ";
            return message.All(c => allowedChars.Contains(c));
        }
    }

    public class RSA
    {
        public BigInteger PublicKeyN { get; private set; }
        public BigInteger PublicKeyE { get; private set; }
        public BigInteger PrivateKeyD { get; private set; }

        private static Random random = new Random();

        public void GenerateKeys()
        {
            BigInteger p = GetRandomPrime(100, 10000);
            BigInteger q = GetRandomPrime(100, 10000);

            PublicKeyN = p * q;
            BigInteger phi = (p - 1) * (q - 1);

            PublicKeyE = 65537;
            if (BigInteger.GreatestCommonDivisor(PublicKeyE, phi) != 1)
            {
                throw new Exception("Nie można znaleźć odpowiedniego klucza publicznego e.");
            }

            PrivateKeyD = ModularInverse(PublicKeyE, phi);
        }

        public List<BigInteger> Encrypt(byte[] message)
        {
            List<BigInteger> cipherBlocks = new List<BigInteger>();
            int blockSize = PublicKeyN.ToByteArray().Length - 1;

            for (int i = 0; i < message.Length; i += blockSize)
            {
                byte[] block = new byte[blockSize];
                Array.Copy(message, i, block, 0, Math.Min(blockSize, message.Length - i));

                BigInteger blockValue = new BigInteger(block.Reverse().ToArray());
                cipherBlocks.Add(BigInteger.ModPow(blockValue, PublicKeyE, PublicKeyN));
            }
            return cipherBlocks;
        }

        public byte[] Decrypt(List<BigInteger> cipherBlocks)
        {
            List<byte> plainText = new List<byte>();
            int blockSize = PublicKeyN.ToByteArray().Length - 1;

            foreach (var cipherBlock in cipherBlocks)
            {
                BigInteger decryptedBlock = BigInteger.ModPow(cipherBlock, PrivateKeyD, PublicKeyN);
                byte[] blockBytes = decryptedBlock.ToByteArray();
                blockBytes = blockBytes.Reverse().ToArray();

                if (blockBytes.Length < blockSize)
                {
                    byte[] paddedBlock = new byte[blockSize];
                    Array.Copy(blockBytes, 0, paddedBlock, blockSize - blockBytes.Length, blockBytes.Length);
                    plainText.AddRange(paddedBlock);
                }
                else
                {
                    plainText.AddRange(blockBytes);
                }
            }

            return plainText.ToArray();
        }

        private BigInteger GetRandomPrime(int min, int max)
        {
            List<int> primes = GetPrimesInRange(min, max);
            int index = random.Next(primes.Count);
            return primes[index];
        }

        private List<int> GetPrimesInRange(int min, int max)
        {
            List<int> primes = new List<int>();
            for (int i = min; i <= max; i++)
            {
                if (IsPrime(i))
                    primes.Add(i);
            }
            return primes;
        }

        private bool IsPrime(int number)
        {
            if (number < 2) return false;
            if (number == 2) return true;
            if (number % 2 == 0) return false;

            int limit = (int)Math.Sqrt(number);
            for (int i = 3; i <= limit; i += 2)
            {
                if (number % i == 0)
                    return false;
            }
            return true;
        }

        private BigInteger ModularInverse(BigInteger a, BigInteger m)
        {
            BigInteger m0 = m, t, q;
            BigInteger x0 = 0, x1 = 1;

            if (m == 1)
                return 0;

            while (a > 1)
            {
                q = a / m;
                t = m;
                m = a % m; a = t;
                t = x0;
                x0 = x1 - q * x0;
                x1 = t;
            }

            if (x1 < 0)
                x1 += m0;

            return x1;
        }
    }
}
