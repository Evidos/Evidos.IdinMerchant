namespace Evidos.Idin.Settings
{
	/// <summary>
	/// Request status settings.
	/// </summary>
	public class RequestStatusSettings
	{
		/// <summary>
		/// Gets or sets the transaction id.
		/// </summary>
		public string TransactionId { get; set; }

		/// <summary>
		/// Gets or sets the sub id of the merchant.
		/// </summary>
		public uint SubId { get; set; }
	}
}
