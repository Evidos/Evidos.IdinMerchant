namespace IdinClientLibrary.Tests
{
	/// <summary>
	/// Test entrance code constants.
	/// </summary>
	public static class TestEntranceCodes
	{
		/// <summary>
		/// Request Successful.
		/// </summary>
		public const string Success = "succesHIO100OIHtest";

		/// <summary>
		/// Request cancelled.
		/// </summary>
		public const string Cancelled = "cancelledHIO200OIHtest";

		/// <summary>
		/// Request expired.
		/// </summary>
		public const string Expired = "expiredHIO300OIHtest";

		/// <summary>
		/// Request open.
		/// </summary>
		public const string Open = "openHIO400OIHtest";

		/// <summary>
		/// Request failed.
		/// </summary>
		public const string Failure = "failureHIO500OIHtest";

		/// <summary>
		/// Invalid Xml.
		/// </summary>
		public const string IX1100 = "receivedXMLNotValidHIO701OIHtest";

		/// <summary>
		/// System unavailable.
		/// </summary>
		public const string SO1100 = "systemUnavailabilityHIO702OIHtest";

		/// <summary>
		/// Invalid signature.
		/// </summary>
		public const string SE2700 = "invalidElectronicSignatureHIO703OIHtest";

		/// <summary>
		/// Version number invalid.
		/// </summary>
		public const string BR1200 = "versionNumberInvalidHIO704OIHtest";

		/// <summary>
		/// Product specific error.
		/// </summary>
		public const string AP3000 = "HIO705OIH";
	}
}
