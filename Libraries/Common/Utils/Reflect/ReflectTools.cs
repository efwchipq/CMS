using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utils.Reflect {
    public class ReflectTools {

        public static TValue GetValue<T, TValue>(T entity, string propertyName, TValue defaultValue) {
            var type = entity.GetType();
            var property = type.GetProperty(propertyName);
            if (property == null) {
                return defaultValue;
            }
            if (property.CanRead) {
                var value = property.GetValue(entity);
                return ConvertUtils.To(value, defaultValue);
            } else {
                return defaultValue;
            }
        }

    }




    public delegate void PropertySetter<T>(T value);
    public delegate T PropertyGetter<T>();

    //public partial class Tools_TestGrid : System.Web.UI.Page {
    //    #region 泛型委托实现
    //    public PropertyGetter<int> PropGet;
    //    public PropertySetter<int> PropSet;
    //    public void BuildSetMethod(TestData td) {
    //        Type t = td.GetType();
    //        PropertyInfo pi = t.GetProperty("Name");
    //        MethodInfo setter = pi.GetSetMethod();

    //        PropSet = (PropertySetter<int>)Delegate.CreateDelegate(typeof(PropertySetter<int>), td, setter);

    //        //string value = strPropGetter();
    //    }

    //    public void BuildGetMethod(TestData td) {
    //        Type t = td.GetType();
    //        PropertyInfo pi = t.GetProperty("Name");
    //        MethodInfo getter = pi.GetGetMethod();

    //        PropGet = (PropertyGetter<int>)Delegate.CreateDelegate(typeof(PropertyGetter<int>), td, getter);

    //        //string value = strPropGetter();
    //    }

    //    #endregion

    //    #region 表达式树实现
    //    Func<object, int> LmdGetProp; //Func<TestData, int>
    //    public void LmdGet(Type entityType, string propName) {
    //        #region 通过方法取值
    //        var p = entityType.GetProperty(propName);
    //        //对象实例
    //        var param_obj = Expression.Parameter(typeof(object), "obj");
    //        //值
    //        //var param_val = Expression.Parameter(typeof(object), "val");
    //        //转换参数为真实类型
    //        var body_obj = Expression.Convert(param_obj, entityType);

    //        //调用获取属性的方法
    //        var body = Expression.Call(body_obj, p.GetGetMethod());
    //        LmdGetProp = Expression.Lambda<Func<object, int>>(body, param_obj).Compile();
    //        #endregion

    //        #region 表达式取值
    //        //var p = entityType.GetProperty(propName);
    //        ////lambda的参数u
    //        //var param_u = Expression.Parameter(entityType, "u");
    //        ////lambda的方法体 u.Age
    //        //var pGetter = Expression.Property(param_u, p);
    //        ////编译lambda
    //        //LmdGetProp = Expression.Lambda<Func<TestData, int>>(pGetter, param_u).Compile();
    //        #endregion
    //    }
    //    Action<object, object> LmdSetProp;
    //    public void LmdSet(Type entityType, string propName) {
    //        var p = entityType.GetProperty(propName);
    //        //对象实例
    //        var param_obj = Expression.Parameter(typeof(object), "obj");
    //        //值
    //        var param_val = Expression.Parameter(typeof(object), "val");
    //        //转换参数为真实类型
    //        var body_obj = Expression.Convert(param_obj, entityType);
    //        var body_val = Expression.Convert(param_val, p.PropertyType);
    //        //调用给属性赋值的方法
    //        var body = Expression.Call(body_obj, p.GetSetMethod(), body_val);
    //        LmdSetProp = Expression.Lambda<Action<object, object>>(body, param_obj, param_val).Compile();

    //    }
    //    #endregion


    //    #region Emit动态方法实现
    //    public delegate void SetValueDelegateHandler(TestData entity, object value);
    //    public SetValueDelegateHandler EmitSetValue;
    //    public void BuildEmitMethod(Type entityType, string propertyName) {
    //        //Type entityType = entity.GetType();
    //        Type parmType = typeof(object);
    //        // 指定函数名
    //        string methodName = "set_" + propertyName;
    //        // 搜索函数，不区分大小写 IgnoreCase
    //        var callMethod = entityType.GetMethod(methodName, BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic);
    //        // 获取参数
    //        var para = callMethod.GetParameters()[0];
    //        // 创建动态函数
    //        DynamicMethod method = new DynamicMethod("EmitCallable", null, new Type[] { entityType, parmType }, entityType.Module);
    //        // 获取动态函数的 IL 生成器
    //        var il = method.GetILGenerator();
    //        // 创建一个本地变量，主要用于 Object Type to Propety Type
    //        var local = il.DeclareLocal(para.ParameterType, true);
    //        // 加载第 2 个参数【(T owner, object value)】的 value
    //        il.Emit(OpCodes.Ldarg_1);
    //        if (para.ParameterType.IsValueType) {
    //            il.Emit(OpCodes.Unbox_Any, para.ParameterType);// 如果是值类型，拆箱 string = (string)object;
    //        } else {
    //            il.Emit(OpCodes.Castclass, para.ParameterType);// 如果是引用类型，转换 Class = object as Class
    //        }
    //        il.Emit(OpCodes.Stloc, local);// 将上面的拆箱或转换，赋值到本地变量，现在这个本地变量是一个与目标函数相同数据类型的字段了。
    //        il.Emit(OpCodes.Ldarg_0); // 加载第一个参数 owner
    //        il.Emit(OpCodes.Ldloc, local);// 加载本地参数
    //        il.EmitCall(OpCodes.Callvirt, callMethod, null);//调用函数
    //        il.Emit(OpCodes.Ret); // 返回
    //        /* 生成的动态函数类似：
    //        * void EmitCallable(T owner, object value)
    //        * {
    //        * T local = (T)value;
    //        * owner.Method(local);
    //        * }
    //        */

    //        EmitSetValue = method.CreateDelegate(typeof(SetValueDelegateHandler)) as SetValueDelegateHandler;

    //    }
    //    #endregion

    //    protected void Page_Load(object sender, EventArgs e) {

    //        this.Response.Write("当前framework版本：" + Environment.Version.Major + "<br/>");
    //        int max = 1000000;
    //        this.Response.Write("循环次数：" + max + "<br/>");
    //        if (!IsPostBack) {
    //            //基本方法
    //            DateTime time = DateTime.Now;
    //            TestData d = new TestData();
    //            for (int i = 0; i < max; i++) {
    //                d.Name = i;
    //            }
    //            TimeSpan ts = DateTime.Now - time;
    //            this.Response.Write("基本方法:" + ts.TotalMilliseconds + "<br/>");

    //            //反射方法
    //            Type type = d.GetType();
    //            PropertyInfo pi = type.GetProperty("Name");
    //            time = DateTime.Now;
    //            for (int i = 0; i < max; i++) {
    //                pi.SetValue(d, i, null);
    //            }
    //            ts = DateTime.Now - time;
    //            this.Response.Write("反射方法:" + ts.TotalMilliseconds + "<br/>");

    //            //dynamic动态类型方法
    //            dynamic dobj = Activator.CreateInstance<TestData>();
    //            time = DateTime.Now;
    //            for (int i = 0; i < max; i++) {
    //                dobj.Name = i;
    //            }
    //            ts = DateTime.Now - time;
    //            this.Response.Write("dynamic动态类型方法:" + ts.TotalMilliseconds + "<br/>");

    //            //泛型委托赋值方法
    //            d.Name = -1;
    //            BuildSetMethod(d);
    //            time = DateTime.Now;
    //            for (int i = 0; i < max; i++) {
    //                this.PropSet(i);
    //            }
    //            ts = DateTime.Now - time;
    //            this.Response.Write("泛型委托赋值方法:" + ts.TotalMilliseconds + "<br/>");
    //            this.Response.Write("v:" + d.Name + "<br/>");

    //            //泛型委托取值方法
    //            d.Name = -1;
    //            BuildGetMethod(d);
    //            time = DateTime.Now;
    //            for (int i = 0; i < max; i++) {
    //                this.PropGet();
    //            }
    //            ts = DateTime.Now - time;
    //            this.Response.Write("泛型委托取值方法:" + ts.TotalMilliseconds + "<br/>");
    //            this.Response.Write("v:" + d.Name + "<br/>");

    //            //表达式树赋值方法
    //            d.Name = -1;
    //            LmdSet(typeof(TestData), "Name");
    //            time = DateTime.Now;
    //            for (int i = 0; i < max; i++) {
    //                this.LmdSetProp(d, i);
    //            }
    //            ts = DateTime.Now - time;
    //            this.Response.Write("表达式树赋值方法:" + ts.TotalMilliseconds + "<br/>");
    //            this.Response.Write("v:" + d.Name + "<br/>");

    //            //表达式树取值方法
    //            d.Name = -132;
    //            this.LmdGet(typeof(TestData), "Name");
    //            time = DateTime.Now;
    //            for (int i = 0; i < max; i++) {
    //                this.LmdGetProp(d);
    //            }
    //            ts = DateTime.Now - time;
    //            this.Response.Write("表达式树取值方法:" + ts.TotalMilliseconds + "<br/>");
    //            this.Response.Write("v:" + this.LmdGetProp(d) + "<br/>");

    //            //EMIT动态方法赋值
    //            d.Name = -1;
    //            this.BuildEmitMethod(d.GetType(), "Name");
    //            time = DateTime.Now;
    //            for (int i = 0; i < max; i++) {
    //                this.EmitSetValue(d, i);
    //            }
    //            ts = DateTime.Now - time;
    //            this.Response.Write("EMIT动态方法:" + ts.TotalMilliseconds + "<br/>");
    //            this.Response.Write("v:" + d.Name + "<br/>");

    //            //TestDataBind();
    //        }
    //    }

    //}

    public class TestData {
        public int Name {
            get;
            set;
        }
    }


}
