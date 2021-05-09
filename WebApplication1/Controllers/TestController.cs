using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Utility;

namespace WebApplication1.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {

            var asymmetricKeys = Encryption.GenerateAsymmetricKeys();
            //signing of data upload.....

            string originaldata = "hello world";

            MemoryStream msIn = new MemoryStream(Encoding.UTF32.GetBytes(originaldata));
            msIn.Position = 0;

           string signature =  Encryption.SignData(msIn, asymmetricKeys.PrivateKey);

            //while it was stored on the server someone tampered with the data

            originaldata = "Hello world";



            //download.....

            MemoryStream msIn2 = new MemoryStream(Encoding.UTF32.GetBytes(originaldata));
            msIn2.Position = 0;

            bool result = Encryption.VerifyData(msIn2, asymmetricKeys.PublicKey, signature);





            //string cipher = Encryption.SymmetricEncrypt("this is a text which i would like to encrypt");

            //string originalString = Encryption.SymmetricDecrypt(cipher);


             

            //aGVsbG8gd29ybGQ=

           

           var cipher =  Encryption.AsymmetricEncrypt("aGVsbG8gd29ybGQ=", asymmetricKeys.PublicKey);
            var originalText = Encryption.AsymmetricDecrypt(cipher, asymmetricKeys.PrivateKey);


            return View();
        }
    }
}
