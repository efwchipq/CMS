using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using Common.Utils.FilePath;

namespace Common.Utils.XML {
    public class XMLTools {

        /// <summary>
        /// 获取XML文件根节点
        /// </summary>
        /// <param name="filePath">文件名称</param>
        /// <param name="isRequest">是否是客户端请求</param>
        /// <returns></returns>
        public XElement GetRootElement(string filePath, bool isRequest = true) {
            var physicalPath = FilePathTools.GetFilePhysicalPath(filePath, isRequest);
            if (!File.Exists(physicalPath)) {
                throw new Exception("文件不存在");
            }
            XDocument xDoc = XDocument.Load(physicalPath);
            return xDoc.Root;
        }

        /// <summary>
        /// 获取下级XML节点集合(只有子集)
        /// </summary>
        /// <param name="element">源XML节点</param>
        /// <param name="nodeName">下级节点名称</param>
        /// <returns></returns>
        public IEnumerable<XElement> GetElements(XElement element, string nodeName) {
            return element.Elements(nodeName);//获取的子级还是所有下级？
        }

        /// <summary>
        /// 获取下级XML节点
        /// </summary>
        /// <param name="element">源XML节点</param>
        /// <param name="nodeName">下级节点名称</param>
        /// <returns></returns>
        public XElement GetElement(XElement element, string nodeName) {
            return element.Element(nodeName);
        }

        /// <summary>
        /// 获取XML节点值
        /// </summary>
        /// <param name="element">源XML节点</param>
        /// <returns></returns>
        public string GetElementValue(XElement element) {
            if (element != null && (!element.HasElements)) {
                return element.Value;
            }
            return null;
        }

        /// <summary>
        /// 获取XML节点属性值
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public string GetElementValue(XElement element, string attributeName) {
            if (element != null && (!element.HasElements)) {
                //return element.Attribute[attributeName];
            }
            return null;
        }

        /// <summary>
        /// 读取最下级元素内容
        /// </summary>
        /// <param name="element">最下级元素的父级元素</param>
        /// <param name="elementName">最下级元素名称</param>
        /// <returns></returns>
        public string GetChildElenmentValue(XElement element, string elementName) {
            if (element != null) {
                element = element.Element(elementName);
                if (element != null && (!element.HasElements)) {
                    return element.Value;
                }
            }
            return null;
        }

        /// <summary>
        /// 反射赋值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element"></param>
        /// <returns></returns>
        public T GetEntity<T>(XElement element) where T : class {
            var type = typeof(T);
            //使用默认构造函数创造类实例
            object obj = Activator.CreateInstance(type);
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var propertyInfo in props) {
                var value = GetChildElenmentValue(element, propertyInfo.Name);
                var propertyValue = ChangeType(value, propertyInfo.PropertyType);
                propertyInfo.SetValue(obj, propertyValue);
            }
            return (T)obj;
        }

        static public object ChangeType(object value, Type type) {
            if (value == null && type.IsGenericType)
                return Activator.CreateInstance(type);
            if (value == null)
                return null;
            if (type == value.GetType())
                return value;
            if (type.IsEnum) {
                if (value is string)
                    return Enum.Parse(type, value as string);
                else
                    return Enum.ToObject(type, value);
            }
            if (!type.IsInterface && type.IsGenericType) {
                Type innerType = type.GetGenericArguments()[0];
                object innerValue = ChangeType(value, innerType);
                return Activator.CreateInstance(type, new object[] { innerValue });
            }
            if (value is string && type == typeof(Guid))
                return new Guid(value as string);
            if (value is string && type == typeof(Version))
                return new Version(value as string);
            if (!(value is IConvertible))
                return value;
            return Convert.ChangeType(value, type);
        }

    }
}
