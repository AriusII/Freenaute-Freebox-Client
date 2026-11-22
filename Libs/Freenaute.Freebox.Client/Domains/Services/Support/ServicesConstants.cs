namespace Freenaute.Freebox.Client.Domains.Services.Support;

internal static class ServicesConstants
{
	internal static class Routes
	{
		internal static class Calls
		{
			internal const string Log = "call/log/";
			internal const string DeleteAll = "call/log/delete_all/";
			internal const string MarkAllAsRead = "call/log/mark_all_as_read/";
			internal const string Account = "call/account";
			internal const string Voicemail = "call/voicemail/";
		}

		internal static class Contacts
		{
			internal const string Numbers = "number/";
			internal const string Addresses = "address/";
			internal const string Urls = "url/";
			internal const string Emails = "email/";
			internal const string Collection = "contact/";
			internal const string NumbersSuffix = "/numbers/";
			internal const string AddressesSuffix = "/addresses/";
			internal const string UrlsSuffix = "/urls/";
			internal const string EmailsSuffix = "/emails/";

			internal static string ContactNumbers(string contactId) => $"{Collection}{contactId}{NumbersSuffix}";
			internal static string ContactAddresses(string contactId) => $"{Collection}{contactId}{AddressesSuffix}";
			internal static string ContactUrls(string contactId) => $"{Collection}{contactId}{UrlsSuffix}";
			internal static string ContactEmails(string contactId) => $"{Collection}{contactId}{EmailsSuffix}";
			internal static string ContactResource(string contactId, string kind) => $"{Collection}{contactId}/{kind}/";
		}

		internal static class Ftp
		{
			internal const string Configuration = "ftp/config/";
		}

		internal static class Parental
		{
			internal const string Configuration = "parental/config/";
			internal const string Filters = "parental/filter/";
		}

		internal static class Players
		{
			internal const string Base = "player";
		}

		internal static class Pvr
		{
			internal const string Configuration = "pvr/config/";
			internal const string Finished = "pvr/finished/";
			internal const string Media = "pvr/media/";
			internal const string Programmed = "pvr/programmed/";
			internal const string Quota = "pvr/quota/";

			internal static string FinishedItem(string id) => $"{Finished}{id}";
			internal static string ProgrammedItem(string id) => $"{Programmed}{id}";
		}

		internal static class Rrd
		{
			internal const string Base = "rrd/";
		}

		internal static class Shares
		{
			internal const string Samba = "netshare/samba/";
			internal const string Afp = "netshare/afp/";
		}

		internal static class Tftp
		{
			internal const string Configuration = "tftp/config/";
		}

		internal static class UpnpAv
		{
			internal const string Configuration = "upnpav/config/";
		}
	}
}
