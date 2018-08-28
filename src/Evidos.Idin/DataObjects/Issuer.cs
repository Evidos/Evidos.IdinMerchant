using System;
using System.Xml.Serialization;

namespace Evidos.Idin.DataObjects
{
	/// <summary>
	/// Issuer class.
	/// </summary>
	public class Issuer
	{
		/// <summary>
		/// Gets or sets the issuer id.
		/// </summary>
		[XmlElement(ElementName = "issuerID")]
		public string IssuerId { get; set; }

		/// <summary>
		/// Gets or sets the issuer name.
		/// </summary>
		public string IssuerName { get; set; }

		/// <summary>
		/// Gets or sets the country name.
		/// </summary>
		public string CountryName { get; set; }

		/// <summary>
		/// Gets or sets the issuer authentication url.
		/// </summary>
		[XmlIgnore]
		public Uri IssuerAuthenticationUrl { get; set; }
	}
}
