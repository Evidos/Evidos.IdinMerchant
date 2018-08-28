using System;
using Evidos.Idin.DataObjects;

namespace Evidos.Idin.ResultObjects
{
	/// <summary>
	/// The status result class.
	/// </summary>
	public class StatusResult
		 : BaseResult
	{
		/// <summary>
		/// Gets the consumer.
		/// </summary>
		public Consumer Consumer { get; internal set; }

		/// <summary>
		/// Gets the status.
		/// </summary>
		public string Status { get; internal set; }

		/// <summary>
		/// Gets a value indicating whether the reqeust was successfull.
		/// </summary>
		public bool HasSuccessStatus => Status.Equals(
			"Success",
			StringComparison.Ordinal);

		/// <summary>
		/// Gets the raw response of the reqeust.
		/// </summary>
		public string RawResponse { get; internal set; }
	}
}
