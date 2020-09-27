using LanguageExt;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;
using static LanguageExt.Prelude;

namespace Isotope80
{
    /// <summary>
    /// Environment-free isotope computation
    /// </summary>
    /// <typeparam name="A">Bound value</typeparam>
    public struct Isotope<A>
    {
        readonly Func<IsotopeState, IsotopeState<A>> Thunk;
        
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="thunk">Thunk</param>
        public Isotope(Func<IsotopeState, IsotopeState<A>> thunk) =>
            Thunk = thunk;

        /// <summary>
        /// Invoke the computation
        /// </summary>
        /// <param name="state">State</param>
        /// <returns>Result of invoking the isotope computation</returns>
        internal IsotopeState<A> Invoke(IsotopeState state)
        {
            try
            {
                return Thunk?.Invoke(state) ?? throw new InvalidOperationException("Isotope computation not initialised");
            }
            catch (Exception e)
            {
                return new IsotopeState<A>(default, state.AddError(Error.New(e)));
            }
        }

        /// <summary>
        /// Invoke the test computation
        /// </summary>
        /// <param name="settings">Optional settings</param>
        /// <returns>Returning an optional error. 
        /// The computation succeeds if result.IsNone is true</returns>
        public (IsotopeState state, A value) Run(IsotopeSettings settings = null)
        {
            var res = Invoke(IsotopeState.Empty.With(Settings: settings));
            if (res.State.Settings.DisposeOnCompletion)
            {
                res.State.DisposeWebDriver();
            }
            return(res.State, res.Value);
        }

        /// <summary>
        /// Invoke the test computation
        /// </summary>
        /// <param name="settings">Optional settings</param>
        /// <returns>Returning an optional error. 
        /// The computation succeeds if result.IsNone is true</returns>
        public (IsotopeState state, A value) RunAndThrowOnError(IsotopeSettings settings = null)
        {
            var (state, value) = Run(settings);
            state.IfFailedThrow();
            return (state, value);
        }
        
        /// <summary>
        /// Invoke the test computation
        /// </summary>
        /// <param name="driver">Web driver</param>
        /// <param name="settings">Optional settings</param>
        /// <returns>Returning an optional error. 
        /// The computation succeeds if result.IsNone is true</returns>
        public (IsotopeState state, A value) Run(IWebDriver driver, IsotopeSettings settings = null)
        {
            var res = Invoke(IsotopeState.Empty.With(Driver: Some(driver), Settings: settings));
            if (res.State.Settings.DisposeOnCompletion)
            {
                res.State.DisposeWebDriver();
            }
            return (res.State, res.Value);
        }        
        
        /// <summary>
        /// Invoke the test computation
        /// </summary>
        /// <param name="driver">Web driver</param>
        /// <param name="settings">Optional settings</param>
        /// <returns>Returning an optional error. 
        /// The computation succeeds if result.IsNone is true</returns>
        public (IsotopeState state, A value) RunAndThrowOnError(IWebDriver driver, IsotopeSettings settings = null)
        {
            var (state, value) = Run(driver, settings);
            state.IfFailedThrow();
            return (state, value);
        }

        /// <summary>
        /// Or operator - evaluates the left hand side, if it fails, it ignores the error and evaluates the right hand side
        /// </summary>
        public static Isotope<A> operator |(Isotope<A> lhs, Isotope<A> rhs) => 
            new Isotope<A>(s =>
            {
                var l = lhs.Invoke(s);
                return l.IsFaulted
                           ? rhs.Invoke(s)
                           : l;
            });

        /// <summary>
        /// Lift the pure value into the monadic space 
        /// </summary>
        public static Isotope<A> Pure(A value) =>
            new Isotope<A>(s => new IsotopeState<A>(value, s));
        
        /// <summary>
        /// Lift the error into the monadic space 
        /// </summary>
        public static Isotope<A> Fail(Error error) =>
            new Isotope<A>(s => new IsotopeState<A>(default(A), s.AddError(error)));

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public Isotope<B> Bind<B>(Func<A, Isotope<B>> f)
        {
            var self = this;   
            return new Isotope<B>(state => 
            { 
                var s = self.Invoke(state);
                if (s.IsFaulted) return s.CastError<B>();
                return f(s.Value).Invoke(s.State);
            });
        }

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public Isotope<Env, B> Bind<Env, B>(Func<A, Isotope<Env, B>> f)
        {
            var self = this;   
            return new Isotope<Env, B>((env, state) => 
            { 
                var s = self.Invoke(state);
                if (s.IsFaulted) return s.CastError<B>();
                return f(s.Value).Invoke(env, s.State);
            });
        }

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public IsotopeAsync<B> Bind<B>(Func<A, IsotopeAsync<B>> f)
        {
            var self = this;   
            return new IsotopeAsync<B>(async state => 
            { 
                var s = self.Invoke(state);
                if (s.IsFaulted) return s.CastError<B>();
                return await f(s.Value).Invoke(s.State);
            });
        }

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public IsotopeAsync<Env, B> Bind<Env, B>(Func<A, IsotopeAsync<Env, B>> f)
        {
            var self = this;   
            return new IsotopeAsync<Env, B>(async (env, state) => 
            { 
                var s = self.Invoke(state);
                if (s.IsFaulted) return s.CastError<B>();
                return await f(s.Value).Invoke(env, s.State);
            });
        }

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public Isotope<B> SelectMany<B>(Func<A, Isotope<B>> f) =>
            Bind(f);

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public Isotope<Env, B> SelectMany<Env, B>(Func<A, Isotope<Env, B>> f) =>
            Bind(f);

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public IsotopeAsync<B> SelectMany<B>(Func<A, IsotopeAsync<B>> f) =>
            Bind(f);

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public IsotopeAsync<Env, B> SelectMany<Env, B>(Func<A, IsotopeAsync<Env, B>> f) =>
            Bind(f);

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public Isotope<C> SelectMany<B, C>(Func<A, Isotope<B>> bind, Func<A, B, C> project) =>
            Bind(a => bind(a).Map(b => project(a, b)));

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public Isotope<Env, C> SelectMany<Env, B, C>(Func<A, Isotope<Env, B>> bind, Func<A, B, C> project) =>
            Bind(a => bind(a).Map(b => project(a, b)));

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public IsotopeAsync<C> SelectMany<B, C>(Func<A, IsotopeAsync<B>> bind, Func<A, B, C> project) =>
            Bind(a => bind(a).Map(b => project(a, b)));

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public IsotopeAsync<Env, C> SelectMany<Env, B, C>(Func<A, IsotopeAsync<Env, B>> bind, Func<A, B, C> project) =>
            Bind(a => bind(a).Map(b => project(a, b)));

        /// <summary>
        /// Functor map
        /// </summary>
        public Isotope<B> Map<B>(Func<A, B> f)
        {
            var self = this;   
            return new Isotope<B>(state => 
            {
                var s = self.Invoke(state);
                if (s.IsFaulted) return s.CastError<B>();
                return new IsotopeState<B>(f(s.Value), s.State);
            });
        }

        /// <summary>
        /// Functor map
        /// </summary>
        public Isotope<B> Select<B>(Func<A, B> f) =>
            Map(f);

        /// <summary>
        /// Map the alternative value (errors)
        /// </summary>
        /// <param name="f">Mapping function</param>
        /// <returns>Mapped isotope computation</returns>
        public Isotope<A> MapFail(Func<Seq<Error>, Seq<Error>> f)
        {
            var self = this;
            return new Isotope<A>(
                s => {
                    var r = self.Invoke(s);
                    return r.IsFaulted
                               ? new IsotopeState<A>(default, r.State.With(Error: f(r.State.Error)))
                               : r;
                });
        }

        /// <summary>
        /// Map both sides of the isotope (success and failure)
        /// </summary>
        /// <param name="Succ">Success mapping function</param>
        /// <param name="Fail">Failure mapping function</param>
        /// <returns>Mapped isotope computation</returns>
        public Isotope<B> BiMap<B>(Func<A, B> Succ,  Func<Seq<Error>, Seq<Error>> Fail)
        {
            var self = this;
            return new Isotope<B>(
                s => {
                    var r = self.Invoke(s);
                    return r.IsFaulted
                               ? new IsotopeState<B>(default, r.State.With(Error: Fail(r.State.Error)))
                               : new IsotopeState<B>(Succ(r.Value), r.State);
                });
        }
        
        /// <summary>
        /// Map the alternative value (errors)
        /// </summary>
        /// <param name="f">Mapping function</param>
        /// <returns>Mapped isotope computation</returns>
        public Isotope<A> MapFail(Func<Seq<Error>, Error> f) =>
            MapFail(s => Seq1(f(s)));

        /// <summary>
        /// Map both sides of the isotope (success and failure)
        /// </summary>
        /// <param name="Succ">Success mapping function</param>
        /// <param name="Fail">Failure mapping function</param>
        /// <returns>Mapped isotope computation</returns>
        public Isotope<B> BiMap<B>(Func<A, B> Succ, Func<Seq<Error>, Error> Fail) =>
            BiMap(Succ, s => Seq1(Fail(s)));
    }

    /// <summary>
    /// Isotope computation with an environment
    /// </summary>
    /// <typeparam name="Env">Environment</typeparam>
    /// <typeparam name="A">Bound value</typeparam>
    public struct Isotope<Env, A>
    {
        readonly Func<Env, IsotopeState, IsotopeState<A>> Thunk;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="thunk">Thunk</param>
        public Isotope(Func<Env, IsotopeState, IsotopeState<A>> thunk) =>
            Thunk = thunk;

        /// <summary>
        /// Invoke the computation
        /// </summary>
        /// <param name="env">Environment</param>
        /// <param name="state">State</param>
        /// <returns>Result of invoking the isotope computation</returns>
        internal IsotopeState<A> Invoke(Env env, IsotopeState state)
        {
            try
            {
                return Thunk?.Invoke(env, state) ?? throw new InvalidOperationException("Isotope computation not initialised");
            }
            catch (Exception e)
            {
                return new IsotopeState<A>(default, state.AddError(Error.New(e)));
            }
        }

