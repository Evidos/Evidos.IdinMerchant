using System.Xml;
using Evidos.Idin.DataObjects;

namespace Evidos.Idin.ResultObjects
{
	internal class XmlResult
	{
		public XmlDocument ResponseDoc { get; set; }

		public XmlNamespaceManager NameSpaceManager { get; set; }

		public bool HasError { get; set; }

		public ErrorDetails ErrorDetails { get; set; }
	}
}
