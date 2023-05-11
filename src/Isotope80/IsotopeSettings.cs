using LanguageExt;
using System;
using System.Reactive.Subjects;
using LanguageExt.Common;

namespace Isotope80
{
    /// <summary>
    /// Common settings environment that is threaded through every Isotope computation
    /// </summary>
    public partial class IsotopeSettings
    {
        /// <summary>
        /// Default wait time
        /// </summary>
        private static readonly TimeSpan defaultWait = TimeSpan.FromSeconds(10);

        /// <summary>
        /// Default interval
        /// </summary>
        private static readonly TimeSpan defaultInterval = TimeSpan.FromMilliseconds(500);
 
        /// <summary>
        /// Errors
        /// </summary>
        public readonly Subject<Error> ErrorStream; 

        /// <summary>
        /// Errors
        /// </summary>
        public readonly Subject<LogOutput> LogStream;

        /// <summary>
        /// Wait time
        /// </summary>
        public readonly TimeSpan Wait;
        
        /// <summary>
        /// Interval time
        /// </summary>
        public readonly TimeSpan Interval;

        /// <summary>
        /// Ctor
        /// </summary>
        private IsotopeSettings(
            Subject<Error> errorStream,
            Subject<LogOutput> logStream,
            TimeSpan wait,
            TimeSpan interval
            )
        {
            ErrorStream = errorStream;
            LogStream   = logStream;
            Wait        = wait;
            Interval    = interval;
        }

        /// <summary>
        /// Create an IsotopeSettings
        /// </summary>
        public static IsotopeSettings Create(
            Subject<Error> errorStream,
            Subject<LogOutput> logStream,
            TimeSpan? wait = null,
            TimeSpan? interval = null) =>
            new IsotopeSettings(
                errorStream,
                logStream,
                wait ?? defaultWait,
                interval ?? defaultInterval);
 
        /// <summary>
        /// Create an IsotopeSettings
        /// </summary>
        public static IsotopeSettings Create(
            TimeSpan? wait = null,
            TimeSpan? interval = null) =>
            new IsotopeSettings(
                new Subject<Error>(),
                new Subject<LogOutput>(),
                wait ?? defaultWait,
                interval ?? defaultInterval);   }
}
