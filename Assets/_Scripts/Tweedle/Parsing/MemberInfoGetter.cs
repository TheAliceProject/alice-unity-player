using System;
using System.Linq.Expressions;

namespace Alice.Tweedle.Unlinked
{
	public static class MemberInfoGetter
	{
		public static string GetMemberName<T>(Expression<Func<T>> memberExpression)
		{
			MemberExpression expressionBody = (MemberExpression)memberExpression.Body;
			return expressionBody.Member.Name;
		}
	}
}