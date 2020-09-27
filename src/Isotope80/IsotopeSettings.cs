using LanguageExt;
using System;
using LanguageExt.Common;

namespace Isotope80
{
    [With]
    public partial class IsotopeSettings
    {
        public readonly bool DisposeOnCompletion;
        private const bool defaultDisposeOnCompletion = true;
        public readonly Action<string, int> LoggingAction;
        private static Action<string, int> defaultLoggingAction = (x,y) => { };
        public readonly Action<Error, Log> FailureAction;
        private static Action<Error, Log> defaultFailureAction = (x, y) => { };
        public readonly TimeSpan Wait;
        private static TimeSpan defaultWait = TimeSpan.FromSeconds(10);
        public readonly TimeSpan Interval;
        private static TimeSpan defaultInterval = TimeSpan.FromMilliseconds(500);

        private IsotopeSettings(
            bool disposeOnCompletion,
            Action<string, int> loggingAction,
            Action<Error, Log> failureAction,
            TimeSpan wait,
            TimeSpan interval)
        {
            DisposeOnCompletion = disposeOnCompletion;
            LoggingAction = loggingAction;
            FailureAction = failureAction;
            Wait = wait;
            Interval = interval;
        }

        public static IsotopeSettings Create(
            bool? disposeOnCompletion = null,
            Action<string, int> loggingAction = null,
            Action<Error, Log> failureAction = null,
            TimeSpan? wait = null,
            TimeSpan? interval = null) =>
            new IsotopeSettings(
                disposeOnCompletion ?? defaultDisposeOnCompletion,
                loggingAction ?? defaultLoggingAction,
                failureAction ?? defaultFailureAction,
                wait ?? defaultWait,
                interval ?? defaultInterval);

        static Action<Error, Log> MakeFail(Action<string, Log> f) =>
            f == null
                ? defaultFailureAction
                : (Error err, Log log) => f(err.ToString(), log);

    }
}
