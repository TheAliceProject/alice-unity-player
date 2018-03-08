using System;
using System.Linq.Expressions;

namespace Alice.Parse
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