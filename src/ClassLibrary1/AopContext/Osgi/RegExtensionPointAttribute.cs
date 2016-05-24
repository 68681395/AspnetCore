using System;
using System.Reflection;
using TSharp.Core.Exceptions;
using TSharp.Core.Osgi.Internal;

namespace TSharp.Core.Osgi
{
    /// <summary>
    /// 扩展点标记
    /// <para>2011/3/4</para> 
    /// <author>tangjingbo</author>
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
    public sealed class RegExtensionPointAttribute : Attribute
    {
        private static readonly Type ExtensionAttributeType = typeof(RegExtensionAttribute);
        private static readonly Type ExtensionPointType = typeof(ExtensionPoint);

        /// <summary>
        /// Initializes a new instance of the <see cref="RegExtensionPointAttribute"/> class.
        /// </summary>
        /// <param name="managerType">扩展点收集类的类型.</param>
        public RegExtensionPointAttribute(Type managerType)
        {

            //todo : modify  managerType.BaseType to  managerType.DeclaringType
            var genericType = managerType.DeclaringType;
            while (genericType != null && !genericType.IsGenericParameter)
            {
                genericType = genericType.DeclaringType;
            }
            if (genericType != null)
            {
                var argTypes = genericType.GetGenericArguments();
                if (argTypes.Length > 0)
                {
                    Ctor(argTypes[0], managerType);
                }
                else
                    throw new ExtensionNotExtendException(managerType.Name +
                                                          " 必须继承ExtensionPoint<>，或者使用ExtensionPointAttribute(Type attributeType, Type pointType)定义扩展点");
            }
        }

        private void Ctor(Type attributeType, Type pointType)
        {
            if (!ExtensionAttributeType.IsAssignableFrom(attributeType))
            {
                throw new ExtensionNotExtendException(attributeType.Name + " not extend ExtensionAttribute");
            }
            if (!ExtensionPointType.IsAssignableFrom(pointType))
            {
                throw new ExtensionNotExtendException(pointType.Name + " not extend ExtensionPoint");
            }
            AttributeType = attributeType;
            PointType = pointType;
        }

        /// <summary>
        /// 扩展标记类型
        /// </summary>
        public Type AttributeType { get; private set; }

        /// <summary>
        /// 扩展点收集类的类型
        /// </summary>
        public Type PointType { get; private set; }

        /// <summary>
        /// 扩展点收集类实例
        /// </summary>
        internal ExtensionPoint ExtensionPoint
        {
            get
            {
                var constructorInfo = PointType.GetConstructor(new Type[0]);
                if (constructorInfo != null)
                    return (ExtensionPoint)constructorInfo.Invoke(new object[0]);
                throw new CoreException(string.Format("类型未找到无参的公共构造函数！'{0}'", PointType.FullName));
            }
        }
    }
}