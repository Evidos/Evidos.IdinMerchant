using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace Evidos.Idin.DataObjects
{
	/// <summary>
	/// Merchant class.
	/// </summary>
	public class Merchant
	{
		/// <summary>
		/// Gets or Sets the merchant id.
		/// </summary>
		[XmlElement(ElementName = "merchantID")]
		public string MerchantId { get; set; }

		/// <summary>
		/// Gets or sets the merchant sub id.
		/// </summary>
		[XmlElement(ElementName = "subID")]
		public uint SubId { get; set; }

		[XmlElement(ElementName = "DocumentID")]
		public string DocumentId { get; set; }

		/// <summary>
		/// Gets or sets the merchant return url.
		/// </summary>
		[XmlElement(ElementName = "merchantReturnURL")]
		[SuppressMessage(
			"Microsoft.Design",
			"CA1056: URI properties should not be strings",
			Justification = "Xml request object should not contain the Uri class.")]
		public string MerchantReturnURL { get; set; }
	}
}
