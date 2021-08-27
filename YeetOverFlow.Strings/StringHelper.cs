using Pluralize.NET;
using StringExtensions;

namespace YeetOverFlow.Common
{
    public static class StringHelpera
    {
        static IPluralize _pluralize = new Pluralizer();

        public static string Pluralize(string input)
        {
            return _pluralize.Pluralize(input);
        }

        public static string TrimEnd(string input, string endsWith)
        {
            return input.TrimEnd(endsWith);
        }
    }
}
