using System.Threading;
using System.Threading.Tasks;
using Evidos.Idin.ResultObjects;
using Evidos.Idin.Settings;

namespace Evidos.Idin
{
	/// <summary>
	/// Idin client.
	/// </summary>
	public interface IIdinClient
	{
		/// <summary>
		/// Gets a list of iDIN issuers.
		/// </summary>
		/// <param name="settings">Settings.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>Directory result.</returns>
		Task<DirectoryResult> GetIssuersAsync(
			GetIssuersSettings settings,
			CancellationToken cancellationToken = default);

		/// <summary>
		/// Initiates a transaction.
		/// </summary>
		/// <param name="settings">Settings.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>Transaction result.</returns>
		Task<TransactionResult> InitiateTransactionAsync(
			InitiateTransactionSettings settings,
			CancellationToken cancellationToken = default);

		/// <summary>
		/// Requests the status of a transaction.
		/// </summary>
		/// <param name="settings">Settings.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>Status result.</returns>
		Task<StatusResult> RequestStatusAsync(
			RequestStatusSettings settings,
			CancellationToken cancellationToken = default);
	}
}
