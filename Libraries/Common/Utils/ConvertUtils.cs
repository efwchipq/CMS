
#region "命名空间"

using System;
using System.Globalization;
using System.Text;

#endregion

namespace Common.Utils {

    /// <summary>
    /// 数据类型转换工具类
    /// </summary>
    public static class ConvertUtils {

        /// <summary>
        /// 转为字符串
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns></returns> 
        public static string To(object value) {
            return To(value, string.Empty);
        }
        /// <summary>
        /// 数据类型转换
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="value">源数据</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>结果</returns>
        public static T To<T>(object value, T defaultValue) {
            /*          T obj;
                      try {
                          if (value == null) {
                              return defaultValue;
                          }
                          obj = (T)Convert.ChangeType(value, typeof(T));
                          if (obj == null) {
                              obj = defaultValue;
                          }
                      } catch {
                          obj = defaultValue;
                      }
                      return obj;*/
            T obj = default(T);
            try {
                if (value == null) {
                    return defaultValue;
                }
                var valueType = value.GetType();
                var targetType = typeof(T);
            tag1:
                if (valueType == targetType) {
                    return (T)value;
                }
                if (targetType.IsEnum) {
                    if (value is string) {
                        return (T)Enum.Parse(targetType, value as string);
                    } else {
                        return (T)Enum.ToObject(targetType, value);
                    }
                }
                if (targetType == typeof(Guid) && value is string) {
                    object obj1 = new Guid(value as string);
                    return (T)obj1;

                }
                if (targetType == typeof(DateTime) && value is string) {
                    DateTime d1;
                    if (DateTime.TryParse(value as string, out d1)) {
                        object obj1 = d1;
                        return (T)obj1;
                    }


                }
                if (targetType.IsGenericType) {
                    if (targetType.GetGenericTypeDefinition() == typeof(Nullable<>)) {
                        targetType = Nullable.GetUnderlyingType(targetType);
                        goto tag1;
                    }
                }
                if (value is IConvertible) {
                    obj = (T)Convert.ChangeType(value, typeof(T));
                }

                if (obj == null) {
                    obj = defaultValue;
                }
            } catch {
                obj = defaultValue;
            }
            return obj;
        }
        ///// <summary>
        ///// 将对象转换Js字符串
        ///// </summary>
        ///// <param name="item"></param>
        ///// <returns>Js字符串</returns>
        //public static string ToJs(object item) {

        //    if (item is string) {
        //        return "'" + ((string)item).Replace("\\", "\\\\").Replace("'", "\\'").Replace("\n", "\\n").Replace("\t", "\\t").Replace("\r", "") + "'";
        //    } else if (item is bool) {
        //        return item.ToString().ToLower();
        //    } else if (item is Enum) {
        //        return ((int)item).ToString();
        //    } else if (item is DateTime) {
        //        return ToJsDate((DateTime)item);
        //    } else if (item is Decimal || item is Double) {
        //        return string.Format(CultureInfo.InvariantCulture, "{0}", item);
        //    } else if (item is Int16 || item is Int32 || item is Int64) {
        //        return item.ToString();
        //    } else if (item != null) {
        //        var jss = new JavaScriptSerializer();
        //        return jss.Serialize(item);
        //    } else {
        //        return string.Empty;
        //    }
        //}
        /// <summary>
        /// 转换为js 脚本可以使用的 Date
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string ToJsDate(DateTime d) {

            var dateConstructor = new StringBuilder();
            dateConstructor.Append("new Date(");
            dateConstructor.Append(d.Year);
            dateConstructor.Append(",");
            dateConstructor.Append(d.Month - 1); // In JavaScript, months are zero-delimited
            dateConstructor.Append(",");
            dateConstructor.Append(d.Day);
            dateConstructor.Append(",");
            dateConstructor.Append(d.Hour);
            dateConstructor.Append(",");
            dateConstructor.Append(d.Minute);
            dateConstructor.Append(",");
            dateConstructor.Append(d.Second);
            if (d.Millisecond > 0) {
                dateConstructor.Append(",");
                dateConstructor.Append(d.Millisecond);
            }
            dateConstructor.Append(")");
            return dateConstructor.ToString();
        }

