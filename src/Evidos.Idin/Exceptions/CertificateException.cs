using System;
using System.Runtime.Serialization;

namespace Evidos.Idin
{
	/// <summary>
	/// The exception that is thrown when a certificate cannot be loaded.
	/// </summary>
	[Serializable]
	public class CertificateException
		: Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CertificateException"/> class.
		/// </summary>
		public CertificateException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CertificateException"/> class.
		/// </summary>
		/// <param name="message">The exception message.</param>
		public CertificateException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CertificateException"/> class.
		/// </summary>
		/// <param name="message">The exception message.</param>
		/// <param name="inner">The inner exception.</param>
		public CertificateException(string message, Exception inner)
			: base(message, inner)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CertificateException"/> class.
		/// </summary>
		/// <param name="message">The exception message.</param>
		/// <param name="thumbprint">The thumbprint that caused the exception.</param>
		public CertificateException(string message, string thumbprint)
			: base(message)
		{
			Thumbprint = thumbprint;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CertificateException"/> class.
		/// </summary>
		/// <param name="message">The exception message.</param>
		/// <param name="thumbprint">The thumbprint that caused the exception.</param>
		/// <param name="inner">The inner exception.</param>
		public CertificateException(
			string message,
			string thumbprint,
			Exception inner)
			: base(message, inner)
		{
			Thumbprint = thumbprint;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CertificateException"/> class.
		/// </summary>
		/// <param name="info">The serialization info.</param>
		/// <param name="context">The streaming context.</param>
		protected CertificateException(
			SerializationInfo info,
			StreamingContext context)
			: base(info, context)
		{
		}

		/// <summary>
		/// Gets the thumbprint that caused the exception.
		/// </summary>
		public string Thumbprint { get; private set; }
	}
}
