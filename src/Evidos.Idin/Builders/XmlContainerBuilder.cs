using System;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;
using Evidos.Idin.DataObjects;
using Evidos.Idin.Namespaces;
using Evidos.Idin.RequestObjects;
using Microsoft.IdentityModel.Tokens.Saml;

namespace Evidos.Idin.Builders
{
	internal static class XmlContainerBuilder
	{
		internal static XmlElement CreateAuthnRequestContainer(
			string merchantReference,
			string merchantId,
			string returnUrl,
			ServiceIds serviceIds)
		{
			var authnRequest = new AuthnRequest {
				Id                             = merchantReference,
				IssueInstant                   = DateTimeOffset.Now.ToString(
					IdinConstants.IdinDateTimeFormat,
					CultureInfo.InvariantCulture),
				Issuer                         = merchantId,
				AssertionConsumerServiceUrl    = returnUrl,
				AttributeConsumingServiceIndex = (uint)serviceIds,
				RequestedAuthnContext          = new RequestedAuthnContext(),
			};

			var namespaces = new XmlSerializerNamespaces();
			namespaces.Add("samlp", SamlNamespaces.Protocol);
			namespaces.Add(SamlConstants.Prefix, SamlNamespaces.Assertion);

			var doc = new XmlDocument();
			var nav = doc.CreateNavigator();

			using (var writer = nav.AppendChild()) {
				var serializer = new XmlSerializer(typeof(AuthnRequest));
				serializer.Serialize(writer, authnRequest, namespaces);
			}

			return doc.DocumentElement;
		}
	}
}
