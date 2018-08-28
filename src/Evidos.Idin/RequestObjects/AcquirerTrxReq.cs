using System.Xml;
using System.Xml.Serialization;
using Evidos.Idin.DataObjects;
using Evidos.Idin.Namespaces;

namespace Evidos.Idin.RequestObjects
{
	/// <summary>
	/// Acquirer transaction request class.
	/// </summary>
	[XmlRoot(Namespace = IdinNamespaces.IdinNamespace)]
	public class AcquirerTrxReq
		: BaseReq
		, IXmlRoot
	{
		/// <summary>
		/// Gets or sets the issuer.
		/// </summary>
		[XmlElement(ElementName = "Issuer")]
		public Issuer Issuer { get; set; }

		/// <summary>
		/// Gets or sets the merchant.
		/// </summary>
		[XmlElement(ElementName = "Merchant")]
		public Merchant Merchant { get; set; }

		/// <summary>
		/// Gets or sets the transaction.
		/// </summary>
		[XmlElement(ElementName = "Transaction")]
		public Transaction Transaction { get; set; }

		/// <summary>
		/// Gets the xml namespaces.
		/// </summary>
		public XmlSerializerNamespaces Namespaces { get; } =
			new XmlSerializerNamespaces(
				new XmlQualifiedName[] {
					new XmlQualifiedName(
						string.Empty,
						IdinNamespaces.IdinNamespace),
					new XmlQualifiedName("ds", XmlNamespaces.Dsig),
				}
			);
	}
}
