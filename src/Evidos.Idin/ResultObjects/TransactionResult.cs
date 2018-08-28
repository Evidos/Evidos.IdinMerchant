using Evidos.Idin.DataObjects;

namespace Evidos.Idin.ResultObjects
{
	/// <summary>
	/// The transaction result class.
	/// </summary>
	public class TransactionResult
		: BaseResult
	{
		/// <summary>
		/// Gets the acquirer.
		/// </summary>
		public Acquirer Acquirer { get; internal set; }

		/// <summary>
		/// Gets the issuer.
		/// </summary>
		public Issuer Issuer { get; internal set; }

		/// <summary>
		/// Gets the transaction.
		/// </summary>
		public Transaction Transaction { get; internal set; }
	}
}
