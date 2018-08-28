using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Evidos.Idin.DataObjects
{
	/// <summary>
	/// Consumer class.
	/// </summary>
	public class Consumer
	{
		/// <summary>
		/// Gets attributes.
		/// </summary>
		public IReadOnlyDictionary<string, string> Attributes { get; internal set; } = new Dictionary<string, string>();
	}
}
