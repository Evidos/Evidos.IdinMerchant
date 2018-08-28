using System;
using Evidos.Idin.DataObjects;

namespace Evidos.Idin.Settings
{
	/// <summary>
	/// Initiate transaction settings.
	/// </summary>
	public class InitiateTransactionSettings
	{
		/// <summary>
		/// Gets or sets the issuer id.
		/// </summary>
		public string IssuerId { get; set; }

		/// <summary>
		/// Gets or sets the return url.
		/// </summary>
		public Uri ReturnUrl { get; set; }

		/// <summary>
		/// Gets or sets the entrance code.
		/// </summary>
		public string EntranceCode { get; set; }

		/// <summary>
		/// Gets or sets the merchant reference.
		/// </summary>
		public string MerchantReference { get; set; }

		/// <summary>
		/// Gets or sets the service id's.
		/// </summary>
		public ServiceIds ServiceIds { get; set; }

		/// <summary>
		/// Gets or sets the language.
		/// </summary>
		public string Language { get; set; }

		/// <summary>
		/// Gets or sets the expiration period.
		/// </summary>
		public TimeSpan? ExpirationPeriod { get; set; }

		/// <summary>
		/// Gets or sets the sub id of the merchant.
		/// </summary>
		public uint SubId { get; set; }
	}
}
