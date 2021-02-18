using System;

namespace Markdown.Render
{
    public class MarkdownDescription : Attribute
    {
        public MarkdownDescription(string path)
        {
            Path = path;
        }
        public string Path { get; set; }
    }
}
