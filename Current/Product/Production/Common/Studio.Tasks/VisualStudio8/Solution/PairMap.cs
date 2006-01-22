using System;
using System.Collections;
using System.Text;
using ASOGS.Collections;

namespace Studio.Tasks.VisualStudio8.Solution
{
    public class PairMap : DictionaryBase
    {
        public void Add(string key, string value)
        {
            this.Dictionary.Add(key, value);
        }
        public void Add(Pair pair)
        {
            this.Dictionary.Add(pair.Key, pair.Value);
        }
        public bool Contains(string entry)
        {
            return this.Dictionary.Contains(entry);
        }
        public bool Contains(Pair pair)
        {
            return this.Dictionary.Contains(pair.Key);
        }
        public void Remove(string entry)
        {
            this.Dictionary.Remove(entry);
        }
        public void Remove(Pair pair)
        {
            this.Dictionary.Remove(pair.Key);
        }
        public string this[string entry]
        {
            get
            {
                return (string)this.Dictionary[entry];
            }
            set
            {
                this.Dictionary[entry] = value;
            }
        }

    }
}
