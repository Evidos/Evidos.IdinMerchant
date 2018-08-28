using Evidos.Idin.DataObjects;

namespace Evidos.Idin.ResultObjects
{
	/// <summary>
	/// The base result.
	/// </summary>
	public abstract class BaseResult
	{
		/// <summary>
		/// Gets a value indicating whether the result contains an error.
		/// </summary>
		public bool HasError { get; internal set; }

		/// <summary>
		/// Gets the error details.
		/// </summary>
		public ErrorDetails ErrorDetails { get; internal set; }
	}
}
