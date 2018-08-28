using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Evidos.Idin.Builders;
using Evidos.Idin.DataObjects;
using Evidos.Idin.Namespaces;
using Evidos.Idin.Parsers;
using Evidos.Idin.RequestObjects;
using Evidos.Idin.ResultObjects;
using Evidos.Idin.Settings;

namespace Evidos.Idin
{
	/// <summary>
	/// iDIN client.
	/// </summary>
	public class IdinClient
		: IIdinClient
	{
		private readonly Func<HttpClient> getClient;
		private readonly Uri idinUrl;
		private readonly Func<X509Certificate2> getSigningCertificate;
		private readonly Func<IEnumerable<X509Certificate2>> getIssuerCertificates;
		private readonly Func<IEnumerable<X509Certificate2>> getSamlCertificates;
		private readonly string merchantId;

		/// <summary>
		/// Initializes a new instance of the <see cref="IdinClient"/> class.
		/// </summary>
		/// <param name="idinUrl">The iDIN url.</param>
		/// <param name="getSigningCertificate">The signing certificate.</param>
		/// <param name="getIssuerCertificates">The issuer certificates.</param>
		/// <param name="getSamlCertificates">The Saml certificates.</param>
		/// <param name="merchantId">The id of the merchant.</param>
		public IdinClient(
			Uri idinUrl,
			Func<X509Certificate2> getSigningCertificate,
			Func<IEnumerable<X509Certificate2>> getIssuerCertificates,
			Func<IEnumerable<X509Certificate2>> getSamlCertificates,
			string merchantId)
			: this(
				idinUrl,
				getSigningCertificate,
				getIssuerCertificates,
				getSamlCertificates,
				merchantId,
				() => new HttpClient())
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="IdinClient"/> class.
		/// </summary>
		/// <param name="idinUrl">The iDIN url.</param>
		/// <param name="getSigningCertificate">The signing certificate.</param>
		/// <param name="getIssuerCertificates">The issuer certificates.</param>
		/// <param name="getSamlCertificates">The Saml certificates.</param>
		/// <param name="merchantId">The id of the merchant.</param>
		/// <param name="getClient">Gets the http client.</param>
		public IdinClient(
			Uri idinUrl,
			Func<X509Certificate2> getSigningCertificate,
			Func<IEnumerable<X509Certificate2>> getIssuerCertificates,
			Func<IEnumerable<X509Certificate2>> getSamlCertificates,
			string merchantId,
			Func<HttpClient> getClient)
		{
			this.idinUrl               = idinUrl;
			this.getSigningCertificate = getSigningCertificate;
			this.getIssuerCertificates = getIssuerCertificates;
			this.getSamlCertificates   = getSamlCertificates;
			this.merchantId            = merchantId;
			this.getClient             = getClient;
		}

		/// <summary>
		/// Gets the list of issuers.
		/// </summary>
		/// <param name="settings">The issuer request settings.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>The directory result containing a list of issuers.</returns>
		public async Task<DirectoryResult> GetIssuersAsync(
			GetIssuersSettings settings,
			CancellationToken cancellationToken = default)
		{
			var request = RequestBuilder.CreateDirectoryRequest(
				merchantId,
				settings);

			var response = await SendObjectAsXmlAsync(
				request,
				cancellationToken);

			return ResultParser.ParseDirectoryResult(response);
		}

		/// <summary>
		/// Initiates a transaction.
		/// </summary>
		/// <param name="settings">The initiate transaction request settings.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>The transaction result.</returns>
		public async Task<TransactionResult> InitiateTransactionAsync(
			InitiateTransactionSettings settings,
			CancellationToken cancellationToken = default)
		{
			var request = RequestBuilder.CreateAcquirerRequest(
				merchantId,
				settings);

			var response = await SendObjectAsXmlAsync(
				request,
				cancellationToken);

			return ResultParser.ParseTransactionResult(response);
		}

		/// <summary>
		/// Requests the status of a transaction.
		/// </summary>
		/// <param name="settings">The status request settings.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>Status result.</returns>
		public async Task<StatusResult> RequestStatusAsync(
			RequestStatusSettings settings,
			CancellationToken cancellationToken = default)
		{
			var request = RequestBuilder.CreateAcquirerStatusRequest(
				merchantId,
				settings);

			var response = await SendObjectAsXmlAsync(
				request,
				cancellationToken);

			return ResultParser.ParseStatusResult(response, GetAssertion);
		}

		private static XmlNamespaceManager CreateNamespaceManager(
			XmlDocument document)
		{
			var namespaceManager = new XmlNamespaceManager(
				document.NameTable);

			namespaceManager.AddNamespace(
				XmlPrefixes.IdinPrefix,
				IdinNamespaces.IdinNamespace);

			return namespaceManager;
		}

		private async Task<XmlResult> SendObjectAsXmlAsync<T>(
			T obj,
			CancellationToken cancellationToken = default)
			where T
				: IXmlRoot
		{
			var document = obj.ToXmlDocument();

			document.SignDocument(getSigningCertificate());

			var client = getClient();

			var responseDocument = await client.PostXmlDocumentAsync(
				idinUrl,
				document,
				cancellationToken);

			var namespaceManager = CreateNamespaceManager(responseDocument);

			var response = responseDocument.ValidateDocument(
				XmlPrefixes.IdinPrefix,
				namespaceManager);

			if (!response.HasError) {
				responseDocument.VerifyDocument(getIssuerCertificates());

				response.NameSpaceManager = namespaceManager;
				response.ResponseDoc = responseDocument;
			}

			return response;
		}

		private Consumer GetAssertion(
			XmlElement assertion,
			XmlNamespaceManager namespaceManager)
		{
			assertion.VerifyAssertion(getSamlCertificates());

			return assertion.DecryptSaml(
				namespaceManager,
				getSigningCertificate());
		}
	}
}
