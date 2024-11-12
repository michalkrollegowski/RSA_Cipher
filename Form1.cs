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

            public static (BigInteger,BigInteger) PublicKey()
            {

            //Generowanie 2 liczb o długości 2048 bitów
            var (p, q) = GeneratePrimes(2048);
                BigInteger n = p * q;
                BigInteger nphi = CalculatePhi(p, q);
                BigInteger e = FindE(nphi);

                return (n, e);
            }
            public static (BigInteger, BigInteger) PrivateKey() 
            {

                //Generowanie 2 liczb o długości 2048 bitów
                var (p, q) = GeneratePrimes(2048);
                BigInteger n = p * q;
                BigInteger nphi = CalculatePhi(p, q);
                BigInteger e = FindE(nphi);
                BigInteger d = FindD(e, nphi);

                return (n, d);
            }

        }

        private (BigInteger n, BigInteger e) publicKey;
        private (BigInteger n, BigInteger d) privateKey;
        private List<BigInteger> encryptedmessage;
        private List<BigInteger> decryptedmessage;
        static char[] alphabet = new char[]
        {
            'A', 'Ą', 'B', 'C', 'Ć', 'D', 'E', 'Ę', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'Ł',
            'M', 'N', 'Ń', 'O', 'Ó', 'P', 'Q', 'R', 'S', 'Ś', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'Ź', 'Ż'
        };
        private void encryption_button_Click(object sender, EventArgs e)
        {

            publicKey = RSA.PublicKey();
            privateKey = RSA.PrivateKey();
            decryption_field.Text = "";
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
                byte[] utf8Bytes = Encoding.UTF8.GetBytes(input_text);

                for (int i = 0; i < utf8Bytes.Length; i++)
                {
                    byte c = utf8Bytes[i];

                    if (new BigInteger(new byte[] { c }) > publicKey.n)
                    {
                        utf8Bytes[i] = (byte)(c % publicKey.n);
                    }
                }

                // Zaktualizowanie zmiennej klasy (nie lokalnej)
                encryptedmessage = new List<BigInteger>();
                foreach (byte c in utf8Bytes)
                {
                    BigInteger public_c = BigInteger.ModPow(c, publicKey.e, publicKey.n);
                    BigInteger private_c = BigInteger.ModPow(public_c, privateKey.d, privateKey.n);
                    encryptedmessage.Add(private_c);
                }

                // Budowanie wyników
                StringBuilder sb = new StringBuilder();
                foreach (BigInteger c in encryptedmessage)
                {
                    sb.Append(c.ToString()); // Zamienia liczby na string
                }
                decryption_field.Text = sb.ToString(); // Wyświetlanie wyników
            }
            else
            {
                throw new Exception("Niepoprawna wiadomość");
            }
        }
        private void decryption_button_Click(object sender, EventArgs e)
        {
            // Lista do przechowywania odszyfrowanych bajtów
            StringBuilder decryptedMessage = new StringBuilder(); // Używamy StringBuilder do efektywnego dodawania tekstu
            encryption_field.Text = "";

            // Odszyfrowanie każdej wartości w zaszyfrowanej wiadomości
            foreach (BigInteger encryptedValue in encryptedmessage)
            {
                // Odszyfrowanie wartości
                BigInteger public_c = BigInteger.ModPow(encryptedValue, publicKey.e, publicKey.n);
                BigInteger decryptedValue = BigInteger.ModPow(public_c, privateKey.d, privateKey.n);

                // Upewnij się, że dekodowanie odbywa się po konwersji na tablicę bajtów
                byte[] decryptedBytes = decryptedValue.ToByteArray(); // Konwertowanie BigInteger na tablicę bajtów

                // Odczytanie tekstu z tablicy bajtów (UTF-8)
                string decodedText = Encoding.UTF8.GetString(decryptedBytes);

                // Dodanie odszyfrowanego tekstu do końcowego wyniku
                decryptedMessage.Append(decodedText);
            }

            // Wyświetlanie odszyfrowanego tekstu w odpowiednim polu
            encryption_field.Text = decryptedMessage.ToString();
        }




    }
}
