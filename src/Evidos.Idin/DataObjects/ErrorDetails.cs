namespace Evidos.Idin.DataObjects
{
	/// <summary>
	/// Error details class.
	/// </summary>
	public class ErrorDetails
	{
		/// <summary>
		/// Gets the error code.
		/// </summary>
		public string ErrorCode { get; internal set; }

		/// <summary>
		/// Gets the error message.
		/// </summary>
		public string ErrorMessage { get; internal set; }

		/// <summary>
		/// Gets the error details.
		/// </summary>
		public string ErrorDetail { get; internal set; }
	}
}
