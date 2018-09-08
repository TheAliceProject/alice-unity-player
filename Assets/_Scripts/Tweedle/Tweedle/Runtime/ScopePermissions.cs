using System;

namespace Alice.Tweedle
{
	[Flags]
	public enum ScopePermissions
	{
		// No special permissions
		None						= 0,

		// Scope can write to readonly fields
		WriteReadonlyFields			= 0x001
	}
}
