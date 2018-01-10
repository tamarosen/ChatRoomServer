using System;
using System.Xml.Linq;

namespace ChatRoomServer.Models
{
    public class ChatRequestResponse : AbstractXmlSerializable
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public string From { get; set; }
        public string To { get; set; }

        public override bool FromXml(XElement xmlElement)
        {
            if (!Valid(xmlElement))
            {
                return false;
            }
            this.Success = Boolean.Parse(xmlElement.Element("Success").Value);
            this.ErrorMessage = xmlElement.Element("ErrorMessage").Value;
            this.From = xmlElement.Element("From").Value;
            this.To = xmlElement.Element("To").Value;
            return true;
        }

        public override XElement ToXml()
        {
            XElement serialized = new XElement(GetStringType(),
                new XElement("Type", GetStringType()),
                new XElement("Success", Success.ToString()),
                new XElement("ErrorMessage", ErrorMessage),
                new XElement("From", From),
                new XElement("To", To));
            return serialized;
        }
    }
}