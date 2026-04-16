using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;

namespace Isotope80
{
    public static partial class Isotope
    {
        /// <summary>
        /// Gets the state from the Isotope monad
        /// </summary>
        public static Isotope<IsotopeState> get =
            iso(identity);

        /// <summary>
        /// Puts the state back into the Isotope monad
        /// </summary>
        public static Isotope<Unit> put(IsotopeState state) =>
            new Isotope<Unit>(_ => new IsotopeState<Unit>(default, state));

        /// <summary>
        /// Modify the state from the Isotope monad
        /// </summary>
        public static Isotope<Unit> modify(Func<IsotopeState, IsotopeState> f) =>
            get.Bind(s => put(f(s)));

        /// <summary>
        /// Identity - lifts a value of `A` into the Isotope monad
        ///
        /// * Always succeeds *
        ///
        /// </summary>
        public static Isotope<A> pure<A>(A value) =>
            Isotope<A>.Pure(value);

        /// <summary>
        /// Useful for starting a linq expression if you need lets first
        /// i.e.
        ///         from _ in unitM
        ///         let foo = "123"
        ///         let bar = "456"
        ///         from x in ....
        /// </summary>
        public static Isotope<Unit> unitM =>
            pure(unit);

        static string showContext(Stck<string> ctx) =>
            String.Join(" → ", ctx.Rev());

        /// <summary>
        /// Failure
        /// </summary>
        /// <param name="err">Error</param>
        public static Error fail(Error err) =>
            err;

        /// <summary>
        /// Failure
        /// </summary>
        /// <param name="err">Error</param>
        public static Error fail(string err) =>
            Error.New(err);

        /// <summary>
        /// Failure
        /// </summary>
        /// <param name="msg">Error message</param>
        /// <param name="ex">Exception</param>
        public static Error fail(string msg, Exception ex) =>
            Error.New(msg, ex);

        /// <summary>
        /// Failure
        /// </summary>
        /// <param name="err">Error</param>
        public static Error fail(Exception err) =>
            Error.New(err);


        /// <summary>
        /// Failure - creates an Isotope monad that always fails
        /// </summary>
        /// <param name="err">Error</param>
        public static Isotope<A> fail<A>(Error err) =>
            from s in get
            from _ in error(err.ToString())
            from r in Isotope<A>.Fail(Error.New($"{err.Message} ({showContext(s.Context)})", err.Exception.IsSome ? (Exception)err : null))
            select r;

        /// <summary>
        /// Failure - creates an Isotope monad that always fails
        /// </summary>
        /// <param name="message">Error message</param>
        public static Isotope<A> fail<A>(string message) =>
            from s in get
            from _ in error(message)
            from r in Isotope<A>.Fail(Error.New($"{message} ({showContext(s.Context)})"))
            select r;

        /// <summary>
        /// Failure - creates an Isotope monad that always fails
        /// </summary>
        /// <param name="ex">Exception</param>
        public static Isotope<A> fail<A>(Exception ex) =>
            from s in get
            from _ in error(ex.Message)
            from r in Isotope<A>.Fail(Error.New($"{ex.Message} ({showContext(s.Context)})", ex))
            select r;

        /// <summary>
        /// Lift the function into the isotope monadic space
        /// </summary>
        public static Isotope<A> iso<A>(Func<A> f) =>
            new Isotope<A>(s => new IsotopeState<A>(f(), s));

        /// <summary>
        /// Lift the function into the isotope monadic space
        /// </summary>
        public static Isotope<A> iso<A>(Func<Fin<A>> f) =>
            new Isotope<A>(s => f().Match(Succ: a => new IsotopeState<A>(a, s),
                                          Fail: e => new IsotopeState<A>(default, s.AddError(e))));

        /// <summary>
        /// Lift the function into the isotope monadic space
        /// </summary>
        public static Isotope<A> iso<A>(Func<IsotopeState, A> f) =>
            new Isotope<A>(s => new IsotopeState<A>(f(s), s));

        /// <summary>
        /// Lift the function into the isotope monadic space
        /// </summary>
        public static Isotope<A> iso<A>(Func<IsotopeState, Fin<A>> f) =>
            new Isotope<A>(s => f(s).Match(Succ: a => new IsotopeState<A>(a, s),
                                           Fail: e => new IsotopeState<A>(default, s.AddError(e))));

        /// <summary>
        /// Lift the function into the isotope monadic space
        /// </summary>
        public static Isotope<Env, A> iso<Env, A>(Func<Env, IsotopeState, A> f) =>
            new Isotope<Env, A>((e, s) => new IsotopeState<A>(f(e, s), s));

        /// <summary>
        /// Lift the function into the isotope monadic space
        /// </summary>
        public static Isotope<Env, A> iso<Env, A>(Func<Env, IsotopeState, Fin<A>> f) =>
            new Isotope<Env, A>((e, s) => f(e, s).Match(Succ: a => new IsotopeState<A>(a, s),
                                                       Fail: e => new IsotopeState<A>(default, s.AddError(e))));

        /// <summary>
        /// Lift the function into the isotope monadic space
        /// </summary>
        public static IsotopeAsync<A> isoAsync<A>(Func<ValueTask<A>> f) =>
            new IsotopeAsync<A>(async s => new IsotopeState<A>(await f().ConfigureAwait(false), s));

        /// <summary>
        /// Lift the function into the isotope monadic space
        /// </summary>
        public static IsotopeAsync<A> isoAsync<A>(Func<ValueTask<Fin<A>>> f) =>
            new IsotopeAsync<A>(async s => (await f().ConfigureAwait(false)).Match(Succ: a => new IsotopeState<A>(a, s),
                                                                                   Fail: e => new IsotopeState<A>(default, s.AddError(e))));

