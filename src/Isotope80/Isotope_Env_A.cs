using System;
using LanguageExt;
using LanguageExt.Common;
using OpenQA.Selenium;
using static LanguageExt.Prelude;

namespace Isotope80
{
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
                                    var r = l.IsFaulted
                                                ? rhs.Invoke(e, s)
                                                : l;

                                    return r.IsFaulted
                                               ? new IsotopeState<A>(default, s.With(Error: l.State.Error + r.State.Error))
                                               : r;
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
}