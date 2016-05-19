namespace Globalstech.Core.Models {
    /// <summary>
    /// 生命周期单一实例依赖,表示在同一生命周期中只存在一个实例
    /// </summary>
    public interface IDependency {}
    /// <summary>
    /// 单一实例依赖,表示全局共享唯一实例
    /// </summary>
    public interface ISingletonDependency : IDependency {}
    /// <summary>
    /// 瞬态实例依赖,表示将会为每个依赖对象项创建唯一的实例(即实例化多个)
    /// </summary>
    public interface ITransientDependency : IDependency {}
}
