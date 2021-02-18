using Markdown.Builder;
using Markdown.Render;

namespace Markdown.Pages
{
    [MarkdownDescription("something-else")]
    public class SomePage
    {
        private readonly MarkDownBuilder _builder;

        public SomePage(MarkDownBuilder builder)
        {
            _builder = builder;
        }

        public string GetContent()
        {
            _builder.AppendTitle("some other page");
            return _builder.ToString();
        }
    }




}
