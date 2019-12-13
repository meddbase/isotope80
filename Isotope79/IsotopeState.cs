using LanguageExt;
using OpenQA.Selenium;
using System;

namespace Isotope79
{
    [With]
    public partial class IsotopeState
    {
        public readonly Option<IWebDriver> Driver;
        public readonly IsotopeSettings Settings;
        public readonly Map<string, string> Configuration;
        public readonly Option<string> Error;
        public readonly Log Log;

        public static IsotopeState Empty =
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

        public IsotopeState Write(string log, Action<string, int> action) =>
            With(Log: Log.Append(log, action));

        public IsotopeState PushLog(string log, Action<string, int> action) =>
            With(Log: Log.Push(log, action));

        public IsotopeState PopLog() =>
            With(Log: Log.Pop());
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