        /// <summary>
        /// Invoke the test computation
        /// </summary>
        /// <param name="env">Environment</param>
        /// <param name="settings">Optional settings</param>
        /// <returns>Returning an optional error. 
        /// The computation succeeds if result.IsNone is true</returns>
        public (IsotopeState state, A value) Run(Env env, IsotopeSettings settings = null)
        {
            var res = Invoke(env, IsotopeState.Empty.With(Settings: settings));
            if (res.State.Settings.DisposeOnCompletion)
            {
                res.State.DisposeWebDriver();
            }

            return (res.State, res.Value);
        }

        /// <summary>
        /// Invoke the test computation
        /// </summary>
        /// <param name="env">Environment</param>
        /// <param name="settings">Optional settings</param>
        /// <returns>Returning an optional error. 
        /// The computation succeeds if result.IsNone is true</returns>
        public (IsotopeState state, A value) RunAndThrowOnError(Env env, IsotopeSettings settings = null)
        {
            var (state, value) = Run(env, settings);
            state.IfFailedThrow();
            return (state, value);
        }

        /// <summary>
        /// Invoke the test computation
        /// </summary>
        /// <param name="env">Environment</param>
        /// <param name="driver">Web driver</param>
        /// <param name="settings">Optional settings</param>
        /// <returns>Returning an optional error. 
        /// The computation succeeds if result.IsNone is true</returns>
        public (IsotopeState state, A value) Run(Env env, IWebDriver driver, IsotopeSettings settings = null)
        {
            var res = Invoke(env, IsotopeState.Empty.With(Driver: Some(driver), Settings: settings));
            if (res.State.Settings.DisposeOnCompletion)
            {
                res.State.DisposeWebDriver();
            }

            return (res.State, res.Value);
        }

        /// <summary>
        /// Invoke the test computation
        /// </summary>
        /// <param name="env">Environment</param>
        /// <param name="driver">Web driver</param>
        /// <param name="settings">Optional settings</param>
        /// <returns>Returning an optional error. 
        /// The computation succeeds if result.IsNone is true</returns>
        public (IsotopeState state, A value) RunAndThrowOnError(Env env, IWebDriver driver, IsotopeSettings settings = null)
        {
            var (state, value) = Run(env, driver, settings);
            state.IfFailedThrow();
            return (state, value);
        }

        /// <summary>
        /// Implicit conversion 
        /// </summary>
        public static implicit operator Isotope<Env, A>(Isotope<A> ma) =>
            new Isotope<Env, A>((_, state) => ma.Invoke(state));

        /// <summary>
        /// Or operator - evaluates the left hand side, if it fails, it ignores the error and evaluates the right hand side
        /// </summary>
        public static Isotope<Env, A> operator |(Isotope<Env, A> lhs, Isotope<Env, A> rhs) =>
            new Isotope<Env, A>((e, s) => {
                                    var l = lhs.Invoke(e, s);
                                    return l.IsFaulted
                                               ? rhs.Invoke(e, s)
                                               : l;
                                });

        /// <summary>
        /// Lift the pure value into the monadic space 
        /// </summary>
        public static Isotope<Env, A> Pure(A value) =>
            new Isotope<Env, A>((e, s) => new IsotopeState<A>(value, s));

        /// <summary>
        /// Lift the error into the monadic space 
        /// </summary>
        public static Isotope<Env, A> Fail(Error error) =>
            new Isotope<Env, A>((e, s) => new IsotopeState<A>(default(A), s.AddError(error)));

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public Isotope<Env, B> Bind<B>(Func<A, Isotope<B>> f)
        {
            var self = this;   
            return new Isotope<Env, B>((env, state) => 
            { 
                var s = self.Invoke(env, state);
                if (s.IsFaulted) return s.CastError<B>();
                return f(s.Value).Invoke(s.State);
            });
        }

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public Isotope<Env, B> Bind<B>(Func<A, Isotope<Env, B>> f)
        {
            var self = this;   
            return new Isotope<Env, B>((env, state) => 
            { 
                var s = self.Invoke(env, state);
                if (s.IsFaulted) return s.CastError<B>();
                return f(s.Value).Invoke(env, s.State);
            });
        }

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public IsotopeAsync<Env, B> Bind<B>(Func<A, IsotopeAsync<B>> f)
        {
            var self = this;   
            return new IsotopeAsync<Env, B>(async (env, state) => 
            { 
                var s = self.Invoke(env, state);
                if (s.IsFaulted) return s.CastError<B>();
                return await f(s.Value).Invoke(s.State);
            });
        }

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public IsotopeAsync<Env, B> Bind<B>(Func<A, IsotopeAsync<Env, B>> f)
        {
            var self = this;   
            return new IsotopeAsync<Env, B>(async (env, state) => 
            { 
                var s = self.Invoke(env, state);
                if (s.IsFaulted) return s.CastError<B>();
                return await f(s.Value).Invoke(env, s.State);
            });
        }

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public Isotope<Env, B> SelectMany<B>(Func<A, Isotope<B>> f) =>
            Bind(f);

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public Isotope<Env, B> SelectMany<B>(Func<A, Isotope<Env, B>> f) =>
            Bind(f);

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public IsotopeAsync<Env, B> SelectMany<B>(Func<A, IsotopeAsync<B>> f) =>
            Bind(f);

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public IsotopeAsync<Env, B> SelectMany<B>(Func<A, IsotopeAsync<Env, B>> f) =>
            Bind(f);

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public Isotope<Env, C> SelectMany<B, C>(Func<A, Isotope<B>> bind, Func<A, B, C> project) =>
            Bind(a => bind(a).Map(b => project(a, b)));

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public Isotope<Env, C> SelectMany<B, C>(Func<A, Isotope<Env, B>> bind, Func<A, B, C> project) =>
            Bind(a => bind(a).Map(b => project(a, b)));

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public IsotopeAsync<Env, C> SelectMany<B, C>(Func<A, IsotopeAsync<B>> bind, Func<A, B, C> project) =>
            Bind(a => bind(a).Map(b => project(a, b)));

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public IsotopeAsync<Env, C> SelectMany<B, C>(Func<A, IsotopeAsync<Env, B>> bind, Func<A, B, C> project) =>
            Bind(a => bind(a).Map(b => project(a, b)));

        /// <summary>
        /// Functor map
        /// </summary>
        public Isotope<Env, B> Map<B>(Func<A, B> f)
        {
            var self = this;   
            return new Isotope<Env, B>((env, state) => 
            {
                var s = self.Invoke(env, state);
                if (s.IsFaulted) return s.CastError<B>();
                return new IsotopeState<B>(f(s.Value), s.State);
            });
        }

        /// <summary>
        /// Functor map
        /// </summary>
        public Isotope<Env, B> Select<B>(Func<A, B> f) =>
            Map(f);

        /// <summary>
        /// Map the alternative value (errors)
        /// </summary>
        /// <param name="f">Mapping function</param>
        /// <returns>Mapped isotope computation</returns>
        public Isotope<Env, A> MapFail(Func<Seq<Error>, Seq<Error>> f)
        {
            var self = this;
            return new Isotope<Env, A>(
                (e, s) => {
                    var r = self.Invoke(e, s);
                    return r.IsFaulted
                               ? new IsotopeState<A>(default, r.State.With(Error: f(r.State.Error)))
                               : r;
                });
        }

        /// <summary>
        /// Map both sides of the isotope (success and failure)
        /// </summary>
        /// <param name="Succ">Success mapping function</param>
        /// <param name="Fail">Failure mapping function</param>
        /// <returns>Mapped isotope computation</returns>
        public Isotope<Env, B> BiMap<B>(Func<A, B> Succ,  Func<Seq<Error>, Seq<Error>> Fail)
        {
            var self = this;
            return new Isotope<Env, B>(
                (e, s) => {
                    var r = self.Invoke(e, s);
                    return r.IsFaulted
                               ? new IsotopeState<B>(default, r.State.With(Error: Fail(r.State.Error)))
                               : new IsotopeState<B>(Succ(r.Value), r.State);
                });
        }    
        
        /// <summary>
        /// Map the alternative value (errors)
        /// </summary>
        /// <param name="f">Mapping function</param>
        /// <returns>Mapped isotope computation</returns>
        public Isotope<Env, A> MapFail(Func<Seq<Error>, Error> f) =>
            MapFail(s => Seq1(f(s)));

        /// <summary>
        /// Map both sides of the isotope (success and failure)
        /// </summary>
        /// <param name="Succ">Success mapping function</param>
        /// <param name="Fail">Failure mapping function</param>
        /// <returns>Mapped isotope computation</returns>
        public Isotope<Env, B> BiMap<B>(Func<A, B> Succ, Func<Seq<Error>, Error> Fail) =>
            BiMap(Succ, s => Seq1(Fail(s)));
    }

    /// <summary>
    /// Asynchronous, environment-free, isotope computation
    /// </summary>
    /// <typeparam name="A">Bound value</typeparam>
    public struct IsotopeAsync<A>
    {
        readonly Func<IsotopeState, ValueTask<IsotopeState<A>>> Thunk;
        
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="thunk">Thunk</param>
        public IsotopeAsync(Func<IsotopeState, ValueTask<IsotopeState<A>>> thunk) =>
            Thunk = thunk;

        /// <summary>
        /// Invoke the computation
        /// </summary>
        /// <param name="state">State</param>
        /// <returns>Result of invoking the isotope computation</returns>
        internal async ValueTask<IsotopeState<A>> Invoke(IsotopeState state)
        {
            try
            {
                if (Thunk == null)
                {
                    throw new InvalidOperationException("Isotope computation not initialised");
                }
                return await Thunk(state);
            }
            catch (Exception e)
            {
                return new IsotopeState<A>(default, state.AddError(Error.New(e)));
            }
        }

