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
            textBox1.Text = "qwrtedsutikh3254";
            encryptionIVtextBox.Text = "hdueiknbmvlqw234";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (fileDialog.ShowDialog(this) == DialogResult.OK)
                textBox3.Text = System.IO.File.ReadAllText(fileDialog.FileName.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                provider = new AesCryptoServiceProvider();
                validateInputDataForDecryption();
                decryption(textBox2.Text, textBox4.Text, decryptionIVtextBox.Text);
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
                validateInputDataForEncryption();
                encryption(textBox1.Text, textBox3.Text, encryptionIVtextBox.Text);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void encryption(string encryptionKey, string encryptionText, string IValue)
        {
            MemoryStream memoryStream = new MemoryStream();
            byte[] encryptionTextBytes = encoding.GetBytes(encryptionText);
            provider.Key = encoding.GetBytes(encryptionKey);

            if (CBCButton.Checked)
            {
                provider.Mode = CipherMode.CBC;
                provider.IV = encoding.GetBytes(IValue);
            }
            if (ECBButton.Checked)
                provider.Mode = CipherMode.ECB;

            CryptoStream cryptoStream = new CryptoStream(memoryStream, provider.CreateEncryptor(), CryptoStreamMode.Write);
            //ICryptoTransform icrypt = provider.CreateEncryptor(provider.Key, provider.IV);
            //byte[] encryptedTextBytes = icrypt.TransformFinalBlock(encryptionTextBytes, 0, encryptionTextBytes.Length);
            //icrypt.Dispose();
            cryptoStream.Write(encryptionTextBytes, 0, encryptionTextBytes.Length);
            cryptoStream.FlushFinalBlock();

            byte[] encryptedTextBytes = memoryStream.ToArray();
            textBox4.Text = encoding.GetString(encryptedTextBytes);
        }

        private void decryption(string decryptionKey, string decryptionText, string IValue)
        {
            byte[] decryptionTextBytes = encoding.GetBytes(decryptionText);
            provider.Key = encoding.GetBytes(decryptionKey);

            if (CBCButton.Checked)
            {
                provider.Mode = CipherMode.CBC;
                provider.IV = encoding.GetBytes(IValue);
            }
            if (ECBButton.Checked)
                provider.Mode = CipherMode.ECB;

            ICryptoTransform icrypt = provider.CreateDecryptor(provider.Key, provider.IV);
            byte[] decryptedTextBytes = icrypt.TransformFinalBlock(decryptionTextBytes, 0, decryptionTextBytes.Length);
            icrypt.Dispose();
            textBox3.Text = encoding.GetString(decryptedTextBytes);
        }

        private void validateInputDataForEncryption()
        {
            if (String.IsNullOrWhiteSpace(textBox1.Text))
                throw new Exception("Plese enter the key value!");
            if (String.IsNullOrWhiteSpace(encryptionIVtextBox.Text) && CBCButton.Checked)
                throw new Exception("Plese enter the IV value!");
            if (String.IsNullOrWhiteSpace(textBox3.Text))
                throw new Exception("Plese fill in text field!");
        }

        private void validateInputDataForDecryption()
        {
            if (String.IsNullOrWhiteSpace(textBox2.Text))
                throw new Exception("Plese enter the key value!");
            if (String.IsNullOrWhiteSpace(decryptionIVtextBox.Text) && CBCButton.Checked)
                throw new Exception("Plese enter the IV value!");
            if (String.IsNullOrWhiteSpace(textBox4.Text))
                throw new Exception("Plese fill in text field!");
        }

        private void CBCButton_CheckedChanged(object sender, EventArgs e)
        {
            //decryptionIVtextBox.Text = "";
            //encryptionIVtextBox.Text = "";
            decryptionIVtextBox.ReadOnly = false;
            encryptionIVtextBox.ReadOnly = false;
        }

        private void ECBButton_CheckedChanged(object sender, EventArgs e)
        {
            decryptionIVtextBox.Text = "";
            encryptionIVtextBox.Text = "";
            decryptionIVtextBox.ReadOnly = true;
            encryptionIVtextBox.ReadOnly = true;
        }
    }
}
