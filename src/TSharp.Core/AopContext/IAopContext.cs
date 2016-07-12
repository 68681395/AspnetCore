using System;

namespace TSharp.Core
{

    /// <summary>
    /// Interface I
    /// </summary>
    public interface I
    {
        /// <summary>
        /// Gets the sync root.
        /// </summary>
        /// <value>The sync root.</value>
        object SyncRoot { get; }
        /// <summary>
        /// Gets or sets the <see cref="System.Object"/> with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.Object.</returns>
        object this[string key] { get; set; }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        void Remove(string key);

        /// <summary>
        /// Gets the or add.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="fac">The fac.</param>
        /// <returns>System.Object.</returns>
        object GetOrAdd(string key, Func<string, object> fac);
    }

    /// <summary>
    /// Interface IApplication
    /// </summary>
    public interface IApplication : I
    {

    }
    /// <summary>
    /// Interface ISession
    /// </summary>
    public interface ISession : I
    {
    }
    /// <summary>
    /// Interface IRequest
    /// </summary>
    public interface IRequest : I
    {
    }

}