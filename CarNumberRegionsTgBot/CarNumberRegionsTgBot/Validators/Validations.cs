using System.Text.RegularExpressions;

namespace CarNumberRegionsTgBot.Validators
{
	public static class Validations
	{
		public static bool isCarNumberPatternValid(string carNumber)
		{
			string pattern = @"\w[A,B,C,E,H,T,I,O,P,K,M,X]{2}\d[0-9]{4}\w[A,B,C,E,H,T,I,O,P,K,M,X]{2}";
			Regex regex = new Regex(pattern);
			if (regex.IsMatch(carNumber))
			{
				return true;
			}
			return false;
		}
	}
}
