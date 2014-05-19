using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Permutation_Cypher
{
    public partial class Form1 : Form
    {

        int n, m;
        List<int> l1;
        List<int> l2;
        List<char> alpha;

        public List<E> ShuffleList<E>(List<E> inputList)
        {
            List<E> randomList = new List<E>();

            Random r = new Random();
            int randomIndex = 0;
            while (inputList.Count > 0)
            {
                randomIndex = r.Next(0, inputList.Count); //Choose a random object in the list
                randomList.Add(inputList[randomIndex]);   //add it to the new, random list
                inputList.RemoveAt(randomIndex);          //remove to avoid duplicates
            }

            return randomList; //return the new random list
        }

        public int index(int e, List<int> l)
        {
            for (int i = 0; i < l.Count; i++)
                if (e == l[i])
                    return i;
            return -1;
        }

        public string keyAsString(List<int> l)
        {
            string s = "";
            for (int i = 0; i < l.Count; i++)
                s = s + l[i];
            return s;
        }

        public Boolean valid(char e)
        {
            bool ok = false;
            for (int i = 0; i < alpha.Count; i++)
                if (e == alpha[i])
                    ok = true;
            return ok;
        }

        public void loadDefaultAlphabet()
        {
            //default alphabet with 27 characters, and the order of permutation 5.
            n = 27;
            m = 5;
            l1 = new List<int>(m);
            l2 = new List<int>(m);

            for (int i = 0; i < m; i++)
                l1.Add(i + 1);

            l2 = ShuffleList<int>(l1);

            alpha = new List<char>(n);
            alpha.Add(' ');
            for (int i = 1; i < n - 1; i++)
                alpha.Add((char)(96 + i));

            txtKey.Text = keyAsString(l2);
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (defaultCheckbox.Checked)
            {
                txtn.ReadOnly = true;
                txtm.ReadOnly = true;
                txtalpha.ReadOnly = true;
                loadButton.Enabled = false;

                loadDefaultAlphabet();
            }

            else if (!defaultCheckbox.Checked)
            {
                txtn.ReadOnly = false;
                txtm.ReadOnly = false;
                txtalpha.ReadOnly = false;
                txtKey.Text = "";
            }

        }

        private void encryptButton_Click(object sender, EventArgs e)
        {
                //take the text as a string of characters and break it into small "lists" of m elements,
                //then, using l2, return a result.

           string s = txtIn.Text.ToLower();

           while (s.Length % m != 0)  //add spaces if the number of characters is not divisible by 5
                 s = s + " ";         //in order to be able to break the text in 5-characters long strings.

           string res = "";

           int p = 0;
           while (p < s.Length)
           {
               for (int j = 0; j < m; j++)
               {
                    if (valid(s[p + l2[j] - 1]))    //verifies if the character is in the alphabet
                        res = res + s[p + l2[j] - 1];
                    else
                    {
                        MessageBox.Show("'" + s[p + l2[j] - 1] + "' is not in the alphabet.");
                        res = "";
                         return;
                    }
                }
                p = p + m;
           }

           txtOut.Text = res;

        }

        private void decryptButton_Click(object sender, EventArgs e)
        {
            string s = txtIn.Text.ToLower();
            while (s.Length % m != 0)
                s = s + " ";
            string res = "";

            //compute the inverse of the permutation
            List<int> aux = new List<int>(m);
            for (int i = 0; i < m; i++)
            {
                int val = index(i + 1, l2);
                aux.Add(val + 1);
            }

            //basically, the whole crypting algorithm is reversed
            int p = 0;
            while (p < s.Length)
            {
                for (int j = 0; j < m; j++)
                {
                    if (valid(s[p + aux[j] - 1]))
                        res = res + s[p + aux[j] - 1];
                    else
                    {
                        MessageBox.Show("'" + s[p + aux[j] - 1] + "' is not in the alphabet.");
                        res = "";
                        return;
                    }
                }
                p = p + m;
            }

            txtOut.Text = res;
        }

        private void defaultCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (defaultCheckbox.Checked)
            {
                txtn.ReadOnly = true;
                txtm.ReadOnly = true;
                txtalpha.ReadOnly = true;
                loadButton.Enabled = false;
                encryptButton.Enabled = true;
                decryptButton.Enabled = true;
                refreshButton.Enabled = true;
                loadDefaultAlphabet();
                txtOut.Text = "";
            }

            else if (!defaultCheckbox.Checked)
            {
                txtn.ReadOnly = false;
                txtm.ReadOnly = false;
                txtalpha.ReadOnly = false;
                txtKey.Text = "";
                loadButton.Enabled = true;
                encryptButton.Enabled = false;
                decryptButton.Enabled = false;
                refreshButton.Enabled = false;
                txtOut.Text = "";
            }
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            //load another permutation
            l1.Clear();
            l2.Clear();

            for (int i = 0; i < m; i++)
                l1.Add(i + 1);

            l2 = ShuffleList<int>(l1);

            txtKey.Text = keyAsString(l2); 

        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            if (txtn.Text == "" || txtm.Text == "")
            {
                MessageBox.Show("Please choose a valid alphabet.");
                return;
            }

            try
            {
                n = Convert.ToInt16(txtn.Text);
            }

            catch (Exception) { MessageBox.Show("Please enter a valid number of characters."); return; }

            try
            {
                m = Convert.ToInt16(txtm.Text);
            }
            catch (Exception) { MessageBox.Show("Please enter a valid order for the permutation."); return; }

            if (m > n)
            {
                MessageBox.Show("The order of the permutation must be smaller\nthan the number of characters in the alphabet.");
                return;
            }
            string a = txtalpha.Text.ToLower();
            if (a.Length != n)
            {
                MessageBox.Show("Incorrect number of characters!");
                return;
            }

            for (int i = 0; i < a.Length - 1; i++)
                for (int j = i + 1; j < a.Length; j++)
                    if (a[i] == a[j])
                    {
                        MessageBox.Show("Alphabet cannot contain duplicates! (" + a[i] + ")");
                        return;
                    }

            l1 = new List<int>(m);
            l2 = new List<int>(m);

            for (int i = 0; i < m; i++)
                l1.Add(i + 1);

            l2 = ShuffleList<int>(l1);

            alpha = new List<char>(n);
            alpha.Add(' '); //add space by default
            for (int i = 0; i < n; i++)
                alpha.Add(a[i]);

            txtKey.Text = keyAsString(l2);

            encryptButton.Enabled = true;
            decryptButton.Enabled = true;
            refreshButton.Enabled = true;

        }

        
 
    }
    }


