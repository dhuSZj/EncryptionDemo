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
using System.Xml;
namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {//
        UnicodeEncoding ByteConverter = new UnicodeEncoding();//string、byte转换器
        public string puKey = string.Empty;
        public string prKey = string.Empty;
        byte[] data;
        public Form1()
        {
            InitializeComponent();
        }
        public static byte[] RSAEncrypt(byte[] DataToEncrypt, string publicKey, bool DoOAEPPadding)
        {
            try
            {
                byte[] encryptedData;
                //Create a new instance of RSACryptoServiceProvider.
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {

                    //Import the RSA Key information. This only needs
                    //toinclude the public key information.
                    RSA.FromXmlString(publicKey);

                    //Encrypt the passed byte array and specify OAEP padding.  
                    //OAEP padding is only available on Microsoft Windows XP or
                    //later.  
                    encryptedData = RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
                }
                return encryptedData;
            }
            //Catch and display a CryptographicException  
            //to the console.
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return null;
            }
        }

        public static byte[] RSADecrypt(byte[] DataToDecrypt, string privateKey, bool DoOAEPPadding)
        {
            try
            {
                byte[] decryptedData;
                //Create a new instance of RSACryptoServiceProvider.
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    //Import the RSA Key information. This needs
                    //to include the private key information.
                    RSA.FromXmlString(privateKey);

                    //Decrypt the passed byte array and specify OAEP padding.  
                    //OAEP padding is only available on Microsoft Windows XP or
                    //later.  
                    decryptedData = RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
                }
                return decryptedData;
            }
            //Catch and display a CryptographicException  
            //to the console.
            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());

                return null;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            byte[] dataToEncrypt = ByteConverter.GetBytes(richTextBox1.Text);
            byte[] encryptedData;

            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                if (puKey != string.Empty && prKey != string.Empty)
                {
                    //Pass the data to ENCRYPT, the public key information 
                    //(using RSACryptoServiceProvider.ExportParameters(false),
                    //and a boolean flag specifying no OAEP padding.
                    encryptedData = RSAEncrypt(dataToEncrypt, puKey, false);
                    data = encryptedData;
                    string encryptedDataString = ByteConverter.GetString(encryptedData);
                    richTextBox4.Text = encryptedDataString;
                }
                else
                {
                    MessageBox.Show("未设置公私匙");
                }

            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                string publicKey = RSA.ToXmlString(false);
                string privateKey = RSA.ToXmlString(true);
                puKey = publicKey;
                prKey = privateKey;
                richTextBox2.Text = publicKey;
                richTextBox3.Text = privateKey;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            byte[] decryptedData;
            //Pass the data to DECRYPT, the private key information 
            //(using RSACryptoServiceProvider.ExportParameters(true),
            //and a boolean flag specifying no OAEP padding.
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                decryptedData = RSADecrypt(data, prKey, false);
                if (decryptedData != null) richTextBox5.Text = ByteConverter.GetString(decryptedData);
                else MessageBox.Show("解密失败");
                //Display the decrypted plaintext to the console. 
                //Console.WriteLine("Decrypted plaintext: {0}", ByteConverter.GetString(decryptedData));
            }

        }

        
    }
}
