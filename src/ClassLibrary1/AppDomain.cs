using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System
{
    internal class AppDomain
    {
        static AppDomain()
        {
            CurrentDomain = new AppDomain();
            CurrentDomain.SetupInformation = new Setup();
        }


        public static AppDomain CurrentDomain { get; private set; }
        public Setup SetupInformation { get; internal set; }

        internal IEnumerable<Assembly> GetAssemblies()
        {
            return Assembly.GetEntryAssembly().GetReferencedAssemblies().Select(x => Assembly.Load(x));

           // throw new NotImplementedException();
        }
    }
    class Setup
    {
        public string ConfigurationFile
        {
            get { return AppContext.BaseDirectory; }
            set
            {

            }
        }
    }

}