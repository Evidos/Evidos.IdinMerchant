using System;
using Evidos.Idin.DataObjects;

namespace Evidos.Idin
{
	/// <summary>
	/// Idin Excpetion.
	/// </summary>
	public class IdinException
		: Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="IdinException"/> class.
		/// </summary>
		public IdinException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="IdinException"/> class.
		/// </summary>
		/// <param name="message">The exception message.</param>
		public IdinException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="IdinException"/> class.
		/// </summary>
		/// <param name="message">The exception message.</param>
		/// <param name="innerException">The inner exception.</param>
		public IdinException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="IdinException"/> class.
		/// </summary>
		/// <param name="errorDetails">The error details.</param>
		public IdinException(ErrorDetails errorDetails)
			: base(errorDetails.ErrorMessage)
		{
			ErrorDetail = errorDetails.ErrorDetail;
			ErrorCode   = errorDetails.ErrorCode;
		}

		/// <summary>
		/// Gets or sets the iDIN error detail.
		/// </summary>
		public string ErrorDetail { get; set; }

		/// <summary>
		/// Gets or sets the iDIN error code.
		/// </summary>
		public string ErrorCode { get; set; }
	}
}
