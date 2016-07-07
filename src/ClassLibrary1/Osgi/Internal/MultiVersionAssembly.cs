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
        private readonly ConcurrentDictionary<Version, Assembly> assemblys;
        private Assembly currentVersionAssembly;
        private Assembly latestVersionAssembly;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiVersionAssembly"/> class.
        /// </summary>
        public MultiVersionAssembly()
        {
            this.assemblys = new ConcurrentDictionary<Version, Assembly>();
            this.latestVersionAssembly = null;
        }

        /// <summary>
        /// Gets the latest version assembly.
        /// </summary>
        /// <value>The latest version assembly.</value>
        public Assembly LatestVersionAssembly => this.latestVersionAssembly;

        /// <summary>
        /// Gets the current version assembly.
        /// </summary>
        /// <value>The current version assembly.</value>
        public Assembly CurrentVersionAssembly => this.currentVersionAssembly;

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return this.latestVersionAssembly.GetName().Name; }
        }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>The version.</value>
        public Version Version => this.currentVersionAssembly.GetName().Version;

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
                    if (this.assemblys.TryGetValue(version, out assembly))
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
            return this.latestVersionAssembly == null
                   || assembly.GetName().Version.CompareTo(this.latestVersionAssembly.GetName().Version) > 0;
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
            if (!this.assemblys.ContainsKey(ver))
            {
                if (this.latestVersionAssembly == null
                    || ver.CompareTo(this.latestVersionAssembly.GetName().Version) > 0)
                {
                    this.latestVersionAssembly = assembly;
                    if (update)
                    {
                        this.currentVersionAssembly = assembly;
                    }
                }
                this.assemblys[ver] = assembly;
            }
            return this;
        }

        /// <summary>
        /// Gets the assemblys.
        /// </summary>
        /// <returns></returns>
        public Assembly[] GetAssemblys()
        {
            return this.assemblys.Values.OrderBy(KeySelector).ToArray();
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