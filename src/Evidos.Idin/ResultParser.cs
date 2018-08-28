using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Evidos.Idin.DataObjects;
using Evidos.Idin.Namespaces;
using Evidos.Idin.ResultObjects;

namespace Evidos.Idin.Parsers
{
	internal static class ResultParser
	{
		internal static DirectoryResult ParseDirectoryResult(
			XmlResult response)
		{
			if (response.HasError) {
				return new DirectoryResult {
					HasError = response.HasError,
					ErrorDetails = response.ErrorDetails,
				};
			}

			var namespaceManager = response.NameSpaceManager;
			var issuers = new List<Issuer>();

			foreach (XmlNode country in response
				.ResponseDoc
				.GetElementsByTagName("Country", IdinNamespaces.IdinNamespace)
			) {
				var countryName = country.SelectSingleNode(
					CreateXmlName("countryNames"),
					namespaceManager).InnerText;

				foreach (
					XmlElement issuer in country
						.SelectNodes(CreateXmlName("Issuer"), namespaceManager)
				) {
					var issuerChilds = issuer.Cast<XmlNode>().ToArray();
					issuers.Add(new Issuer {
						IssuerId    = issuerChilds
							.GetFirstOccuringValue("issuerID"),
						IssuerName  = issuerChilds
							.GetFirstOccuringValue("issuerName"),
						CountryName = countryName,
					});
				}
			}

			return new DirectoryResult {
				Issuers = issuers,
			};
		}

		internal static TransactionResult ParseTransactionResult(
			XmlResult response)
		{
			if (response.HasError) {
				return new TransactionResult {
					HasError = response.HasError,
					ErrorDetails = response.ErrorDetails,
				};
			}

			var doc = response.ResponseDoc;
			var namespaceManager = response.NameSpaceManager;

			return new TransactionResult {
				Acquirer = new Acquirer {
					AcquirerId = doc.GetInnerTextFromChild(
						"Acquirer",
						"acquirerID",
						namespaceManager),
				},
				Issuer = new Issuer {
					IssuerAuthenticationUrl = new Uri(doc.GetInnerTextFromChild(
						"Issuer",
						"issuerAuthenticationURL",
						namespaceManager)),
				},
				Transaction = new Transaction {
					TransactionId = doc.GetInnerTextFromChild(
						"Transaction",
						"transactionID",
						namespaceManager),
					TransactionCreateDateTimeStamp = doc.GetInnerTextFromChild(
						"Transaction",
						"transactionCreateDateTimestamp",
						namespaceManager),
				},
			};
		}

		internal static StatusResult ParseStatusResult(
			XmlResult response,
			Func<XmlElement, XmlNamespaceManager, Consumer> extractConsumer)
		{
			if (response.HasError) {
				return new StatusResult {
					HasError = response.HasError,
					ErrorDetails = response.ErrorDetails,
				};
			}

			var namespaceManager = response.NameSpaceManager;
			namespaceManager.AddNamespaces(
				(XmlPrefixes.SamlProtocolPrefix,  SamlNamespaces.Protocol),
				(XmlPrefixes.SamlAssertionPrefix, SamlNamespaces.Assertion),
				(XmlPrefixes.DsigPrefix,          XmlNamespaces.Dsig),
				(XmlPrefixes.XmlEncPrefix,        XmlNamespaces.XMLEncryptionSyntax)
			);

			var doc = response.ResponseDoc;

			var statusResult = new StatusResult {
				Status = doc.GetInnerTextFromChild(
					"Transaction",
					"status",
					namespaceManager),
			};

			if (!statusResult.HasSuccessStatus) {
				return statusResult;
			}

			var assertionXpath =
				$"//{CreateXmlName("Transaction")}/" +
				$"{CreateXmlName("container")}/" +
				$"{CreateXmlName("Response", XmlPrefixes.SamlProtocolPrefix)}/" +
				$"{CreateXmlName("Assertion", XmlPrefixes.SamlAssertionPrefix)}";

			var assertion = (XmlElement)response.ResponseDoc.SelectSingleNode(
				assertionXpath,
				namespaceManager);

			statusResult.Consumer = extractConsumer(assertion, namespaceManager);
			statusResult.RawResponse = response.ResponseDoc.OuterXml;

			return statusResult;
		}

		private static string CreateXmlName(
			string name,
			string prefix = XmlPrefixes.IdinPrefix)
			=> $"{prefix}:{name}";
	}
}
