using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Markdown.Render;

namespace Markdown.Builder
{
    public class MarkDownBuilder
    {
        private readonly StringBuilder _stringBuilder;



        public MarkDownBuilder()
        {
            _stringBuilder = new StringBuilder();
        }

        IEnumerable<Type> GetTypesWith<TAttribute>(bool inherit)
            where TAttribute: System.Attribute
        { return from a in AppDomain.CurrentDomain.GetAssemblies()
            from t in a.GetTypes()
            where t.IsDefined(typeof(TAttribute),inherit)
            select t;
        }

        public MarkDownBuilder AppendPageTable(params string[] hidden)
        {
            var pageDescriptions = GetTypesWith<MarkdownDescription>(true)
                .Select(x => x.GetCustomAttribute(typeof(MarkdownDescription)))
                .Cast<MarkdownDescription>()
                .Where(x => x!=null && !hidden.Contains(x.Path));

            foreach (var pageDescription in pageDescriptions)
            {
                AppendLink(pageDescription.Path, pageDescription.Path);

            }

            return this;
        }

        public MarkDownBuilder AppendRange<T>(IEnumerable<T> elements, Action<T, MarkDownBuilder> action)
        {
            foreach (var element in elements)
            {
                action(element, this);
            }
            return this;
        }
        public MarkDownBuilder AppendIf<T>(T element, Func<T, bool> validation, Action<T, MarkDownBuilder> action)
        {
            if (validation(element))
                action(element, this);
            return this;
        }

        public MarkDownBuilder Append(string value)
        {
            _stringBuilder.AppendLine(value);
            return this;
        }

        public MarkDownBuilder AppendTitle(string title)
        {
            _stringBuilder.AppendLine($"# {title}");
            return this;
        }
        public MarkDownBuilder AppendTitle2(string title)
        {
            _stringBuilder.AppendLine($"## {title}");
            return this;
        }
        public MarkDownBuilder AppendTitle3(string title)
        {
            _stringBuilder.AppendLine($"### {title}");
            return this;
        }
        public MarkDownBuilder AppendTitle4(string title)
        {
            _stringBuilder.AppendLine($"#### {title}");
            return this;
        }
        public MarkDownBuilder AppendParagraph(string text)
        {
            _stringBuilder.AppendLine(text);
            return this;
        }


        public MarkDownBuilder AppendLink(string text, string link)
        {
            _stringBuilder.AppendLine($"[{text}]({link})");
            _stringBuilder.AppendLine(string.Empty);
            return this;
        }

        public override string ToString()
        {
            return _stringBuilder.ToString();
        }
    }
}
