using LanguageExt;
using OpenQA.Selenium;
using System;
using static LanguageExt.Prelude;

namespace Isotope79
{
    [With]
    public partial class IsotopeState
    {
        public readonly Option<IWebDriver> Driver;
        public readonly Map<string, string> Configuration;
        public readonly Option<IWebElement> TargettedElement;
        public readonly Option<string> Error;
        public readonly Log Log;
        public readonly TimeSpan DefaultWait;
        public readonly TimeSpan DefaultInterval;

        public static IsotopeState Empty =
            new IsotopeState(default, default, default, default, Log.Empty, TimeSpan.FromSeconds(10), TimeSpan.FromMilliseconds(500));

        private IsotopeState(
            Option<IWebDriver> driver,
            Map<string, string> configuration, 
            Option<IWebElement> targettedElement, 
            Option<string> error, 
            Log log, 
            TimeSpan defaultWait,
            TimeSpan defaultInterval)
        {            
            Driver = driver;
            Configuration = configuration;
            TargettedElement = targettedElement;
            Error = error;
            Log = log;
            DefaultWait = defaultWait;
            DefaultInterval = defaultInterval;
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
