using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.Configuration;

namespace System
{
    public static class StringExtension
    {
        public static T GetAppSetting<T>(this string obj, T devalutValue)
        {
            IConfiguration cfg = null;
            return default(T);
        }
    }
}

namespace System.Reflection
{

    public static class AssemblyExtension
    {
        public static Assembly LoadFrom(this Assembly assm, string assemblyPath)
        { 
            return AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyPath);
        }
    }
}