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
        public static readonly Log Empty = new Log(-1, default, "", default);
        
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
            Indent   = indent;
            Type     = type;
            Message  = message;
            Children = children;
        }

        /// <summary>
        /// Add a message to the log
        /// </summary>
        /// <param name="ctx">Context</param>
        public (Log Parent, Log Child) Context(string ctx)
        {
            var child = new Log(Indent + 1, LogType.Info, ctx, default);
            return (new Log(Indent, Type, Message, Children.Add(child)), child);
        }

        /// <summary>
        /// Add a log entry
        /// </summary>
        public Log Add(Log log) =>
            new Log(Indent, Type, Message, Children.Add(log));

        /// <summary>
        /// Add a message to the log
        /// </summary>
        /// <param name="message">Message to log</param>
        public Log Info(string message) =>
            new Log(Indent, Type, Message, Children.Add(new Log(Indent + 1, LogType.Info, $"INFO: {message}", default))); 

        /// <summary>
        /// Add a message to the log
        /// </summary>
        /// <param name="message">Message to log</param>
        public Log Warning(string message) =>
            new Log(Indent, Type, Message, Children.Add(new Log(Indent + 1, LogType.Warn, $"WARN: {message}", default)));

        /// <summary>
        /// Add a message to the log
        /// </summary>
        /// <param name="message">Message to log</param>
        public Log Error(string message) =>
            new Log(Indent, Type, Message, Children.Add(new Log(Indent + 1, LogType.Error, $"ERRO: {message}", default)));

        /// <summary>
        /// Create a new Node
        /// </summary>
        public static Log New(string message) => 
            new Log(0, LogType.Info, message, default);

        /// <summary>
        /// ToString
        /// </summary>
        public override string ToString() =>
            String.Join(Environment.NewLine, ToSeq());

        /// <summary>
        /// ToSeq
        /// </summary>
        public Seq<string> ToSeq() =>
            Seq1(new string('\t', Indent) + Message)
                .Append(Children.Map(c => c.ToSeq()));
    }

    /// <summary>
    /// Log output
    /// </summary>
    public struct LogOutput
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
        public LogOutput(string message, LogType type, int index) =>
            (Message, Type, Indent) = (message, type, Math.Max(0, index));

        /// <summary>
        /// Tabbed format display 
        /// </summary>
        public override string ToString() =>
            new string('\t', Indent) + Message;
    }
}
