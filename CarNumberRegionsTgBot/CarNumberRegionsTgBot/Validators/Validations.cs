using System.Text.RegularExpressions;

namespace CarNumberRegionsTgBot.Validators
{
    public static class Validations
    {
        public static bool isCarNumberPatternValid(string carNumber)
        {
            string pattern = @"[ABCEHTIOPKMX]{2}[0-9]{4}[ABCEHTIOPKMX]{2}";
            Regex regex = new Regex(pattern);
            if (regex.IsMatch(carNumber))
            {
                return true;
            }
            return false;
        }
    }
}
