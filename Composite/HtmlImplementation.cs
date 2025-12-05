using System;
using System.Collections.Generic;
using System.Text;

namespace Composite
{
    // Component
    public abstract class Node
    {
        public abstract string ToHtml(int indent = 0);

        public string Render() => ToHtml(0);
    }

    // Leaf
    public sealed class TextNode : Node
    {
        private readonly string _text;

        public TextNode(string text) => _text = text ?? string.Empty;

        public override string ToHtml(int indent = 0)
        {
            return new string(' ', indent * 2) + Escape(_text);
        }

        private static string Escape(string s) =>
            s.Replace("&", "&amp;")
             .Replace("<", "&lt;")
             .Replace(">", "&gt;")
             .Replace("\"", "&quot;");
    }

    // Composite
    public sealed class Element : Node
    {
        private readonly List<Node> _children = new();
        private readonly Dictionary<string, string> _attributes = new();

        public string Tag { get; }

        public Element(string tag) => Tag = tag ?? throw new ArgumentNullException(nameof(tag));

        public Element Add(params Node[] nodes)
        {
            if (nodes == null) return this;
            foreach (var n in nodes)
            {
                if (n != null) _children.Add(n);
            }
            return this;
        }

        public Element Remove(Node node)
        {
            _children.Remove(node);
            return this;
        }

        public Element SetAttribute(string name, string value)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Attribute name required", nameof(name));
            _attributes[name] = value ?? string.Empty;
            return this;
        }

        public override string ToHtml(int indent = 0)
        {
            var sb = new StringBuilder();
            var indentStr = new string(' ', indent * 2);

            // opening tag + attributes
            sb.Append(indentStr).Append('<').Append(Tag);
            foreach (var kvp in _attributes)
            {
                sb.Append(' ').Append(kvp.Key).Append("=\"").Append(EscapeAttr(kvp.Value)).Append('"');
            }

            if (_children.Count == 0)
            {
                // self-close empty element
                sb.Append(" />");
                return sb.ToString();
            }

            sb.Append('>').AppendLine();

            // children
            for (int i = 0; i < _children.Count; i++)
            {
                sb.Append(_children[i].ToHtml(indent + 1));
                sb.AppendLine();
            }

            // closing tag
            sb.Append(indentStr).Append("</").Append(Tag).Append('>');
            return sb.ToString();
        }

        private static string EscapeAttr(string s) =>
            s.Replace("&", "&amp;")
             .Replace("\"", "&quot;")
             .Replace("<", "&lt;")
             .Replace(">", "&gt;");
    }

    // Small demo builder (call HtmlExample.Build() to get the HTML string)
    public static class HtmlExample
    {
        public static string Build()
        {
            var title = new Element("title").Add(new TextNode("Composite HTML Example"));
            var head = new Element("head").Add(title);

            var h1 = new Element("h1").Add(new TextNode("Hello, Composite!"));
            var paragraph = new Element("p").Add(new TextNode("This is an HTML tree built using the Composite pattern."));
            var list = new Element("ul")
                .Add(
                    new Element("li").Add(new TextNode("Item 1")),
                    new Element("li").Add(new TextNode("Item 2")),
                    new Element("li").Add(new TextNode("Item 3"))
                );

            var body = new Element("body").Add(h1, paragraph, list);

            var html = new Element("html").Add(head, body);
            return "<!doctype html>" + Environment.NewLine + html.Render();
        }

        // Optional convenience to print to console
        public static void Demo()
        {
            Console.WriteLine(Build());
        }
    }
}