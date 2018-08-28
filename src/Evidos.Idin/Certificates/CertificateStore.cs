using System.Security.Cryptography.X509Certificates;

namespace Evidos.Idin.Certificates
{
	/// <summary>
	/// Service for getting certificates.
	/// </summary>
	public class CertificateStore
		: ICertificateStore
	{
		/// <summary>
		/// Gets a certificate.
		/// </summary>
		/// <param name="thumbprint">Thumbprint of the certificate.</param>
		/// <param name="validOnly">Whether the certificate should be valid.</param>
		/// <returns>A certificate.</returns>
		public X509Certificate2 GetCertificate(
			string thumbprint,
			bool validOnly)
		{
			using (var currentUser = new X509Store(
				StoreName.My,
				StoreLocation.CurrentUser))
			using (var localMachine = new X509Store(
				StoreName.My,
				StoreLocation.LocalMachine)
			) {
				var certificates = new X509Certificate2Collection();

				foreach (var store in new[] { currentUser, localMachine }) {
					store.Open(OpenFlags.ReadOnly);
					certificates.AddRange(
						store.Certificates.Find(
							X509FindType.FindByThumbprint,
							thumbprint,
							validOnly));
				}

				switch (certificates.Count) {
					case 0:
						throw new CertificateException(
							"Cannot find certificate.",
							thumbprint);

					case 1:
						return certificates[0];

					default:
						throw new CertificateException(
							"Multiple certificates were found.",
							thumbprint);
				}
			}
		}
	}
}
