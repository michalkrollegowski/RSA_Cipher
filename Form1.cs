using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Reflection;

namespace RSA_Cipher
{
    public partial class window_application : Form
    {
        public window_application()
        {
            InitializeComponent();
        }

        public class RSA : Creating_Keys
        {

            public static (BigInteger, BigInteger, BigInteger) PublicKey()
            {

                //Generowanie 2 liczb o długości 2048 bitów
                var (p, q) = GeneratePrimes(2048);
                BigInteger n = p * q;
                BigInteger nphi = CalculatePhi(p, q);
                BigInteger e = FindE(nphi);

                return (n, e, nphi);
            }
            public static (BigInteger, BigInteger) PrivateKey()
            {
                BigInteger n = PublicKey().Item1;
                BigInteger e = PublicKey().Item2;
                BigInteger d = FindD(e, PublicKey().Item3);

                return (n, d);
            }

            
            public static Dictionary<int, char> intToCharMap = new Dictionary<int, char>();
            public static Dictionary<char, BigInteger> charToBigIntMap = new Dictionary<char, BigInteger>();
            BigInteger baseValue = 1;
            static RSA()
            {

                // Tworzymy mapowanie z liter na liczby
                char[] alphabet = new char[]
                {
            'A', 'Ą', 'B', 'C', 'Ć', 'D', 'E', 'Ę', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'Ł',
            'M', 'N', 'Ń', 'O', 'Ó', 'P', 'Q', 'R', 'S', 'Ś', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'Ź', 'Ż'
                };

                for (int i = 0; i < alphabet.Length; i++)
                {
                    charToBigIntMap[alphabet[i]] = i;
                    intToCharMap[i] = alphabet[i];
                }
                foreach (char c in alphabet)
                {
                    charToBigIntMap[c] = baseValue;
                    baseValue++;
                }
            }
                // Tworzenie mapy znaków na liczby
            // Metoda szyfrowania
            // Szyfrowanie pojedynczego znaku
            public BigInteger EncryptChar(char character, BigInteger e, BigInteger n)
            {
                BigInteger charValue = charToBigIntMap[character];  // Pobieramy wartość liczbową znaku
                BigInteger encryptedChar = BigInteger.ModPow(charValue, e, n);
                return encryptedChar;
            }

            // Deszyfrowanie pojedynczego znaku
            public char DecryptChar(BigInteger encryptedChar, BigInteger d, BigInteger n)
            {
                BigInteger decryptedCharValue = BigInteger.ModPow(encryptedChar, d, n);
                return charToBigIntMap.FirstOrDefault(x => x.Value == decryptedCharValue).Key;
            }


        }

        private (BigInteger n, BigInteger e, BigInteger nphi) publicKey = RSA.PublicKey();
        private (BigInteger n, BigInteger d) privateKey = RSA.PrivateKey();
        private List<BigInteger> encryptedmessage;
        private List<BigInteger> decryptedmessage;
        static char[] alphabet = new char[]
        {
            'A', 'Ą', 'B', 'C', 'Ć', 'D', 'E', 'Ę', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'Ł',
            'M', 'N', 'Ń', 'O', 'Ó', 'P', 'Q', 'R', 'S', 'Ś', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'Ź', 'Ż'
        };
        // Szyfrowanie wiadomości
        private void encryption_button_Click(object sender, EventArgs e)
        {
            decryption_field.Text = "";  // Wyczyść pole do wprowadzenia tekstu szyfrowanego
            string input_text = encryption_field.Text.ToUpper();
            bool isProperInput = true;

            foreach (char c in input_text)
            {
                if (!alphabet.Contains(c))
                {
                    isProperInput = false;
                    break;
                }
            }
            if (string.IsNullOrEmpty(input_text))
            {
                isProperInput = false;
            }

            if (isProperInput)
            {
                encryptedmessage = new List<BigInteger>();

                foreach (char character in input_text)
                {
                    BigInteger encryptedChar = RSA.Encrypt(character, publicKey.e, publicKey.n);
                    encryptedmessage.Add(encryptedChar);
                }

                decryption_field.Text = string.Join(" ", encryptedmessage);
            }
            else
            {
                throw new Exception("Niepoprawna wiadomość");
            }
        }

        // Deszyfrowanie wiadomości
        private void decryption_button_Click(object sender, EventArgs e)
        {
            encryption_field.Text = "";  // Wyczyść pole do wprowadzenia tekstu szyfrowanego
            string input_text = decryption_field.Text;
            bool isProperInput = true;
            List<BigInteger> encryptedMessage = new List<BigInteger>();

            foreach (string item in input_text.Split(' '))
            {
                if (!BigInteger.TryParse(item, out BigInteger result))
                {
                    isProperInput = false;
                    break;
                }
                encryptedMessage.Add(result);
            }

            if (string.IsNullOrEmpty(input_text) || !isProperInput)
            {
                isProperInput = false;
            }

            if (isProperInput)
            {
                StringBuilder decryptedText = new StringBuilder();

                foreach (BigInteger encryptedChar in encryptedMessage)
                {
                    char decryptedChar = RSA.Decrypt(encryptedChar, privateKey.d, privateKey.n);
                    decryptedText.Append(decryptedChar);
                }

                encryption_field.Text = decryptedText.ToString();
            }
            else
            {
                throw new Exception("Niepoprawny format wiadomości");
            }
        }
    }

}
