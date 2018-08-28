using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Evidos.Idin.ResultObjects;

namespace Evidos.Idin
{
	/// <summary>
	/// Idin client extension methods.
	/// </summary>
	public static class IdinClientExtensions
	{
		/// <summary>
		/// Ensures that the request was successful. Throws an exception if it is not.
		/// </summary>
		/// <typeparam name="T">The type of the result.</typeparam>
		/// <param name="resultTask">The result.</param>
		/// <returns>Result.</returns>
		public static async Task<T> EnsureSuccess<T>(
			this Task<T> resultTask)
			where T : BaseResult
		{
			var result = await resultTask;

			return result.EnsureSuccess();
		}

		/// <summary>
		/// Ensures the success status is set to true. Throws an exception if it is not.
		/// </summary>
		/// <param name="resultTask">The status result.</param>
		/// <returns>Status result.</returns>
		public static async Task<StatusResult> EnsureSuccessStatus(
			this Task<StatusResult> resultTask)
		{
			var result = await resultTask;

			return result.EnsureSuccessStatus();
		}
	}
}
