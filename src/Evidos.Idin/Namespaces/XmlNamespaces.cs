namespace Evidos.Idin.Namespaces
{
	/// <summary>
	/// Contains the constants of the iDIN library.
	/// </summary>
	internal class XmlNamespaces
	{
		/// <summary>
		/// SHA256 digest namespace.
		/// </summary>
		public const string SHA256Digest =
			"http://www.w3.org/2001/04/xmlenc#sha256";

		/// <summary>
		/// SHA256 RSA signature namespace.
		/// </summary>
		public const string SHA256RSA =
			"http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";

		/// <summary>
		/// Canonical xml namespace.
		/// </summary>
		public const string CanonicalXML =
			"http://www.w3.org/2001/10/xml-exc-c14n#";

		/// <summary>
		/// Dsignature namespace.
		/// </summary>
		public const string Dsig =
			"http://www.w3.org/2000/09/xmldsig#";

		/// <summary>
		/// XML encryptino syntax namespace.
		/// </summary>
		public const string XMLEncryptionSyntax =
			"http://www.w3.org/2001/04/xmlenc#";

		/// <summary>
		/// XML schema namespace.
		/// </summary>
		public const string XmlSchema =
			"http://www.w3.org/2001/XMLSchema-instance";
	}
}