        /// <summary>
        /// Lift the function into the isotope monadic space
        /// </summary>
        public static IsotopeAsync<A> isoAsync<A>(Func<IsotopeState, ValueTask<A>> f) =>
            new IsotopeAsync<A>(async s => new IsotopeState<A>(await f(s).ConfigureAwait(false), s));

        /// <summary>
        /// Lift the function into the isotope monadic space
        /// </summary>
        public static IsotopeAsync<A> isoAsync<A>(Func<IsotopeState, ValueTask<Fin<A>>> f) =>
            new IsotopeAsync<A>(async s => (await f(s).ConfigureAwait(false)).Match(Succ: a => new IsotopeState<A>(a, s),
                                                                                    Fail: e => new IsotopeState<A>(default, s.AddError(e))));

        /// <summary>
        /// Lift the function into the isotope monadic space
        /// </summary>
        public static IsotopeAsync<Env, A> isoAsync<Env, A>(Func<Env, IsotopeState, ValueTask<A>> f) =>
            new IsotopeAsync<Env, A>(async (e, s) => new IsotopeState<A>(await f(e, s).ConfigureAwait(false), s));

        /// <summary>
        /// Lift the function into the isotope monadic space
        /// </summary>
        public static IsotopeAsync<Env, A> isoAsync<Env, A>(Func<Env, IsotopeState, ValueTask<Fin<A>>> f) =>
            new IsotopeAsync<Env, A>(async (e, s) => (await f(e, s).ConfigureAwait(false)).Match(Succ: a => new IsotopeState<A>(a, s),
                                                                                                 Fail: e => new IsotopeState<A>(default, s.AddError(e))));

        /// <summary>
        /// Gets the environment from the Isotope monad
        /// </summary>
        /// <typeparam name="Env">Environment</typeparam>
        public static Isotope<Env, Env> ask<Env>() =>
            iso((Env env, IsotopeState _) => env);

        /// <summary>
        /// Gets a function of the current environment
        /// </summary>
        public static Isotope<Env, R> asks<Env, R>(Func<Env, R> f) =>
            ask<Env>().Map(f);

        /// <summary>
        /// Map a local environment
        /// </summary>
        public static Isotope<EnvA, A> local<EnvA, EnvB, A>(Func<EnvA, EnvB> f, Isotope<EnvB, A> ma) =>
            new Isotope<EnvA, A>((ea, s) => ma.Invoke(f(ea), s));

        /// <summary>
        /// Map a local environment
        /// </summary>
        public static IsotopeAsync<EnvA, A> local<EnvA, EnvB, A>(Func<EnvA, EnvB> f, IsotopeAsync<EnvB, A> ma) =>
            new IsotopeAsync<EnvA, A>((ea, s) => ma.Invoke(f(ea), s));

        /// <summary>
        /// Write a log entry to the log stream
        /// </summary>
        static Isotope<Unit> writeToLogStream(Log entry) =>
            new Isotope<Unit>(s => {
                  if (!String.IsNullOrWhiteSpace(entry.Message) && !s.Mute)
                  {
                      s.Settings.LogStream.OnNext(new LogOutput(entry.Message, entry.Type, s.Context.Count, entry.Time, entry.CallerMemberName, entry.CallerFilePath, entry.CallerLineNumber));
                  }
                  return new IsotopeState<Unit>(default, s);
              });

        /// <summary>
        /// Log some output
        /// </summary>
        [Obsolete("Use `info | warn | error` instead")]
        public static Isotope<Unit> log(
            string message,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0) =>
            from s in get
            let p = s.Log.Add(Log.Info(message, DateTime.UtcNow, callerMemberName, callerFilePath, callerLineNumber))
            from x in put(s.With(Log: p.Log))
            from y in writeToLogStream(p.Added)
            select unit;

        /// <summary>
        /// Log some output as info
        /// </summary>
        public static Isotope<Unit> info(
            string message,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0) =>
            from s in get
            let p = s.Log.Add(Log.Info(message, DateTime.UtcNow, callerMemberName, callerFilePath, callerLineNumber))
            from x in put(s.With(Log: p.Log))
            from y in writeToLogStream(p.Added)
            select unit;

        /// <summary>
        /// Log some output as a warning
        /// </summary>
        public static Isotope<Unit> warn(
            string message,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0) =>
            from s in get
            let p = s.Log.Add(Log.Warning(message, DateTime.UtcNow, callerMemberName, callerFilePath, callerLineNumber))
            from x in put(s.With(Log: p.Log))
            from y in writeToLogStream(p.Added)
            select unit;

        /// <summary>
        /// Log some output as an error
        /// </summary>
        /// <remarks>Note: This only logs the error, it doesn't stop the computation.  Use `fail` for computation
        /// termination.  `fail` also logs to the output using this function.</remarks>
        public static Isotope<Unit> error(
            string message,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0) =>
            from s in get
            let p = s.Log.Add(Log.Error(message, DateTime.UtcNow, callerMemberName, callerFilePath, callerLineNumber))
            from x in put(s.With(Log: p.Log))
            from y in writeToLogStream(p.Added)
            select unit;

        /// <summary>
        /// Create a logging context
        /// </summary>
        public static Isotope<A> context<A>(
            string context,
            Isotope<A> iso,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0) =>
            from s in get
            let p = Log.Context(context, DateTime.UtcNow, callerMemberName, callerFilePath, callerLineNumber).Rebase(s.Log.Indent)
            from y in writeToLogStream(p)
            from x in put(s.With(Log: p, Context: s.Context.Push(context)))
            from r in iso
            from _ in modify(s2 => s2.With(Context: s.Context,
                                           Log: s.Log.Add(s2.Log).Log))
            select r;