        /// <summary>
        /// Invoke the test computation
        /// </summary>
        /// <param name="settings">Optional settings</param>
        /// <returns>Returning an optional error. 
        /// The computation succeeds if result.IsNone is true</returns>
        public async ValueTask<(IsotopeState state, A value)> Run(IsotopeSettings settings = null)
        {
            var res = await Invoke(IsotopeState.Empty.With(Settings: settings));
            if (res.State.Settings.DisposeOnCompletion)
            {
                res.State.DisposeWebDriver();
            }
            return(res.State, res.Value);
        }
        
        /// <summary>
        /// Invoke the test computation
        /// </summary>
        /// <param name="settings">Optional settings</param>
        /// <returns>Returning an optional error. 
        /// The computation succeeds if result.IsNone is true</returns>
        public async ValueTask<(IsotopeState state, A value)> RunAndThrowOnError(IsotopeSettings settings = null)
        {
            var (state, value) = await Run(settings);
            state.IfFailedThrow();
            return (state, value);
        }
        
        /// <summary>
        /// Invoke the test computation
        /// </summary>
        /// <param name="driver">Web driver</param>
        /// <param name="settings">Optional settings</param>
        /// <returns>Returning an optional error. 
        /// The computation succeeds if result.IsNone is true</returns>
        public async ValueTask<(IsotopeState state, A value)> Run(IWebDriver driver, IsotopeSettings settings = null)
        {
            var res = await Invoke(IsotopeState.Empty.With(Driver: Some(driver), Settings: settings));
            if (res.State.Settings.DisposeOnCompletion)
            {
                res.State.DisposeWebDriver();
            }
            return (res.State, res.Value);
        }  
        
        /// <summary>
        /// Invoke the test computation
        /// </summary>
        /// <param name="driver">Web driver</param>
        /// <param name="settings">Optional settings</param>
        /// <returns>Returning an optional error. 
        /// The computation succeeds if result.IsNone is true</returns>
        public async ValueTask<(IsotopeState state, A value)> RunAndThrowOnError(IWebDriver driver, IsotopeSettings settings = null)
        {
            var (state, value) = await Run(driver, settings);
            state.IfFailedThrow();
            return (state, value);
        }

        /// <summary>
        /// Implicit conversion 
        /// </summary>
        public static implicit operator IsotopeAsync<A>(Isotope<A> ma) =>
            new IsotopeAsync<A>(state => new ValueTask<IsotopeState<A>>(ma.Invoke(state)));
        
        /// <summary>
        /// Or operator - evaluates the left hand side, if it fails, it ignores the error and evaluates the right hand side
        /// </summary>
        public static IsotopeAsync<A> operator |(IsotopeAsync<A> lhs, IsotopeAsync<A> rhs) => 
            new IsotopeAsync<A>(async s =>
            {
                var l = await lhs.Invoke(s);
                return l.IsFaulted
                           ? await rhs.Invoke(s)
                           : l;
            });

        /// <summary>
        /// Lift the pure value into the monadic space 
        /// </summary>
        public static IsotopeAsync<A> Pure(A value) =>
            new IsotopeAsync<A>(s => new ValueTask<IsotopeState<A>>(new IsotopeState<A>(value, s)));
        
        /// <summary>
        /// Lift the error into the monadic space 
        /// </summary>
        public static IsotopeAsync<A> Fail(Error error) =>
            new IsotopeAsync<A>(s => new ValueTask<IsotopeState<A>>(new IsotopeState<A>(default(A), s.AddError(error))));

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public IsotopeAsync<B> Bind<B>(Func<A, Isotope<B>> f)
        {
            var self = this;   
            return new IsotopeAsync<B>(async state => 
            { 
                var s = await self.Invoke(state);
                if (s.IsFaulted) return s.CastError<B>();
                return f(s.Value).Invoke(s.State);
            });
        }

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public IsotopeAsync<Env, B> Bind<Env, B>(Func<A, Isotope<Env, B>> f)
        {
            var self = this;   
            return new IsotopeAsync<Env, B>(async (env, state) => 
            { 
                var s = await self.Invoke(state);
                if (s.IsFaulted) return s.CastError<B>();
                return f(s.Value).Invoke(env, s.State);
            });
        }

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public IsotopeAsync<B> Bind<B>(Func<A, IsotopeAsync<B>> f)
        {
            var self = this;   
            return new IsotopeAsync<B>(async state => 
            { 
                var s = await self.Invoke(state);
                if (s.IsFaulted) return s.CastError<B>();
                return await f(s.Value).Invoke(s.State);
            });
        }

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public IsotopeAsync<Env, B> Bind<Env, B>(Func<A, IsotopeAsync<Env, B>> f)
        {
            var self = this;   
            return new IsotopeAsync<Env, B>(async (env, state) => 
            { 
                var s = await self.Invoke(state);
                if (s.IsFaulted) return s.CastError<B>();
                return await f(s.Value).Invoke(env, s.State);
            });
        }

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public IsotopeAsync<B> SelectMany<B>(Func<A, Isotope<B>> f) =>
            Bind(f);

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public IsotopeAsync<Env, B> SelectMany<Env, B>(Func<A, Isotope<Env, B>> f) =>
            Bind(f);

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public IsotopeAsync<B> SelectMany<B>(Func<A, IsotopeAsync<B>> f) =>
            Bind(f);

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public IsotopeAsync<Env, B> SelectMany<Env, B>(Func<A, IsotopeAsync<Env, B>> f) =>
            Bind(f);

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public IsotopeAsync<C> SelectMany<B, C>(Func<A, Isotope<B>> bind, Func<A, B, C> project) =>
            Bind(a => bind(a).Map(b => project(a, b)));

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public IsotopeAsync<Env, C> SelectMany<Env, B, C>(Func<A, Isotope<Env, B>> bind, Func<A, B, C> project) =>
            Bind(a => bind(a).Map(b => project(a, b)));

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public IsotopeAsync<C> SelectMany<B, C>(Func<A, IsotopeAsync<B>> bind, Func<A, B, C> project) =>
            Bind(a => bind(a).Map(b => project(a, b)));

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public IsotopeAsync<Env, C> SelectMany<Env, B, C>(Func<A, IsotopeAsync<Env, B>> bind, Func<A, B, C> project) =>
            Bind(a => bind(a).Map(b => project(a, b)));

        /// <summary>
        /// Functor map
        /// </summary>
        public IsotopeAsync<B> Map<B>(Func<A, B> f)
        {
            var self = this;   
            return new IsotopeAsync<B>(async state => 
            {
                var s = await self.Invoke(state);
                if (s.IsFaulted) return s.CastError<B>();
                return new IsotopeState<B>(f(s.Value), s.State);
            });
        }

        /// <summary>
        /// Functor map
        /// </summary>
        public IsotopeAsync<B> Select<B>(Func<A, B> f) =>
            Map(f);        
    
        /// <summary>
        /// Map the alternative value (errors)
        /// </summary>
        /// <param name="f">Mapping function</param>
        /// <returns>Mapped isotope computation</returns>
        public IsotopeAsync<A> MapFail(Func<Seq<Error>, Seq<Error>> f)
        {
            var self = this;
            return new IsotopeAsync<A>(
                async s => {
                    var r = await self.Invoke(s);
                    return r.IsFaulted
                               ? new IsotopeState<A>(default, r.State.With(Error: f(r.State.Error)))
                               : r;
                });
        }

        /// <summary>
        /// Map both sides of the isotope (success and failure)
        /// </summary>
        /// <param name="Succ">Success mapping function</param>
        /// <param name="Fail">Failure mapping function</param>
        /// <returns>Mapped isotope computation</returns>
        public IsotopeAsync<B> BiMap<B>(Func<A, B> Succ,  Func<Seq<Error>, Seq<Error>> Fail)
        {
            var self = this;
            return new IsotopeAsync<B>(
                async s => {
                    var r = await self.Invoke(s);
                    return r.IsFaulted
                               ? new IsotopeState<B>(default, r.State.With(Error: Fail(r.State.Error)))
                               : new IsotopeState<B>(Succ(r.Value), r.State);
                });
        }
        
        /// <summary>
        /// Map the alternative value (errors)
        /// </summary>
        /// <param name="f">Mapping function</param>
        /// <returns>Mapped isotope computation</returns>
        public IsotopeAsync<A> MapFail(Func<Seq<Error>, Error> f) =>
            MapFail(s => Seq1(f(s)));

        /// <summary>
        /// Map both sides of the isotope (success and failure)
        /// </summary>
        /// <param name="Succ">Success mapping function</param>
        /// <param name="Fail">Failure mapping function</param>
        /// <returns>Mapped isotope computation</returns>
        public IsotopeAsync<B> BiMap<B>(Func<A, B> Succ, Func<Seq<Error>, Error> Fail) =>
            BiMap(Succ, s => Seq1(Fail(s)));
    }

    /// <summary>
    /// Asynchronous isotope computation with an environment
    /// </summary>
    /// <typeparam name="Env">Environment</typeparam>
    /// <typeparam name="A">Bound value</typeparam>
    public struct IsotopeAsync<Env, A>
    {
        readonly Func<Env, IsotopeState, ValueTask<IsotopeState<A>>> Thunk;
        
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="thunk">Thunk</param>
        public IsotopeAsync(Func<Env, IsotopeState, ValueTask<IsotopeState<A>>> thunk) =>
            Thunk = thunk;

