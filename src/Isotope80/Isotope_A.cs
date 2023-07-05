using System;
using LanguageExt;
using LanguageExt.Common;
using OpenQA.Selenium;
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
        /// Or operator - evaluates the left hand side, if it fails, it ignores the error and evaluates the right hand side
        /// </summary>
        public static Isotope<A> operator |(Isotope<A> lhs, Isotope<A> rhs) =>
            lhs | (_ => rhs);
        
        /// <summary>
        /// Or operator - evaluates the left hand side, if it fails, it ignores the error and evaluates the right hand side
        /// Allows to inspect errors of left hand side.
        /// </summary>
        public static Isotope<A> operator |(Isotope<A> lhs, Func<Seq<Error>, Isotope<A>> rhs) => 
            new Isotope<A>(s =>
            {
                var l = lhs.Invoke(s);
                var r = l.IsFaulted
                            ? rhs(l.State.Error).Invoke(s)
                            : l;

                return r.IsFaulted
                           ? new IsotopeState<A>(default, s.With(Error: l.State.Error + r.State.Error))
                           : r;
            });
        
        /// <summary>
        /// Implicit conversion from Error
        /// </summary>
        /// <returns></returns>
        public static implicit operator Isotope<A>(Error err) =>
            Fail(err);

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
}