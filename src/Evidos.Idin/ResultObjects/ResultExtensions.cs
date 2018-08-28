namespace Evidos.Idin.ResultObjects
{
	/// <summary>
	/// Result extension methods.
	/// </summary>
	public static class ResultExtensions
	{
		/// <summary>
		/// Ensures that the request was successful. Throws an exception if it is not.
		/// </summary>
		/// <typeparam name="T">The type of the result.</typeparam>
		/// <param name="result">The result.</param>
		/// <returns>Result.</returns>
		/// <exception cref="IdinException">Thrown when the result contains an error.</exception>
		public static T EnsureSuccess<T>(this T result)
			where T : BaseResult
		{
			if (result.HasError) {
				throw new IdinException(result.ErrorDetails);
			}

			return result;
		}

		/// <summary>
		/// Ensures the success status is set to true. Throws an exception if it is not.
		/// </summary>
		/// <param name="result">The status result.</param>
		/// <returns>Status result.</returns>
		/// <exception cref="IdinException">Thrown when the result contains no success status.</exception>
		public static StatusResult EnsureSuccessStatus(this StatusResult result)
		{
			if (!result.HasSuccessStatus) {
				throw new IdinException(result.Status);
			}

			return result;
		}
	}
}