        /// <summary>
        /// Invoke the computation
        /// </summary>
        /// <param name="env">Environment</param>
        /// <param name="state">State</param>
        /// <returns>Result of invoking the isotope computation</returns>
        internal async ValueTask<IsotopeState<A>> Invoke(Env env, IsotopeState state)
        {
            try
            {
                if (Thunk == null)
                {
                    throw new InvalidOperationException("Isotope computation not initialised");
                }
                return await Thunk(env, state);
            }
            catch (Exception e)
            {
                return new IsotopeState<A>(default, state.AddError(Error.New(e)));
            }
        }

        /// <summary>
        /// Invoke the test computation
        /// </summary>
        /// <param name="env">Environment</param>
        /// <param name="settings">Optional settings</param>
        /// <returns>Returning an optional error. 
        /// The computation succeeds if result.IsNone is true</returns>
        public async ValueTask<(IsotopeState state, A value)> Run(Env env, IsotopeSettings settings = null)
        {
            var res = await Invoke(env, IsotopeState.Empty.With(Settings: settings));
            if (res.State.Settings.DisposeOnCompletion)
            {
                res.State.DisposeWebDriver();
            }

            return (res.State, res.Value);
        }

        /// <summary>
        /// Invoke the test computation
        /// </summary>
        /// <param name="env">Environment</param>
        /// <param name="settings">Optional settings</param>
        /// <returns>Returning an optional error. 
        /// The computation succeeds if result.IsNone is true</returns>
        public async ValueTask<(IsotopeState state, A value)> RunAndThrowOnError(Env env, IsotopeSettings settings = null)
        {
            var (state, value) = await Run(env, settings);
            state.IfFailedThrow();
            return (state, value);
        }
        
        /// <summary>
        /// Invoke the test computation
        /// </summary>
        /// <param name="env">Environment</param>
        /// <param name="driver">Web driver</param>
        /// <param name="settings">Optional settings</param>
        /// <returns>Returning an optional error. 
        /// The computation succeeds if result.IsNone is true</returns>
        public async ValueTask<(IsotopeState state, A value)> Run(Env env, IWebDriver driver, IsotopeSettings settings = null)
        {
            var res = await Invoke(env, IsotopeState.Empty.With(Driver: Some(driver), Settings: settings));
            if (res.State.Settings.DisposeOnCompletion)
            {
                res.State.DisposeWebDriver();
            }

            return (res.State, res.Value);
        }

        /// <summary>
        /// Invoke the test computation
        /// </summary>
        /// <param name="env">Environment</param>
        /// <param name="driver">Web driver</param>
        /// <param name="settings">Optional settings</param>
        /// <returns>Returning an optional error. 
        /// The computation succeeds if result.IsNone is true</returns>
        public async ValueTask<(IsotopeState state, A value)> RunAndThrowOnError(Env env, IWebDriver driver, IsotopeSettings settings = null)
        {
            var (state, value) = await Run(env, driver, settings);
            state.IfFailedThrow();
            return (state, value);
        }

        /// <summary>
        /// Implicit conversion 
        /// </summary>
        public static implicit operator IsotopeAsync<Env, A>(Isotope<A> ma) =>
            new IsotopeAsync<Env, A>((_, state) => new ValueTask<IsotopeState<A>>(ma.Invoke(state)));

        /// <summary>
        /// Implicit conversion 
        /// </summary>
        public static implicit operator IsotopeAsync<Env, A>(Isotope<Env, A> ma) =>
            new IsotopeAsync<Env, A>((env, state) => new ValueTask<IsotopeState<A>>(ma.Invoke(env, state)));

        /// <summary>
        /// Implicit conversion 
        /// </summary>
        public static implicit operator IsotopeAsync<Env, A>(IsotopeAsync<A> ma) =>
            new IsotopeAsync<Env, A>((_, state) => ma.Invoke(state));

        /// <summary>
        /// Or operator - evaluates the left hand side, if it fails, it ignores the error and evaluates the right hand side
        /// </summary>
        public static IsotopeAsync<Env, A> operator |(IsotopeAsync<Env, A> lhs, IsotopeAsync<Env, A> rhs) =>
            new IsotopeAsync<Env, A>(async (e, s) => {
                                         var l = await lhs.Invoke(e, s);
                                         return l.IsFaulted
                                                    ? await rhs.Invoke(e, s)
                                                    : l;
                                     });

        /// <summary>
        /// Lift the pure value into the monadic space 
        /// </summary>
        public static IsotopeAsync<Env, A> Pure(A value) =>
            new IsotopeAsync<Env, A>((e, s) => new ValueTask<IsotopeState<A>>(new IsotopeState<A>(value, s)));

        /// <summary>
        /// Lift the error into the monadic space 
        /// </summary>
        public static IsotopeAsync<Env, A> Fail(Error error) =>
            new IsotopeAsync<Env, A>((e, s) => new ValueTask<IsotopeState<A>>(new IsotopeState<A>(default(A), s.AddError(error))));

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public IsotopeAsync<Env, B> Bind<B>(Func<A, Isotope<B>> f)
        {
            var self = this;   
            return new IsotopeAsync<Env, B>( async (env, state) => 
            { 
                var s = await self.Invoke(env, state);
                if (s.IsFaulted) return s.CastError<B>();
                return f(s.Value).Invoke(s.State);
            });
        }

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public IsotopeAsync<Env, B> Bind<B>(Func<A, Isotope<Env, B>> f)
        {
            var self = this;   
            return new IsotopeAsync<Env, B>( async (env, state) => 
            { 
                var s = await self.Invoke(env, state);
                if (s.IsFaulted) return s.CastError<B>();
                return f(s.Value).Invoke(env, s.State);
            });
        }

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public IsotopeAsync<Env, B> Bind<B>(Func<A, IsotopeAsync<B>> f)
        {
            var self = this;   
            return new IsotopeAsync<Env, B>(async (env, state) => 
            { 
                var s = await self.Invoke(env, state);
                if (s.IsFaulted) return s.CastError<B>();
                return await f(s.Value).Invoke(s.State);
            });
        }

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public IsotopeAsync<Env, B> Bind<B>(Func<A, IsotopeAsync<Env, B>> f)
        {
            var self = this;   
            return new IsotopeAsync<Env, B>(async (env, state) => 
            { 
                var s = await self.Invoke(env, state);
                if (s.IsFaulted) return s.CastError<B>();
                return await f(s.Value).Invoke(env, s.State);
            });
        }

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public IsotopeAsync<Env, B> SelectMany<B>(Func<A, Isotope<B>> f) =>
            Bind(f);

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public IsotopeAsync<Env, B> SelectMany<B>(Func<A, Isotope<Env, B>> f) =>
            Bind(f);

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public IsotopeAsync<Env, B> SelectMany<B>(Func<A, IsotopeAsync<B>> f) =>
            Bind(f);

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public IsotopeAsync<Env, B> SelectMany<B>(Func<A, IsotopeAsync<Env, B>> f) =>
            Bind(f);

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public IsotopeAsync<Env, C> SelectMany<B, C>(Func<A, Isotope<B>> bind, Func<A, B, C> project) =>
            Bind(a => bind(a).Map(b => project(a, b)));

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public IsotopeAsync<Env, C> SelectMany<B, C>(Func<A, Isotope<Env, B>> bind, Func<A, B, C> project) =>
            Bind(a => bind(a).Map(b => project(a, b)));

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public IsotopeAsync<Env, C> SelectMany<B, C>(Func<A, IsotopeAsync<B>> bind, Func<A, B, C> project) =>
            Bind(a => bind(a).Map(b => project(a, b)));

        /// <summary>
        /// Monadic bind 
        /// </summary>
        public IsotopeAsync<Env, C> SelectMany<B, C>(Func<A, IsotopeAsync<Env, B>> bind, Func<A, B, C> project) =>
            Bind(a => bind(a).Map(b => project(a, b)));

        /// <summary>
        /// Functor map
        /// </summary>
        public IsotopeAsync<Env, B> Map<B>(Func<A, B> f)
        {
            var self = this;   
            return new IsotopeAsync<Env, B>( async (env, state) => 
            {
                var s = await self.Invoke(env, state);
                if (s.IsFaulted) return s.CastError<B>();
                return new IsotopeState<B>(f(s.Value), s.State);
            });
        }

        /// <summary>
        /// Functor map
        /// </summary>
        public IsotopeAsync<Env, B> Select<B>(Func<A, B> f) =>
            Map(f);
        /// <summary>
        /// Map the alternative value (errors)
        /// </summary>
        /// <param name="f">Mapping function</param>
        /// <returns>Mapped isotope computation</returns>
        public IsotopeAsync<Env, A> MapFail(Func<Seq<Error>, Seq<Error>> f)
        {
            var self = this;
            return new IsotopeAsync<Env, A>(
                async (e, s) => {
                    var r = await self.Invoke(e, s);
                    return r.IsFaulted
                               ? new IsotopeState<A>(default, r.State.With(Error: f(r.State.Error)))
                               : r;
                });
        }

        /// <summary>
        /// Map both sides of the isotope (success and failure)
        /// </summary>
        /// <param name="Succ">Success mapping function</param>
        /// <param name="Fail">Failure mapping function</param>
        /// <returns>Mapped isotope computation</returns>
        public IsotopeAsync<Env, B> BiMap<B>(Func<A, B> Succ,  Func<Seq<Error>, Seq<Error>> Fail)
        {
            var self = this;
            return new IsotopeAsync<Env, B>(
                async (e, s) => {
                    var r = await self.Invoke(e, s);
                    return r.IsFaulted
                               ? new IsotopeState<B>(default, r.State.With(Error: Fail(r.State.Error)))
                               : new IsotopeState<B>(Succ(r.Value), r.State);
                });
        }
        
        /// <summary>
        /// Map the alternative value (errors)
        /// </summary>
        /// <param name="f">Mapping function</param>
        /// <returns>Mapped isotope computation</returns>
        public IsotopeAsync<Env, A> MapFail(Func<Seq<Error>, Error> f) =>
            MapFail(s => Seq1(f(s)));

