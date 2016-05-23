using System;
using System.Collections.Generic;

namespace TSharp.Core.Osgi
{
    /// <summary>
    /// Osgi事件收集
    /// </summary>
    /// <author>
    /// tangjingbo
    /// </author>
    internal sealed class OsgiEventManager : ExtensionPoint<RegOsgiEventAttribute>
    {
        private static List<IOsgiEventHandler> evts = new List<IOsgiEventHandler>(50);

        public static IList<IOsgiEventHandler> Events
        {
            get { return evts.AsReadOnly(); }
        }
        public static void Clear()
        {
            evts.Clear();
        }
        internal override void EngineAdd(OsgiEngine.RegExtensionAttributeItem regAttribute)
        {
            var att = regAttribute.ExtensionAttribute as RegOsgiEventAttribute;
            if (att != null)
            {
                var constructorInfo = att.EventType.GetConstructor(new Type[0]);
                if (constructorInfo != null)
                {
                    var handler = constructorInfo.Invoke(new object[0]) as IOsgiEventHandler;
                    evts.Add(handler);
                }
            }
        }

        protected override void Register(System.Reflection.Assembly assembly, RegOsgiEventAttribute attribute)
        {
        }

        protected override void UnRegister(System.Reflection.Assembly assembly, RegOsgiEventAttribute attribute)
        {
        }
    }
}