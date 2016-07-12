// ***********************************************************************
// Assembly         : TSharp.Core
// Author           : admin
// Created          : 05-25-2013
//
// Last Modified By : admin
// Last Modified On : 05-26-2013
// ***********************************************************************
// <copyright file="IState.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace TSharp.Core
{
    using System;
    /// <summary>
    /// Interface IState
    /// </summary>
    public interface IState
    {
        /// <summary>
        /// Gets the sync root.
        /// </summary>
        /// <value>The sync root.</value>
        object SyncRoot { get; }
        /// <summary>
        /// Gets or sets the <see cref="System.Object" /> with the specified key.
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
}