using System.Xml;
using Evidos.Idin.DataObjects;
using Evidos.Idin.RequestObjects;
using Evidos.Idin.Settings;

namespace Evidos.Idin.Builders
{
	internal static class RequestBuilder
	{
		internal static AcquirerTrxReq CreateAcquirerRequest(
			string merchantId,
			InitiateTransactionSettings settings)
		{
			settings.Validate();

			string xmlExpirationPeriod = settings.ExpirationPeriod.HasValue
				? XmlConvert.ToString(settings.ExpirationPeriod.Value)
				: default;

			return new AcquirerTrxReq {
				Issuer = new Issuer {
					IssuerId = settings.IssuerId,
				},
				Merchant = new Merchant {
					MerchantId        = merchantId,
					SubId             = settings.SubId,
					MerchantReturnURL = settings.ReturnUrl.OriginalString,
				},
				Transaction = new Transaction {
					Language         = settings.Language,
					EntranceCode     = settings.EntranceCode,
					ExpirationPeriod = xmlExpirationPeriod,
					Container        = XmlContainerBuilder.CreateAuthnRequestContainer(
						settings.MerchantReference,
						merchantId,
						settings.ReturnUrl.AbsoluteUri,
						settings.ServiceIds),
				},
			};
		}

		internal static DirectoryReq CreateDirectoryRequest(
			string merchantId,
			GetIssuersSettings settings)
		{
			return new DirectoryReq {
				Merchant = new Merchant {
					MerchantId = merchantId,
					SubId      = settings.SubId,
				},
			};
		}

		internal static AcquirerStatusReq CreateAcquirerStatusRequest(
			string merchantId,
			RequestStatusSettings settings)
		{
			settings.Validate();

			return new AcquirerStatusReq {
				Merchant = new Merchant {
					MerchantId = merchantId,
					SubId      = settings.SubId,
				},
				Transaction = new Transaction {
					TransactionId = settings.TransactionId,
				},
			};
		}
	}
}
