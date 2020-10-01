using System;
using static LanguageExt.Prelude;

namespace Isotope80
{
    /// <summary>
    /// Text utilities
    /// </summary>
    internal class Text
    {
        /// <summary>
        /// Build a new string with `indent` tabs before
        /// </summary>
        public static string Tabs(int indent, string str) =>
            indent < tabs.Length
                ? $"{tabs[indent]}{str}"
                : $"{String.Concat(Range(0, indent).Map(_ => "    "))}{str}";

        /// <summary>
        /// Lookup table of tabs
        /// </summary>
        static readonly string[] tabs = new[]
        {
            "", "    ", "        ", "            ", "                ", "                    ", "                        ",
            "                            ", "                                ", "                                    ",
            "                                        ", "                                            ",
            "                                                ", "                                                    ",
            "                                                        ", "                                                            "
        };        
    }
}