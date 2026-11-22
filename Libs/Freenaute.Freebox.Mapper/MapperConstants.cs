namespace Freenaute.Freebox.Mapper;

internal static class MapperConstants
{
	internal static class JsonProperties
	{
		public const string Action = "action";
		public const string Id = "id";
		public static ReadOnlySpan<byte> IdUtf8 => "id"u8;
	}

	internal static class RouteTemplates
	{
		public const string ApiVersionWithoutBasePath = "/v{0}/";
		public const string ApiVersionWithBasePath = "/{0}/v{1}/";
	}

	internal static class ExceptionMessages
	{
		internal static class Adapter
		{
			public const string TypeNotRegisteredInSourceGeneratedContextsFormat =
				"The type '{0}' is not registered in the Freebox source-generated JSON contexts.";
		}

		internal static class JsonConverters
		{
			public const string UninitializedWireValueCannotBeSerialized =
				"An uninitialized wire value cannot be serialized.";
		}

		internal static class Validation
		{
			public const string UndefinedToken = "Undefined token.";
		}
	}

	internal static class WebSocketActions
	{
		public const string Register = "register";
		public const string UploadStart = "upload_start";
		public const string UploadFinalize = "upload_finalize";
		public const string UploadCancel = "upload_cancel";
	}

	internal static class BoxModels
	{
		public const string FreeboxServerV6R1 = "fbxgw-r1/full";
		public const string FreeboxServerV6R2 = "fbxgw-r2/full";
		public const string FreeboxMiniR1 = "fbxgw-r1/mini";
		public const string FreeboxMiniR2 = "fbxgw-r2/mini";
		public const string FreeboxOneR1 = "fbxgw-r1/one";
		public const string FreeboxOneR2 = "fbxgw-r2/one";
		public const string FreeboxV7R1 = "fbxgw7-r1/full";
		public const string FreeboxV8R1 = "fbxgw8-r1/full";
		public const string FreeboxV9R1 = "fbxgw9-r1/full";
		public const string FreeboxV9R1Compact = "fbxgw9-r1";
	}

	internal static class ServiceWireTokens
	{
		internal static class Calls
		{
			public const string Missed = "missed";
			public const string Accepted = "accepted";
			public const string Outgoing = "outgoing";
		}

		internal static class Contacts
		{
			internal static class AddressType
			{
				public const string Home = "home";
				public const string Work = "work";
				public const string Other = "other";
			}

			internal static class EmailType
			{
				public const string Home = "home";
				public const string Work = "work";
				public const string Other = "other";
			}

			internal static class NumberType
			{
				public const string Fixed = "fixed";
				public const string Mobile = "mobile";
				public const string Work = "work";
				public const string Fax = "fax";
				public const string Other = "other";
			}

			internal static class UrlType
			{
				public const string Profile = "profile";
				public const string Blog = "blog";
				public const string Site = "site";
				public const string Other = "other";
			}
		}

		internal static class Parental
		{
			public const string Allowed = "allowed";
			public const string Denied = "denied";
			public const string Webonly = "webonly";
			public const string Forced = "forced";
			public const string Temporary = "temporary";
			public const string Planning = "planning";
		}

		internal static class Players
		{
			public const string PlayPause = "play_pause";
			public const string Stop = "stop";
			public const string Prev = "prev";
			public const string Next = "next";
			public const string SelectStream = "select_stream";
			public const string SelectAudioTrack = "select_audio_track";
			public const string SelectSrtTrack = "select_srt_track";
		}

		internal static class ServerConfigs
		{
			internal static class AfpServerType
			{
				public const string Powerbook = "powerbook";
				public const string Powermac = "powermac";
				public const string Macmini = "macmini";
				public const string Imac = "imac";
				public const string Macbook = "macbook";
				public const string Macbookpro = "macbookpro";
				public const string Macbookair = "macbookair";
				public const string Macpro = "macpro";
				public const string Appletv = "appletv";
				public const string Airport = "airport";
				public const string Xserve = "xserve";
			}
		}
	}

	internal static class SystemHomeWireTokens
	{
		internal static class LcdScreensaver
		{
			public const string Disabled = "disabled";
			public const string On = "on";
			public const string Night = "night";
		}

		internal static class StandbyPlanningMode
		{
			public const string WifiOff = "wifi_off";
			public const string Standby = "standby";
		}

		internal static class LedstripPlanningMode
		{
			public const string LedstripOff = "ledstrip_off";
		}

		internal static class NetworkControlMode
		{
			public const string Allowed = "allowed";
			public const string Denied = "denied";
			public const string Webonly = "webonly";
		}

		internal static class SystemDiskStatus
		{
			public const string NotDetected = "not_detected";
			public const string Disabled = "disabled";
			public const string Initializing = "initializing";
			public const string Error = "error";
			public const string Active = "active";
		}

		internal static class SystemExpansionType
		{
			public const string Unknown = "unknown";
			public const string DslLte = "dsl_lte";
			public const string DslLteExternalAntennas = "dsl_lte_external_antennas";
			public const string FtthP2P = "ftth_p2p";
			public const string FtthPon = "ftth_pon";
			public const string Security = "security";
		}

		internal static class SystemUpdateState
		{
			public const string Initializing = "initializing";
			public const string Upgrading = "upgrading";
			public const string UpToDate = "up_to_date";
			public const string Error = "error";
		}

		internal static class SystemUpgradeState
		{
			public const string Downloading = "downloading";
			public const string DownloadFailed = "download_failed";
			public const string Checking = "checking";
			public const string CheckFailed = "check_failed";
			public const string PrepareWrite = "prepare_write";
			public const string PrepareWriteFailed = "prepare_write_failed";
			public const string Writing = "writing";
			public const string WriteFailed = "write_failed";
			public const string Reread = "reread";
			public const string RereadFailed = "reread_failed";
			public const string Commit = "commit";
			public const string CommitFailed = "commit_failed";
		}
	}
}
