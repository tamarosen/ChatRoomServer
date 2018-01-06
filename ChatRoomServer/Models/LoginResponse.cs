using System;
using System.Xml.Linq;

namespace ChatRoomServer.Models
{
    class LoginResponse: AbstractXmlSerializable
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }

        public override bool FromXml(XElement xmlElement)
        {
            if (!Valid(xmlElement))
            {
                return false;
            }
            this.Success = Boolean.Parse(xmlElement.Element("Success").Value);
            this.ErrorMessage = xmlElement.Element("ErrorMessage").Value;
            return true;
        }

        public override XElement ToXml()
        {
            XElement serialized = new XElement(GetStringType(),
                new XElement("Type", GetStringType()),
                new XElement("Success", Success.ToString()),
                new XElement("ErrorMessage", ErrorMessage));
            return serialized;
        }
    }
}
