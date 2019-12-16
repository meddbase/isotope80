using LanguageExt;
using System;
using static LanguageExt.Prelude;

namespace Isotope79
{
    public class Log
    {
        private readonly Seq<Node> Nodes;
        private readonly Option<Node> Current;

        public Log(Seq<Node> nodes, Option<Node> current)
        {
            Nodes = nodes;
            Current = current;
        }

        public Log With(Seq<Node>? nodes = null, Option<Node>? current = null) =>
            new Log(nodes ?? Nodes, current ?? Current);

        public Log Append(string message, Action<string, int> action, int depth = 0) =>
            Current.Match(
                Some: x => With(current: x.Append(message, action, depth)),
                None: () => {
                    action(message, depth);
                    return With(nodes: Nodes.Add(Node.New(message)));
                });

        public Log Push(string message, Action<string, int> action, int depth = 0) =>
            Current.Match(
                Some: x => With(current: x.Push(message, action, depth)),
                None: () => {
                    action(message, depth);
                    return With(current: Node.New(message));
                });

        public Log Pop() =>
            Current.Match(
                Some: x => x.Children.Current.IsSome
                           ? With(current: x.Pop())
                           : With(nodes: Nodes.Add(x), current: None),
                None: () => this);

        public static Log Empty => new Log(new Seq<Node>(), None);

        public override string ToString() =>
            string.Join(Environment.NewLine, ToString(0));

        public Seq<string> ToString(int indent) =>
            Nodes
                .Append(Current.ToSeq())
                .Bind(x => x.ToString(indent));

        public string Trace() =>
            string.Join(Environment.NewLine, Trace(0));

        public Seq<string> Trace(int indent) =>
            Nodes.Map(x => new string('\t', indent) + x.Message)
                 .Append(Current.Bind(x => x.Trace(indent)))
                 .ToSeq();
    }

    public class Node
    {
        public readonly string Message;
        public readonly Log Children;

        private Node(string message, Log children)
        {
            Message = message;
            Children = children;
        }

        public Node With(string message = null, Log children = null) =>
            new Node(message ?? Message, children ?? Children);

        public Node Append(string message, Action<string, int> action, int depth) => 
            With(children: Children.Append(message, action, depth+1));

        public Node Push(string message, Action<string, int> action, int depth) => 
            With(children: Children.Push(message, action, depth+1));

        public Node Pop() => With(children: Children.Pop());

        public static Node New(string message) => new Node(message, Log.Empty);

        public Seq<string> ToString(int indent) =>
            Seq1(new string('\t', indent) + Message)
                .Append(Children.ToString(++indent));

        public Seq<string> Trace(int indent) =>
            Seq1(new string('\t', indent) + Message)
               .Append(Children.Trace(++indent));

    }
}
