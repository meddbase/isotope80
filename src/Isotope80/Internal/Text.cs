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
        public static string Tabs(int indent) =>
            indent < tabs.Length
                ? $"{tabs[indent]}"
                : $"{String.Concat(Range(0, indent).Map(_ => "    "))}";

        /// <summary>
        /// Build a new string with `indent` tabs before
        /// </summary>
        public static string Tabs(int indent, string str) =>
            $"{Tabs(indent)}{str}";

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