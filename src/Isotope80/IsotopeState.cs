using LanguageExt;
using OpenQA.Selenium;
using System;
using System.Reactive.Subjects;
using System.Runtime.ExceptionServices;
using LanguageExt.Common;
using static LanguageExt.Prelude;

namespace Isotope80
{
    /// <summary>
    /// Untyped isotope state
    /// Used to pass the state into a isotope computation
    /// </summary>
    public partial class IsotopeState
    {
        internal readonly Option<IWebDriver> Driver;
        internal readonly IsotopeSettings Settings;
        internal readonly HashMap<string, string> Configuration;
        
        /// <summary>
        /// Errors
        /// </summary>
        public readonly Seq<Error> Error;
        
        /// <summary>
        /// Log
        /// </summary>
        public readonly Log Log;
        
        /// <summary>
        /// Context stack
        /// </summary>
        public readonly Stck<string> Context;

        /// <summary>
        /// Mute log
        /// </summary>
        public readonly bool Mute;
        
        /// <summary>
        /// Creates a new instance of IsotopeState with the supplied settings.
        /// </summary>
        public static IsotopeState Create(IsotopeSettings settings) =>
            Empty.With(Settings: settings);

        /// <summary>
        /// Immutable transformation
        /// </summary>
        internal IsotopeState With(
            Option<IWebDriver>? Driver = null,
            IsotopeSettings Settings = null,
            HashMap<string, string>? Configuration = null,
            Seq<Error>? Error = null,
            Log Log = null,
            Stck<string>? Context = null,
            bool? Mute = null) =>
            new IsotopeState(
                Driver ?? this.Driver,
                Settings ?? this.Settings,
                Configuration ?? this.Configuration, 
                Error ?? this.Error, 
                Log ?? this.Log,
                Context ?? this.Context,
                Mute ?? this.Mute);

        /// <summary>
        /// Empty state
        /// </summary>
        internal static IsotopeState Empty =
            new IsotopeState(
                default,
                IsotopeSettings.Create(
                    new Subject<Error>(), 
                    new Subject<LogOutput>()), 
                default, 
                default, 
                Log.Empty,
                default,
                default);

        private IsotopeState(
            Option<IWebDriver> driver,
            IsotopeSettings settings,
            HashMap<string, string> configuration,
            Seq<Error> error, 
            Log log,
            Stck<string> context,
            bool mute)
        {            
            Driver        = driver;
            Settings      = settings;
            Configuration = configuration;
            Error         = error;
            Log           = log;
            Context       = context;
            Mute          = mute;
        }

        internal IsotopeState AddError(Error err) =>
            With(Error: Error.Add(err));

        internal IsotopeState AddError(string err) =>
            AddError(LanguageExt.Common.Error.New(err));

        internal IsotopeState AddErrors(Seq<Error> err) =>
            With(Error: Error + err);

        internal IsotopeState AddErrors(Seq<string> err) =>
            AddErrors(err.Map(LanguageExt.Common.Error.New));

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
            Error.Iter(Settings.ErrorStream.OnNext);
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

    /// <summary>
    /// Typed isotope state, contains an untyped state and a value of A
    /// </summary>
    /// <typeparam name="A">Bound value type</typeparam>
    public class IsotopeState<A>
    {
        /// <summary>
        /// Return value
        /// </summary>
        public readonly A Value;
        
        /// <summary>
        /// Resulting state
        /// </summary>
        public readonly IsotopeState State;

        /// <summary>
        /// Ctor
        /// </summary>
        public IsotopeState(A value, IsotopeState state)
        {
            Value = value;
            State = state;
        }

        /// <summary>
        /// Functor map
        /// </summary>
        public IsotopeState<B> Map<B>(Func<A, B> f) =>
            new IsotopeState<B>(f(Value), State);

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
