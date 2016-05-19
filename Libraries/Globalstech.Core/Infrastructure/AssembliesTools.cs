using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;

namespace Globalstech.Core.Infrastructure {
    public static class AssembliesTools {

        /// <summary>Gets the assemblies related to the current implementation.</summary>
        /// <returns>A list of assemblies that should be loaded by the Nop factory.</returns>
        public static IList<Assembly> GetAssemblies() {
            var addedAssemblyNames = new List<string>();
            var assemblies = new List<Assembly>();

            AddAssembliesInAppDomain(addedAssemblyNames, assemblies);
            //AddConfiguredAssemblies(addedAssemblyNames, assemblies);

            return assemblies;
        }

        /// <summary>
        /// Iterates all assemblies in the AppDomain and if it's name matches the configured patterns add it to our list.
        /// </summary>
        /// <param name="addedAssemblyNames"></param>
        /// <param name="assemblies"></param>
        private static void AddAssembliesInAppDomain(List<string> addedAssemblyNames, List<Assembly> assemblies) {
            //AppDomain.CurrentDomain.GetAssemblies()和BuildManager.GetReferencedAssemblies().Cast<Assembly>().ToList()
            //http://stackoverflow.com/questions/3552223/asp-net-appdomain-currentdomain-getassemblies-assemblies-missing-after-app
            // var assemblys = AppDomain.CurrentDomain.GetAssemblies();
            var assemblys = BuildManager.GetReferencedAssemblies().Cast<Assembly>().ToList();
            foreach (Assembly assembly in assemblys) {
                if (!addedAssemblyNames.Contains(assembly.FullName)) {
                    assemblies.Add(assembly);
                    addedAssemblyNames.Add(assembly.FullName);
                }
            }
        }

        ///// <summary>
        ///// Adds specificly configured assemblies.
        ///// </summary>
        ///// <param name="addedAssemblyNames"></param>
        ///// <param name="assemblies"></param>
        //public static void AddConfiguredAssemblies(List<string> addedAssemblyNames, List<Assembly> assemblies) {
        //    foreach (string assemblyName in AssemblyNames) {
        //        Assembly assembly = Assembly.Load(assemblyName);
        //        if (!addedAssemblyNames.Contains(assembly.FullName)) {
        //            assemblies.Add(assembly);
        //            addedAssemblyNames.Add(assembly.FullName);
        //        }
        //    }
        //}


        public static IEnumerable<Type> FindClassesOfType<T>(bool onlyConcreteClasses = true) {
            return FindClassesOfType(typeof(T), onlyConcreteClasses);
        }

        public static IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, bool onlyConcreteClasses = true) {
            return FindClassesOfType(assignTypeFrom, GetAssemblies(), onlyConcreteClasses);
        }

        public static IEnumerable<Type> FindClassesOfType<T>(IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true) {
            return FindClassesOfType(typeof(T), assemblies, onlyConcreteClasses);
        }

        public static IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true) {
            var result = new List<Type>();
            try {
                foreach (var a in assemblies) {
                    Type[] types = null;
                    try {
                        types = a.GetTypes();
                    } catch {
                        ////Entity Framework 6 doesn't allow getting types (throws an exception)
                        //if (!ignoreReflectionErrors) {
                        //    throw;
                        //}
                    }
                    if (types != null) {
                        foreach (var t in types) {
                            if (assignTypeFrom.IsAssignableFrom(t) || (assignTypeFrom.IsGenericTypeDefinition && DoesTypeImplementOpenGeneric(t, assignTypeFrom))) {
                                if (!t.IsInterface) {
                                    if (onlyConcreteClasses) {
                                        if (t.IsClass && !t.IsAbstract) {
                                            result.Add(t);
                                        }
                                    } else {
                                        result.Add(t);
                                    }
                                }
                            }
                        }
                    }
                }
            } catch (ReflectionTypeLoadException ex) {
                var msg = string.Empty;
                foreach (var e in ex.LoaderExceptions)
                    msg += e.Message + Environment.NewLine;

                var fail = new Exception(msg, ex);
                Debug.WriteLine(fail.Message, fail);

                throw fail;
            }
            return result;
        }

        /// <summary>
        /// Does type implement generic?
        /// </summary>
        /// <param name="type"></param>
        /// <param name="openGeneric"></param>
        /// <returns></returns>
        public static bool DoesTypeImplementOpenGeneric(Type type, Type openGeneric) {
            try {
                var genericTypeDefinition = openGeneric.GetGenericTypeDefinition();
                foreach (var implementedInterface in type.FindInterfaces((objType, objCriteria) => true, null)) {
                    if (!implementedInterface.IsGenericType)
                        continue;

                    var isMatch = genericTypeDefinition.IsAssignableFrom(implementedInterface.GetGenericTypeDefinition());
                    return isMatch;
                }
                return false;
            } catch {
                return false;
            }
        }


    }
}
