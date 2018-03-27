using System;
using System.Collections.Generic;

namespace _1_3_1_HashTable
{
    public class HashTable<TKey, TValue> : IHashTable<TKey, TValue> //where TKey : IEqualityComparer<TKey>
    {
        internal struct Bucket
        {
            public TKey Key { get; set; }
            public TValue Value { get; set; }
        }

        private Bucket[] _buckets;
        private int _bucketsSize;
        private int _filledBuckets;
        private const int MinimumTableSize = 4;
        private const decimal MaximumLoadFactor = 0.75m;
        private const decimal MinimumLoadFactor = 0.25m;

        public int Size
        {
            get
            {
                return _bucketsSize;
            }
            private set
            {
                _bucketsSize = value;
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                TryGet(key, out TValue value);
                return value;
            }

            set
            { 
                if(value  == null)
                {
                    Remove(key);
                }
                Add(key, value);
            }
        }

        public HashTable() : this(MinimumTableSize)
        {

        }

        public HashTable(int capacity)
        {
            _buckets = new Bucket[capacity];
            Size = capacity;
            _filledBuckets = 0;
        }        

        public void Add(TKey key, TValue value)
        {           
            ValidateInputValues(key, value);

            var loadFactor = CalculateLoadFactor();
            if (loadFactor >= MaximumLoadFactor)
            {
                Size = Size * 2;
                Rehash();
            }

            var hashCode = GetHash(key);
            if (_buckets[hashCode].Value != null)
            {
                throw new TypeAccessException(nameof(hashCode));
            }
            _buckets[hashCode] = new Bucket() { Key = key, Value = value };
            _filledBuckets++;
        }

        public bool Contains(TKey key)
        {
            var searchBucket = GetBucketByKey(key);
            return searchBucket.Value == null ? false : true;           
        }

        public bool TryGet(TKey key, out TValue value)
        {
            var searchBucket = GetBucketByKey(key);

            if (searchBucket.Value == null)
            {
                throw new KeyNotFoundException(nameof(key));
            }
            else
            {
                value = searchBucket.Value;
                return true;
            }
        }

        public void Remove(TKey key)
        {
            var hashCode = GetHash(key);
            var bucketForDeletion = _buckets[hashCode];

            if (bucketForDeletion.Value != null)
            {
                bucketForDeletion = default(Bucket);
                _buckets[hashCode] = bucketForDeletion;
                _filledBuckets--;
            }

            var loadFactor = CalculateLoadFactor();
            if (loadFactor <= MinimumLoadFactor && Size > MinimumTableSize)
            {
                Size = Size / 2;
                Rehash();
            }
        }

        #region Private Methods
        private int GetHash(TKey key)
        {
            return Math.Abs(EqualityComparer<TKey>.Default.GetHashCode(key) % Size);
        }

        private void ValidateInputValues(TKey key, TValue value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
        }

        private void Rehash()
        {
            var newBuckets = new Bucket[Size];

            for (var i = 0; i < _buckets.Length; i++)
            {
                var bucket = _buckets[i];
                if (bucket.Key != null)
                {
                    var hashCode = GetHash(bucket.Key);
                    newBuckets[hashCode] = bucket;
                }
            }
            _buckets = newBuckets;
        }

        private Bucket GetBucketByKey(TKey key)
        {
            var hashCode = GetHash(key);
            return _buckets[hashCode];
        }

        private decimal CalculateLoadFactor()
        {
            return Decimal.Divide(_filledBuckets, Size);
        }
        #endregion 
    }
}