using LanguageExt;
using Microsoft.Playwright;
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
        internal readonly Option<IPage> Page;
        internal readonly Option<IBrowserContext> BrowserContext;
        internal readonly Option<IBrowser> Browser;
        internal readonly Option<IPlaywright> Playwright;
        internal readonly Stck<IFrameLocator> FrameScope;
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
            Option<IPage>? Page = null,
            Option<IBrowserContext>? BrowserContext = null,
            Option<IBrowser>? Browser = null,
            Option<IPlaywright>? Playwright = null,
            Stck<IFrameLocator>? FrameScope = null,
            IsotopeSettings Settings = null,
            HashMap<string, string>? Configuration = null,
            Seq<Error>? Error = null,
            Log Log = null,
            Stck<string>? Context = null,
            bool? Mute = null) =>
            new IsotopeState(
                Page ?? this.Page,
                BrowserContext ?? this.BrowserContext,
                Browser ?? this.Browser,
                Playwright ?? this.Playwright,
                FrameScope ?? this.FrameScope,
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
                default,
                default,
                default,
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
            Option<IPage> page,
            Option<IBrowserContext> browserContext,
            Option<IBrowser> browser,
            Option<IPlaywright> playwright,
            Stck<IFrameLocator> frameScope,
            IsotopeSettings settings,
            HashMap<string, string> configuration,
            Seq<Error> error,
            Log log,
            Stck<string> context,
            bool mute)
        {
            Page           = page;
            BrowserContext = browserContext;
            Browser        = browser;
            Playwright     = playwright;
            FrameScope     = frameScope;
            Settings       = settings;
            Configuration  = configuration;
            Error          = error;
            Log            = log;
            Context        = context;
            Mute           = mute;
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
}
