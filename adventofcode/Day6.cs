using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adventofcode
{
    [TestFixture]
    public class Day6
    {
        [Test]
        public void test_day_6_part_1()
        {
            var mb = new MemoryBank16(5, 1, 10, 0, 1, 7, 13, 14, 3, 12, 8, 10, 7, 12, 0, 6);
            //var mb = new MemoryBank4(0, 2, 7, 0);
            var ma = new MemoryAllocator(mb);

            var infinateLoopDetected = false;
            while (!infinateLoopDetected)
            {
                infinateLoopDetected = !ma.RealocateMemory();
            }
            var x = ma.LoopCount;
            Assert.That(x, Is.EqualTo(5042));
        }

        [Test]
        public void test_day_6_part_2()
        {
            var mb = new MemoryBank16(5, 1, 10, 0, 1, 7, 13, 14, 3, 12, 8, 10, 7, 12, 0, 6);
            //var mb = new MemoryBank4(0, 2, 7, 0);
            var ma = new MemoryAllocator(mb);

            var infinateLoopDetected = false;
            while (!infinateLoopDetected)
            {
                infinateLoopDetected = !ma.RealocateMemory();
            }
            var x = ma.GetLoopCountSinceFirstSighting(); ;


            Assert.That(x, Is.EqualTo(1086));
        }
    }
    public static class Utils
    {
        public static Dictionary<TKey, TValue> CloneDictionaryCloningValues<TKey, TValue> (Dictionary<TKey, TValue> original) where TValue : ICloneable
        {
            Dictionary<TKey, TValue> ret = new Dictionary<TKey, TValue>(original.Count, original.Comparer);
            foreach (KeyValuePair<TKey, TValue> entry in original)
            {
                ret.Add(entry.Key, (TValue)entry.Value.Clone());
            }
            return ret;
        }

        public static Dictionary<int, int> ShallowClone(this Dictionary<int, int> original)
        {
            return (from x in original
                    select x).ToDictionary(x => x.Key, x => x.Value);
        }
    }

    public interface IMemoryBank
    {
        Dictionary<int, int> MB { get; }

        int Id { get; }

        IMemoryBank Reallocate();
    }

    public class BaseMemoryBank: IMemoryBank
    {
        // not thread safe
        public static int maxId = 0;

        public Dictionary<int, int> MB { get; protected set; }

        public int Id { get; private set; }

        protected BaseMemoryBank() {
            maxId++;
            Id = maxId;
        }
        
        public IMemoryBank Reallocate()
        {
            var clone = MB.ShallowClone();
            double numberOfMemoryBanks = clone.Count;
            var amountToRealocated = clone.Max(x => x.Value);
            var keyOfPotToRealocate = clone.Where(x => x.Value == amountToRealocated).Min(x => x.Key);

            clone[keyOfPotToRealocate] = 0;
            
            for (int i = 1; i <= amountToRealocated; i++)
            {   
                var key = keyOfPotToRealocate + i;

                if (key > numberOfMemoryBanks)
                {
                    var multiplier = Math.Floor(key / numberOfMemoryBanks);
                    var temp = (int)(numberOfMemoryBanks * multiplier);
                    var tempKey = ((key - temp) == 0) ? (int)numberOfMemoryBanks : (key - temp);
                    key = tempKey;
                }

                clone[key] += 1;
            }

            return new BaseMemoryBank() { MB = clone }; ;
        }

        public override bool Equals(object obj)
        {
            var other = obj as IMemoryBank;
            if (other==null)
                return false;
            if (MB.Count != other.MB.Count)
                return false;
            if (MB.Keys.Except(other.MB.Keys).Any())
                return false;
            if (MB.Keys.Except(other.MB.Keys).Any())
                return false;
            foreach (var pair in MB)
                if (!Comparer<int>.Equals(pair.Value, other.MB[pair.Key]))
                    return false;
            return true;
        }
    }

    public class MemoryBank16 :BaseMemoryBank
    {
        public MemoryBank16(int B1, int B2, int B3, int B4, int B5, int B6, int B7, int B8, int B9, int B10, int B11, int B12, int B13, int B14, int B15, int B16)
        {
            MB = new Dictionary<int, int>()
            {
                { 1, B1 }, { 2, B2 }, { 3, B3 }, { 4, B4 }, { 5, B5 }, { 6, B6 }, { 7, B7 }, { 8, B8 }, { 9, B9 }, { 10, B10 }, { 11, B11 }, { 12, B12 },
                { 13, B13 }, { 14, B14 }, { 15, B15 }, { 16, B16 }
            };
        }      
    }

    public class MemoryBank4 : BaseMemoryBank
    {
        public MemoryBank4(int B1, int B2, int B3, int B4)
        {
            MB = new Dictionary<int, int>()
            {
                { 1, B1 }, { 2, B2 }, { 3, B3 }, { 4, B4 }
            };
        }        
    }

    public class MemoryBankComparer : IEqualityComparer<IMemoryBank>
    {
        public bool Equals(IMemoryBank x, IMemoryBank y)
        {
            if (x.MB.Count != y.MB.Count)
                return false;
            if (x.MB.Keys.Except(y.MB.Keys).Any())
                return false;
            if (y.MB.Keys.Except(x.MB.Keys).Any())
                return false;
            foreach (var pair in x.MB)
                if (!Comparer<int>.Equals(pair.Value, y.MB[pair.Key]))
                    return false;
            return true;
        }

        public int GetHashCode(IMemoryBank obj)
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = (int)2166136261;
                foreach (KeyValuePair<int, int> pair in obj.MB) { 
                    var tempHash = (hash * 16777619);
                    hash = tempHash ^= pair.Key.GetHashCode();
                    hash = tempHash ^= pair.Value.GetHashCode();
                }
                return hash;
            }
        }
    }

    

    public class MemoryAllocator
    {
        private HashSet<IMemoryBank> _history;
        private IMemoryBank _current;

        public int LoopCount => _history.Count;

        public int GetLoopCountSinceFirstSighting()
        {
            var first = _history.Single(x => x.Equals(_current));

            var count = _history.Count + 1 - first.Id;
            return count;
        }

        public MemoryAllocator(MemoryBank16 mb)
        {
            _history = new HashSet<IMemoryBank>(new MemoryBankComparer());
            _current = mb;
            _history.Add(mb);
        }

        public MemoryAllocator(MemoryBank4 mb)
        {
            _history = new HashSet<IMemoryBank>(new MemoryBankComparer());
            _current = mb;
            _history.Add(mb);
        }

        public bool RealocateMemory()
        {
            var mb = _current.Reallocate();
            _current = mb;
            return _history.Add(mb);
        }
    }
}
