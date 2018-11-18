using System;
using System.Globalization;

namespace Meiswinkel.NightScoutReporter.NightScoutCommon
{
    public static class FormattableStringExtensions
    {
        public static string Invariant(this FormattableString formattable)
        {
            return formattable.ToString(CultureInfo.InvariantCulture);
        }
    }
}
