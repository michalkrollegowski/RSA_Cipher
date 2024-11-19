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
        public window_application()
        {
            InitializeComponent();
        }

        public class RSA : Creating_Keys
        {
            private static BigInteger p, q, n, nphi, e, d;

            public static (BigInteger n, BigInteger e) GeneratePublicKey()
            {
                if (p == 0 || q == 0)
                {
                    (p, q) = GeneratePrimes(2048);
                }

                n = p * q;
                nphi = CalculatePhi(p, q);
                e = FindE(nphi);

                return (n, e);
            }

            public static (BigInteger n, BigInteger d) GeneratePrivateKey()
            {
                if (p == 0 || q == 0)
                {
                    (p, q) = GeneratePrimes(2048);
                }

                n = p * q;
                nphi = CalculatePhi(p, q);
                e = FindE(nphi);
                d = FindD(e, nphi);

                return (n, d);
            }
        }

        private (BigInteger n, BigInteger e) publicKey = RSA.GeneratePublicKey();
        private (BigInteger n, BigInteger d) privateKey = RSA.GeneratePrivateKey();
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

            // Sprawdzamy, czy wiadomość zawiera tylko dozwolone znaki
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

                // Rozmiar bloku zależny od długości klucza (dla RSA 2048-bitowego to 256 bajtów)
                int blockSize = publicKey.n.ToByteArray().Length;

                // Podzielamy wiadomość na bloki
                for (int i = 0; i < utf8Bytes.Length; i += blockSize - 3)  // -3 na padding
                {
                    byte[] block = utf8Bytes.Skip(i).Take(blockSize - 3).ToArray();

                    // Dodajemy padding PKCS#1 do danych
                    byte[] paddedBlock = AddPKCS1Padding(block, blockSize);

                    // Konwertujemy blok na BigInteger
                    BigInteger blockInt = new BigInteger(paddedBlock.Reverse().ToArray()); // Konwersja w odwrotnej kolejności bajtów
                    BigInteger encryptedBlock = BigInteger.ModPow(blockInt, publicKey.e, publicKey.n);
                    encryptedmessage.Add(encryptedBlock);
                }

                // Wyświetl zaszyfrowaną wiadomość
                decryption_field.Text = string.Join(" ", encryptedmessage);
            }
            else
            {
                throw new Exception("Niepoprawna wiadomość");
            }
        }


        private void decryption_button_Click(object sender, EventArgs e)
        {
            encryption_field.Text = "";  // Wyczyść pole do wprowadzenia tekstu odszyfrowanego
            string input_text = decryption_field.Text;
            bool isProperInput = true;
            encryptedmessage = new List<BigInteger>();

            // Sprawdzamy, czy dane wejściowe są poprawne (czy to liczby BigInteger)
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

                // Deszyfrowanie RSA z paddingiem
                foreach (BigInteger encryptedChar in encryptedmessage)
                {
                    BigInteger decryptedChar = BigInteger.ModPow(encryptedChar, privateKey.d, privateKey.n);
                    decryptedmessage.Add(decryptedChar);
                }

                // Konwertowanie danych na tekst
                foreach (BigInteger decryptedChar in decryptedmessage)
                {
                    byte[] byteArray = decryptedChar.ToByteArray();
                    byte[] unpadded = RemovePKCS1Padding(byteArray); // Usuwamy padding
                    decryptedText.Append(Encoding.UTF8.GetString(unpadded));
                }

                encryption_field.Text = decryptedText.ToString();
            }
            else
            {
                throw new Exception("Niepoprawny format wiadomości");
            }
        }
        private byte[] AddPKCS1Padding(byte[] data, int blockSize)
        {
            // Obliczamy, ile bajtów paddingu musimy dodać
            int paddingSize = blockSize - data.Length - 3; // 3 bajty to 0x00 i dwa bajty paddingu 0xFF
            if (paddingSize < 8)
            {
                throw new Exception("Dane są zbyt duże, aby pomieścić je w jednym bloku.");
            }

            // Tworzymy nową tablicę na dane z paddingiem
            byte[] paddedData = new byte[blockSize];

            // Wypełniamy pierwsze 'paddingSize' bajtów tablicy 0xFF
            for (int i = 0; i < paddingSize; i++)
            {
                paddedData[i] = 0xFF;
            }

            // Dodajemy separator 0x00
            paddedData[paddingSize] = 0x00;

            // Kopiujemy dane w miejsce po paddingu
            Array.Copy(data, 0, paddedData, paddingSize + 1, data.Length);

            return paddedData;
        }

        private byte[] RemovePKCS1Padding(byte[] data)
        {
            // Szukamy separatora 0x00
            int separatorIndex = Array.IndexOf(data, (byte)0x00);

            if (separatorIndex == -1)
            {
                throw new Exception("Brak separatora PKCS#1.");
            }

            // Pobieramy dane po separatorze
            byte[] unpaddedData = new byte[data.Length - separatorIndex - 1];
            Array.Copy(data, separatorIndex + 1, unpaddedData, 0, unpaddedData.Length);

            return unpaddedData;
        }

    }
}
