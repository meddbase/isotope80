using System;

namespace Isotope80
{
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
