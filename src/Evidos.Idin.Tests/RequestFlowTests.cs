using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Evidos.Idin;
using Evidos.Idin.Certificates;
using Evidos.Idin.DataObjects;
using Evidos.Idin.ResultObjects;
using Evidos.Idin.Settings;
using FluentAssertions;
using NUnit.Framework;

namespace IdinClientLibrary.Tests
{
	/// <summary>
	/// Request flow tests.
	/// </summary>
	[TestFixture]
	[SuppressMessage(
		"Microsoft.Reliability",
		"CA2007:DoNotDirectlyAwaitATaskAnalyzer",
		Justification = "Tests should not use ConfigureAwait.")]
	public class RequestFlowTests
	{
		private const string AttributePrefix = "urn:nl:bvn:bankid:1.0:consumer.";

		private const ServiceIds SelectedServiceIds =
			ServiceIds.Name |
			ServiceIds.DateOfBirth |
			ServiceIds.Address |
			ServiceIds.ConsumerBin;

		private const string MerchantReference = "ABCDEFGHIJKLMOPQRSTUVWXYZ123456789";

		private readonly ICertificateStore certificateStore;

		public RequestFlowTests()
		{
			certificateStore = new CertificateStore();
		}

		[Test]
		public async Task When_a_correct_transaction_is_send_then_the_returned_status_should_be_successfull()
		{
			var statusResult = await SendTransactionWithoutErrorRequestAsync(
				TestEntranceCodes.Success,
				SelectedServiceIds);

			statusResult.HasSuccessStatus.Should().BeTrue();
			statusResult.Consumer.Attributes.Should().NotBeEmpty();
		}

		[Test]
		public async Task When_a_transaction_is_cancelled_then_the_returned_status_should_not_be_successfull()
		{
			var result = await SendTransactionWithoutErrorRequestAsync(
				TestEntranceCodes.Cancelled,
				SelectedServiceIds);

			result.HasError.Should().BeFalse();
			result.Status.Should().Be("Cancelled");
		}

		[Test]
		public async Task When_a_transaction_fails_then_the_returned_status_should_not_be_successfull()
		{
			var result = await SendTransactionWithoutErrorRequestAsync(
				TestEntranceCodes.Failure,
				SelectedServiceIds);

			result.HasError.Should().BeFalse();
			result.Status.Should().Be("Failure");
		}

		[Test]
		public async Task When_a_transaction_is_still_open_then_the_returned_status_should_not_be_successfull()
		{
			var result = await SendTransactionWithoutErrorRequestAsync(
				TestEntranceCodes.Open,
				SelectedServiceIds);

			result.HasError.Should().BeFalse();
			result.Status.Should().Be("Open");
		}

		[Test]
		public async Task When_a_transaction_is_expired_then_the_returned_status_should_not_be_successfull()
		{
			var result = await SendTransactionWithoutErrorRequestAsync(
				TestEntranceCodes.Expired,
				SelectedServiceIds);

			result.HasError.Should().BeFalse();
			result.Status.Should().Be("Expired");
		}

		[Test]
		public async Task When_an_error_occures_in_a_transaction_then_it_should_be_returned()
		{
			var trxResult = await SendTransactionRequestAsync(
				CreateIdinClient(),
				TestEntranceCodes.AP3000,
				SelectedServiceIds);

			trxResult.HasError.Should().BeTrue();
			trxResult.ErrorDetails.ErrorMessage.Should().Be("ProductID invalid");
		}

		[Test]
		public async Task When_a_wrong_merchant_id_is_sent_then_the_transaction_should_fail()
		{
			var idinUrl = new Uri("https://idintest.rabobank.nl/bvn-idx-bankid-rs/bankidGateway");
			var client = new IdinClient(
				idinUrl,
				() => certificateStore.GetCertificate(
					"BC2262A6F2E6DB3A63138A167A0E2AA114E2E3AD",
					false),
				() => new List<X509Certificate2> {
					certificateStore.GetCertificate(
						"ad96a0ce00209e56f536f0e017415e68460b2b22",
						false),
				},
				() => new List<X509Certificate2> {
					certificateStore.GetCertificate(
						"4CC9A1382521DA468F657191EA9B6B996EFFFCF8",
						false),
				},
				"Test");

			var idinResult = await client.GetIssuersAsync(
				new GetIssuersSettings {
					SubId = 0,
				});

			idinResult.HasError.Should().BeTrue();
			idinResult.ErrorDetails.ErrorMessage.Should().Contain("Value too short");
		}

		[Test]
		public void When_a_transaction_fails_an_exception_should_be_thrown()
		{
			Func<Task> test = async () => {
				var client = CreateIdinClient();

				var merchantReference = MerchantReference;

				var dirRes = await client.GetIssuersAsync(
					new GetIssuersSettings { SubId = 0 });

				var issuerId = dirRes.Issuers[0].IssuerId;
				var returnUrl = new Uri("http://localhost/");

				var trxResult = await client.InitiateTransactionAsync(
					new InitiateTransactionSettings {
						IssuerId = issuerId,
						ReturnUrl = returnUrl,
						MerchantReference = merchantReference,
						EntranceCode = TestEntranceCodes.Failure,
						ServiceIds = SelectedServiceIds,
						Language = "nl",
						SubId = 0,
					}).EnsureSuccess();

				await client.RequestStatusAsync(
					new RequestStatusSettings {
						TransactionId = trxResult.Transaction.TransactionId,
						SubId = 0,
					}).EnsureSuccess().EnsureSuccessStatus();
			};

			test.Should().Throw<IdinException>().WithMessage("Failure");
		}

