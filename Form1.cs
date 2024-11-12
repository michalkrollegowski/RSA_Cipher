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
            private static (BigInteger, BigInteger) PrivateKey() 
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
        private void encryption_button_Click(object sender, EventArgs e)
        {

        }

        private void decrypting_button_Click(object sender, EventArgs e)
        {

        }
    }
}