        /// <summary>
        /// Create a logging context
        /// </summary>
        public static Isotope<Env, A> context<Env, A>(
            string context,
            Isotope<Env, A> iso,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0) =>
            from s in get
            let p = Log.Context(context, DateTime.UtcNow, callerMemberName, callerFilePath, callerLineNumber).Rebase(s.Log.Indent)
            from y in writeToLogStream(p)
            from x in put(s.With(Log: p, Context: s.Context.Push(context)))
            from r in iso
            from _ in modify(s2 => s2.With(Context: s.Context,
                                           Log: s.Log.Add(s2.Log).Log))
            select r;

        /// <summary>
        /// Create a logging context
        /// </summary>
        public static IsotopeAsync<A> context<A>(
            string context,
            IsotopeAsync<A> iso,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0) =>
            from s in get
            let p = Log.Context(context, DateTime.UtcNow, callerMemberName, callerFilePath, callerLineNumber).Rebase(s.Log.Indent)
            from y in writeToLogStream(p)
            from x in put(s.With(Log: p, Context: s.Context.Push(context)))
            from r in iso
            from _ in modify(s2 => s2.With(Context: s.Context,
                                           Log: s.Log.Add(s2.Log).Log))
            select r;

        /// <summary>
        /// Create a logging context
        /// </summary>
        public static IsotopeAsync<Env, A> context<Env, A>(
            string context,
            IsotopeAsync<Env, A> iso,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0) =>
            from s in get
            let p = Log.Context(context, DateTime.UtcNow, callerMemberName, callerFilePath, callerLineNumber).Rebase(s.Log.Indent)
            from y in writeToLogStream(p)
            from x in put(s.With(Log: p, Context: s.Context.Push(context)))
            from r in iso
            from _ in modify(s2 => s2.With(Context: s.Context,
                                           Log: s.Log.Add(s2.Log).Log))
            select r;

        /// <summary>
        /// Mute log
        /// </summary>
        public static Isotope<A> mute<A>(Isotope<A> iso) =>
            from s in get
            from x in put(s.With(Mute: true))
            from r in iso
            from _ in modify(s2 => s2.With(Mute: s.Mute))
            select r;

        /// <summary>
        /// Mute log
        /// </summary>
        public static Isotope<Env, A> mute<Env, A>(Isotope<Env, A> iso) =>
            from s in get
            from x in put(s.With(Mute: true))
            from r in iso
            from _ in modify(s2 => s2.With(Mute: s.Mute))
            select r;

        /// <summary>
        /// Mute log
        /// </summary>
        public static IsotopeAsync<A> mute<A>(IsotopeAsync<A> iso) =>
            from s in get
            from x in put(s.With(Mute: true))
            from r in iso
            from _ in modify(s2 => s2.With(Mute: s.Mute))
            select r;

        /// <summary>
        /// Mute log
        /// </summary>
        public static IsotopeAsync<Env, A> mute<Env, A>(IsotopeAsync<Env, A> iso) =>
            from s in get
            from x in put(s.With(Mute: true))
            from r in iso
            from _ in modify(s2 => s2.With(Mute: s.Mute))
            select r;

        /// <summary>
        /// Measure the time interval of an isotope
        /// </summary>
        public static Isotope<(A Result, TimeSpan Time)> stopwatch<A>(Isotope<A> iso) =>
            from _1 in info("Start stopwatch")
            from t in pure(Stopwatch.StartNew())
            from r in iso
            from _2 in trya(() => t.Stop(), "Unable to stop stopwatch")
            from e in tryf(() => t.Elapsed, "Unable to get elapsed time")
            from _3 in info($"Stop stopwatch, elapsed time: {e.ToString(@"m\:ss\.fff")}")
            select (r, e);

        /// <summary>
        /// Measure the time interval of an isotope
        /// </summary>
        public static Isotope<Env, (A Result, TimeSpan Time)> stopwatch<Env, A>(Isotope<Env, A> iso) =>
            from _1 in info("Start stopwatch")
            from t in pure(Stopwatch.StartNew())
            from r in iso
            from _2 in trya(() => t.Stop(), "Unable to stop stopwatch")
            from e in tryf(() => t.Elapsed, "Unable to get elapsed time")
            from _3 in info($"Stop stopwatch, elapsed time: {e.ToString(@"m\:ss\.fff")}")
            select (r, e);

        /// <summary>
        /// Measure the time interval of an isotope
        /// </summary>
        public static IsotopeAsync<(A Result, TimeSpan Time)> stopwatch<A>(IsotopeAsync<A> iso) =>
            from _1 in info("Start stopwatch")
            from t in pure(Stopwatch.StartNew())
            from r in iso
            from _2 in trya(() => t.Stop(), "Unable to stop stopwatch")
            from e in tryf(() => t.Elapsed, "Unable to get elapsed time")
            from _3 in info($"Stop stopwatch, elapsed time: {e.ToString(@"m\:ss\.fff")}")
            select (r, e);

        /// <summary>
        /// Measure the time interval of an isotope
        /// </summary>
        public static IsotopeAsync<Env, (A Result, TimeSpan Time)> stopwatch<Env, A>(IsotopeAsync<Env, A> iso) =>
            from _1 in info("Start stopwatch")
            from t in pure(Stopwatch.StartNew())
            from r in iso
            from _2 in trya(() => t.Stop(), "Unable to stop stopwatch")
            from e in tryf(() => t.Elapsed, "Unable to get elapsed time")
            from _3 in info($"Stop stopwatch, elapsed time: {e.ToString(@"m\:ss\.fff")}")
            select (r, e);

