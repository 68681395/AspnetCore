using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class Class
    {
        public static void Main(string[] args)
        {
            var ctx = new MyAssemblyLoadContext();
            var d=AssemblyLoadContext.Default.LoadFromAssemblyPath("");
         
            var asm = Assembly.Load(new AssemblyName("Lib"));
        }

    }


    public class MyAssemblyLoadContext : AssemblyLoadContext
    {
        protected override Assembly Load(AssemblyName assemblyName)
        {
            return base.LoadFromAssemblyPath("C:\\Github\\DefALC\\1\\" + assemblyName.Name + ".dll");
        }
    }
}
