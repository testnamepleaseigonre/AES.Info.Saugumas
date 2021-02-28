using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;

namespace AES.Info.Saugumas
{
    public partial class Form1 : Form
    {
        private AesCryptoServiceProvider provider;
        private Encoding encoding = Encoding.GetEncoding("437");
        //private Encoding encoding = Encoding.Unicode;

        public Form1()
        {
            InitializeComponent();
            textBox1.Text = "adsfhfgdsfds3254";
            encryptionIVtextBox.Text = "jdue758j";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (fileDialog.ShowDialog(this) == DialogResult.OK)
                richTextBox1.Text = System.IO.File.ReadAllText(fileDialog.FileName.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                provider = new AesCryptoServiceProvider();
                //provider.Padding = PaddingMode.PKCS7;
                //provider.BlockSize = 128; //128 bits
                // provider.KeySize = 256; //128, 192 or 256 bits
                validateInputDataForDecryption();
                decryption(textBox2.Text, richTextBox2.Text, decryptionIVtextBox.Text);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                provider = new AesCryptoServiceProvider();
                //provider.Padding = PaddingMode.PKCS7;
                //provider.BlockSize = 128; //128 bits
                //provider.KeySize = 256; //128, 192 or 256 bits
                validateInputDataForEncryption();
                encryption(textBox1.Text, richTextBox1.Text, encryptionIVtextBox.Text);
            }
            catch(Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void encryption(string encryptionKey, string encryptionText, string IValue)
        {
            byte[] encryptionTextBytes = encoding.GetBytes(encryptionText);
            provider.Key = encoding.GetBytes(encryptionKey);
            provider.IV = encoding.GetBytes(IValue);

            if (CBCButton.Checked)
                provider.Mode = CipherMode.CBC;
            if (ECBButton.Checked)
                provider.Mode = CipherMode.ECB;
            /*
            using (provider)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, provider.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(encryptionTextBytes, 0, encryptionTextBytes.Length);
                    }
                    richTextBox2.Text = encoding.GetString(ms.ToArray());
                    textBox3.Text = encoding.GetString(ms.ToArray());
                }
            }
            */
            ICryptoTransform icrypt = provider.CreateEncryptor(provider.Key, provider.IV);
            byte[] encryptedTextBytes = icrypt.TransformFinalBlock(encryptionTextBytes, 0, encryptionTextBytes.Length);
            icrypt.Dispose();
            richTextBox2.Text = encoding.GetString(encryptedTextBytes);
            textBox3.Text = encoding.GetString(encryptedTextBytes);
        }

        private void decryption(string decryptionKey, string decryptionText, string IValue)
        {
            byte[] decryptionTextBytes = encoding.GetBytes(decryptionText);
            provider.Key = encoding.GetBytes(decryptionKey);
            provider.IV = encoding.GetBytes(IValue);

            if (CBCButton.Checked)
                provider.Mode = CipherMode.CBC;
            if (ECBButton.Checked)
                provider.Mode = CipherMode.ECB;
            /*
            using (provider)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, provider.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(decryptionTextBytes, 0, decryptionTextBytes.Length);
                    }
                    richTextBox1.Text = encoding.GetString(ms.ToArray());
                }
            }*/
            
            ICryptoTransform icrypt = provider.CreateDecryptor(provider.Key, provider.IV);
            byte[] decryptedTextBytes = icrypt.TransformFinalBlock(decryptionTextBytes, 0, decryptionTextBytes.Length);
            icrypt.Dispose();
            richTextBox1.Text = encoding.GetString(decryptedTextBytes);
            
        }
         
        private void validateInputDataForEncryption()
        {
            if (String.IsNullOrWhiteSpace(textBox1.Text))
                throw new Exception("Plese enter the key value!");
            if (String.IsNullOrWhiteSpace(encryptionIVtextBox.Text))
                throw new Exception("Plese enter the IV value!");
            if (String.IsNullOrWhiteSpace(richTextBox1.Text))
                throw new Exception("Plese fill in text field!");
        }

        private void validateInputDataForDecryption()
        {
            if (String.IsNullOrWhiteSpace(textBox2.Text))
                throw new Exception("Plese enter the key value!");
            if (String.IsNullOrWhiteSpace(decryptionIVtextBox.Text))
                throw new Exception("Plese enter the IV value!");
            if (String.IsNullOrWhiteSpace(richTextBox2.Text))
                throw new Exception("Plese fill in text field!");
        }
    }
}
