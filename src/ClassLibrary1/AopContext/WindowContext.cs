// ***********************************************************************
// Assembly         : TSharp.Core
// Author           : tangjingbo
// Created          : 05-24-2013
//
// Last Modified By : tangjingbo
// Last Modified On : 05-24-2013
// ***********************************************************************
// <copyright file="WindowContext.cs" company="Extendsoft">
//     Copyright (c) Extendsoft. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Concurrent;

namespace TSharp.Core
{
    /// <summary>
    /// Class Class1
    /// </summary>
    public class WindowContext : IContext
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="WebContext"/> class from being created.
        /// </summary>
        private WindowContext()
        {

        }
        /// <summary>
        /// The _instance
        /// </summary>
        private static IContext _instance = new WindowContext();
        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static IContext Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// Class ApplicationImpl
        /// </summary>
        class ApplicationImpl : IApplicationState
        {
            /// <summary>
            /// The instance
            /// </summary>
            static internal ApplicationImpl Instance = new ApplicationImpl();
            /// <summary>
            /// Prevents a default instance of the <see cref="ApplicationImpl"/> class from being created.
            /// </summary>
            private ApplicationImpl()
            {

            }
            private volatile static ConcurrentDictionary<string, object> hash = new ConcurrentDictionary<string, object>();
            /// <summary>
            /// Gets or sets the <see cref="System.Object"/> with the specified key.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns>System.Object.</returns>
            public object this[string key]
            {
                get
                {
                    object value;
                    if (hash.TryGetValue(key, out value))
                        return value;
                    return null;
                }
                set
                {
                    hash.TryAdd(key, value);
                }
            }


            /// <summary>
            /// Removes the specified key.
            /// </summary>
            /// <param name="key">The key.</param>
            public void Remove(string key)
            {
                object value;
                hash.TryRemove(key, out value);
            }

            /// <summary>
            /// Gets the sync root.
            /// </summary>
            /// <value>The sync root.</value>
            /// <exception cref="NotImplementedException"></exception>
            /// <exception cref="System.NotImplementedException"></exception>
            public object SyncRoot
            {
                get { return hash; }
            }


            public object GetOrAdd(string key, Func<string, object> fac)
            {
                return hash.GetOrAdd(key, fac);
            }
        }

        /// <summary>
        /// Class ApplicationImpl,请求级别容器和session级别容器必须进行缓存，否则ioc容器会在每次请求时创建子容器，但又无法想web一样自动释放！
        /// </summary>
        class SessionImpl : ISessionState
        {
            /// <summary>
            /// The instance
            /// </summary>
            static internal SessionImpl Instance = new SessionImpl();
            /// <summary>
            /// Prevents a default instance of the <see cref="SessionImpl"/> class from being created.
            /// </summary>
            private SessionImpl()
            {

            }
            private volatile static ConcurrentDictionary<string, object> hash = new ConcurrentDictionary<string, object>();
            /// <summary>
            /// Gets or sets the <see cref="System.Object"/> with the specified key.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns>System.Object.</returns>
            public object this[string key]
            {
                get
                {
                    object value;
                    if (hash.TryGetValue(key, out value))
                        return value;
                    return null;
                }
                set
                {
                    hash.TryAdd(key, value);
                }
            }


            /// <summary>
            /// Removes the specified key.
            /// </summary>
            /// <param name="key">The key.</param>
            public void Remove(string key)
            {
                object value;
                hash.TryRemove(key, out value);
            }

            /// <summary>
            /// Gets the sync root.
            /// </summary>
            /// <value>The sync root.</value>
            /// <exception cref="NotImplementedException"></exception>
            /// <exception cref="System.NotImplementedException"></exception>
            public object SyncRoot
            {
                get { return hash; }
            }


            public object GetOrAdd(string key, Func<string, object> fac)
            {
                return hash.GetOrAdd(key, fac);
            }
        }



        /// <summary>
        /// Class ApplicationImpl,请求级别容器和session级别容器必须进行缓存，否则ioc容器会在每次请求时创建子容器，但又无法想web一样自动释放！
        /// </summary>
        class RequestImpl : IRequestState
        {
            /// <summary>
            /// The instance
            /// </summary>
            static internal RequestImpl Instance = new RequestImpl();
            /// <summary>
            /// Prevents a default instance of the <see cref="RequestImpl"/> class from being created.
            /// </summary>
            private RequestImpl()
            {

            }
            //   private volatile static ConcurrentDictionary<string, object> hash = new ConcurrentDictionary<string, object>();
            /// <summary>
            /// Gets or sets the <see cref="System.Object"/> with the specified key.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns>System.Object.</returns>
            public object this[string key]
            {
                get
                {
                    //object value;
                    //if (hash.TryGetValue(key, out value))
                    //    return value;
                    return null;
                }
                set
                {
                    //  hash.TryAdd(key, value);
                }
            }


            /// <summary>
            /// Removes the specified key.
            /// </summary>
            /// <param name="key">The key.</param>
            public void Remove(string key)
            {
                //object value;
                //hash.TryRemove(key, out value);
            }

            /// <summary>
            /// Gets the sync root.
            /// </summary>
            /// <value>The sync root.</value>
            /// <exception cref="NotImplementedException"></exception>
            /// <exception cref="System.NotImplementedException"></exception>
            public object SyncRoot
            {
                get { return _instance; }
            }


            public object GetOrAdd(string key, Func<string, object> fac)
            {
                return fac(key);
                // return hash.GetOrAdd(key, fac);
            }
        }




        /// <summary>
        /// Gets the application.
        /// </summary>
        /// <value>The application.</value>
        public IApplicationState Application
        {
            get { return ApplicationImpl.Instance; }
        }
        /// <summary>
        /// Gets the session.
        /// </summary>
        /// <value>The session.</value>
        public ISessionState Session
        {
            get { return SessionImpl.Instance; }
        }
        /// <summary>
        /// Gets the request.
        /// </summary>
        /// <value>The request.</value>
        public IRequestState Request
        {
            get { return RequestImpl.Instance; }
        }
    }
}