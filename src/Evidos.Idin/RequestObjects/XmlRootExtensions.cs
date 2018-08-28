using System.Xml;
using System.Xml.Serialization;

namespace Evidos.Idin.RequestObjects
{
	internal static class XmlRootExtensions
	{
		public static XmlDocument ToXmlDocument<T>(this T xmlObject)
			where T : IXmlRoot
		{
			var doc = new XmlDocument();
			var nav = doc.CreateNavigator();
			using (var writer = nav.AppendChild()) {
				var serializer = new XmlSerializer(typeof(T));
				serializer.Serialize(writer, xmlObject, xmlObject.Namespaces);
			}

			return doc;
		}
	}
}
