using System;
using System.Collections;
using System.Text;

namespace Studio.VisualStudio9.Solution
{
    public abstract class Section
    {
        private string _Name;
        private string _ApplicationTime;
        private PairMap _Pairs;

        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (_Name == value)
                    return;
                _Name = value;
            }
        }
        public string ApplicationTime
        {
            get
            {
                return _ApplicationTime;
            }
            set
            {
                if (_ApplicationTime == value)
                    return;
                _ApplicationTime = value;
            }
        }
        public PairMap Pairs
        {
            get
            {
                if (_Pairs == null)
                    _Pairs = new PairMap();
                return _Pairs;
            }
            set
            {
                _Pairs = value;
            }
        }
    }
}
