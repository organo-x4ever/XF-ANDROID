using com.organo.xchallenge.Extensions;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace com.organo.xchallenge.Models.Local
{
    public class Grouping<T, K> : ObservableCollection<T>
    {
        public K Key { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see
        /// cref="com.organo.xchallenge.Models.Grouping&lt;T,K&gt;" /> class.
        /// </summary>
        /// <param name="items">
        /// A collection of items of type T
        /// </param>
        /// <param name="key">
        /// A key of type K.
        /// </param>
        public Grouping(IEnumerable<T> items, K key)
        {
            Key = key;
            Items.AddRange(items);
        }
    }
}