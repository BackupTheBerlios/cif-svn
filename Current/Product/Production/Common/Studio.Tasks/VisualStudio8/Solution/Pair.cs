using System;
using System.Collections;
using System.Text;

namespace Studio.Tasks.VisualStudio8.Solution
{
    public class Pair
    {
        private string _Key;
        private string _Value;

        public string Key
        {
            get
            {
                return _Key;
            }
            set
            {
                if (_Key == value)
                    return;
                _Key = value;
            }
        }
        public string Value
        {
            get
            {
                return _Value;
            }
            set
            {
                if (_Value == value)
                    return;
                _Value = value;
            }
        }
    }
}
