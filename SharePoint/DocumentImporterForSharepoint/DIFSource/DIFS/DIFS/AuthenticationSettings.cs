using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DIFS
{
    public class AuthenticationSettings
    {
        // The type of authentication
        public enum AuthenticationTypes { Current, Specified, Forms, Office365};
        public AuthenticationTypes AuthenticationType = AuthenticationTypes.Current;
        public string domain = string.Empty;
        public string username = string.Empty;
        public string encryptedpassed { get; set; }
        [XmlIgnoreAttribute]
        public string password
                { 
                    get 
                    {
                        return decrypt(encryptedpassed); 
                    } 
                    set 
                    {
                        encryptedpassed = encrypt(value); 
                    } 
                }


        private string encrypt(string password)
        {
            if (password == string.Empty)
            {
                return string.Empty;
            }
            byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(password); 
            string encryptedString = Convert.ToBase64String(b); 
            return encryptedString;
        }

        private string decrypt(string password)
        {
            if (password == string.Empty)
            {
                return string.Empty;
            }
            byte[] b = Convert.FromBase64String(password); 
            string decryptedString = System.Text.ASCIIEncoding.ASCII.GetString(b); 
            return decryptedString;
        }
            
    }
}
