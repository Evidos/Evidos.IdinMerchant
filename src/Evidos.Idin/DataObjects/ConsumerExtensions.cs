using System.Linq;

namespace Evidos.Idin.DataObjects
{
	/// <summary>
	/// Extension for the <see cref="Consumer"/> class.
	/// </summary>
	public static class ConsumerExtensions
	{
		/// <summary>
		/// Tries to get an attribute by name.
		/// </summary>
		/// <param name="consumer">Consumer.</param>
		/// <param name="name">The name by which to find the attribute.</param>
		/// <param name="value">The value to output.</param>
		/// <returns>Whether the value is found.</returns>
		public static bool TryGetAttribute(
			this Consumer consumer,
			string name,
			out string value)
		{
			var prefix = "urn:nl:bvn:bankid:1.0:";
			var consumerPrefix = $"{prefix}consumer.";

			if (name.Contains(prefix)) {
				consumer.Attributes.TryGetValue(name, out value);
			}
			else {
				var length = prefix.Length;

				if (!name.Contains("consumer.")) {
					length = consumerPrefix.Length;
				}

				value = consumer
					.Attributes
					.FirstOrDefault(a => a.Key.Substring(length) == name)
					.Value;
			}

			return value != null;
		}
	}
}
