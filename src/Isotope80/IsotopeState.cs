using LanguageExt;
using OpenQA.Selenium;
using System;
using System.Runtime.ExceptionServices;
using LanguageExt.Common;
using static LanguageExt.Prelude;

namespace Isotope80
{
    public partial class IsotopeState
    {
        internal readonly Option<IWebDriver> Driver;
        internal readonly IsotopeSettings Settings;
        internal readonly Map<string, string> Configuration;
        public readonly Seq<Error> Error;
        public readonly Log Log;

        /// <summary>
        /// Creates a new instance of IsotopeState with the supplied settings.
        /// </summary>
        public static IsotopeState Create(IsotopeSettings settings) =>
            Empty.With(Settings: settings);

        internal IsotopeState With(
            Option<IWebDriver>? Driver = null,
            IsotopeSettings Settings = null,
            Map<string, string>? Configuration = null,
            Seq<Error>? Error = null,
            Log Log = null) =>
            new IsotopeState(
                Driver ?? this.Driver,
                Settings ?? this.Settings,
                Configuration ?? this.Configuration, 
                Error ?? this.Error, 
                Log ?? this.Log);

        internal static IsotopeState Empty =
            new IsotopeState(default, IsotopeSettings.Create(), default, default, Log.Empty);

        private IsotopeState(
            Option<IWebDriver> driver,
            IsotopeSettings settings,
            Map<string, string> configuration,
            Seq<Error> error, 
            Log log)
        {            
            Driver = driver;
            Settings = settings;
            Configuration = configuration;
            Error = error;
            Log = log;
        }

        internal IsotopeState AddError(Error err) =>
            With(Error: Error.Add(err));

        internal IsotopeState AddError(string err) =>
            AddError(LanguageExt.Common.Error.New(err));

        internal IsotopeState AddErrors(Seq<Error> err) =>
            With(Error: Error + err);

        internal IsotopeState AddErrors(Seq<string> err) =>
            AddErrors(err.Map(LanguageExt.Common.Error.New));

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

        /// <summary>
        /// Throw if the state is faulted
        /// </summary>
        /// <remarks>
        /// If there's one error, then its original context is maintained (stack trace, etc)
        /// </remarks>
        public Unit IfFailedThrow()
        {
            if (Error.IsEmpty)
            {
                return unit;
            }
            Error.Iter(err => Settings.FailureAction(err, Log));
            if (Error.Count == 1)
            {
                ExceptionDispatchInfo.Capture(Error.Head).Throw(); 
            }
            else
            {
                throw new AggregateException(Error.Map(e => (Exception) e));
            }
            return unit;
        }

        /// <summary>
        /// True if the state is faulted
        /// </summary>
        public bool IsFaulted =>
            !Error.IsEmpty;
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

        /// <summary>
        /// True if the state is faulted
        /// </summary>
        public bool IsFaulted =>
            State.IsFaulted;

        internal IsotopeState<B> CastError<B>() =>
            IsFaulted
                ? new IsotopeState<B>(default, State)
                : throw new InvalidOperationException("Only cast when state is faulted");
    }
}
