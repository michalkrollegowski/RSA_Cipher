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
                byte[] utf8Bytes = Encoding.UTF8.GetBytes(input_text);
                foreach (BigInteger character in utf8Bytes)
                {
                    BigInteger encryptedChar = BigInteger.ModPow(encryptedChar, publicKey.e, publicKey.n);
                    encryptedmessage.Add(encryptedChar);
                }

                decryption_field.Text = string.Join("", encryptedmessage);
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

            foreach (string item in input_text.Split(' '))
            {
                if (!BigInteger.TryParse(item, out BigInteger result))
                {
                    isProperInput = false;
                    break;
                }
                encryptedmessage.Add(result);
            }

            if (string.IsNullOrEmpty(input_text))
            {
                isProperInput = false;
            }

            if (isProperInput)
            {
                decryptedmessage = new List<BigInteger>();
                StringBuilder decryptedText = new StringBuilder();


                foreach (BigInteger encryptedChar in encryptedmessage)
                {
                    char decryptedChar = BigInteger.ModPow(encryptedChar, privateKey.d, privateKey.n);
                    decryptedmessage.Add(decryptedChar);
                }
                foreach (BigInteger decryptedChar in decryptedmessage)
                {
                    decryptedText.Append(Encoding.UTF8.GetString(decryptedChar));
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