        /// <summary>
        /// Simple configuration setup
        /// </summary>
        /// <param name="config">Map of config items</param>
        public static Isotope<Unit> initConfig(params (string, string)[] config) =>
            initConfig(toHashMap(config));

        /// <summary>
        /// Simple configuration setup
        /// </summary>
        /// <param name="config">Map of config items</param>
        public static Isotope<Unit> initConfig(Map<string, string> config) =>
            initConfig(toHashMap(config));

        /// <summary>
        /// Simple configuration setup
        /// </summary>
        /// <param name="config">Map of config items</param>
        public static Isotope<Unit> initConfig(HashMap<string, string> config) =>
            from s in get
            from _ in put(s.With(Configuration: config))
            select unit;

        /// <summary>
        /// Get a config key
        /// </summary>
        /// <param name="key">Configuration key</param>
        public static Isotope<string> config(string key) =>
            from s in get
            from r in s.Configuration.Find(key).ToIsotope($"Configuration key not found: {key}")
            select r;

        /// <summary>
        /// Update the settings within the Isotope computation
        /// </summary>
        /// <param name="settings">Settings to use</param>
        public static Isotope<Unit> initSettings(IsotopeSettings settings) =>
            from s in get
            from _ in put(s.With(Settings: settings))
            select unit;

        /// <summary>
        /// Use a disposable resource, and clean it up afterwards
        /// </summary>
        /// <param name="resource">Disposable resource</param>
        /// <param name="f">Function to receive the resource and return an isotope run in that context</param>
        /// <typeparam name="A">Disposable resource type</typeparam>
        /// <typeparam name="B">Resulting bound value type</typeparam>
        public static Isotope<B> use<A, B>(A resource, Func<A, Isotope<B>> f) where A : IDisposable =>
            new Isotope<B>(s =>
            {
                try
                {
                    return f(resource).Invoke(s);
                }
                finally
                {
                    resource?.Dispose();
                }
            });

        /// <summary>
        /// Use a disposable resource, and clean it up afterwards
        /// </summary>
        /// <param name="resource">Disposable resource</param>
        /// <param name="f">Function to receive the resource and return an isotope run in that context</param>
        /// <typeparam name="Env">Environment type</typeparam>
        /// <typeparam name="A">Disposable resource type</typeparam>
        /// <typeparam name="B">Resulting bound value type</typeparam>
        public static Isotope<Env, B> use<Env, A, B>(A resource, Func<A, Isotope<Env, B>> f) where A : IDisposable =>
            new Isotope<Env, B>((e, s) =>
            {
                try
                {
                    return f(resource).Invoke(e, s);
                }
                finally
                {
                    resource?.Dispose();
                }
            });

        /// <summary>
        /// Use a disposable resource, and clean it up afterwards
        /// </summary>
        /// <param name="resource">Disposable resource</param>
        /// <param name="f">Function to receive the resource and return an isotope run in that context</param>
        /// <typeparam name="A">Disposable resource type</typeparam>
        /// <typeparam name="B">Resulting bound value type</typeparam>
        public static IsotopeAsync<B> use<A, B>(A resource, Func<A, IsotopeAsync<B>> f) where A : IDisposable =>
            new IsotopeAsync<B>(async s =>
            {
                try
                {
                    return await f(resource).Invoke(s).ConfigureAwait(false);
                }
                finally
                {
                    resource?.Dispose();
                }
            });

        /// <summary>
        /// Use a disposable resource, and clean it up afterwards
        /// </summary>
        /// <param name="resource">Disposable resource</param>
        /// <param name="f">Function to receive the resource and return an isotope run in that context</param>
        /// <typeparam name="Env">Environment type</typeparam>
        /// <typeparam name="A">Disposable resource type</typeparam>
        /// <typeparam name="B">Resulting bound value type</typeparam>
        public static IsotopeAsync<Env, B> use<Env, A, B>(A resource, Func<A, IsotopeAsync<Env, B>> f) where A : IDisposable =>
            new IsotopeAsync<Env, B>(async (e, s) =>
            {
                try
                {
                    return await f(resource).Invoke(e, s).ConfigureAwait(false);
                }
                finally
                {
                    resource?.Dispose();
                }
            });

        /// <summary>
        /// Use a resource with a custom dispose function, and clean it up afterwards
        /// </summary>
        /// <param name="resource">Resource</param>
        /// <param name="dispose">Function to clean up the resource on completion</param>
        /// <param name="f">Function to receive the resource and return an isotope run in that context</param>
        /// <typeparam name="A">Resource type</typeparam>
        /// <typeparam name="B">Resulting bound value type</typeparam>
        public static Isotope<B> use<A, B>(A resource, Func<A, Unit> dispose, Func<A, Isotope<B>> f) =>
            new Isotope<B>(s =>
            {
                try
                {
                    return f(resource).Invoke(s);
                }
                finally
                {
                    dispose(resource);
                }
            });

        /// <summary>
        /// Use a resource with a custom dispose function, and clean it up afterwards
        /// </summary>
        /// <param name="resource">Resource</param>
        /// <param name="dispose">Function to clean up the resource on completion</param>
        /// <param name="f">Function to receive the resource and return an isotope run in that context</param>
        /// <typeparam name="Env">Environment type</typeparam>
        /// <typeparam name="A">Resource type</typeparam>
        /// <typeparam name="B">Resulting bound value type</typeparam>
        public static Isotope<Env, B> use<Env, A, B>(A resource, Func<A, Unit> dispose, Func<A, Isotope<Env, B>> f) =>
            new Isotope<Env, B>((e, s) =>
            {
                try
                {
                    return f(resource).Invoke(e, s);
                }
                finally
                {
                    dispose(resource);
                }
            });

