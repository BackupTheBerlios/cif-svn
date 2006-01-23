using System;
using System.Collections;
using System.Text;
using NAnt.Core;
using NAnt.Core.Attributes;

namespace Studio.VisualStudio8.DataTypes
{
    [ElementName("pair8")]
    public class PairElement : DataTypeBase
    {
        private string _Key;
        private string _Value;

        [TaskAttribute("key", Required=true)]
        public string Key
        {
            get
            {
                return _Key;
            }
            set
            {
                _Key = value;
            }
        }

        [TaskAttribute("value", Required = true)]
        public string Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
            }
        }
    }
}
