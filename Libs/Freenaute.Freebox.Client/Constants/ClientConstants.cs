namespace Freenaute.Freebox.Client.Constants;

internal static class ClientConstants
{
	internal static class Headers
	{
		internal const string Authentication = "X-Fbx-App-Auth";
		internal const string Authorization = "Authorization";
		internal const string Cookie = "Cookie";
		internal const string SetCookie = "Set-Cookie";
		internal static readonly string[] Redacted = [Authentication, Authorization, Cookie, SetCookie];
	}

	internal static class ContentTypes
	{
		internal const string ApplicationJson = "application/json";
		internal const string Utf8 = "utf-8";
	}

	internal static class Routes
	{
		internal const string ApiVersion = "api_version";
		internal const string Login = "login/";
		internal const string LoginSession = "login/session/";
		internal const string LoginLogout = "login/logout/";
		internal const string LoginAuthorize = "login/authorize/";
		internal const string WebSocketUpload = "ws/upload";

		internal static class AirMedia
		{
			internal const string Configuration = "airmedia/config/";
			internal const string Receivers = "airmedia/receivers/";
			internal const string LegacyReceiversTypo = "airmedia/receviers/";
		}
	}

	internal static class JsonEnvelope
	{
		internal const string Result = "result";
		internal const string Success = "success";
		internal const string ErrorCode = "error_code";
		internal const string Message = "msg";
		internal const string Uid = "uid";
	}

	internal static class MdnsFields
	{
		internal const string Uid = "uid";
		internal const string ApiVersion = "api_version";
		internal const string ApiBaseUrl = "api_base_url";
		internal const string ApiDomain = "api_domain";
		internal const string HttpsAvailable = "https_available";
		internal const string HttpsPort = "https_port";
		internal const string DeviceName = "device_name";
		internal const string BoxModel = "box_model";
		internal const string BoxModelName = "box_model_name";
	}

	internal static class Api
	{
		internal const string AuthRequiredError = "auth_required";
		internal const string InvalidSessionError = "invalid_session";
		internal const string VersionPrefix = "v";
	}
}