		[TestCase(ServiceIds.Name)]
		[TestCase(ServiceIds.Address)]
		[TestCase(ServiceIds.DateOfBirth)]
		[TestCase(ServiceIds.ConsumerBin)]
		[TestCase(ServiceIds.EighteenOrOlder)]
		[TestCase(ServiceIds.Gender)]
		[TestCase(ServiceIds.Phone)]
		[TestCase(ServiceIds.Email)]
		[TestCase(ServiceIds.Address | ServiceIds.DateOfBirth | ServiceIds.Name | ServiceIds.ConsumerBin)]
		[Test]
		public async Task When_service_ids_are_requested_then_the_corresponding_attributes_should_be_returned(ServiceIds serviceIds)
		{
			var statusResult = await SendTransactionWithoutErrorRequestAsync(TestEntranceCodes.Success, serviceIds);
			var consumer = statusResult.Consumer;
			consumer.Attributes.Should().NotBeEmpty();

			var attributeNames = GetSelectedAttributeNames(serviceIds);

			if (attributeNames.Count > 0) {
				consumer.Attributes.Keys.Should().Contain(attributeNames);
			}
		}

		private static IdinClient CreateIdinClient()
		{
			var certificateStore = new CertificateStore();
			var idinUrl = new Uri("https://idintest.rabobank.nl/bvn-idx-bankid-rs/bankidGateway");
			var client = new IdinClient(
				idinUrl,
				() => certificateStore.GetCertificate(
					"BC2262A6F2E6DB3A63138A167A0E2AA114E2E3AD",
					false),
				() => new List<X509Certificate2> {
					certificateStore.GetCertificate(
						"ad96a0ce00209e56f536f0e017415e68460b2b22",
						false),
				},
				() => new List<X509Certificate2> {
					certificateStore.GetCertificate(
						"4CC9A1382521DA468F657191EA9B6B996EFFFCF8",
						false),
				},
				"0020000012");

			return client;
		}

		private static async Task<TransactionResult> SendTransactionRequestAsync(
			IdinClient client,
			string entranceCode,
			ServiceIds serviceIds)
		{
			var merchantReference = MerchantReference;

			var dirRes = await client.GetIssuersAsync(
				new GetIssuersSettings { SubId = 0 });

			var issuerId = dirRes.Issuers[0].IssuerId;
			var returnUrl = new Uri("http://localhost/");

			return await client.InitiateTransactionAsync(
				new InitiateTransactionSettings {
					IssuerId          = issuerId,
					ReturnUrl         = returnUrl,
					MerchantReference = merchantReference,
					EntranceCode      = entranceCode,
					ServiceIds        = serviceIds,
					Language          = "nl",
					SubId             = 0,
				});
		}

		private List<string> GetSelectedAttributeNames(ServiceIds serviceIds)
		{
			var attributeNames = new List<string>();

			foreach (ServiceIds value in Enum.GetValues(serviceIds.GetType())) {
				if (serviceIds.HasFlag(value)) {
					switch (value) {
						case ServiceIds.Name:
							attributeNames.Add($"{AttributePrefix}legallastname");
							attributeNames.Add($"{AttributePrefix}preferredlastname");
							attributeNames.Add($"{AttributePrefix}partnerlastname");
							attributeNames.Add($"{AttributePrefix}legallastnameprefix");
							attributeNames.Add($"{AttributePrefix}preferredlastnameprefix");
							attributeNames.Add($"{AttributePrefix}partnerlastnameprefix");
							attributeNames.Add($"{AttributePrefix}initials");
							break;

						case ServiceIds.Address:
							attributeNames.Add($"{AttributePrefix}street");
							attributeNames.Add($"{AttributePrefix}houseno");
							attributeNames.Add($"{AttributePrefix}postalcode");
							attributeNames.Add($"{AttributePrefix}city");
							attributeNames.Add($"{AttributePrefix}country");
							break;

						case ServiceIds.DateOfBirth:
							attributeNames.Add($"{AttributePrefix}dateofbirth");
							break;

						case ServiceIds.EighteenOrOlder:
							if (!serviceIds.HasFlag(ServiceIds.DateOfBirth)) {
								attributeNames.Add($"{AttributePrefix}18orolder");
							}

							break;

						case ServiceIds.Gender:
							attributeNames.Add($"{AttributePrefix}gender");
							break;

						case ServiceIds.Phone:
							attributeNames.Add($"{AttributePrefix}telephone");
							break;

						case ServiceIds.Email:
							attributeNames.Add($"{AttributePrefix}email");
							break;
					}
				}
			}

			attributeNames.Add($"{AttributePrefix}NameID");

			return attributeNames;
		}

		private async Task<StatusResult> SendTransactionWithoutErrorRequestAsync(
			string entranceCode,
			ServiceIds serviceIds)
		{
			var client = CreateIdinClient();

			var trxRes = await SendTransactionRequestAsync(
				client,
				entranceCode,
				serviceIds);

			return await client.RequestStatusAsync(
				new RequestStatusSettings {
					TransactionId = trxRes.Transaction.TransactionId,
					SubId         = 0,
				});
		}
	}
}
