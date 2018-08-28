using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using Evidos.Idin;
using Evidos.Idin.DataObjects;
using Evidos.Idin.Namespaces;
using Evidos.Idin.ResultObjects;

namespace System.Xml
{
	/// <summary>
	/// Xml doucment extenion methods.
	/// </summary>
	internal static class XmlDocumentExtensions
	{
		/// <summary>
		/// Validates a document.
		/// </summary>
		/// <param name="document">Xml document.</param>
		/// <param name="prefix">Namespace prefix.</param>
		/// <param name="namespaceManager">Namespace manager.</param>
		/// <returns>Error details.</returns>
		internal static XmlResult ValidateDocument(
			this XmlDocument document,
			string prefix,
			XmlNamespaceManager namespaceManager)
		{
			var errorPath = $"//{prefix}:Error";
			var error = document.SelectNodes(errorPath, namespaceManager);

			var response = new XmlResult();

			if (error.Count > 0) {
				var errorChilds = error.Item(0).Cast<XmlNode>().ToArray();
				var errorDetails = new ErrorDetails
				{
					ErrorCode = errorChilds
						.GetFirstOccuringValue("errorCode"),
					ErrorMessage = errorChilds
						.GetFirstOccuringValue("errorMessage"),
					ErrorDetail = errorChilds
						.GetFirstOccuringValue("errorDetail"),
				};

				response.ErrorDetails = errorDetails;
				response.HasError = true;
			}

			return response;
		}

		/// <summary>
		/// Gets the inner text from a child element.
		/// </summary>
		/// <param name="document">Xml document.</param>
		/// <param name="name">Name of the element.</param>
		/// <param name="childname">Name of the child element.</param>
		/// <param name="namespaceManager">Namespace manager.</param>
		/// <param name="prefix">Namepsace prefix.</param>
		/// <returns>Inner text from child element.</returns>
		internal static string GetInnerTextFromChild(
			this XmlDocument document,
			string name,
			string childname,
			XmlNamespaceManager namespaceManager,
			string prefix = XmlPrefixes.IdinPrefix)
		{
			var xPathString = $"//{prefix}:{name}/{prefix}:{childname}";

			return document
				.SelectSingleNode(xPathString, namespaceManager)
				.InnerText;
		}

		/// <summary>
		/// Verifies a document.
		/// </summary>
		/// <param name="document">Xml document.</param>
		/// <param name="issuerCertificates">Certificates with which the document is verified.</param>
		internal static void VerifyDocument(
			this XmlDocument document,
			IEnumerable<X509Certificate2> issuerCertificates)
		{
			var signature = (XmlElement)document
				.DocumentElement
				.GetElementsByTagName("Signature", XmlNamespaces.Dsig)[0];

			var thumbprint = signature
				.GetElementsByTagName("KeyInfo", XmlNamespaces.Dsig)[0]
				.InnerXml;

			if (
				!document.DocumentElement.VerifyElement(
					signature,
					issuerCertificates,
					thumbprint)
			) {
				throw new IdinException("Cannot verify response document.");
			}
		}

		/// <summary>
		/// Signs a document.
		/// </summary>
		/// <param name="document">Xml document.</param>
		/// <param name="signingCertificate">Certificate used to sign the document.</param>
		internal static void SignDocument(
			this XmlDocument document,
			X509Certificate2 signingCertificate)
		{
			var keyInfo = new KeyInfo();
			keyInfo.AddClause(new KeyInfoName(signingCertificate.Thumbprint));

			var reference = new Reference();
			reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
			reference.AddTransform(new XmlDsigExcC14NTransform());
			reference.Uri = string.Empty;
			reference.DigestMethod = XmlNamespaces.SHA256Digest;

			var signedXml = new SignedXml(document) {
				SigningKey = signingCertificate.PrivateKey,
				KeyInfo    = keyInfo,
			};

			signedXml.SignedInfo.SignatureMethod = XmlNamespaces.SHA256RSA;
			signedXml.SignedInfo.CanonicalizationMethod = XmlNamespaces
				.CanonicalXML;

			signedXml.AddReference(reference);
			signedXml.ComputeSignature();

			document.DocumentElement.AppendChild(
				document.ImportNode(signedXml.GetXml(), true));
		}
	}
}
