namespace Freenaute.Freebox.Mapper.Contracts.SystemHome.Common;

public interface ISystemHomeWireToken<TSelf> where TSelf : struct, ISystemHomeWireToken<TSelf>
{
	string Value { get; }
	static abstract TSelf FromWire(string value);
}