        /// <summary>
        /// Use a resource with a custom dispose function, and clean it up afterwards
        /// </summary>
        /// <param name="resource">Resource</param>
        /// <param name="dispose">Function to clean up the resource on completion</param>
        /// <param name="f">Function to receive the resource and return an isotope run in that context</param>
        /// <typeparam name="A">Resource type</typeparam>
        /// <typeparam name="B">Resulting bound value type</typeparam>
        public static IsotopeAsync<B> use<A, B>(A resource, Func<A, Unit> dispose, Func<A, IsotopeAsync<B>> f) =>
            new IsotopeAsync<B>(async s =>
            {
                try
                {
                    return await f(resource).Invoke(s).ConfigureAwait(false);
                }
                finally
                {
                    dispose(resource);
                }
            });

        /// <summary>
        /// Use a resource with a custom dispose function, and clean it up afterwards
        /// </summary>
        /// <param name="resource">Resource</param>
        /// <param name="dispose">Function to clean up the resource on completion</param>
        /// <param name="f">Function to receive the resource and return an isotope run in that context</param>
        /// <typeparam name="Env">Environment type</typeparam>
        /// <typeparam name="A">Resource type</typeparam>
        /// <typeparam name="B">Resulting bound value type</typeparam>
        public static IsotopeAsync<Env, B> use<Env, A, B>(A resource, Func<A, Unit> dispose, Func<A, IsotopeAsync<Env, B>> f) =>
            new IsotopeAsync<Env, B>(async (e, s) =>
            {
                try
                {
                    return await f(resource).Invoke(e, s).ConfigureAwait(false);
                }
                finally
                {
                    dispose(resource);
                }
            });

        /// <summary>
        /// Try an action
        /// </summary>
        /// <param name="action">Action to try</param>
        /// <param name="label">Error string if exception is thrown</param>
        public static Isotope<Unit> trya(Action action, string label) =>
            iso(fun(action))
               .MapFail(e => Error.New(label, Aggregate(e)));

        /// <summary>
        /// Try an action
        /// </summary>
        /// <param name="action">Action to try</param>
        /// <param name="makeError">Convert errors to string</param>
        public static Isotope<Unit> trya(Action action, Func<Error, string> makeError) =>
            iso(fun(action))
               .MapFail(e => Error.New(makeError(e.Last), Aggregate(e)));

        /// <summary>
        /// Try a function
        /// </summary>
        /// <typeparam name="A">Return type of the function</typeparam>
        /// <param name="func">Function to try</param>
        /// <param name="label">Error string if exception is thrown</param>
        public static Isotope<A> tryf<A>(Func<A> func, string label) =>
            iso(func)
               .MapFail(e => Error.New(label, Aggregate(e)));

        /// <summary>
        /// Try a function
        /// </summary>
        /// <typeparam name="A">Return type of the function</typeparam>
        /// <param name="func">Function to try</param>
        /// <param name="makeError">Convert errors to string</param>
        public static Isotope<A> tryf<A>(Func<A> func, Func<Error, string> makeError) =>
            iso(func)
               .MapFail(e => Error.New(makeError(e.Last), Aggregate(e)));

        /// <summary>
        /// Run a void returning action
        /// </summary>
        /// <param name="action">Action to run</param>
        /// <returns>Unit</returns>
        public static Isotope<Unit> voida(Action action) => new Isotope<Unit>(state =>
        {
            action();
            return new IsotopeState<Unit>(unit, state);
        });

        /// <summary>
        /// Default wait accessor
        /// </summary>
        public static Isotope<TimeSpan> defaultWait =>
            from s in get
            select s.Settings.Wait;

        /// <summary>
        /// Default interval accessor
        /// </summary>
        public static Isotope<TimeSpan> defaultInterval =>
            from s in get
            select s.Settings.Interval;

        /// <summary>
        /// Flips the sequence of Isotopes to an Isotope of Sequences.  It does this by running each Isotope within
        /// the Seq and collects the results into a single Seq and then re-wraps within an Isotope.
        /// </summary>
        /// <remarks>
        /// If an error is encountered along the way, then the computation ends immediately.  Therefore the items in the
        /// sequence after that point are not evaluated.
        ///
        /// The resulting state will contain the log of all items evaluated or the first error encountered.
        ///
        /// Each item runs in an indexed `context`. i.e.
        ///
        ///     [0], [1], [2] ...
        ///
        /// Which makes it easier to know which index a log entry is for.
        /// </remarks>
        public static Isotope<Seq<A>> Sequence<A>(this Seq<Isotope<A>> mas) =>
            new Isotope<Seq<A>>(
                state => {
                    var vals  = new A[mas.Count];
                    int index = 0;

                    var nstate = state;

                    foreach (var ma in mas)
                    {
                        var rstate = context($"[{index}]", ma).Invoke(nstate);
                        if (rstate.IsFaulted)
                        {
                            return new IsotopeState<Seq<A>>(
                                vals.ToSeq(),
                                state.With(Error: rstate.State.Error, Log: rstate.State.Log));
                        }

                        nstate      = rstate.State;
                        vals[index] = rstate.Value;
                        index++;
                    }

                    return new IsotopeState<Seq<A>>(vals.ToSeq(), state.With(Log: nstate.Log));
                });

