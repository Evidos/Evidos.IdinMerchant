using System;
using System.Globalization;
using System.Xml.Serialization;

namespace Evidos.Idin.RequestObjects
{
	/// <summary>
	/// The base request.
	/// </summary>
	public abstract class BaseReq
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BaseReq"/> class.
		/// </summary>
		public BaseReq()
		{
			CreateDateTimestamp =
				DateTimeOffset.Now.ToString(
					IdinConstants.IdinDateTimeFormat,
					CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Gets or sets the version.
		/// </summary>
		[XmlAttribute(AttributeName = "version")]
		public string Version { get; set; } = "1.0.0";

		/// <summary>
		/// Gets or sets the product id.
		/// </summary>
		[XmlAttribute(AttributeName = "productID")]
		public string ProductId { get; set; } = "NL:BVN:BankID:1.0";

		/// <summary>
		/// Gets or sets the create date timestamp.
		/// </summary>
		[XmlElement(ElementName = "createDateTimestamp")]
		public string CreateDateTimestamp { get; set; }
	}
}
