using System.Text.RegularExpressions;

namespace Application.Extension;

public static class StringsExtensions
{
	public static bool ValidationCNPJ(this string cnpj)
	{
		var expression = "";
		return Regex.Match(cnpj, expression).Success;
	}
}