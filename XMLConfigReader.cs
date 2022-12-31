using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace WpfAndSQL
{
    class XMLConfigReader
    {

        private string strReadPageA;
        private string strReadPageB;
        private string strReadPageC;

        public string ConnetionStr { get; private set; }
        public string[] StrPageReadCommands
        {
            get
            {
                return new string[] { strReadPageA, strReadPageB, strReadPageC };
            }
        }

        public XMLConfigReader(string XMLFileName)
        {
            try
            {
                XmlDocument document = new XmlDocument();
                document.Load(XMLFileName);

                XmlNode nodeStrReadPageA = document.GetElementsByTagName("StrReadPageA")[0];
                XmlNode nodeStrReadPageB = document.GetElementsByTagName("StrReadPageB")[0];
                XmlNode nodeStrReadPageC = document.GetElementsByTagName("StrReadPageC")[0];
                XmlNode nodeConnetionStr = document.GetElementsByTagName("ConnetionStr")[0];

                strReadPageA = nodeStrReadPageA.InnerText;
                strReadPageB = nodeStrReadPageB.InnerText;
                strReadPageC = nodeStrReadPageC.InnerText;
                ConnetionStr = nodeConnetionStr.InnerText;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