        public static string ToSqlString(object value) {
            if (value == null || value == DBNull.Value) {
                return "null";
            }
            var typeName = value.GetType().Name;
            switch (typeName) {
                case "Int16":
                case "Int32":
                case "Int64":
                case "Single":
                case "Double":
                case "Decimal":
                    return value.ToString();
                case "DateTime":
                    return string.Format("'{0:yyyy-MM-dd HH:mm:ss}'", (DateTime)value);
                case "String":
                case "Char":
                    return string.Format("'{0}'", value);
                case "Boolean":
                    return value.ToString();
                case "Guid":
                    return ((Guid)value).ToString();
                default:
                    return string.Format("'{0}'", value);

            }
        }

        /// <summary>         
        /// 获取中文首字母         
        /// </summary>     
        /// <param name="chineseStr">中文字符串</param>     
        /// <returns>首字母</returns>       
        public static string ToPinYinString(string chineseStr) {
            string capstr = string.Empty;
            string chinaStr = "";
            for (int i = 0; i <= chineseStr.Length - 1; i++) {
                string charStr = chineseStr.Substring(i, 1);
                byte[] chineseChar = Encoding.Default.GetBytes(charStr);
                if (chineseChar.Length == 2) {
                    int i1 = (short)(chineseChar[0]);
                    int i2 = (short)(chineseChar[1]);
                    long chineseStrInt = i1 * 256 + i2;
                    if ((chineseStrInt >= 45217) && (chineseStrInt <= 45252)) {
                        chinaStr = "a";
                    } else if ((chineseStrInt >= 45253) && (chineseStrInt <= 45760)) {
                        chinaStr = "b";
                    } else if ((chineseStrInt >= 45761) && (chineseStrInt <= 46317)) {
                        chinaStr = "c";
                    } else if ((chineseStrInt >= 46318) && (chineseStrInt <= 46825)) {
                        chinaStr = "d";
                    } else if ((chineseStrInt >= 46826) && (chineseStrInt <= 47009)) {
                        chinaStr = "e";
                    } else if ((chineseStrInt >= 47010) && (chineseStrInt <= 47296)) {
                        chinaStr = "f";
                    } else if ((chineseStrInt >= 47297) && (chineseStrInt <= 47613)) {
                        chinaStr = "g";
                    } else if ((chineseStrInt >= 47614) && (chineseStrInt <= 48118)) {
                        chinaStr = "h";
                    } else if ((chineseStrInt >= 48119) && (chineseStrInt <= 49061)) {
                        chinaStr = "j";
                    } else if ((chineseStrInt >= 49062) && (chineseStrInt <= 49323)) {
                        chinaStr = "k";
                    } else if ((chineseStrInt >= 49324) && (chineseStrInt <= 49895)) {
                        chinaStr = "l";
                    } else if ((chineseStrInt >= 49896) && (chineseStrInt <= 50370)) {
                        chinaStr = "m";
                    } else if ((chineseStrInt >= 50371) && (chineseStrInt <= 50613)) {
                        chinaStr = "n";
                    } else if ((chineseStrInt >= 50614) && (chineseStrInt <= 50621)) {
                        chinaStr = "o";
                    } else if ((chineseStrInt >= 50622) && (chineseStrInt <= 50905)) {
                        chinaStr = "p";
                    } else if ((chineseStrInt >= 50906) && (chineseStrInt <= 51386)) {
                        chinaStr = "q";
                    } else if ((chineseStrInt >= 51387) && (chineseStrInt <= 51445)) {
                        chinaStr = "r";
                    } else if ((chineseStrInt >= 51446) && (chineseStrInt <= 52217)) {
                        chinaStr = "s";
                    } else if ((chineseStrInt >= 52218) && (chineseStrInt <= 52697)) {
                        chinaStr = "t";
                    } else if ((chineseStrInt >= 52698) && (chineseStrInt <= 52979)) {
                        chinaStr = "w";
                    } else if ((chineseStrInt >= 52980) && (chineseStrInt <= 53640)) {
                        chinaStr = "x";
                    } else if ((chineseStrInt >= 53689) && (chineseStrInt <= 54480)) {
                        chinaStr = "y";
                    } else if ((chineseStrInt >= 54481) && (chineseStrInt <= 55289)) {
                        chinaStr = "z";
                    }
                    capstr += chinaStr;
                } else {
                    capstr += charStr;
                }

            }
            return capstr;
        }


    }
}
