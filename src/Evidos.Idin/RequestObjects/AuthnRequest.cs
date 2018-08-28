using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using System.Xml.Serialization;
using Evidos.Idin.Namespaces;

namespace Evidos.Idin.RequestObjects
{
	/// <summary>
	/// AuthnRequest class.
	/// </summary>
	[XmlType(Namespace = SamlNamespaces.Protocol)]
	[XmlRoot("AuthnRequest", Namespace = SamlNamespaces.Protocol, IsNullable = false)]
	public class AuthnRequest
		: IXmlRoot
	{
		/// <summary>
		/// Gets or sets the id.
		/// </summary>
		[XmlAttribute(AttributeName = "ID")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the version.
		/// </summary>
		[XmlAttribute(AttributeName = "Version")]
		public string Version { get; set; } = "2.0";

		/// <summary>
		/// Gets or sets the issue instant.
		/// </summary>
		[XmlAttribute(AttributeName = "IssueInstant")]
		public string IssueInstant { get; set; }

		/// <summary>
		/// Gets or sets the protocol binding.
		/// </summary>
		[XmlAttribute(AttributeName = "ProtocolBinding")]
		public string ProtocolBinding { get; set; } =
			"nl:bvn:bankid:1.0:protocol:iDx";

		/// <summary>
		/// Gets or sets the assertion consumer service url (merchant return url).
		/// </summary>
		[XmlAttribute(AttributeName = "AssertionConsumerServiceURL")]
		[SuppressMessage(
			"Microsoft.Design",
			"CA1056: URI properties should not be strings",
			Justification = "Xml request object should not contain the Uri class.")]
		public string AssertionConsumerServiceUrl { get; set; }

		/// <summary>
		/// Gets or sets the attribute consuming service index (Service id's).
		/// </summary>
		[XmlAttribute(AttributeName = "AttributeConsumingServiceIndex")]
		public uint AttributeConsumingServiceIndex { get; set; }

		/// <summary>
		/// Gets or sets the issuer.
		/// </summary>
		[XmlElement(ElementName = "Issuer", Namespace = SamlNamespaces.Assertion)]
		public string Issuer { get; set; }

		/// <summary>
		/// Gets or sets the Requested authn context.
		/// </summary>
		[XmlElement(ElementName = "RequestedAuthnContext", Namespace = SamlNamespaces.Protocol)]
		public RequestedAuthnContext RequestedAuthnContext { get; set; }

		/// <summary>
		/// Gets the xml namespaces.
		/// </summary>
		public XmlSerializerNamespaces Namespaces { get; } = new XmlSerializerNamespaces(
			new XmlQualifiedName[] {
				new XmlQualifiedName(string.Empty, SamlNamespaces.Protocol),
		});
	}
}
