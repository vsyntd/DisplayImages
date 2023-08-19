using DuckGame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DisplayImages;

namespace BetterCommunication
{
    public static class Resolver
    {
        public static void ResolveDependencies()
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(GetDependency);
        }
        private static Assembly GetDependency(object sender, ResolveEventArgs args)
        {
            string assemblyFullName = args.Name;
            string assemblyShortName = assemblyFullName;

            try
            {
                assemblyShortName = assemblyFullName.Substring(0, assemblyFullName.IndexOf(",", StringComparison.Ordinal));
            }
            catch (Exception)
            {

            }

            if (Assembly.GetCallingAssembly() != Assembly.GetExecutingAssembly())
            {
                return null;
            }

            try
            {
                var assembly = AppDomain.CurrentDomain.GetAssemblies().First(x => x.FullName == assemblyFullName);

                if (assembly != null)
                {
                    return assembly;
                }
            }
            catch (InvalidOperationException)
            {

            }
            //string path2 = Mod.GetPath<Translator>("/files/" + assemblyShortName + ".exe");
            string path1 = Mod.GetPath<DisplayImages.DisplayImages>(assemblyShortName + ".dll");

            Assembly loadedAssembly = null;

            if (File.Exists(path1))
            {
                try
                {
                    loadedAssembly = Assembly.LoadFrom(path1);
                }
                catch (Exception)
                {
                    try
                    {
                        loadedAssembly = Assembly.Load(File.ReadAllBytes(path1));
                    }
                    catch (Exception)
                    {

                    }
                }

            }
            return loadedAssembly;
        }
    }
}
