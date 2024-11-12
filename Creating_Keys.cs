using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RSA_Cipher
{
    public class Creating_Keys
    {
        //Pierwiastkowanie na dużych liczbach
        private static BigInteger Sqrt(BigInteger n)
        {
            if (n == 0) return 0;
            return (BigInteger)Math.Sqrt((double)n);
        }

        // Sprawdzanie, czy liczba jest prawie pierwiastkiem z iloczynu p i q
        private static bool IsApproximatelySqrtN(BigInteger p, BigInteger q)
        {
            BigInteger n = p * q;
            BigInteger sqrtN = Sqrt(n);
            return Math.Abs((p - sqrtN).GetBitLength() - (q - sqrtN).GetBitLength()) < 2;
        }

        // Sprawdzanie czy jest to liczba pierwsza
        private static bool IsProbablyPrime(BigInteger number, int k = 10)
        {
            if (number <= 1) return false;
            if (number <= 3 || number % 2 == 0 || number % 3 == 0) return true;

            // Wyznaczenie d i s (faktoryzacja liczby number-1)
            BigInteger d = number - 1;
            int s = 0;
            while (d % 2 == 0)
            {
                d /= 2;
                s++;
            }
            //generowanie kryptograficzniej bezpiecznej liczby losowej
            using (var rng = new RNGCryptoServiceProvider())
            {
                for (int i = 0; i < k; i++)
                {
                    // Losowanie podstawy 'a' w zakresie [2, number-2]
                    byte[] bytes = new byte[number.ToByteArray().Length];
                    rng.GetBytes(bytes);
                    BigInteger a = new BigInteger(bytes) % (number - 4) + 2;

                    BigInteger x = BigInteger.ModPow(a, d, number);
                    if (x == 1 || x == number - 1) continue;

                    // Test Millera-Rabina (potęgowanie x mod number), który pozwoli
                    // z dużym prawdopodobieństwem określić, czy liczba jest liczbą pierwszą
                    for (int r = 0; r < s - 1; r++)
                    {
                        x = BigInteger.ModPow(x, 2, number);
                        if (x == 1) return false;
                        if (x == number - 1) break;
                    }

                    if (x != number - 1) return false;
                }
            }

            return true;
        }


        // Funkcja generująca dużą liczbę pierwszą
        private static BigInteger GenerateLargePrime(int bitLength)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] bytes = new byte[bitLength / 8];
                BigInteger prime;
                do
                {
                    rng.GetBytes(bytes);
                    bytes[bytes.Length - 1] &= 0x7F;
                    prime = new BigInteger(bytes);
                } while (!IsProbablyPrime(prime));
                return prime;
            }
        }

        // Generowanie pary liczb pierwszych
        public static (BigInteger p, BigInteger q) GeneratePrimes(int bitLength)
        {
            BigInteger p, q;
            do
            {
                p = GenerateLargePrime(bitLength);
                q = GenerateLargePrime(bitLength);
            } while (!IsApproximatelySqrtN(p, q));

            return (p, q);
        }
        // Funkcja obliczająca funkcję Eulera (phi)
        public static BigInteger CalculatePhi(BigInteger p, BigInteger q)
        {
            return (p - 1) * (q - 1);
        }
        // Funkcja do obliczenia największego wspólnego dzielnika (gcd)
        public static BigInteger GCD(BigInteger a, BigInteger b)
        {
            while (b != 0)
            {
                BigInteger temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        // Funkcja do znalezienia liczby e, która jest względnie pierwsza z phi(n)
        public static BigInteger FindE(BigInteger phiN)
        {
            BigInteger e = 2;  // Najmniejsza wartość większa niż 1

            while (e < phiN)
            {
                if (GCD(e, phiN) == 1)  // e musi być względnie pierwsza z phi(n)
                {
                    return e;  // Znaleziono odpowiednią liczbę e
                }
                e++;
            }

            throw new Exception("Nie udało się znaleźć odpowiedniego e");
        }
        // Funkcja do znalezienia liczby d, która jest odwrotnością modularną liczby e
        //na bazie algorytmu Euklidesa rozszerzonego, która zwraca odwrotność modularną liczby e
        public static BigInteger FindD (BigInteger e, BigInteger phiN) 
        {
            
            BigInteger t = 0, newT = 1;
            BigInteger r = phiN, newR = e;

            while (newR != 0)
            {
                BigInteger quotient = r / newR;

                // Oblicza t i r za pomocą prostej wymiany
                (t, newT) = (newT, t - quotient * newT);
                (r, newR) = (newR, r - quotient * newR);
            }

            if (r > 1) throw new Exception("Nie istnieje odwrotność modulo.");

            return t < 0 ? t + phiN : t;
            
        }
    }
}
