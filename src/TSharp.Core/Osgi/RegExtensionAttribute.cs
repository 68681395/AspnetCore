using System;

namespace TSharp.Core.Osgi
{
	/// <summary>
	/// 扩展标记
	/// </summary>
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = true)]
	public class ExtensionAttribute : Attribute, IComparable<ExtensionAttribute>
	{
		private int _order = 0;

		/// <summary>
		/// 序号，注册时指定的一个整数
		/// </summary>
		/// <value>
		/// The order.
		/// </value>
		public int Order
		{
			get { return this._order; }
			set { this._order = value; }
		}

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.
        /// </returns>
		public int CompareTo(ExtensionAttribute other)
		{
			if (other != null)
				return this.Order.CompareTo(other.Order);
			return 1;
		}
	}
}