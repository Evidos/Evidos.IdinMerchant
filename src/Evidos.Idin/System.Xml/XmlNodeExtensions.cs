using System.Linq;

namespace System.Xml
{
	/// <summary>
	/// Xml node extension methods.
	/// </summary>
	internal static class XmlNodeExtensions
	{
		/// <summary>
		/// Gets the first occuring value in a xml node array.
		/// </summary>
		/// <param name="nodes">Xml nodes.</param>
		/// <param name="name">Node name to look for.</param>
		/// <returns>Inner text of the node.</returns>
		public static string GetFirstOccuringValue(
			this XmlNode[] nodes,
			string name)
		{
			return nodes.First(ic => ic.Name.Contains($"{name}")).InnerText;
		}
	}
}
