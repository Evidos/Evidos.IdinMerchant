using System.Xml.Serialization;

namespace Evidos.Idin.RequestObjects
{
	/// <summary>
	/// Xml root interface.
	/// </summary>
	public interface IXmlRoot
	{
		/// <summary>
		/// Gets the xml namespaces of the request.
		/// </summary>
		XmlSerializerNamespaces Namespaces { get; }
	}
}
