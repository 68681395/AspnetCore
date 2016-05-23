using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.PlatformAbstractions;

namespace Common.Logging
{



    public class DirectoryLoader : IAssemblyLoader
    {
        private readonly IAssemblyLoadContext _context;
        private readonly DirectoryInfo _path;

        public DirectoryLoader(DirectoryInfo path, IAssemblyLoadContext context)
        {
            _path = path;
            _context = context;
        }

        public Assembly Load(AssemblyName assemblyName)
        {
            return _context.LoadFile(Path.Combine(_path.FullName, assemblyName.Name + ".dll"));
        }

        public IntPtr LoadUnmanagedLibrary(string name)
        {
            //this isn't going to load any unmanaged libraries, just throw
            throw new NotImplementedException();
        }
    }

    public class Class1
    {
    }
}

namespace System
{

    public static class StringExtension
    {
        public static T GetAppSetting<T>(this string obj, T devalutValue)
        {
            return default(T);
        }
    }

}