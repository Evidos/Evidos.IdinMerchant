using System.Xml;
using System.Xml.Serialization;
using Evidos.Idin.DataObjects;
using Evidos.Idin.Namespaces;

namespace Evidos.Idin.RequestObjects
{
	/// <summary>
	/// Directory request class.
	/// </summary>
	[XmlRoot(Namespace = IdinNamespaces.IdinNamespace)]
	public class DirectoryReq
		: BaseReq
		, IXmlRoot
	{
		/// <summary>
		/// Gets or sets the merchant.
		/// </summary>
		[XmlElement]
		public Merchant Merchant { get; set; }

		/// <summary>
		/// Gets the xml namespaces.
		/// </summary>
		public XmlSerializerNamespaces Namespaces { get; } =
			new XmlSerializerNamespaces(
				new XmlQualifiedName[] {
					new XmlQualifiedName(
						string.Empty,
						IdinNamespaces.IdinNamespace),
				}
			);
	}
}
