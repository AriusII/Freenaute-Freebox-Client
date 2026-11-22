namespace Freenaute.Freebox.Mapper.Tests;

public sealed class ContactsContractTests
{
	private static void AssertRoundTrip<T>(string wire, JsonTypeInfo<T> typeInfo) =>
		WireRoundTrip.AssertByteIdentical(wire, typeInfo);

	[Fact]
	public void ContactEntry_RoundTripsByteIdentical_WithAllSubResources() =>
		AssertRoundTrip(
			"""{"id":1,"display_name":"Bob","first_name":"Bob","last_name":"Martin","company":"ACME","photo_url":"https://x/p.jpg","last_update":1700000000,"notes":"n","addresses":[{"id":1,"contact_id":1,"type":"home","number":"1","street":"1 rue","street2":"","city":"Paris","zipcode":"75001","country":"FR"}],"emails":[{"id":1,"contact_id":1,"type":"work","email":"bob@acme.fr"}],"numbers":[{"id":1,"contact_id":1,"type":"mobile","number":"0601020304","is_default":true,"is_own":false}],"urls":[{"id":1,"contact_id":1,"type":"profile","url":"https://x/bob"}],"birthday":"1990-01-01"}""",
			ServicesJsonSerializerContext.Default.ContactEntry);

	[Fact]
	public void ContactNumber_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"id":1,"contact_id":1,"type":"fax","number":"0102030405","is_default":false,"is_own":true}""",
			ServicesJsonSerializerContext.Default.ContactNumber);

	[Fact]
	public void ContactEmail_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"id":1,"contact_id":1,"type":"other","email":"bob@home.fr"}""",
			ServicesJsonSerializerContext.Default.ContactEmail);

	[Fact]
	public void ContactAddress_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"id":1,"contact_id":1,"type":"work","number":"2","street":"rue","street2":"b","city":"Lyon","zipcode":"69000","country":"FR"}""",
			ServicesJsonSerializerContext.Default.ContactAddress);

	[Fact]
	public void ContactUrl_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"id":1,"contact_id":1,"type":"blog","url":"https://blog.example.com"}""",
			ServicesJsonSerializerContext.Default.ContactUrl);

	[Fact]
	public void ContactWriteRequest_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"display_name":"Bob","first_name":"Bob","notes":"n"}""",
			ServicesJsonSerializerContext.Default.ContactWriteRequest);

	[Fact]
	public void ContactNumberWriteRequest_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"contact_id":1,"type":"mobile","number":"0601020304","is_default":true,"is_own":false}""",
			ServicesJsonSerializerContext.Default.ContactNumberWriteRequest);

	[Fact]
	public void ContactAddressWriteRequest_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"contact_id":1,"type":"home","number":"11","street":"8 rue du pont","street2":"","city":"Paris","zipcode":"75008","country":"France"}""",
			ServicesJsonSerializerContext.Default.ContactAddressWriteRequest);

	[Fact]
	public void ContactEmailWriteRequest_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"contact_id":1,"type":"work","email":"bob@acme.fr"}""",
			ServicesJsonSerializerContext.Default.ContactEmailWriteRequest);

	[Fact]
	public void ContactUrlWriteRequest_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"contact_id":1,"type":"profile","url":"https://x/bob"}""",
			ServicesJsonSerializerContext.Default.ContactUrlWriteRequest);
}
