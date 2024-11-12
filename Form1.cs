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
            private BigInteger n;
            private BigInteger public_e;
            private BigInteger private_e;
            private BigInteger d;          

            public void PublicKey(BigInteger n, BigInteger e)
            {
                //Generowanie 2 liczb o długości 2048 bitów
                var (p, q) = GeneratePrimes(2048);
                n = p * q;
                BigInteger nphi = CalculatePhi(p, q);
                e = FindE(nphi);

                this.n = n;
                public_e = e;
            }
            private void PrivateKey(BigInteger n, BigInteger e, BigInteger d) 
            {

                //Generowanie 2 liczb o długości 2048 bitów
                var (p, q) = GeneratePrimes(2048);
                n = p * q;
                BigInteger nphi = CalculatePhi(p, q);
                e = FindE(nphi);
                d = FindD(e, nphi);

                this.n = n;
                this.d = d;
                private_e = e;
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
