using System;
using System.Linq;
using System.Reflection;
using System.Collections.Concurrent;

namespace TSharp.Core.Osgi.Internal
{
    /// <summary>
    /// 多版本Assembly
    /// </summary>
    public class MultiVersionAssembly
    {
        private readonly ConcurrentDictionary<Version, Assembly> _assemblys;
        private Assembly _currentVersionAssembly;
        private Assembly _latestVersionAssembly;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiVersionAssembly"/> class.
        /// </summary>
        public MultiVersionAssembly()
        {
            _assemblys = new ConcurrentDictionary<Version, Assembly>();
            _latestVersionAssembly = null;
        }

        /// <summary>
        /// Gets the latest version assembly.
        /// </summary>
        /// <value>The latest version assembly.</value>
        public Assembly LatestVersionAssembly
        {
            get { return _latestVersionAssembly; }
        }

        /// <summary>
        /// Gets the current version assembly.
        /// </summary>
        /// <value>The current version assembly.</value>
        public Assembly CurrentVersionAssembly
        {
            get { return _currentVersionAssembly; }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return _latestVersionAssembly.GetName().Name; }
        }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>The version.</value>
        public Version Version
        {
            get { return _currentVersionAssembly.GetName().Version; }
        }

        /// <summary>
        /// 根据版本获取
        /// </summary>
        /// <value></value>
        public Assembly this[Version version]
        {
            get
            {
                if (version != null)
                {
                    Assembly assembly;
                    if (_assemblys.TryGetValue(version, out assembly))
                        return assembly;
                }
                return null;
            }
        }

        /// <summary>
        /// Determines whether [is latest version] [the specified assembly].
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>
        /// 	<c>true</c> if [is latest version] [the specified assembly]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsLatestVersion(Assembly assembly)
        {
            return _latestVersionAssembly == null
                   || assembly.GetName().Version.CompareTo(_latestVersionAssembly.GetName().Version) > 0;
        }

        /// <summary>
        /// Adds the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        public MultiVersionAssembly Add(Assembly assembly)
        {
            return Add(assembly, true);
        }

        /// <summary>
        /// Adds the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="update">if set to <c>true</c> [update].</param>
        /// <returns></returns>
        public MultiVersionAssembly Add(Assembly assembly, bool update)
        {
            Version ver = assembly.GetName().Version;
            if (!_assemblys.ContainsKey(ver))
            {
                if (_latestVersionAssembly == null
                    || ver.CompareTo(_latestVersionAssembly.GetName().Version) > 0)
                {
                    _latestVersionAssembly = assembly;
                    if (update)
                    {
                        _currentVersionAssembly = assembly;
                    }
                }
                _assemblys[ver] = assembly;
            }
            return this;
        }

        /// <summary>
        /// Gets the assemblys.
        /// </summary>
        /// <returns></returns>
        public Assembly[] GetAssemblys()
        {
            return _assemblys.Values.OrderBy(KeySelector).ToArray();
        }

        private static Version KeySelector(Assembly assembly)
        {
            return assembly.GetName().Version;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// 	<paramref name="obj"/> 参数为 null。
        /// </exception>
        public override bool Equals(object obj)
        {
            return Name.Equals(((MultiVersionAssembly)obj).Name);
        }
    }
}