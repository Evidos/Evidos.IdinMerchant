using System;
using System.Collections.Generic;
using System.Text;

namespace Evidos.Idin.Settings
{
	internal static class SettingsExtensions
	{
		public static void Validate(this InitiateTransactionSettings settings)
		{
			if (settings.IssuerId == null) {
				throw new ArgumentException(
					nameof(InitiateTransactionSettings.IssuerId),
					nameof(settings));
			}

			if (settings.ReturnUrl == null) {
				throw new ArgumentException(
					nameof(InitiateTransactionSettings.ReturnUrl),
					nameof(settings));
			}

			if (settings.EntranceCode == null) {
				throw new ArgumentException(
					nameof(InitiateTransactionSettings.EntranceCode),
					nameof(settings));
			}
		}

		public static void Validate(this RequestStatusSettings settings)
		{
			if (settings.TransactionId == null) {
				throw new ArgumentException(
					nameof(RequestStatusSettings.TransactionId),
					nameof(settings));
			}
		}
	}
}
