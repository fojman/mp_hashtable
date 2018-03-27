namespace _1_3_1_HashTable
{
    interface IHashTable<TKey, TValue>
    {
        TValue this[TKey key] { get; set; }

        void Add(TKey key, TValue value);

        bool TryGet(TKey key, out TValue value);

        bool Contains(TKey key);
    }
}
