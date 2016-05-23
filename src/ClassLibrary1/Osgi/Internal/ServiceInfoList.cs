using System;
using System.Collections;
using System.Collections.Generic;

namespace TSharp.Core.Osgi.Internal
{
    internal class ServiceInfoList : IEnumerable<RegServiceAttribute>
    {
        private readonly SortedSet<RegServiceAttribute> _list =
            new SortedSet<RegServiceAttribute>(new RegServiceAttributeComparer());

        public void Add(RegServiceAttribute attribute)
        {
            _list.Add(attribute);
        }

        public bool Remove(RegServiceAttribute attribute)
        {
            return _list.Remove(attribute);
        }


        private class RegServiceAttributeComparer : Comparer<RegServiceAttribute>
        {
            public override int Compare(RegServiceAttribute x, RegServiceAttribute y)
            {
                if (x.Level == y.Level)
                {
                    if (x.Group == y.Group)
                    {
                        if (x.IntfType == y.IntfType)
                        {
                            if (x.Order == y.Order)
                            {
                                return 0;
                            }
                            else return x.Order.CompareTo(y.Order);
                        }
                        else return Compare(x.IntfType, y.IntfType);
                    }
                    else return Comparer<string>.Default.Compare(x.Group, y.Group);
                }
                else return x.Level - y.Level;
            }

            private int Compare(Type x, Type y)
            {
                if (x != null && y != null)
                    return Comparer<string>.Default.Compare(x.FullName, y.FullName);
                else if (x == null && y == null)
                    return 0;
                if (x == null)
                    return -1;
                if (y == null)
                    return 1;
                return 0;
            }
        }

        IEnumerator<RegServiceAttribute> IEnumerable<RegServiceAttribute>.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }
}