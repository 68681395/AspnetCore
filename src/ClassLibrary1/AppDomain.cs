using System.Collections.Generic;
using System.Reflection;

namespace System
{
    internal class AppDomain
    {
        static AppDomain()
        {
            CurrentDomain = new AppDomain();
        }


        public static AppDomain CurrentDomain { get; private set; }
        public Setup SetupInformation { get; internal set; }

        internal IEnumerable<Assembly> GetAssemblies()
        {
            throw new NotImplementedException();
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