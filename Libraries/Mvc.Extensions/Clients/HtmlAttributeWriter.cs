using System;
using System.Collections.Generic;
using System.Linq;

namespace Mvc.Extensions.Clients {
    /// <summary>
    /// Html属性构建
    /// </summary>
    public class HtmlAttributeWriter {
        private readonly List<string> _styles = new List<string>();
        private readonly Dictionary<string, object> _attrs = new Dictionary<string, object>();
        private readonly List<string> _classes = new List<string>();
        private readonly List<string> _jsonAttr = new List<string>();

        public void WriteAttr(string name, object value, bool skipOnExists = false, bool isJson = false) {
            if (skipOnExists) {
                if (_attrs.ContainsKey(name)) {
                    return;
                }
            }
            if (isJson) {
                value = value?.ToString()
                              .Replace("\"", "$$")
                              .Replace("\'", "\\\"")
                              .Replace("$$", "'");
            }
            _attrs[name] = value;
        }

        public void WriteStyle(string styleName, string styleValue) {
            _styles.Add($"{styleName}:{styleValue}");
        }

        public void WriteData(string name, string value) {
            _attrs["data-" + name] = value;
        }

        public void WriteClass(params string[] @classes) {
            if (classes != null) {
                _classes.AddRange(classes);
            }
        }

        public void RemoveClass(string @class) {
            _classes.Remove(@class);
        }

        public void RemoveAttr(string attr) {
            _attrs.Remove(attr);
        }

        public void WriteName(string name, bool appendId = true) {
            WriteAttr("name", name);
            if (appendId) {
                WriteAttr("id", name, true);
            }
        }

        public void WriteId(string id) {
            WriteAttr("id", id);
        }

        public void WriteType(string type) {
            WriteAttr("type", type);
        }

        public void WriteHref(string url) {
            WriteAttr("href", url);
        }

        public void WriteValue(object value, bool isJson = false) {
            WriteAttr("value", value?.ToString(), isJson: isJson);
        }

        public void WritePlaceHolder(string placeHolder) {
            WriteAttr("placeholder", placeHolder);
        }

        public void WriteSrc(string url) {
            WriteAttr("src", url);
        }

        public void WriteJson(string name, object value) {
            _jsonAttr.Add($"{name}:{ConvertUtils.ToJs(value)}");
        }
        [JsonIgnore]
        public IDictionary<string, object> Attributes {
            get {
                PreprocessAttributes();
                return _attrs;
            }
        }
        /// <summary>
        /// 合并属性
        /// </summary>
        /// <param name="attrs"></param>

        public void MergeAttributes(IDictionary<string, object> attrs) {
            if (attrs != null) {
                foreach (var attr in attrs) {
                    object value = attr.Value;
                    if (attr.Value is HtmlString) {
                        value = ((HtmlString)attr.Value).ToHtmlString();
                    }
                    switch (attr.Key.ToLowerInvariant()) {
                        case "class":
                            WriteClass(value?.ToString().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                            break;
                        case "style":
                            var styles = value?.ToString().Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                            if (styles != null)
                                foreach (var style in styles) {
                                    var arr = style.Split(':');
                                    WriteStyle(arr[0], arr[1]);
                                }
                            break;
                        default:
                            WriteAttr(attr.Key, value);
                            break;
                    }
                    /*                    if (attr.Key.ToLowerInvariant() == "class") {
                                            WriteClass(attr.Value?.ToString().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                                        }
                                        if (attr.Key.ToLowerInvariant() == "style") {

                                        }*/
                }
            }
        }

        private void PreprocessAttributes() {
            if (_classes.Count > 0) {
                WriteAttr("class", string.Join(" ", _classes));
            }
            if (_styles.Count > 0) {
                WriteAttr("style", string.Join(";", _styles));
            }
        }

        private void ClearAttribtes() {
            _classes.Clear();
            _attrs.Clear();
            _styles.Clear();
        }
        /// <summary>
        /// 输出缓冲区的Json并清空已有
        /// </summary>
        /// <returns></returns>
        public string FlushJson() {
            if (_jsonAttr.Count == 0) {
                return String.Empty;
            }
            var res = $"{{{string.Join(",", _jsonAttr)}}}";
            _jsonAttr.Clear();
            return res;
        }

        public Dictionary<string, object> FlushAttribtes() {
            PreprocessAttributes();
            var flushAttribtes = new Dictionary<string, object>(_attrs);
            ClearAttribtes();
            return flushAttribtes;
        }

        public string FlushStringAttribtes() {
            PreprocessAttributes();
            var result = string.Empty;
            if (_attrs.Count > 0) {
                result = string.Join(" ", _attrs.Select(t => $"{t.Key}=\"{t.Value}\""));
                result = $" {result} ";
            }
            ClearAttribtes();
            return result;
        }
    }
}
