﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XmlHandler.XmlEntities
{
    public class XmlDocParagraph : XmlDocNode
    {
        public XmlDocParagraph(XmlNode xmlNode) : base(xmlNode)
        {

        }

        public string GetParagraphText()
        {
            return xmlNode.InnerText.Trim();
        }

        public string[] GetLinesText()
        {
            List<string> lines = new List<string>();
            var nodeLines = xmlNode.SelectNodes(".//w:t", this.namespaceManager);
            foreach (XmlNode node in nodeLines)
                lines.Add(node.InnerText);

            return lines.ToArray();
        }

        public string[] GetLinesWithTab()
        {
            List<string> lines = new List<string>();
            string currentLine = string.Empty;

            var nodeLines = xmlNode.SelectNodes(".//w:t | .//w:tab", namespaceManager);

            foreach (XmlNode node in nodeLines)
            {
                if (node.Name == "w:t")
                    currentLine = string.Concat(currentLine, node.InnerText);
                else if(node.Name == "w:tab")
                {
                    if(!string.IsNullOrEmpty(currentLine))
                        lines.Add(currentLine);

                    currentLine = string.Empty;
                }                   
            }

            if (!string.IsNullOrEmpty(currentLine))
                lines.Add(currentLine);

            return lines.ToArray();

        }

      
    }
}