using System;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;

namespace Aes_Alorithm_For_encypt_decrypt
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string encrypted;
        private byte[] _key;


        private void button1_Click(object sender, EventArgs e)
        {
            string _text = textBox1.Text;

            if (string.IsNullOrEmpty(_text))
            {
                MessageBox.Show("please Enter a value in TextBox to Encrypt");
            }

            using (Aes _Aes = Aes.Create())
            {                
                encrypted = EncryptString(_text);
            }

            EncrptionResult.Text = encrypted;
            EncrptionResultKey.Text = Convert.ToBase64String(_key);
        }

        private string EncryptString(string plainText)
        {
            if (plainText == null || plainText.Length <= 0)
                MessageBox.Show("Text Value is Empty");


            using (Aes _aesAlg = Aes.Create())
            {
                _key = _aesAlg.Key;
                ICryptoTransform _encryptor = _aesAlg.CreateEncryptor(_aesAlg.Key, _aesAlg.IV);

                using (MemoryStream _memoryStream = new MemoryStream())
                {
                    _memoryStream.Write(_aesAlg.IV, 0, 16);
                    using (CryptoStream _cryptoStream = new CryptoStream(_memoryStream, _encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter _streamWriter = new StreamWriter(_cryptoStream))
                        {
                            _streamWriter.Write(plainText);
                        }
                        return Convert.ToBase64String(_memoryStream.ToArray());
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string _originalString;

            _originalString = DecryptString(encrypted, _key);

            MessageBox.Show("Original Text:" + _originalString);
        }

        private string DecryptString(string cipherText, byte[] Key)
        {
            if (cipherText == null || cipherText.Length <= 0)
                MessageBox.Show("encrypted Text is not available. You have to Encrypt first.");
            if (Key == null || Key.Length <= 0)
                MessageBox.Show("Key Value is Empty while decrypting. You have to Encrypt first.");

            string plaintext = null;

            byte[] _initialVector = new byte[16];
            byte[] _cipherTextBytesArray = Convert.FromBase64String(cipherText);
            byte[] _originalString = new byte[_cipherTextBytesArray.Length - 16];

            Array.Copy(_cipherTextBytesArray, 0, _initialVector, 0, _initialVector.Length);
            Array.Copy(_cipherTextBytesArray, 16, _originalString, 0, _cipherTextBytesArray.Length - 16);

            using (Aes _aesAlg = Aes.Create())
            {
                _aesAlg.Key = Key;
                _aesAlg.IV = _initialVector;
                ICryptoTransform decryptor = _aesAlg.CreateDecryptor(_aesAlg.Key, _aesAlg.IV);

                using (MemoryStream _memoryStream = new MemoryStream(_originalString))
                {
                    using (CryptoStream _cryptoStream = new CryptoStream(_memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader _streamReader = new StreamReader(_cryptoStream))
                        {
                            plaintext = _streamReader.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
