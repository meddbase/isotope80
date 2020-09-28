using System;
using System.Threading.Tasks;
using LanguageExt;
using LanguageExt.Common;
using OpenQA.Selenium;
using static LanguageExt.Prelude;

namespace Isotope80
{
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
                return await Thunk(env, state).ConfigureAwait(false);
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
            var res = await Invoke(env, IsotopeState.Empty.With(Settings: settings)).ConfigureAwait(false);
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
            var (state, value) = await Run(env, settings).ConfigureAwait(false);
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
                                         var l = await lhs.Invoke(e, s).ConfigureAwait(false);
                                         var r = l.IsFaulted
                                                     ? await rhs.Invoke(e, s).ConfigureAwait(false)
                                                     : l;

                                         return r.IsFaulted
                                                    ? new IsotopeState<A>(default, s.With(Error: l.State.Error + r.State.Error))
                                                    : r;
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
                var s = await self.Invoke(env, state).ConfigureAwait(false);
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
                var s = await self.Invoke(env, state).ConfigureAwait(false);
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
                var s = await self.Invoke(env, state).ConfigureAwait(false);
                if (s.IsFaulted) return s.CastError<B>();
                return await f(s.Value).Invoke(s.State).ConfigureAwait(false);
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
                var s = await self.Invoke(env, state).ConfigureAwait(false);
                if (s.IsFaulted) return s.CastError<B>();
                return await f(s.Value).Invoke(env, s.State).ConfigureAwait(false);
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
                var s = await self.Invoke(env, state).ConfigureAwait(false);
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
                    var r = await self.Invoke(e, s).ConfigureAwait(false);
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
                    var r = await self.Invoke(e, s).ConfigureAwait(false);
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
}