        /// <summary>
        /// Flips the sequence of Isotopes to an Isotope of Sequences.  It does this by running each Isotope within
        /// the Seq and collects the results into a single Seq and then re-wraps within an Isotope.
        /// </summary>
        /// <remarks>
        /// If an error is encountered along the way, then the computation ends immediately.  Therefore the items in the
        /// sequence after that point are not evaluated.
        ///
        /// The resulting state will contain the log of all items evaluated or the first error encountered.
        ///
        /// Each item runs in an indexed `context`. i.e.
        ///
        ///     [0], [1], [2] ...
        ///
        /// Which makes it easier to know which index a log entry is for.
        /// </remarks>
        public static Isotope<Env, Seq<A>> Sequence<Env, A>(this Seq<Isotope<Env, A>> mas) =>
            new Isotope<Env, Seq<A>>(
                (env, state) => {
                    var vals  = new A[mas.Count];
                    var log   = state.Log;
                    int index = 0;

                    var nstate = state;

                    foreach (var ma in mas)
                    {
                        var rstate = context($"[{index}]", ma).Invoke(env, nstate);
                        if (rstate.IsFaulted)
                        {
                            return new IsotopeState<Seq<A>>(
                                vals.ToSeq(),
                                state.With(Error: rstate.State.Error, Log: rstate.State.Log));
                        }

                        nstate      = rstate.State;
                        vals[index] = rstate.Value;
                        index++;
                    }

                    return new IsotopeState<Seq<A>>(vals.ToSeq(), state.With(Log: nstate.Log));
                });

        /// <summary>
        /// Flips the sequence of Isotopes to an Isotope of Sequences.  It does this by running each Isotope within
        /// the Seq and collects the results into a single Seq and then re-wraps within an Isotope.
        /// </summary>
        /// <remarks>
        /// If an error is encountered along the way, then the computation ends immediately.  Therefore the items in the
        /// sequence after that point are not evaluated.
        ///
        /// The resulting state will contain the log of all items evaluated or the first error encountered.
        ///
        /// Each item runs in an indexed `context`. i.e.
        ///
        ///     [0], [1], [2] ...
        ///
        /// Which makes it easier to know which index a log entry is for.
        /// </remarks>
        public static IsotopeAsync<Seq<A>> Sequence<A>(this Seq<IsotopeAsync<A>> mas) =>
            new IsotopeAsync<Seq<A>>(
                async state => {
                    var vals  = new A[mas.Count];
                    var log   = state.Log;
                    int index = 0;

                    var nstate = state;

                    foreach (var ma in mas)
                    {
                        var rstate = await context($"[{index}]", ma).Invoke(nstate).ConfigureAwait(false);
                        if (rstate.IsFaulted)
                        {
                            return new IsotopeState<Seq<A>>(
                                vals.ToSeq(),
                                state.With(Error: rstate.State.Error, Log: rstate.State.Log));
                        }

                        nstate      = rstate.State;
                        vals[index] = rstate.Value;
                        index++;
                    }

                    return new IsotopeState<Seq<A>>(vals.ToSeq(), state.With(Log: nstate.Log));
                });

        /// <summary>
        /// Flips the sequence of Isotopes to an Isotope of Sequences.  It does this by running each Isotope within
        /// the Seq and collects the results into a single Seq and then re-wraps within an Isotope.
        /// </summary>
        /// <remarks>
        /// If an error is encountered along the way, then the computation ends immediately.  Therefore the items in the
        /// sequence after that point are not evaluated.
        ///
        /// The resulting state will contain the log of all items evaluated or the first error encountered.
        ///
        /// Each item runs in an indexed `context`. i.e.
        ///
        ///     [0], [1], [2] ...
        ///
        /// Which makes it easier to know which index a log entry is for.
        /// </remarks>
        public static IsotopeAsync<Env, Seq<A>> Sequence<Env, A>(this Seq<IsotopeAsync<Env, A>> mas) =>
            new IsotopeAsync<Env, Seq<A>>(
                async (env, state) => {
                    var vals  = new A[mas.Count];
                    var log   = state.Log;
                    int index = 0;

                    var nstate = state;

                    foreach (var ma in mas)
                    {
                        var rstate = await context($"[{index}]", ma).Invoke(env, nstate).ConfigureAwait(false);
                        if (rstate.IsFaulted)
                        {
                            return new IsotopeState<Seq<A>>(
                                vals.ToSeq(),
                                state.With(Error: rstate.State.Error, Log: rstate.State.Log));
                        }

                        nstate      = rstate.State;
                        vals[index] = rstate.Value;
                        index++;
                    }

                    return new IsotopeState<Seq<A>>(vals.ToSeq(), state.With(Log: nstate.Log));
                });

        /// <summary>
        /// Flips the sequence of Isotopes to an Isotope of Sequences.  It does this by running each Isotope within
        /// the Seq and collects the results into a single Seq and then re-wraps within an Isotope.
        /// </summary>
        /// <remarks>
        /// If an error is encountered along the way then it is collected.  The process then continues to evaluate the
        /// subsequent items.  The resulting resulting state will contain the log of all items evaluated.  If there was
        /// an error, the resulting state will have an aggregated list of errors.
        ///
        /// Each item runs in an indexed `context`. i.e.
        ///
        ///     [0], [1], [2] ...
        ///
        /// Which makes it easier to know which index a log entry is for.
        /// </remarks>
        public static Isotope<Seq<A>> Collect<A>(this Seq<Isotope<A>> mas) =>
            new Isotope<Seq<A>>(
                state => {
                    var vals   = new A[mas.Count];
                    var errs   = new Seq<Error>[mas.Count];
                    int index  = 0;
                    var nstate = state;

                    foreach (var ma in mas)
                    {
                        var rstate = context($"[{index}]", ma).Invoke(nstate);

                        vals[index] = rstate.Value;
                        errs[index] = rstate.State.Error;
                        nstate = state.With(Error: Empty, Log: rstate.State.Log);

                        index++;
                    }

                    // Aggregate all the errors
                    var nerr = errs.Fold(Seq<Error>(), (s, e) => s + e);

                    return nerr.IsEmpty
                               ? new IsotopeState<Seq<A>>(vals.ToSeq(), state.With(Log: nstate.Log))
                               : new IsotopeState<Seq<A>>(vals.ToSeq(), state.With(Error: nerr, Log: nstate.Log));
                });

