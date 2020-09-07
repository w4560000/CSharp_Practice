using Jose;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace AppleOauth.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EecryptAuthKey()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            dynamic a;
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage httpResponseMessage = client.GetAsync("https://appleid.apple.com/auth/keys").GetAwaiter().GetResult();
                string response = httpResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                a = JsonConvert.DeserializeObject<dynamic>(response);

                var b = new JsonWebKey(JsonConvert.SerializeObject(a.keys[0]));

                // identityToken
                string token = "jwt";

                bool ww = Verify(token, b.N, b.E);
            }

            return Json(new { result = a.keys[0] }, JsonRequestBehavior.AllowGet);
        }

        public bool Verify(string accessToken, string n, string e)
        {
            string[] parts = accessToken.Split('.');

            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            provider.ImportParameters(new RSAParameters
            {
                Exponent = Base64UrlDecode(e),
                Modulus = Base64UrlDecode(n)
            });

            SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
            byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(parts[0] + "." + parts[1]));

            var c = new JwtSecurityToken(jwtEncodedString: accessToken);


            RSAPKCS1SignatureDeformatter rsaDeformatter = new RSAPKCS1SignatureDeformatter(provider);
            rsaDeformatter.SetHashAlgorithm(sha256.GetType().FullName);

            // 驗證 jwt 是否為apple加密產生的
            if (!rsaDeformatter.VerifySignature(hash, Base64UrlDecode(parts[2])))
                throw new ApplicationException(string.Format("Invalid signature"));

            return true;
        }

        // from JWT spec
        private static byte[] Base64UrlDecode(string input)
        {
            var output = input;
            output = output.Replace('-', '+'); // 62nd char of encoding
            output = output.Replace('_', '/'); // 63rd char of encoding
            switch (output.Length % 4) // Pad with trailing '='s
            {
                case 0: break; // No pad chars in this case
                case 1: output += "==="; break; // Three pad chars
                case 2: output += "=="; break; // Two pad chars
                case 3: output += "="; break; // One pad char
                default: throw new System.Exception("Illegal base64url string!");
            }
            var converted = Convert.FromBase64String(output); // Standard base64 decoder
            return converted;
        }

        static public byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            try
            {
                //byte[] decryptedData;
                //Create a new instance of RSACryptoServiceProvider.
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    //Import the RSA Key information. This needs
                    //to include the private key information.
                    RSA.ImportParameters(RSAKeyInfo);

                    //Decrypt the passed byte array and specify OAEP padding.  
                    //OAEP padding is only available on Microsoft Windows XP or
                    //later.  
                    //decryptedData = RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
                    using (MemoryStream ms = new MemoryStream(DataToDecrypt.Length))
                    {

                        //The buffer that will hold the encrypted chunks

                        byte[] buffer = new byte[2048 / 8];

                        int pos = 0;

                        int copyLength = buffer.Length;

                        while (true)
                        {

                            //Copy a chunk of encrypted data / iteration

                            Array.Copy(DataToDecrypt, pos, buffer, 0, copyLength);

                            //Set the next start position

                            pos += copyLength;

                            //Decrypt the data using the private key

                            //We need to store the decrypted data temporarily because we don't know the size of it; 
                            //unlike with encryption where we know the size is 128 bytes. 
                            //The only thing we know is that it's between 1-117 bytes

                            byte[] resp = RSA.Decrypt(buffer, false);

                            ms.Write(resp, 0, resp.Length);

                            //Cleat the buffers

                            Array.Clear(resp, 0, resp.Length);

                            Array.Clear(buffer, 0, copyLength);

                            //Are we ready to exit?

                            if (pos >= DataToDecrypt.Length)

                                break;

                        }

                        //Return the decoded data

                        return ms.ToArray();

                    }
                }
                //return decryptedData;
            }
            //Catch and display a CryptographicException  
            //to the console.
            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());

                return null;
            }

        }

        static byte[] FromBase64Url(string base64Url)
        {
            string padded = base64Url.Length % 4 == 0
                ? base64Url : base64Url + "====".Substring(base64Url.Length % 4);
            string base64 = padded.Replace("_", "/")
                                  .Replace("-", "+");
            return Convert.FromBase64String(base64);
        }

        public ActionResult Auth(string code)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var privateKeyContent = System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "AuthKey_{kid}.p8");
            var privateKeyList = privateKeyContent.Split('\n');
            int upperIndex = privateKeyList.Length;
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i < upperIndex - 1; i++)
            {
                sb.Append(privateKeyList[i]);
            }

            string jwtToken = Encode(sb.ToString());

            AuthToken authToken = new AuthToken()
            {
                client_id = "client_id",
                client_secret = jwtToken,
                code = code
            };

            var a = Post("https://appleid.apple.com/auth/token", authToken);

            return Json(new { result = a }, JsonRequestBehavior.AllowGet);
        }

        public static string Encode(string privateKey)
        {
            var a = Convert.FromBase64String(privateKey);
            byte[] keyBytes = Encoding.UTF8.GetBytes(privateKey);
            var header = new Dictionary<string, object>()
            {
                { "kid", "kid" }
            };

            var payload = new Dictionary<string, object>()
            {
                { "iss", "teamId" },
                { "iat", new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds() },
                { "exp", new DateTimeOffset(DateTime.UtcNow.AddMinutes(5)).ToUnixTimeSeconds() },
                { "aud", "https://appleid.apple.com" },
                { "sub", "client_id" },
            };

            var key = CngKey.Import(Convert.FromBase64String(privateKey),
                                    CngKeyBlobFormat.Pkcs8PrivateBlob);

            var privatekey = new ECDsaCng(CngKey.Create(CngAlgorithm.ECDsaP256));
            privatekey.SignData(a, 0, a.Length, HashAlgorithmName.SHA256);

            return Jose.JWT.Encode(payload, key, JwsAlgorithm.ES256, header);
        }

        /// <summary>
        /// Post
        /// </summary>
        /// <param name="uri">uri</param>
        /// <returns>response</returns>
        public static dynamic Post(string url, AuthToken param)
        {
            using (HttpClient client = new HttpClient())
            {
                //string response = client.GetAsync(url).GetAwaiter().GetResult().Content.ReadAsStringAsync().GetAwaiter().GetResult();

                Dictionary<string, string> formDataDictionary = new Dictionary<string, string>()
                {
                    {nameof(param.client_id), param.client_id },
                    {nameof(param.client_secret), param.client_secret },
                    {nameof(param.code), param.code },
                    {nameof(param.grant_type), param.grant_type },
                };
                var formData = new FormUrlEncodedContent(formDataDictionary);

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                HttpResponseMessage httpResponseMessage = client.PostAsync(url, formData).Result;
                string response = httpResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return JsonConvert.DeserializeObject<dynamic>(response);
            }
        }
    }

    public class AuthClass
    {
        public string AuthorizationCode { get; set; }
        public string IdentityToken { get; set; }
    }

    public class AuthToken
    {
        public string client_id { get; set; }

        public string client_secret { get; set; }

        public string code { get; set; }

        public string grant_type { get; set; } = "authorization_code";
    }
}