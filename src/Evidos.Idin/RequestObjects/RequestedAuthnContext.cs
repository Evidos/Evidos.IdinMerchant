using System.Xml.Serialization;
using Evidos.Idin.Namespaces;

namespace Evidos.Idin.RequestObjects
{
	/// <summary>
	/// Requested authn context class.
	/// </summary>
	public class RequestedAuthnContext
	{
		/// <summary>
		/// Gets or sets the comparison.
		/// </summary>
		[XmlAttribute(AttributeName = "Comparison")]
		public string Comparison { get; set; } = "minimum";

		/// <summary>
		/// Gets or sets the authn context class reference.
		/// </summary>
		[XmlElement(ElementName = "AuthnContextClassRef", Namespace = SamlNamespaces.Assertion)]
		public string AuthnContextClassRef { get; set; } = "nl:bvn:bankid:1.0:loa3";
	}
}
