using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using DomainServices.EnvironmentConfiguration.ConfigModule;

namespace TechnicalServices.Configuration.LoadModules
{
    public class AssemblyReflectionLoadHelper : MarshalByRefObject
    {
        private readonly Dictionary<string, Assembly> _assemblies;

        public AssemblyReflectionLoadHelper()
        {
            _assemblies = new Dictionary<string, Assembly>();
            AppDomain.CurrentDomain.GetAssemblies().ToList().ForEach(a => _assemblies.Add(a.FullName, a));
        }

        /// <summary>
        /// Обработчик ReflectionOnlyAssemblyResolve
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private Assembly handlerResolveEvent(object sender, ResolveEventArgs args)
        {
            if (_assemblies.ContainsKey(args.Name))
                return Assembly.ReflectionOnlyLoadFrom(_assemblies[args.Name].CodeBase);

            Assembly result = Assembly.ReflectionOnlyLoad(args.Name);
            _assemblies.Add(result.FullName, result);
            return result;
        }

        public string[] GetFilesByModuleAttribute(string[] files)
        {
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += handlerResolveEvent;

            List<string> result = new List<string>(8);
            foreach (string fileName in files)
            {
                try
                {
                    string fullName = Path.GetFullPath(fileName);
                    string name = Path.GetFileName(fullName);

                    Assembly lib;
                    if (!_assemblies.ContainsKey(name))
                    {
                        lib = Assembly.ReflectionOnlyLoadFrom(fullName);
                        _assemblies.Add(name, lib);
                    }
                    else
                        lib = _assemblies[name];

                    IList<CustomAttributeData> list = CustomAttributeData.GetCustomAttributes(lib);
                    foreach (CustomAttributeData attr in list)
                    {
                        if (attr.Constructor.DeclaringType.AssemblyQualifiedName ==
                            typeof (ModuleAttribute).AssemblyQualifiedName)
                            result.Add(fullName);
                    }
                }
                catch (Exception)
                {
                }
            }

            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= handlerResolveEvent;
            return result.ToArray();
        }

        /// <summary>
        /// Получение сборок содержащих атрибут ModuleAttribute
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public static IEnumerable<string> AppDomainGetFilesByModuleAttribute(IEnumerable<string> files)
        {
            AppDomain domain = AppDomain.CreateDomain("ReflectDomain");

            string asmName = Assembly.GetExecutingAssembly().CodeBase;
            string typeName = typeof (AssemblyReflectionLoadHelper).FullName;

            AssemblyReflectionLoadHelper helper =
                (AssemblyReflectionLoadHelper) domain.CreateInstanceFromAndUnwrap(asmName, typeName);
            string[] result = helper.GetFilesByModuleAttribute(files.ToArray());

            AppDomain.Unload(domain);

            return result;
        }
    }
}