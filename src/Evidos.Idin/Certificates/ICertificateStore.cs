using System.Security.Cryptography.X509Certificates;

namespace Evidos.Idin.Certificates
{
	/// <summary>
	/// Certificate store.
	/// </summary>
	public interface ICertificateStore
	{
		/// <summary>
		/// Gets a certificate from the store.
		/// </summary>
		/// <param name="thumbprint">Thumbprint of the certificate.</param>
		/// <param name="nonExpired">Wether only non expired certificates should be retrieved.</param>
		/// <returns>A x509Certificate.</returns>
		X509Certificate2 GetCertificate(string thumbprint, bool nonExpired);
	}
}
