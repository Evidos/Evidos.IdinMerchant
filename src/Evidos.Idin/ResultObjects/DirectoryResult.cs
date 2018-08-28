using System.Collections.Generic;
using Evidos.Idin.DataObjects;

namespace Evidos.Idin.ResultObjects
{
	/// <summary>
	/// The directory result class.
	/// </summary>
	public class DirectoryResult
		: BaseResult
	{
		/// <summary>
		/// Gets the list of issuers.
		/// </summary>
		public List<Issuer> Issuers { get; internal set; }
	}
}
