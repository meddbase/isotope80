using LanguageExt;
using System;

namespace Isotope80
{
    [With]
    public partial class IsotopeSettings
    {
        public readonly Action<string, int> LoggingAction;
        private static Action<string, int> defaultLoggingAction = (x,y) => { };
        public readonly TimeSpan Wait;
        private static TimeSpan defaultWait = TimeSpan.FromSeconds(10);
        public readonly TimeSpan Interval;
        private static TimeSpan defaultInterval = TimeSpan.FromMilliseconds(500);

        private IsotopeSettings(
            Action<string, int> loggingAction,
            TimeSpan wait,
            TimeSpan interval)
        {
            LoggingAction = loggingAction;
            Wait = wait;
            Interval = interval;
        }

        public static IsotopeSettings Create(
            Action<string, int> loggingAction = null,
            TimeSpan? wait = null,
            TimeSpan? interval = null) =>
            new IsotopeSettings(
                loggingAction ?? defaultLoggingAction,
                wait ?? defaultWait,
                interval ?? defaultInterval);
    }
}
