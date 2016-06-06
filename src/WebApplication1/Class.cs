using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.PlatformAbstractions;

namespace WebApplication9
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
    
    /// <summary>
    /// This will return assemblies found in App_Plugins plugin's /bin folders
    /// </summary>
    public class CustomDirectoryAssemblyProvider : IAssemblyProvider
    {
        private readonly IFileProvider _fileProvider;
        private readonly IAssemblyLoadContextAccessor _loadContextAccessor;
        private readonly IAssemblyLoaderContainer _assemblyLoaderContainer;

        public CustomDirectoryAssemblyProvider(
                IFileProvider fileProvider,
                IAssemblyLoadContextAccessor loadContextAccessor,
                IAssemblyLoaderContainer assemblyLoaderContainer)
        {
            _fileProvider = fileProvider;
            _loadContextAccessor = loadContextAccessor;
            _assemblyLoaderContainer = assemblyLoaderContainer;
        }

        public IEnumerable<Assembly> CandidateAssemblies
        {
            get
            {
                var content = _fileProvider.GetDirectoryContents("/App_Plugins");
                if (!content.Exists) yield break;
                foreach (var pluginDir in content.Where(x => x.IsDirectory))
                {
                    var binDir = new DirectoryInfo(Path.Combine(pluginDir.PhysicalPath, "bin"));
                    if (!binDir.Exists) continue;
                    foreach (var assembly in GetAssembliesInFolder(binDir))
                    {
                        yield return assembly;
                    }
                }
            }
        }

        /// <summary>
        /// Returns assemblies loaded from /bin folders inside of App_Plugins
        /// </summary>
        /// <param name="binPath"></param>
        /// <returns></returns>
        private IEnumerable<Assembly> GetAssembliesInFolder(DirectoryInfo binPath)
        {
            // Use the default load context
            var loadContext = _loadContextAccessor.Default;

            // Add the loader to the container so that any call to Assembly.Load 
            // will call the load context back (if it's not already loaded)
            using (_assemblyLoaderContainer.AddLoader(
                new DirectoryLoader(binPath, loadContext)))
            {
                foreach (var fileSystemInfo in binPath.GetFileSystemInfos("*.dll"))
                {
                    //// In theory you should be able to use Assembly.Load() here instead
                    //var assembly1 = Assembly.Load(AssemblyName.GetAssemblyName(fileSystemInfo.FullName));
                    var assembly2 = loadContext.Load(new AssemblyName(fileSystemInfo.FullName));
                    yield return assembly2;
                }
            }
        }
    }

    public class CompositeAssemblyProvider : DefaultAssemblyProvider
    {
        private readonly IAssemblyProvider[] _additionalProviders;
        private readonly string[] _referenceAssemblies;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="libraryManager"></param>
        /// <param name="additionalProviders">
        /// If passed in will concat the assemblies returned from these 
        /// providers with the default assemblies referenced
        /// </param>
        /// <param name="referenceAssemblies">
        /// If passed in it will filter the candidate libraries to ones
        /// that reference the assembly names passed in. 
        /// (i.e. "MyProduct.Web", "MyProduct.Core" )
        /// </param>
        public CompositeAssemblyProvider(
            ILibraryManager libraryManager,
            IAssemblyProvider[] additionalProviders = null,
            string[] referenceAssemblies = null) : base(libraryManager)
        {

            _additionalProviders = additionalProviders;
            _referenceAssemblies = referenceAssemblies;
        }

        /// <summary>
        /// Uses the default filter if a custom list of reference
        /// assemblies has not been provided
        /// </summary>
        protected override HashSet<string> ReferenceAssemblies
            => _referenceAssemblies == null
                ? base.ReferenceAssemblies
                : new HashSet<string>(_referenceAssemblies);

        /// <summary>
        /// Returns the base Libraries referenced along with any DLLs/Libraries
        /// returned from the custom IAssemblyProvider passed in
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<Library> GetCandidateLibraries()
        {
            var baseCandidates = base.GetCandidateLibraries();
            if (_additionalProviders == null) return baseCandidates;
            return baseCandidates
                .Concat(
                _additionalProviders.SelectMany(provider => provider.CandidateAssemblies.Select(
                    x => new Library(x.FullName, null, Path.GetDirectoryName(x.Location), null, Enumerable.Empty<string>(),
                        new[] { new AssemblyName(x.FullName) }))));
        }
    }
    
}