        /// <summary>
        /// Map both sides of the isotope (success and failure)
        /// </summary>
        /// <param name="Succ">Success mapping function</param>
        /// <param name="Fail">Failure mapping function</param>
        /// <returns>Mapped isotope computation</returns>
        public IsotopeAsync<Env, B> BiMap<B>(Func<A, B> Succ, Func<Seq<Error>, Error> Fail) =>
            BiMap(Succ, s => Seq1(Fail(s)));
    }

    /// <summary>
    /// Isotope extensions
    /// </summary>
    public static partial class Isotope
    {
        /// <summary>
        /// Simple configuration setup
        /// </summary>
        /// <param name="config">Map of config items</param>
        public static Isotope<Unit> initConfig(params (string, string)[] config) =>
            initConfig(toMap(config));

        /// <summary>
        /// Simple configuration setup
        /// </summary>
        /// <param name="config">Map of config items</param>
        public static Isotope<Unit> initConfig(Map<string, string> config) =>
            from s in get
            from _ in put(s.With(Configuration: config))
            select unit;

        /// <summary>
        /// Get a config key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Isotope<string> config(string key) =>
            from s in get
            from r in s.Configuration.Find(key).ToIsotope($"Configuration key not found: {key}")
            select r;

        public static Isotope<Unit> initSettings(IsotopeSettings settings) =>
            from s in get
            from _ in put(s.With(Settings: settings))
            select unit;

        public static Isotope<Unit> setWindowSize(int width, int height) =>
            setWindowSize(new Size(width, height));

        public static Isotope<Unit> setWindowSize(Size size) =>
            from d in webDriver
            from _ in trya(() => d.Manage().Window.Size = size, $"Failed to change browser window size to {size.Width} by {size.Height}")
            select unit;

        /// <summary>
        /// Navigate to a URL
        /// </summary>
        /// <param name="url">URL to navigate to</param>
        public static Isotope<Unit> nav(string url) =>
            from d in webDriver
            from _ in trya(() => d.Navigate().GoToUrl(url), $"Failed to navigate to: {url}")
            select unit;

        /// <summary>
        /// Gets the URL currently displayed by the browser
        /// </summary>
        public static Isotope<string> url =>
            from d in webDriver
            select d.Url;

        /// <summary>
        /// Find an HTML element
        /// </summary>
        /// <param name="selector">CSS selector</param>
        public static Isotope<IWebElement> findElement(By selector, bool wait = true, string errorMessage = "Unable to find element") =>
            from d in webDriver
            from e in wait ? waitUntilElementExists(selector) : fail<IWebElement>(errorMessage)
            select e;

        /// <summary>
        /// Find an HTML element
        /// </summary>
        /// <param name="selector">Selector</param>
        public static Isotope<Option<IWebElement>> findOptionalElement(IWebElement element, By selector, string errorMessage = null) =>
            from es in findElementsOrEmpty(element, selector, errorMessage)
            from e in pure(es.HeadOrNone())
            select e;

        public static Isotope<Option<IWebElement>> findOptionalElement(By selector, string errorMessage = null) =>
            from es in findElementsOrEmpty(selector, errorMessage)
            from e in pure(es.HeadOrNone())
            select e;

        /// <summary>
        /// Find an HTML element
        /// </summary>
        /// <param name="selector">CSS selector</param>
        public static Isotope<IWebElement> findElement(
            IWebElement element, 
            By selector, 
            bool wait = true, 
            string errorMessage = null) =>
            from d in webDriver
            from e in wait 
                      ? waitUntilElementExists(element, selector)
                      : findChildElement(element, selector) 
            select e;

        private static Isotope<IWebElement> findChildElement(
            IWebElement parent,
            By selector,
            string errorMessage = null) =>
            tryf(() => parent.FindElement(selector),
                 errorMessage ?? $"Can't find element {selector}");

        /// <summary>
        /// Find HTML elements
        /// </summary>
        /// <param name="selector">Selector</param>
        public static Isotope<Seq<IWebElement>> findElements(By selector, bool wait = true, string error = null) =>
            wait ? waitUntilElementsExists(selector)
                 : from d in webDriver
                   from es in tryf(() => d.FindElements(selector).ToSeq(),
                                   error ?? $"Can't find any elements {selector}")
                   select es;

        /// <summary>
        /// Find HTML elements within an element
        /// </summary>
        /// <param name="parent">Element to search within</param>
        /// <param name="selector">Selector</param>
        /// <param name="wait">If none are found wait and retry</param>
        /// <param name="error">Custom error message</param>
        /// <returns>Matching elements</returns>
        public static Isotope<Seq<IWebElement>> findElements(IWebElement parent, By selector, bool wait = true, string error = null) =>
            wait ? waitUntilElementsExists(parent, selector)
                 : Try(() => parent.FindElements(selector).ToSeq()).
                    Match(
                     Succ: x => x.IsEmpty ? fail<Seq<IWebElement>>(error ?? $"Can't find any elements {selector}")
                                          : pure(x),
                     Fail: e => fail<Seq<IWebElement>>(error ?? $"Can't find any elements {selector}"));

        /// <summary>
        /// Find a sequence of elements matching a selector
        /// </summary>
        /// <param name="selector">Web Driver selector</param>
        /// <param name="error"></param>
        /// <returns>Sequence of matching elements</returns>
        public static Isotope<Seq<IWebElement>> findElementsOrEmpty(By selector, string error = null) =>
            from d in webDriver
            from e in tryf(() => d.FindElements(selector).ToSeq(), error ?? $"Can't find any elements {selector}")
            select e;

        /// <summary>
        /// Find a sequence of elements within an existing element matching a selector
        /// </summary>
        /// <param name="parent">Parent element</param>
        /// <param name="selector">Web Driver selector</param>
        /// <param name="error"></param>
        /// <returns>Sequence of matching elements</returns>
        public static Isotope<Seq<IWebElement>> findElementsOrEmpty(IWebElement parent, By selector, string error = null) =>
            from e in tryf(() => parent.FindElements(selector).ToSeq(), error ?? $"Can't find any elements {selector}")
            select e;

        /// <summary>
        /// Find a &lt;select&gt; element within an existing element
        /// </summary>  
        public static Isotope<SelectElement> findSelectElement(IWebElement container, By selector) =>
            from el in findElement(container, selector)
            from se in toSelectElement(el)
            select se;

        /// <summary>
        /// Find a &lt;select&gt; element
        /// </summary>        
        public static Isotope<SelectElement> findSelectElement(By selector) =>
            from el in findElement(selector)
            from se in toSelectElement(el)
            select se;

        /// <summary>
        /// Convert an IWebElement to a SelectElement
        /// </summary>  
        public static Isotope<SelectElement> toSelectElement(IWebElement element) =>
            tryf(() => new SelectElement(element), x => "Problem creating select element: " + x.Message);

        /// <summary>
        /// Select a &lt;select&gt; option by text
        /// </summary>     
        public static Isotope<Unit> selectByText(By selector, string text) =>
            from se in findSelectElement(selector)
            from _  in selectByText(se, text)
            select unit;

        /// <summary>
        /// Select a &lt;select&gt; option by text
        /// </summary>        
        public static Isotope<Unit> selectByText(SelectElement select, string text) =>
            trya(() => select.SelectByText(text), x => "Unable to select" + x.Message);

        /// <summary>
        /// Select a &lt;select&gt; option by value
        /// </summary>     
        public static Isotope<Unit> selectByValue(By selector, string value) =>
            from se in findSelectElement(selector)
            from _  in selectByValue(se, value)
            select unit;

        /// <summary>
        /// Select a &lt;select&gt; option by value
        /// </summary>        
        public static Isotope<Unit> selectByValue(SelectElement select, string value) =>
            trya(() => select.SelectByValue(value), x => "Unable to select" + x.Message);

        /// <summary>
        /// Retrieves the selected option element in a Select Element
        /// </summary>
        /// <param name="sel">Web Driver Select Element</param>
        /// <returns>The selected Option Web Element</returns>
        public static Isotope<IWebElement> getSelectedOption(SelectElement sel) =>
            tryf(() => sel.SelectedOption, x => "Unable to get selected option" + x.Message);

        /// <summary>
        /// Retrieves the text for the selected option element in a Select Element
        /// </summary>
        /// <param name="sel">Web Driver Select Element</param>
        /// <returns>The selected Option Web Element</returns>
        public static Isotope<string> getSelectedOptionText(SelectElement sel) =>
            from opt in getSelectedOption(sel)
            from txt in text(opt)
            select txt;

        /// <summary>
        /// Retrieves the value for the selected option element in a Select Element
        /// </summary>
        /// <param name="sel">Web Driver Select Element</param>
        /// <returns>The selected Option Web Element</returns>
        public static Isotope<string> getSelectedOptionValue(SelectElement sel) =>
            from opt in getSelectedOption(sel)
            from val in value(opt)
            select val;

        /// <summary>
        /// Finds a checkbox element by selector and identifies whether it is checked
        /// </summary>
        /// <param name="selector">Web Driver Selector</param>
        /// <returns>Is checked\s</returns>
        public static Isotope<bool> isCheckboxChecked(By selector) =>
            from el in findElement(selector)
            from res in isCheckboxChecked(el)
            select res;

        /// <summary>
        /// Identifies whether an existing checkbox is checked
        /// </summary>
        /// <param name="el">Web Driver Element</param>
        /// <returns>Is checked\s</returns>
        public static Isotope<bool> isCheckboxChecked(IWebElement el) =>
            pure(el.Selected);

        /// <summary>
        /// Set checkbox value for existing element
        /// </summary>
        /// <param name="el">Web Driver Element</param>
        /// <param name="ticked">Check the box or not</param>
        /// <returns>Unit</returns>
        public static Isotope<Unit> setCheckbox(IWebElement el, bool ticked) =>
            from val in isCheckboxChecked(el)
            from _   in val == ticked
                        ? pure(unit)
                        : click(el)
            select unit;

