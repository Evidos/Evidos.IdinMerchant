using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml.Linq;
using Evidos.Idin;
using Evidos.Idin.DataObjects;
using Evidos.Idin.Namespaces;

namespace System.Xml
{
	/// <summary>
	/// Xml element extension methods.
	/// </summary>
	internal static class XmlElementExtensions
	{
		/// <summary>
		/// Verifies an element.
		/// </summary>
		/// <param name="element">Xml element.</param>
		/// <param name="signature">Signature.</param>
		/// <param name="issuerCertificates">Certificates used to verify the element.</param>
		/// <param name="thumbprint">Thumbprint used to select the right certificate.</param>
		/// <returns>Whether or not the element is verified.</returns>
		public static bool VerifyElement(
			this XmlElement element,
			XmlElement signature,
			IEnumerable<X509Certificate2> issuerCertificates,
			string thumbprint)
		{
			if (!issuerCertificates.Any(c => c.Thumbprint == thumbprint)) {
				return false;
			}

			var certificate = issuerCertificates
				.First(ic => ic.Thumbprint == thumbprint);

			var signedXml = new SignedXml(element);
			signedXml.LoadXml(signature);

			return signedXml.CheckSignature(certificate.GetRSAPublicKey());
		}

		/// <summary>
		/// Verifies an assertion.
		/// </summary>
		/// <param name="assertion">Assertion.</param>
		/// <param name="samlCertificates">Certificates used to verify the saml.</param>
		public static void VerifyAssertion(
			this XmlElement assertion,
			IEnumerable<X509Certificate2> samlCertificates)
		{
			var signature = (XmlElement)assertion
				.GetElementsByTagName("Signature", XmlNamespaces.Dsig)[0];

			var rawCertificate = signature
				.GetElementsByTagName("KeyInfo", XmlNamespaces.Dsig)[0]
				.FirstChild
				.InnerText;

			var certificate = new X509Certificate2(
				Encoding.UTF8.GetBytes(rawCertificate));

			if (
				!assertion.VerifyElement(
					signature,
					samlCertificates,
					certificate.Thumbprint)
			) {
				throw new IdinException("Can not verify saml.");
			}
		}

		/// <summary>
		/// Decrypts a saml element.
		/// </summary>
		/// <param name="samlElement">Saml element.</param>
		/// <param name="namespaceManager">Namespace manager.</param>
		/// <param name="signingCertificate">Certificate used to decrypt the element.</param>
		/// <returns>Decrypted consumer.</returns>
		public static Consumer DecryptSaml(
			this XmlElement samlElement,
			XmlNamespaceManager namespaceManager,
			X509Certificate2 signingCertificate)
		{
			var encryptedNodesList = samlElement.GetElementsByTagName(
				"EncryptedData",
				XmlNamespaces.XMLEncryptionSyntax);

			var attributes = new Dictionary<string, string>();
			var encryptedXml = new EncryptedXml();

			foreach (XmlElement node in encryptedNodesList) {
				var encryptedData = new EncryptedData();
				encryptedData.LoadXml(node);

				var plaintext = encryptedXml.DecryptData(
					encryptedData,
					node.ExtractSessionKey(
						namespaceManager,
						signingCertificate));

				var newNode = XDocument
					.Parse(Encoding.UTF8.GetString(plaintext))
					.Root;

				var name = newNode.Attribute("Name")?.Value ??
					newNode.LastAttribute.Value;

				var localName = newNode.Name.LocalName;
				if (localName == "NameID") {
					name = $"urn:nl:bvn:bankid:1.0:consumer.{localName}";
				}

				attributes.Add(name, newNode.Value);
			}

			return new Consumer { Attributes = attributes };
		}

		private static SymmetricAlgorithm ExtractSessionKey(
			this XmlElement samlElement,
			XmlNamespaceManager namespaceManager,
			X509Certificate2 signingCertificate)
		{
			var xPath =
				$"./{XmlPrefixes.DsigPrefix}:KeyInfo/" +
				$"{XmlPrefixes.XmlEncPrefix}:EncryptedKey";

			var node = (XmlElement)samlElement.SelectSingleNode(
				xPath,
				namespaceManager);

			var encryptedKey = new EncryptedKey();
			encryptedKey.LoadXml(node);

			var decryptedKey = EncryptedXml.DecryptKey(
				encryptedKey.CipherData.CipherValue,
				(RSA)signingCertificate.PrivateKey,
				true);

			var key = new RijndaelManaged {
				KeySize = 256,
				Key     = decryptedKey,
			};

			return key;
		}
	}
}
