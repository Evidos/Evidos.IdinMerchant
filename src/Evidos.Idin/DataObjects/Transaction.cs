using System.Xml;
using System.Xml.Serialization;

namespace Evidos.Idin.DataObjects
{
	/// <summary>
	/// Transaction class.
	/// </summary>
	public class Transaction
	{
		/// <summary>
		/// Gets or sets the transaction id.
		/// </summary>
		[XmlElement(ElementName = "transactionID")]
		public string TransactionId { get; set; }

		/// <summary>
		/// Gets or sets the expiration period.
		/// </summary>
		[XmlElement(ElementName = "expirationPeriod")]
		public string ExpirationPeriod { get; set; }

		/// <summary>
		/// Gets or sets the language.
		/// </summary>
		[XmlElement(ElementName = "language")]
		public string Language { get; set; }

		/// <summary>
		/// Gets or sets the entrance code.
		/// </summary>
		[XmlElement(ElementName = "entranceCode")]
		public string EntranceCode { get; set; }

		/// <summary>
		/// Gets or sets the container element.
		/// </summary>
		[XmlElement(ElementName = "container")]
		public XmlElement Container { get; set; }

		/// <summary>
		/// Gets or sets the timestamp at which the transaction was created.
		/// </summary>
		public string TransactionCreateDateTimeStamp { get; set; }
	}
}
