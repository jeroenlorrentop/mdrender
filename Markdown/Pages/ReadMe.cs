using Markdown.Builder;
using Markdown.Render;

namespace Markdown.Pages
{
    [MarkdownDescription("")]
    public class ReadMe
    {
        private readonly MarkDownBuilder _builder;

        public ReadMe(MarkDownBuilder builder)
        {
            _builder = builder;
        }

        public string GetContent()
        {
            _builder.AppendPageTable();
            return _builder.ToString();
        }
    }




}
