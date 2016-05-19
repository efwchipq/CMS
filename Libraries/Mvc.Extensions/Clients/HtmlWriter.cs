using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Mvc.Extensions.Clients {
    /// <summary>
    /// Html构建
    /// </summary>
    public class HtmlWriter : HtmlAttributeWriter {

        private readonly StringBuilder _builder = new StringBuilder();
        private readonly Stack<string> _tags = new Stack<string>();

        public void Write(IHtmlContent content) {
            if (content != null) {
                Write(content.ToHtmlString());
            }
        }

        public void Write(object obj) {
            if (obj != null && obj.ToString() != "") {
                _builder.Append(obj);
            }
        }

        public void WriteLine(object obj = null) {
            if (obj != null) {
                Write(obj);
            }
            _builder.AppendLine();
        }

        public void WriteIndent(int count = 1) {
            _builder.Append(String.Empty.PadLeft(count, '\t'));
        }

        public void Tag(string tagName) {
            WriteTag(tagName);
        }

        public void WriteTag(string tagName) {
            WriteLine();
            Write("<");
            Write(tagName);
            Write(FlushStringAttribtes());
            Write(">");
            _tags.Push(tagName);
        }

        public void WriteCloseTag(string tagName) {

            Write("<");
            Write(tagName);
            Write(FlushStringAttribtes());
            Write(" />");
            WriteLine();

        }

        public void EndTag() {
            if (_tags.Count == 0) {
                return;
            }
            var current = _tags.Pop();
            if (string.IsNullOrEmpty(current)) {
                throw new Exception("没有匹配的开始标签");
            }
            Write($"</{current}>");

        }

        public void End() {
            EndTag();
        }

        public override string ToString() {
            if (_tags.Count > 0) {
                while (_tags.Count > 0) {
                    EndTag();
                }
            }
            return _builder.ToString();
        }

        public HtmlString ToHtmlString() {
            return new HtmlString(ToString());
        }

    }

    public enum Tags {
        [Text("div")]
        Div,
        [Text("span")]
        Span,
        Input,
        A,
        B,
        Link

    }
}