        /// <summary>
        /// Flips the sequence of Isotopes to an Isotope of Sequences.  It does this by running each Isotope within
        /// the Seq and collects the results into a single Seq and then re-wraps within an Isotope.
        /// </summary>
        /// <remarks>
        /// If an error is encountered along the way then it is collected.  The process then continues to evaluate the
        /// subsequent items.  The resulting resulting state will contain the log of all items evaluated.  If there was
        /// an error, the resulting state will have an aggregated list of errors.
        ///
        /// Each item runs in an indexed `context`. i.e.
        ///
        ///     [0], [1], [2] ...
        ///
        /// Which makes it easier to know which index a log entry is for.
        /// </remarks>
        public static Isotope<Env,  Seq<A>> Collect<Env, A>(this Seq<Isotope<Env,  A>> mas) =>
            new Isotope<Env,  Seq<A>>(
                (env, state) => {
                    var vals   = new A[mas.Count];
                    var errs   = new Seq<Error>[mas.Count];
                    int index  = 0;
                    var nstate = state;

                    foreach (var ma in mas)
                    {
                        var rstate = context($"[{index}]", ma).Invoke(env, nstate);

                        vals[index] = rstate.Value;
                        errs[index] = rstate.State.Error;
                        nstate      = state.With(Error: Empty, Log: rstate.State.Log);

                        index++;
                    }

                    // Aggregate all the errors
                    var nerr = errs.Fold(Seq<Error>(), (s, e) => s + e);

                    return nerr.IsEmpty
                               ? new IsotopeState<Seq<A>>(vals.ToSeq(), state.With(Log: nstate.Log))
                               : new IsotopeState<Seq<A>>(vals.ToSeq(), state.With(Error: nerr, Log: nstate.Log));
                });

        /// <summary>
        /// Flips the sequence of Isotopes to an Isotope of Sequences.  It does this by running each Isotope within
        /// the Seq and collects the results into a single Seq and then re-wraps within an Isotope.
        /// </summary>
        /// <remarks>
        /// If an error is encountered along the way then it is collected.  The process then continues to evaluate the
        /// subsequent items.  The resulting resulting state will contain the log of all items evaluated.  If there was
        /// an error, the resulting state will have an aggregated list of errors.
        ///
        /// Each item runs in an indexed `context`. i.e.
        ///
        ///     [0], [1], [2] ...
        ///
        /// Which makes it easier to know which index a log entry is for.
        /// </remarks>
        public static IsotopeAsync<Seq<A>> Collect<A>(this Seq<IsotopeAsync<A>> mas) =>
            new IsotopeAsync<Seq<A>>(
                async state => {
                    var vals   = new A[mas.Count];
                    var errs   = new Seq<Error>[mas.Count];
                    int index  = 0;
                    var nstate = state;

                    foreach (var ma in mas)
                    {
                        var rstate = await context($"[{index}]", ma).Invoke(nstate).ConfigureAwait(false);

                        vals[index] = rstate.Value;
                        errs[index] = rstate.State.Error;
                        nstate      = state.With(Error: Empty, Log: rstate.State.Log);

                        index++;
                    }

                    // Aggregate all the errors
                    var nerr = errs.Fold(Seq<Error>(), (s, e) => s + e);

                    return nerr.IsEmpty
                               ? new IsotopeState<Seq<A>>(vals.ToSeq(), state.With(Log: nstate.Log))
                               : new IsotopeState<Seq<A>>(vals.ToSeq(), state.With(Error: nerr, Log: nstate.Log));
                });

        /// <summary>
        /// Flips the sequence of Isotopes to an Isotope of Sequences.  It does this by running each Isotope within
        /// the Seq and collects the results into a single Seq and then re-wraps within an Isotope.
        /// </summary>
        /// <remarks>
        /// If an error is encountered along the way then it is collected.  The process then continues to evaluate the
        /// subsequent items.  The resulting resulting state will contain the log of all items evaluated.  If there was
        /// an error, the resulting state will have an aggregated list of errors.
        ///
        /// Each item runs in an indexed `context`. i.e.
        ///
        ///     [0], [1], [2] ...
        ///
        /// Which makes it easier to know which index a log entry is for.
        /// </remarks>
        public static IsotopeAsync<Env,  Seq<A>> Collect<Env, A>(this Seq<IsotopeAsync<Env,  A>> mas) =>
            new IsotopeAsync<Env,  Seq<A>>(
                async (env, state) => {
                    var vals   = new A[mas.Count];
                    var errs   = new Seq<Error>[mas.Count];
                    int index  = 0;
                    var nstate = state;

                    foreach (var ma in mas)
                    {
                        var rstate = await context($"[{index}]", ma).Invoke(env, nstate).ConfigureAwait(false);

                        vals[index] = rstate.Value;
                        errs[index] = rstate.State.Error;
                        nstate      = state.With(Error: Empty, Log: rstate.State.Log);

                        index++;
                    }

                    // Aggregate all the errors
                    var nerr = errs.Fold(Seq<Error>(), (s, e) => s + e);

                    return nerr.IsEmpty
                               ? new IsotopeState<Seq<A>>(vals.ToSeq(), state.With(Log: nstate.Log))
                               : new IsotopeState<Seq<A>>(vals.ToSeq(), state.With(Error: nerr, Log: nstate.Log));
                });