        /// <summary>
        /// Looks for a particular style attribute on an existing element
        /// </summary>
        /// <param name="el">Web Driver Element</param>
        /// <param name="style">Style attribute to look up</param>
        /// <returns>A string representing the style value</returns>
        public static Isotope<string> getStyle(IWebElement el, string style) =>
            tryf(() => el.GetCssValue(style), $"Could not find style {style}");

        /// <summary>
        /// Gets the Z Index style attribute value for an existing element
        /// </summary>
        /// <param name="el">Web Driver Element</param>
        /// <returns>The Z Index value</returns>
        public static Isotope<int> getZIndex(IWebElement el) =>
            from zis in getStyle(el, "zIndex")
            from zii in parseInt(zis).ToIsotope($"z-Index was not valid integer: {zis}.")
            select zii;

        /// <summary>
        /// Looks for a particular style attribute on an existing element
        /// </summary>
        /// <param name="el">Web Driver Element</param>
        /// <param name="att">Attribute to look up</param>
        /// <returns>A string representing the attribute value</returns>
        public static Isotope<string> attribute(IWebElement el, string att) =>
            tryf(() => el.GetAttribute(att), $"Attribute {att} could not be found.");

        /// <summary>
        /// Simulates keyboard by sending `keys` 
        /// </summary>
        /// <param name="selector">Selector for element to type into</param>
        /// <param name="keys">String of characters that are typed</param>
        public static Isotope<Unit> sendKeys(By selector, string keys) =>
            from el in findElement(selector)
            from _  in sendKeys(el, keys)
            select unit;

        /// <summary>
        /// Simulates keyboard by sending `keys` 
        /// </summary>
        /// <param name="element">Element to type into</param>
        /// <param name="keys">String of characters that are typed</param>
        public static Isotope<Unit> sendKeys(IWebElement element, string keys) =>
            trya(() => element.SendKeys(keys), $@"Error sending keys ""{keys}"" to element: {element.PrettyPrint()}");

        /// <summary>
        /// Simulates the mouse-click
        /// </summary>
        /// <param name="selector">Web Driver Selector</param>
        /// <returns>Unit</returns>
        public static Isotope<Unit> click(By selector) =>
            from el in findElement(selector)
            from _ in click(el)
            select unit;

        /// <summary>
        /// Simulates the mouse-click
        /// </summary>
        /// <param name="element">Element to click</param>
        public static Isotope<Unit> click(IWebElement element) =>
            trya(() => element.Click(), $@"Error clicking element: {element.PrettyPrint()}");

        /// <summary>
        /// Clears the content of an element
        /// </summary>
        /// <param name="element">Web Driver Element</param>
        /// <returns>Unit</returns>
        public static Isotope<Unit> clear(IWebElement element) =>
            trya(() => element.Clear(), $@"Error clearing element: {element.PrettyPrint()}");

        /// <summary>
        /// ONLY USE AS A LAST RESORT
        /// Pauses the processing for an interval to brute force waiting for actions to complete
        /// </summary>
        public static Isotope<Unit> pause(TimeSpan interval)
        {
            Thread.Sleep((int)interval.TotalMilliseconds);
            return pure(unit);
        }

        /// <summary>
        /// Gets the text inside an element
        /// </summary>
        /// <param name="element">Element containing txt</param>
        public static Isotope<string> text(IWebElement element) =>
            tryf(() => element.Text, $@"Error getting text from element: {element.PrettyPrint()}");

        /// <summary>
        /// Gets the value attribute of an element
        /// </summary>
        /// <param name="element">Element containing value</param>
        public static Isotope<string> value(IWebElement element) =>
            tryf(() => element.GetAttribute("Value"), $@"Error getting value from element: {element.PrettyPrint()}");

        /// <summary>
        /// Web driver accessor
        /// </summary>
        public static Isotope<IWebDriver> webDriver =>
            from s in get
            from r in s.Driver.ToIsotope("web-driver hasn't been selected yet")
            select r;

        /// <summary>
        /// Web driver setter
        /// </summary>
        public static Isotope<Unit> setWebDriver(IWebDriver d) =>
            from s in get
            from _ in put(s.With(Driver: Some(d)))
            select unit;

        public static Isotope<Unit> disposeWebDriver =>
            from s in get
            select s.DisposeWebDriver();

        /// <summary>
        /// Default wait accessor
        /// </summary>
        public static Isotope<TimeSpan> defaultWait =>
            from s in get
            select s.Settings.Wait;

        /// <summary>
        /// Default wait accessor
        /// </summary>
        public static Isotope<TimeSpan> defaultInterval =>
            from s in get
            select s.Settings.Interval;

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

        /// <summary>
        /// Failure - creates an Isotope monad that always fails
        /// </summary>
        /// <param name="message">Error message</param>
        public static Isotope<A> fail<A>(string message) =>
            Isotope<A>.Fail(Error.New(message));

        /// <summary>
        /// Failure - creates an Isotope monad that always fails
        /// </summary>
        /// <param name="error">Error</param>
        public static Isotope<A> fail<A>(Error error) =>
            Isotope<A>.Fail(error);

        /// <summary>
        /// Failure - creates an Isotope monad that always fails
        /// </summary>
        /// <param name="ex">Exception</param>
        public static Isotope<A> fail<A>(Exception ex) =>
            Isotope<A>.Fail(ex);

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
            new IsotopeAsync<A>(async s => new IsotopeState<A>(await f(), s)); 

        /// <summary>
        /// Lift the function into the isotope monadic space
        /// </summary>
        public static IsotopeAsync<A> isoAsync<A>(Func<ValueTask<Fin<A>>> f) =>
            new IsotopeAsync<A>(async s => (await f()).Match(Succ: a => new IsotopeState<A>(a, s),
                                                             Fail: e => new IsotopeState<A>(default, s.AddError(e)))); 

        /// <summary>
        /// Lift the function into the isotope monadic space
        /// </summary>
        public static IsotopeAsync<A> isoAsync<A>(Func<IsotopeState, ValueTask<A>> f) =>
            new IsotopeAsync<A>(async s => new IsotopeState<A>(await f(s), s)); 

        /// <summary>
        /// Lift the function into the isotope monadic space
        /// </summary>
        public static IsotopeAsync<A> isoAsync<A>(Func<IsotopeState, ValueTask<Fin<A>>> f) =>
            new IsotopeAsync<A>(async s => (await f(s)).Match(Succ: a => new IsotopeState<A>(a, s),
                                                              Fail: e => new IsotopeState<A>(default, s.AddError(e)))); 

        /// <summary>
        /// Lift the function into the isotope monadic space
        /// </summary>
        public static IsotopeAsync<Env, A> isoAsync<Env, A>(Func<Env, IsotopeState, ValueTask<A>> f) =>
            new IsotopeAsync<Env, A>(async (e, s) => new IsotopeState<A>(await f(e, s), s)); 

