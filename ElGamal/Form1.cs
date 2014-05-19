using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Numerics;

namespace Lab_5___ElGamal
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private bool isPrime(BigInteger p)
        {
            for (BigInteger i = 2; i < p / 2; i++)
                if (p % i == 0) return false;
            return true;
        }

        private BigInteger genPrime()
        {
            Random r = new Random();
            int a = r.Next()%10000000;
            while (!isPrime(a))
                a = r.Next()%10000000;
            return a;
        }

        private int chooseGen(int p)
        {
            Random r = new Random();
            int a = r.Next() % 10;
            while (!isPrime(a))
                a = r.Next() % 10;
            return a;
        }

        public String Encryption(BigInteger p, BigInteger g, BigInteger ga, BigInteger k, String message)
        {
            List<BigInteger> m = new List<BigInteger>();
            List<char> c = new List<char>();

            for (int i = 0; i < message.Length; i++)
                c.Add(message[i]);

            for (int i = 0; i < c.Count; i++)
            {
                m.Add(text2big(c[i]));
            }

            BigInteger alpha = BigInteger.ModPow(g, k, p);
            BigInteger beta;
            String result = alpha.ToString();

            for (int i = 0; i < m.Count ; i++)
            {
                beta = (BigInteger.ModPow(ga, k, p) * m[i]) % p;
                result += " " + beta.ToString();
            }

            return result;
        }

        public String Decryption(BigInteger alpha, BigInteger beta, BigInteger a, BigInteger p)
        {

            int m = (int)((BigInteger.ModPow(alpha, p - 1 - a, p) * beta) % p);

            return big2text(m).ToString();
        }

        private char big2text(int x)
        {
            if (x == 0)
                return ' ';
            else
            {
                List<char> alpha = new List<char>(27);
                alpha.Add(' ');
                for (int i = 1; i < 26; i++)
                    alpha.Add((char)(96 + i));

                return alpha.ElementAt(x);
            }
        }

        private BigInteger text2big(char x)
        {
            List<char> alpha = new List<char>(27);
            alpha.Add(' ');
            for (int i = 1; i < 26; i++)
                alpha.Add((char)(96 + i));
            for (int j = 0; j < alpha.Count; j++)
                if (x.Equals(alpha.ElementAt(j)))
                    return j;
            return 0;
        }

        private void btnPrimeNumber_Click(object sender, EventArgs e)
        {
            txtP.Text = genPrime().ToString();
        }

        private int Generator(BigInteger p)
        {
            BigInteger res = 1, saveP = p - 1;
            int i = 2;
            List<int> primes = new List<int>();
            while (i <= ((p - 1) / 2))
            {
                if (isPrime(i))
                {
                    while (saveP % i == 0)
                    {
                        primes.Add(i);
                        saveP = saveP / i;
                    }
                }
                if (saveP == 1)
                {
                    break;
                }
                i++;
            }

            int[] primes_arr = primes.Distinct().ToArray();

            i = 1;
            while (i < (p - 1))
            {
                int k = 0;

                for (int j = 0; j < primes_arr.Length; j++)
                {
                    if (BigInteger.ModPow(i, primes_arr[j], p) == 1)
                    {
                        continue;
                    }
                    k++;
                }

                if (k == primes_arr.Length)
                {
                    return i;
                }

                i++;
            }
            return 0;
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            BigInteger p;
            BigInteger a;
            try
            {
                p = BigInteger.Parse(txtP.Text);
                a = BigInteger.Parse(txtA.Text);
            }
            catch (Exception) { MessageBox.Show("Please give valid values."); return; }

            if (!isPrime(p))
            {
                MessageBox.Show(p + " is not prime.");
                return;
            }

            BigInteger g = Generator(p);

            BigInteger b = BigInteger.ModPow(g, a, p);

            txtB.Text = p + " " + g + " " + b;
            txtPubKey.Text = txtB.Text;
            txtPrivateKey.Text = txtA.Text;
            txtPrimeDecr.Text = txtP.Text;
            txtMes.Clear();
            txtMesDecr.Clear();
            txtCipher.Clear();
            txtCipherDecr.Clear();
        }

        private void btnCipher_Click(object sender, EventArgs e)
        {
            String[] splits = txtPubKey.Text.Split(' ');

            BigInteger p;
            BigInteger g;
            BigInteger b;
            BigInteger k;

            try
            {
                p = BigInteger.Parse(splits[0]);
                g = BigInteger.Parse(splits[1]);
                b = BigInteger.Parse(splits[2]);
                k = BigInteger.Parse(txtK.Text);
            }
            catch (Exception) { MessageBox.Show("Please give valid values."); return; }

            txtCipher.Text = Encryption(p, g, b, k, txtMes.Text);
            txtCipherDecr.Text = txtCipher.Text;
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            txtMesDecr.Clear();

            String[] splits = txtCipherDecr.Text.Split(' ');

            BigInteger alpha;
            BigInteger a;
            BigInteger p;
            
            try
            {
                alpha = BigInteger.Parse(splits[0]);
                a = BigInteger.Parse(txtPrivateKey.Text);
                p = BigInteger.Parse(txtPrimeDecr.Text);
            }
            catch (Exception) { MessageBox.Show("Please give valid values."); return; }
            int i = 1;
            while (i < splits.Length)
            {
                BigInteger beta;
                try
                {
                    beta = BigInteger.Parse(splits[i]);
                }
                catch (Exception) { MessageBox.Show("Please give valid values."); return; }
                txtMesDecr.Text += Decryption(alpha, beta, a, p);
                i++;
            }
        }

        private void txtCipherDecr_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
