using System;
using Alice.Tweedle.Parse;
using UnityEngine;

namespace Alice.Tweedle
{
	/// <summary>
	/// Represents a member of a TweedleType.
	/// </summary>
	public interface ITypeMember
	{
		string Name { get; }
		TTypeRef Type { get; }
        MemberFlags Flags { get; }

        void Link(TAssemblyLinkContext inContext, TType inOwnerType);
    }

	/// <summary>
	/// Extension methods for dealing with ITweedleMembers.
	/// </summary>
	public static class ITypeMemberExtensions
	{
		/// <summary>
		/// Returns if the member has all of the given modifiers.
		/// </summary>
		static public bool HasModifiers(this ITypeMember inMember, MemberFlags inFlags)
		{
            Debug.Assert(inMember != null, "ITweedleMember is null");
            return (inMember.Flags & inFlags) == inFlags;
        }

		/// <summary>
		/// Returns if the member has any of the given modifiers.
		/// </summary>
		static public bool HasAnyModifiers(this ITypeMember inMember, MemberFlags inFlags)
		{
			Debug.Assert(inMember != null, "ITweedleMember is null");
            return (inMember.Flags & inFlags) != 0;
        }
	}
}
