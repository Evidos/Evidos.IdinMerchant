using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace System.Net.Http
{
	internal static class HttpClientExtensions
	{
		[Fody.ConfigureAwait(false)]
		[SuppressMessage(
			"Microsoft.Reliability",
			"CA2007:DoNotDirectlyAwaitATaskAnalyzer",
			Justification = "Fody ConfigureAwait")]
		public static async Task<XmlDocument> PostXmlDocumentAsync(
			this HttpClient client,
			Uri requestUri,
			XmlDocument document,
			CancellationToken cancellationToken = default)
		{
			var stringContent = new StringContent(
				document.OuterXml,
				Encoding.UTF8,
				"text/xml");

			var response = await client.PostAsync(
				requestUri,
				stringContent,
				cancellationToken);

			using (response) {
				var responseDoc = new XmlDocument() {
					PreserveWhitespace = true,
				};

				var content = await response.Content.ReadAsStringAsync();
				responseDoc.LoadXml(content);

				return responseDoc;
			}
		}
	}
}