        /// <summary>
        /// Convert an option to a pure isotope
        /// </summary>
        /// <param name="maybe">Optional value</param>
        /// <param name="label">Failure value to use if None</param>
        /// <typeparam name="A">Bound value type</typeparam>
        /// <returns>Pure isotope</returns>
        public static Isotope<A> ToIsotope<A>(this Option<A> maybe, string label) =>
            maybe.Match(Some: pure, None: fail(label));

        /// <summary>
        /// Convert an option to a pure isotope
        /// </summary>
        /// <param name="maybe">Optional value</param>
        /// <param name="alternative">Alternative to use if None</param>
        /// <typeparam name="A">Bound value type</typeparam>
        /// <returns>Pure isotope</returns>
        public static Isotope<A> ToIsotope<A>(this Option<A> maybe, Isotope<A> alternative) =>
            maybe.ToIsotope("") | alternative;

        /// <summary>
        /// Convert a try to an isotope computation
        /// </summary>
        /// <param name="tried">Try value</param>
        /// <typeparam name="A">Bound value type</typeparam>
        /// <returns>Try computation wrapped in an isotope computation</returns>
        public static Isotope<A> ToIsotope<A>(this Try<A> tried) =>
            tried.Match(
                Succ: pure,
                Fail: x => fail(Error.New(x)));

        /// <summary>
        /// Convert a try to an isotope computation
        /// </summary>
        /// <param name="tried">Try value</param>
        /// <param name="label">Failure value to use if Fail</param>
        /// <typeparam name="A">Bound value type</typeparam>
        /// <returns>Try computation wrapped in an isotope computation</returns>
        public static Isotope<A> ToIsotope<A>(this Try<A> tried, string label) =>
            tried.ToIsotope().MapFail(e => Error.New(label, Aggregate(e)));

        /// <summary>
        /// Convert a try to an isotope computation
        /// </summary>
        /// <param name="tried">Try value</param>
        /// <param name="makeError">Failure value to use if Fail</param>
        /// <typeparam name="A">Bound value type</typeparam>
        /// <returns>Try computation wrapped in an isotope computation</returns>
        public static Isotope<A> ToIsotope<A>(this Try<A> tried, Func<Error, string> makeError) =>
            tried.ToIsotope().MapFail(e => Error.New(makeError(e.Last), Aggregate(e)));

        /// <summary>
        /// Convert a try to an isotope computation
        /// </summary>
        /// <param name="tried">Try value</param>
        /// <param name="alternative">Alternative to use if Fail</param>
        /// <typeparam name="A">Bound value type</typeparam>
        /// <returns>Try computation wrapped in an isotope computation</returns>
        public static Isotope<A> ToIsotope<A>(this Try<A> tried, Isotope<A> alternative) =>
            tried.ToIsotope() | alternative;

        /// <summary>
        /// Convert an Either to a pure isotope
        /// </summary>
        /// <param name="either">Either to convert</param>
        /// <typeparam name="R">Right param</typeparam>
        /// <returns>Pure isotope</returns>
        public static Isotope<R> ToIsotope<R>(this Either<Error, R> either) =>
            either.Match(Right: pure, Left: fail<R>);

        /// <summary>
        /// Convert an Either to a pure isotope
        /// </summary>
        /// <param name="either">Either to convert</param>
        /// <param name="label">Label for the failure</param>
        /// <returns>Pure isotope</returns>
        public static Isotope<B> ToIsotope<A, B>(this Either<A, B> either, string label) =>
            either.Match(
                Left: _ => fail(Error.New(label)),
                Right: pure);

        /// <summary>
        /// Convert an Either to a pure isotope
        /// </summary>
        /// <param name="either">Either to convert</param>
        /// <param name="makeError">Label for the failure</param>
        /// <returns>Pure isotope</returns>
        public static Isotope<B> ToIsotope<A, B>(this Either<A, B> either, Func<A, string> makeError) =>
            either.Match(
                Left: e => fail(Error.New(makeError(e))),
                Right: pure);

        /// <summary>
        /// Do while the `condition` is `true`, or it times-out
        /// </summary>
        public static Isotope<A> doWhile<A>(
            Isotope<A> iso,
            Func<A, bool> condition,
            int maxRepeats = 100) =>
            maxRepeats <= 0
                ? pure(default(A))
                : from x in iso
                  from y in condition(x)
                                ? doWhile(iso, condition, maxRepeats - 1)
                                : pure(x)
                  select y;

        /// <summary>
        /// Run `iso` while the `condition` is `true`.
        ///
        ///     * If it turns `false` or the result of `iso` is returned
        ///     * If the max-attempts are reached, then `fail`.
        ///
        /// </summary>
        public static Isotope<A> doWhileOrFail<A>(
            Isotope<A> iso,
            Func<A, bool> condition,
            int maxAttempts = 100) =>
            maxAttempts <= 0
                ? fail("do while reached the max-attempts")
                : from x in iso
                  from y in condition(x)
                                ? doWhileOrFail(iso, condition, maxAttempts - 1)
                                : pure(x)
                  select y;

        static Exception Aggregate(Seq<Error> errs) =>
            errs.IsEmpty
                ? null
                : errs.Count == 1
                    ? (Exception)errs.Head
                    : new AggregateException(errs.Map(e => (Exception)e));
    }
}
