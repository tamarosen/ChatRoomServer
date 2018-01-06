using System;
using System.Xml.Linq;

namespace ChatRoomServer.Models
{
    public abstract class AbstractXmlSerializable : IXmlSerializable
    {
        public abstract bool FromXml(XElement xmlElement);

        public abstract XElement ToXml();

        public static string GetStringType(Type type)
        {
            string fqType = type.ToString();
            int lastDot = fqType.LastIndexOf('.');
            return fqType.Substring(lastDot + 1);
        }
        public string GetStringType()
        {
            return GetStringType(GetType());
        }

        public bool Valid(XElement xmlElement)
        {
            XElement t = xmlElement.Element("Type");
            if (t == null || !t.Value.Equals(GetStringType()))
            {
                return false;
            }
            return true;
        }
    }
}
