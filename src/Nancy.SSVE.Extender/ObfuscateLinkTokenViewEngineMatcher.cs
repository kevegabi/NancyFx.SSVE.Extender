using Nancy.ViewEngines.SuperSimpleViewEngine;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Nancy.SSVE.Extender
{
    public sealed class ObfuscateLinkTokenViewEngineMatcher : ISuperSimpleViewEngineMatcher
    {
        private static readonly Regex ObfuscateLinkTokenRegEx = new Regex(@"@ObfuscateLink?\.(?<PlainLink>[a-zA-Z0-9-_\@\.\:\\\/]+);?", RegexOptions.Compiled);
        //private static readonly Regex ObfuscateLinkTokenRegEx = new Regex(@"@ObfuscateLink?\.(?<PlainLink>[^;]+);?", RegexOptions.Compiled);
        

        public string Invoke(string content, dynamic model, IViewEngineHost host)
        {
            return ObfuscateLinkTokenRegEx.Replace(
                content,
                m =>
                {
                    var PlainLink = m.Groups["PlainLink"].Value;
                    if (!String.IsNullOrEmpty(PlainLink))
                    {
                        return DoObfuscate(PlainLink);
                    }
                    else
                    {
                        return "[ERR]";
                    }
                });
        }

        /// <summary>
        /// Does the obfuscate.
        /// </summary>
        /// <param name="referenceString">The reference string.</param>
        /// <returns>Obfuscated string</returns>
        private string DoObfuscate(string referenceString)
        {
            Random rnd = new Random(101);
            string neverEncode = ".@+";

            StringBuilder urlEncodedEmail = new StringBuilder();
            for (var i = 0; i < referenceString.Length; i++)
            {
                // Encode 25% of characters
                if (!neverEncode.Contains(referenceString[i].ToString()) && rnd.Next(1,100) < 50)
                {
                    char charCode = referenceString[i];
                    urlEncodedEmail.Append('%');
                    urlEncodedEmail.Append(((charCode >> 4) & 0xF).ToString("X"));
                    urlEncodedEmail.Append((charCode & 0xF).ToString("X"));
                }
                else
                {
                    urlEncodedEmail.Append(referenceString[i]);
                }
            }
            return urlEncodedEmail.ToString();
        }
    }
}

