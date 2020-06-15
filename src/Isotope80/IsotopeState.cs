using LanguageExt;
using OpenQA.Selenium;
using System;
using static LanguageExt.Prelude;

namespace Isotope80
{
    public partial class IsotopeState
    {
        internal readonly Option<IWebDriver> Driver;
        internal readonly IsotopeSettings Settings;
        internal readonly Map<string, string> Configuration;
        public readonly Option<string> Error;
        public readonly Log Log;

        /// <summary>
        /// Creates a new instance of IsotopeState with the supplied settings.
        /// </summary>
        public static IsotopeState Create(IsotopeSettings settings) =>
            Empty.With(Settings: settings);

        internal IsotopeState With(Option<IWebDriver>? Driver = null, IsotopeSettings Settings = null, Map<string, string>? Configuration = null, Option<string>? Error = null, Log Log = null) => new IsotopeState(Driver ?? this.Driver, Settings ?? this.Settings, Configuration ?? this.Configuration, Error ?? this.Error, Log ?? this.Log);

        internal static IsotopeState Empty =
            new IsotopeState(default, IsotopeSettings.Create(), default, default, Log.Empty);

        private IsotopeState(
            Option<IWebDriver> driver,
            IsotopeSettings settings,
            Map<string, string> configuration,
            Option<string> error, 
            Log log)
        {            
            Driver = driver;
            Settings = settings;
            Configuration = configuration;
            Error = error;
            Log = log;
        }

        internal IsotopeState Write(string log, Action<string, int> action) =>
            With(Log: Log.Append(log, action));

        internal IsotopeState PushLog(string log, Action<string, int> action) =>
            With(Log: Log.Push(log, action));

        internal IsotopeState PopLog() =>
            With(Log: Log.Pop());

        internal Unit DisposeWebDriver() =>
            Driver.Match(
                Some: d => { d.Quit(); return unit; },
                None: () => unit);       
    }

    public class IsotopeState<A>
    {
        public readonly A Value;
        public readonly IsotopeState State;

        public IsotopeState(A value, IsotopeState state)
        {
            Value = value;
            State = state;
        }

        public IsotopeState<T> Map<T>(Func<A, T> mapper) =>
            new IsotopeState<T>(mapper(Value), State);
    }
}
