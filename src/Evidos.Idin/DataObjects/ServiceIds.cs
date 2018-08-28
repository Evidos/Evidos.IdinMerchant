using System;

namespace Evidos.Idin.DataObjects
{
	/// <summary>
	/// Service id's enum.
	/// </summary>
	[Flags]
	public enum ServiceIds
	{
		/// <summary>
		/// None.
		/// </summary>
		None            = 0b_0000_0000_0000_0000,

		/// <summary>
		/// Request Email attribute.
		/// </summary>
		Email           = 0b_0000_0000_0000_0010,

		/// <summary>
		/// Request phone attribute.
		/// </summary>
		Phone           = 0b_0000_0000_0000_0100,

		/// <summary>
		/// Request gender attribute.
		/// </summary>
		Gender          = 0b_0000_0000_0001_0000,

		/// <summary>
		/// Request eighteen or older attribute.
		/// </summary>
		EighteenOrOlder = 0b_0000_0000_0100_0000,

		/// <summary>
		/// Request Date of Birth attribute.
		/// </summary>
		DateOfBirth     = 0b_0000_0001_1100_0000,

		/// <summary>
		/// Request address attributes.
		/// </summary>
		Address         = 0b_0000_0100_0000_0000,

		/// <summary>
		/// Request Name attributes.
		/// </summary>
		Name            = 0b_0001_0000_0000_0000,

		/// <summary>
		/// Request Consumer BIN
		/// </summary>
		ConsumerBin     = 0b_0100_0000_0000_0000,
	}
}