        /// <summary>
        /// Lift the function into the isotope monadic space
        /// </summary>
        public static IsotopeAsync<Env, A> isoAsync<Env, A>(Func<Env, IsotopeState, ValueTask<Fin<A>>> f) =>
            new IsotopeAsync<Env, A>(async (e, s) => (await f(e, s)).Match(Succ: a => new IsotopeState<A>(a, s),
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
        /// Try and action
        /// </summary>
        /// <param name="action">Action to try</param>
        /// <param name="label">Error string if exception is thrown</param>
        /// <returns></returns>
        public static Isotope<Unit> trya(Action action, string label) =>
            iso(fun(action))
               .MapFail(e => Error.New(label, Aggregate(e)));

        /// <summary>
        /// Try and action
        /// </summary>
        /// <param name="action">Action to try</param>
        /// <param name="makeError">Convert errors to string</param>
        /// <returns></returns>
        public static Isotope<Unit> trya(Action action, Func<Error, string> makeError) =>
            iso(fun(action))
               .MapFail(e => Error.New(makeError(e.Last), Aggregate(e)));        

        /// <summary>
        /// Try a function
        /// </summary>
        /// <typeparam name="A">Return type of the function</typeparam>
        /// <param name="func">Function to try</param>
        /// <param name="label">Error string if exception is thrown</param>
        /// <returns></returns>
        public static Isotope<A> tryf<A>(Func<A> func, string label) =>
            iso(func)
               .MapFail(e => Error.New(label, Aggregate(e)));

        /// <summary>
        /// Try a function
        /// </summary>
        /// <typeparam name="A">Return type of the function</typeparam>
        /// <param name="func">Function to try</param>
        /// <param name="makeError">Convert errors to string</param>
        /// <returns></returns>
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
        /// Log some output
        /// </summary>
        public static Isotope<Unit> log(string message) =>
            from st in get
            from _1 in put(st.Write(message, st.Settings.LoggingAction))
            select unit;

        public static Isotope<Unit> pushLog(string message) =>
            from st in get
            from _1 in put(st.PushLog(message, st.Settings.LoggingAction))
            select unit;

        public static Isotope<Unit> popLog =>
            from st in get
            from _1 in put(st.PopLog())
            select unit;

        public static Isotope<A> context<A>(string context, Isotope<A> iso) =>
            from _1 in pushLog(context)
            from re in iso
            from _2 in popLog
            select re;

        public static Isotope<Seq<IWebElement>> waitUntilElementsExists(
            By selector,
            Option<TimeSpan> interval = default,
            Option<TimeSpan> wait = default) =>
            from el in waitUntil(findElementsOrEmpty(selector), x => x.IsEmpty, interval: interval, wait: wait)
            select el;

        public static Isotope<Seq<IWebElement>> waitUntilElementsExists(
            IWebElement parent,
            By selector,
            Option<TimeSpan> interval = default,
            Option<TimeSpan> wait = default) =>
            from el in waitUntil(findElementsOrEmpty(parent, selector), x => x.IsEmpty, interval: interval, wait: wait)
            select el;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="interval"></param>
        /// <param name="wait"></param>
        /// <returns></returns>
        public static Isotope<IWebElement> waitUntilElementExists(
            By selector, 
            Option<TimeSpan> interval = default,
            Option<TimeSpan> wait = default) =>
            from x in waitUntil(
                            findOptionalElement(selector),
                            el => el.IsNone,
                            interval,
                            wait)
            from y in x.Match(
                            Some: s => pure(s),
                            None: () => fail<IWebElement>("Element not found within timeout period"))
            select y;

        /// <summary>
        /// Attempts to find a child element within an existing element and if not present retries for a period.
        /// </summary>
        /// <param name="element">Parent element</param>
        /// <param name="selector">Selector within element</param>
        /// <param name="interval">The time period between attempts to check, if not provided the default value from Settings is used.</param>
        /// <param name="wait">The overall time period to attempt for, if not provided the default value from Settings is used.</param>
        /// <returns></returns>
        public static Isotope<IWebElement> waitUntilElementExists(
            IWebElement element, 
            By selector, 
            Option<TimeSpan> interval = default,
            Option<TimeSpan> wait = default) =>
            from x in waitUntil(
                            findOptionalElement(element, selector),
                            el => el.IsNone,
                            interval,
                            wait)
            from y in x.ToIsotope("Element not found within timeout period")
            select y;

        /// <summary>
        /// Wait for an element to be rendered and clickable, fail if exceeds default timeout
        /// </summary>
        public static Isotope<IWebElement> waitUntilClickable(By selector) =>
            from w  in defaultWait
            from el in waitUntilClickable(selector, w)
            select el;

        /// <summary>
        /// Wait for an element to be rendered and clickable, fail if exceeds default timeout
        /// </summary>
        public static Isotope<Unit> waitUntilClickable(IWebElement element) =>
            from w in defaultWait
            from _ in waitUntilClickable(element, w)
            select unit;

        public static Isotope<IWebElement> waitUntilClickable(By selector, TimeSpan timeout) =>
            from _1 in log($"Waiting until clickable: {selector}")
            from el in waitUntilElementExists(selector)
            from _2 in waitUntilClickable(el, timeout)
            select el;

        public static Isotope<Unit> waitUntilClickable(IWebElement el, TimeSpan timeout) =>
            from _ in waitUntil(
                        from _1a in log($"Checking clickability " + el.PrettyPrint())
                        from d in displayed(el)
                        from e in enabled(el)
                        from o in obscured(el)
                        from _2a in log($"Displayed: {d}, Enabled: {e}, Obscured: {o}")
                        select d && e && (!o),
                        x => !x)
            select unit;

        public static string PrettyPrint(this IWebElement x)
        {
            var tag = x.TagName;
            var css = x.GetAttribute("class");
            var id = x.GetAttribute("id");

            return $"<{tag} class='{css}' id='{id}'>";
        }

        /// <summary>
        /// Flips the sequence of Isotopes to be a Isotope of Sequences
        /// </summary>
        public static Isotope<Seq<A>> Sequence<A>(this Seq<Isotope<A>> mas) =>
            new Isotope<Seq<A>>(
                state => {
                    var rs    = new A[mas.Count];
                    int index = 0;

                    foreach (var ma in mas)
                    {
                        var s = ma.Invoke(state);
                        if (s.State.IsFaulted)
                        {
                            return new IsotopeState<Seq<A>>(default, s.State);
                        }

                        state     = s.State;
                        rs[index] = s.Value;
                        index++;
                    }

                    return new IsotopeState<Seq<A>>(rs.ToSeq(), state);
                });

        /// <summary>
        /// Flips the sequence of Isotopes to be a Isotope of Sequences
        /// </summary>
        public static Isotope<Env, Seq<A>> Sequence<Env, A>(this Seq<Isotope<Env, A>> mas) =>
            new Isotope<Env, Seq<A>>(
                (env, state) => {
                    var rs    = new A[mas.Count];
                    int index = 0;

                    foreach (var ma in mas)
                    {
                        var s = ma.Invoke(env, state);
                        if (s.State.IsFaulted)
                        {
                            return new IsotopeState<Seq<A>>(default, s.State);
                        }

                        state     = s.State;
                        rs[index] = s.Value;
                        index++;
                    }

                    return new IsotopeState<Seq<A>>(rs.ToSeq(), state);
                });


        /// <summary>
        /// Flips the sequence of Isotopes to be a Isotope of Sequences
        /// </summary>
        public static IsotopeAsync<Seq<A>> Sequence<A>(this Seq<IsotopeAsync<A>> mas) =>
            new IsotopeAsync<Seq<A>>(
                async state => {
                    var rs    = new A[mas.Count];
                    int index = 0;

                    foreach (var ma in mas)
                    {
                        var s = await ma.Invoke(state);
                        if (s.State.IsFaulted)
                        {
                            return new IsotopeState<Seq<A>>(default, s.State);
                        }

                        state     = s.State;
                        rs[index] = s.Value;
                        index++;
                    }

                    return new IsotopeState<Seq<A>>(rs.ToSeq(), state);
                });

        /// <summary>
        /// Flips the sequence of Isotopes to be a Isotope of Sequences
        /// </summary>
        public static IsotopeAsync<Env, Seq<A>> Sequence<Env, A>(this Seq<IsotopeAsync<Env, A>> mas) =>
            new IsotopeAsync<Env, Seq<A>>(
                async (env, state) => {
                    var rs    = new A[mas.Count];
                    int index = 0;

                    foreach (var ma in mas)
                    {
                        var s = await ma.Invoke(env, state);
                        if (s.State.IsFaulted)
                        {
                            return new IsotopeState<Seq<A>>(default, s.State);
                        }

                        state     = s.State;
                        rs[index] = s.Value;
                        index++;
                    }

                    return new IsotopeState<Seq<A>>(rs.ToSeq(), state);
                });

        /// <summary>
        /// Flips the sequence of Isotopes to be a Isotope of Sequences
        /// </summary>
        public static Isotope<Seq<A>> Collect<A>(this Seq<Isotope<A>> mas) =>
            new Isotope<Seq<A>>(
                state => {
                    if (state.IsFaulted)
                    {
                        return new IsotopeState<Seq<A>>(default, state);
                    }

                    var rs    = new A[mas.Count];
                    int index = 0;

                    // Create an empty log TODO
                    //var logs = state.Log.Cons(Seq<Seq<string>>());

                    // Clear log from the state
                    state = state.With(Log: Log.Empty);

                    bool hasFaulted = false;
                    var  errors     = new Seq<Error>();

                    foreach (var ma in mas)
                    {
                        var s = ma.Invoke(state);

                        // Collect error
                        hasFaulted = hasFaulted || s.State.IsFaulted;
                        if (s.State.IsFaulted)
                        {
                            errors = errors + s.State.Error;
                        }

                        // Collect logs TODO
                        //logs = logs.Add(s.State.Log);

                        // Record value
                        rs[index] = s.Value;
                        index++;
                    }

                    return new IsotopeState<Seq<A>>(rs.ToSeq(), state.With(Error: errors, Log: Log.Empty));
                });


        /// <summary>
        /// Flips the sequence of Isotopes to be a Isotope of Sequences
        /// </summary>
        public static Isotope<Env,  Seq<A>> Collect<Env, A>(this Seq<Isotope<Env,  A>> mas) =>
            new Isotope<Env,  Seq<A>>(
                (env, state) => {
                    if (state.IsFaulted)
                    {
                        return new IsotopeState<Seq<A>>(default, state);
                    }

                    var rs    = new A[mas.Count];
                    int index = 0;

                    // Create an empty log TODO
                    //var logs = state.Log.Cons(Seq<Seq<string>>());

                    // Clear log from the state
                    state = state.With(Log: Log.Empty);

                    bool hasFaulted = false;
                    var  errors     = new Seq<Error>();

                    foreach (var ma in mas)
                    {
                        var s = ma.Invoke(env, state);

                        // Collect error
                        hasFaulted = hasFaulted || s.State.IsFaulted;
                        if (s.State.IsFaulted)
                        {
                            errors = errors + s.State.Error;
                        }

                        // Collect logs TODO
                        //logs = logs.Add(s.State.Log);

                        // Record value
                        rs[index] = s.Value;
                        index++;
                    }

                    return new IsotopeState<Seq<A>>(rs.ToSeq(), state.With(Error: errors, Log: Log.Empty));
                });        
        
        
        

        /// <summary>
        /// Flips the sequence of Isotopes to be a Isotope of Sequences
        /// </summary>
        public static IsotopeAsync<Seq<A>> Collect<A>(this Seq<IsotopeAsync<A>> mas) =>
            new IsotopeAsync<Seq<A>>(
                async state => {
                    if (state.IsFaulted)
                    {
                        return new IsotopeState<Seq<A>>(default, state);
                    }

                    var rs    = new A[mas.Count];
                    int index = 0;

                    // Create an empty log TODO
                    //var logs = state.Log.Cons(Seq<Seq<string>>());

                    // Clear log from the state
                    state = state.With(Log: Log.Empty);

                    bool hasFaulted = false;
                    var  errors     = new Seq<Error>();

                    foreach (var ma in mas)
                    {
                        var s = await ma.Invoke(state);

                        // Collect error
                        hasFaulted = hasFaulted || s.State.IsFaulted;
                        if (s.State.IsFaulted)
                        {
                            errors = errors + s.State.Error;
                        }

                        // Collect logs TODO
                        //logs = logs.Add(s.State.Log);

                        // Record value
                        rs[index] = s.Value;
                        index++;
                    }

                    return new IsotopeState<Seq<A>>(rs.ToSeq(), state.With(Error: errors, Log: Log.Empty));
                });


        /// <summary>
        /// Flips the sequence of Isotopes to be a Isotope of Sequences
        /// </summary>
        public static IsotopeAsync<Env,  Seq<A>> Collect<Env, A>(this Seq<IsotopeAsync<Env,  A>> mas) =>
            new IsotopeAsync<Env,  Seq<A>>(
                async (env, state) => {
                    if (state.IsFaulted)
                    {
                        return new IsotopeState<Seq<A>>(default, state);
                    }

                    var rs    = new A[mas.Count];
                    int index = 0;

                    // Create an empty log TODO
                    //var logs = state.Log.Cons(Seq<Seq<string>>());

                    // Clear log from the state
                    state = state.With(Log: Log.Empty);

                    bool hasFaulted = false;
                    var  errors     = new Seq<Error>();

                    foreach (var ma in mas)
                    {
                        var s = await ma.Invoke(env, state);

                        // Collect error
                        hasFaulted = hasFaulted || s.State.IsFaulted;
                        if (s.State.IsFaulted)
                        {
                            errors = errors + s.State.Error;
                        }

                        // Collect logs TODO
                        //logs = logs.Add(s.State.Log);

                        // Record value
                        rs[index] = s.Value;
                        index++;
                    }

                    return new IsotopeState<Seq<A>>(rs.ToSeq(), state.With(Error: errors, Log: Log.Empty));
                });              
        
        
        /// <summary>
        /// Convert an option to a pure isotope
        /// </summary>
        /// <param name="maybe">Optional value</param>
        /// <param name="label">Failure value to use if None</param>
        /// <typeparam name="A">Bound value type</typeparam>
        /// <returns>Pure isotope</returns>
        public static Isotope<A> ToIsotope<A>(this Option<A> maybe, string label) =>
            maybe.Match(Some: pure, None: fail<A>(label));

        /// <summary>
        /// Convert a try to an isotope computation
        /// </summary>
        /// <param name="tried">Try value</param>
        /// <typeparam name="A">Bound value type</typeparam>
        /// <returns>Try computation wrapped in an isotope computation</returns>
        public static Isotope<A> ToIsotope<A>(this Try<A> tried) =>
            tried.Match(
                Succ: pure,
                Fail: x => fail<A>(Error.New(x)));

        /// <summary>
        /// Convert a try to an isotope computation
        /// </summary>
        /// <param name="tried">Try value</param>
        /// <param name="label">Failure value to use if None</param>
        /// <typeparam name="A">Bound value type</typeparam>
        /// <returns>Try computation wrapped in an isotope computation</returns>
        public static Isotope<A> ToIsotope<A>(this Try<A> tried, string label) =>
            tried.ToIsotope().MapFail(e => Error.New(label, Aggregate(e)));

        /// <summary>
        /// Convert a try to an isotope computation
        /// </summary>
        /// <param name="tried">Try value</param>
        /// <param name="makeError">Failure value to use if None</param>
        /// <typeparam name="A">Bound value type</typeparam>
        /// <returns>Try computation wrapped in an isotope computation</returns>
        public static Isotope<A> ToIsotope<A>(this Try<A> tried, Func<Error, string> makeError) =>
            tried.ToIsotope().MapFail(e => Error.New(makeError(e.Last), Aggregate(e)));

        /// <summary>
        /// Convert an Either to a pure isotope
        /// </summary>
        /// <param name="either">Either to convert</param>
        /// <typeparam name="R">Right param</typeparam>
        /// <returns>Pure isotope</returns>
        public static Isotope<R> ToIsotope<R>(this Either<Error, R> either) =>
            either.Match(Left: fail<R>, Right: pure);

        /// <summary>
        /// Convert an Either to a pure isotope
        /// </summary>
        /// <param name="either">Either to convert</param>
        /// <param name="label">Label for the failure</param>
        /// <returns>Pure isotope</returns>
        public static Isotope<B> ToIsotope<A, B>(this Either<A, B> either, string label) =>
            either.Match(
                Left: _ => fail<B>(Error.New(label)),
                Right: pure);

        /// <summary>
        /// Convert an Either to a pure isotope
        /// </summary>
        /// <param name="either">Either to convert</param>
        /// <param name="makeError">Label for the failure</param>
        /// <returns>Pure isotope</returns>
        public static Isotope<B> ToIsotope<A, B>(this Either<A, B> either, Func<A, string> makeError) =>
            either.Match(
                Left: e => fail<B>(Error.New(makeError(e))),
                Right: pure);

        /// <summary>
        /// Finds an element by a selector and checks if it is currently displayed
        /// </summary>
        /// <param name="selector">WebDriver selector</param>
        /// <returns>True if the element is currently displayed</returns>
        public static Isotope<bool> displayed(By selector) =>
            from el in findElement(selector)
            from d in displayed(el)
            select d;

        /// <summary>
        /// Checks if an element is currently displayed
        /// </summary>
        /// <param name="el">WebDriver element</param>
        /// <returns>True if the element is currently displayed</returns>
        public static Isotope<bool> displayed(IWebElement el) =>
            tryf(() => el.Displayed, $"Error getting display status of {el}");

        public static Isotope<bool> enabled(IWebElement el) =>
            tryf(() => el.Enabled, $"Error getting enabled status of {el}");

        /// <summary>
        /// Checks if an element exists that matches the selector
        /// </summary>
        /// <param name="selector">WebDriver selector</param>
        /// <returns>True if a matching element exists</returns>
        public static Isotope<bool> exists(By selector) =>
            from op in findOptionalElement(selector)
            from bl in op.Match(
                        Some: _ => pure(true),
                        None: () => pure(false))
            select bl;

        /// <summary>
        /// Checks whether the centre point of an element is the foremost element at that position on the page.
        /// (Uses the JavaScript document.elementFromPoint function)
        /// </summary>
        /// <param name="element">Target element</param>
        /// <returns>true if the element is foremost</returns>
        public static Isotope<bool> obscured(IWebElement element) =>
            from dvr in webDriver
            let jsExec = (IJavaScriptExecutor)dvr
            let coords = element.Location
            let x = coords.X + (int)Math.Floor((double)(element.Size.Width / 2))
            let y = coords.Y + (int)Math.Floor((double)(element.Size.Height / 2))
            from _ in log($"X: {x}, Y: {y}")
            from top in pure((IWebElement)jsExec.ExecuteScript($"return document.elementFromPoint({x}, {y});"))
            from _1  in log($"Target: {element.PrettyPrint()}, Top: {top.PrettyPrint()}")
            select !element.Equals(top);

        /// <summary>
        /// Compares the text of an element with a string
        /// </summary>
        /// <param name="element">Element to compare</param>
        /// <param name="comparison">String to match</param>
        /// <returns>true if exact match</returns>
        public static Isotope<bool> hasText(IWebElement element, string comparison) =>
            from t in text(element)
            select t == comparison;

        /// <summary>
        /// Repeatedly runs an Isotope function and checks whether the condition is met.
        /// </summary>        
        public static Isotope<A> waitUntil<A>(
            Isotope<A> iso,
            Func<A, bool> continueCondition,
            Option<TimeSpan> interval = default,
            Option<TimeSpan> wait = default) =>
            from w in wait.Match(Some: s => pure(s), None: () => defaultWait)
            from i in interval.Match(Some: s => pure(s), None: () => defaultInterval)
            from r in waitUntil(iso, continueCondition, i, w, DateTime.Now)
            select r;

        private static Isotope<A> waitUntil<A>(
            Isotope<A> iso,
            Func<A, bool> continueCondition,
            TimeSpan interval,
            TimeSpan wait,
            DateTime started) =>
            DateTime.Now - started >= wait
                ? fail<A>("Timed Out")
                : from x in iso
                  from y in continueCondition(x)
                            ? from _ in pause(interval)
                              from r in waitUntil(iso, continueCondition, interval, wait, started)
                              select r
                            : pure(x)
                  select y;

        public static Isotope<A> doWhile<A>(
            Isotope<A> iso,
            Func<A, bool> continueCondition,
            int maxRepeats = 100) =>
            maxRepeats <= 0
                ? pure(default(A))
                : from x in iso
                  from y in continueCondition(x)
                              ? doWhile(iso, continueCondition, maxRepeats - 1)
                              : pure(x)
                  select y;

        public static Isotope<A> doWhileOrFail<A>(
            Isotope<A> iso,
            Func<A, bool> continueCondition,
            string failureMessage,
            int maxRepeats = 100) =>
            maxRepeats <= 0
                ? fail<A>(failureMessage)
                : from x in iso
                  from y in continueCondition(x)
                              ? doWhileOrFail(iso, continueCondition, failureMessage, maxRepeats - 1)
                              : pure(x)
                  select y;

        public static Isotope<A> doWhileOrFail<A>(
            Isotope<A> iso,
            Func<A, bool> continueCondition,
            string failureMessage,
            TimeSpan interval,
            int maxRepeats = 1000) =>
            maxRepeats <= 0
                ? fail<A>(failureMessage)
                : from x in iso
                  from y in continueCondition(x)
                              ? from _ in pause(interval)
                                from z in doWhileOrFail(iso, continueCondition, failureMessage, interval, maxRepeats - 1)
                                select z
                              : pure(x)
                  select y;

        /// <summary>
        /// Takes a screenshot if the current WebDriver supports that functionality
        /// </summary>
        public static Isotope<Option<Screenshot>> getScreenshot =>
            from dvr in webDriver
            let ts = dvr as ITakesScreenshot
            select ts == null ? None : Some(ts.GetScreenshot());

        static Exception Aggregate(Seq<Error> errs) =>
            errs.IsEmpty
                ? null
                : errs.Count == 1
                    ? (Exception) errs.Head
                    : new AggregateException(errs.Map(e => (Exception) e));
    }
}
