using LanguageExt;
using System;
using static LanguageExt.Prelude;

namespace Isotope80
{
    /// <summary>
    /// Log entry type
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// Contextual header
        /// </summary>
        Context,
        
        /// <summary>
        /// Information message
        /// </summary>
        Info,
        
        /// <summary>
        /// Warning message
        /// </summary>
        Warn,
        
        /// <summary>
        /// Error message
        /// </summary>
        Error
    }

    /// <summary>
    /// Nested log
    /// </summary>
    public class Log
    {
        /// <summary>
        /// Empty log
        /// </summary>
        public static readonly Log Empty = new Log(0, default, "", default);
        
        /// <summary>
        /// Number of tabs to indent
        /// </summary>
        public readonly int Indent;

        /// <summary>
        /// Log type
        /// </summary>
        public readonly LogType Type;
        
        /// <summary>
        /// Node message
        /// </summary>
        public readonly string Message;
        
        /// <summary>
        /// Child messages
        /// </summary>
        public readonly Seq<Log> Children;

        /// <summary>
        /// Ctor
        /// </summary>
        internal Log(int indent, LogType type, string message, Seq<Log> children)
        {
            Indent   = indent >= 0 ? indent : throw new ArgumentOutOfRangeException(nameof(indent));
            Type     = type;
            Message  = message ?? throw new ArgumentNullException(nameof(message));
            Children = children;
        }

        /// <summary>
        /// Add a log entry
        /// </summary>
        public (Log Log, Log Added) Add(Log log)
        {
            var nlog = log.Type == LogType.Context ? log.Rebase(Indent + 1) : log.Rebase(Indent);
            return (new Log(Indent, Type, Message, Children.Add(nlog)), nlog);
        }

        /// <summary>
        /// Set the new base indent for the log
        /// </summary>
        /// <param name="indent">Base indent</param>
        /// <returns>Rebased log</returns>
        public Log Rebase(int indent) =>
            new Log(indent, Type, Message, Children.Map(c => c.Rebase(indent + 1)));

        /// <summary>
        /// Add a message to the log
        /// </summary>
        /// <param name="ctx">Context</param>
        public static Log Context(string ctx) =>
            new Log(0, LogType.Info, ctx, default);

        /// <summary>
        /// Add a message to the log
        /// </summary>
        /// <param name="message">Message to log</param>
        public static Log Info(string message) =>
            new Log(0, LogType.Info, $"INFO: {message}", default); 

        /// <summary>
        /// Add a message to the log
        /// </summary>
        /// <param name="message">Message to log</param>
        public static Log Warning(string message) =>
            new Log(0, LogType.Warn, $"WARN: {message}", default);

        /// <summary>
        /// Add a message to the log
        /// </summary>
        /// <param name="message">Message to log</param>
        public static Log Error(string message) =>
            new Log(0, LogType.Error, $"ERRO: {message}", default);

        /// <summary>
        /// ToString
        /// </summary>
        public override string ToString() =>
            String.Join(Environment.NewLine, ToSeq());

        /// <summary>
        /// ToSeq
        /// </summary>
        public Seq<string> ToSeq() =>
            Seq1(Text.Tabs(Indent, Message))
                .Filter(s => !String.IsNullOrWhiteSpace(s))
                .Append(Children.Map(c => c.ToSeq()));

        /// <summary>
        /// Add a log entry
        /// </summary>
        public static Log operator +(Log lhs, Log rhs) =>
            lhs.Add(rhs).Log;
    }

    /// <summary>
    /// Log output
    /// </summary>
    public readonly struct LogOutput
    {
        /// <summary>
        /// Log message
        /// </summary>
        public readonly string Message;
        
        /// <summary>
        /// Severity type
        /// </summary>
        public readonly LogType Type;
        
        /// <summary>
        /// Indentation
        /// </summary>
        public readonly int Indent;

        /// <summary>
        /// Ctor
        /// </summary>
        public LogOutput(string message, LogType type, int indent)
        {
            Message = message ?? throw new ArgumentNullException(nameof(message));
            Type    = type;
            Indent  = indent >= 0 ? indent :  throw new ArgumentOutOfRangeException(nameof(indent));
        }

        /// <summary>
        /// Tabbed format display 
        /// </summary>
        public override string ToString() =>
            Text.Tabs(Indent, Message);
    }
}
