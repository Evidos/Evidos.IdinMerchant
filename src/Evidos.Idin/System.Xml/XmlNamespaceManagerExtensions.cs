using System.Collections.Generic;

namespace System.Xml
{
	/// <summary>
	/// Xml namespace manager extension methods.
	/// </summary>
	internal static class XmlNamespaceManagerExtensions
	{
		/// <summary>
		/// Adds multiple namespaces.
		/// </summary>
		/// <param name="namespaceManager">Namespace manager.</param>
		/// <param name="namespaces">Namespaces to be added.</param>
		internal static void AddNamespaces(
			this XmlNamespaceManager namespaceManager,
			params (string, string)[] namespaces)
		{
			foreach (var ns in namespaces) {
				namespaceManager.AddNamespace(ns.Item1, ns.Item2);
			}
		}
	}
}
