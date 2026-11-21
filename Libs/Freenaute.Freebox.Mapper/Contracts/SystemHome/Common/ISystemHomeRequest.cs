namespace Freenaute.Freebox.Mapper.Contracts.SystemHome.Common;

/// <summary>An immutable operation body with validation before any HTTP request is sent.</summary>
public interface ISystemHomeRequest
{
	void Validate();
}
