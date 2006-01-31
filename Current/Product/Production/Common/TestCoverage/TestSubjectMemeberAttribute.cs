using System;
using System.Collections;
using System.Text;

namespace TestCoverage
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class TestSubjectMemeberAttribute : Attribute
    {
        private string _MemeberName;

        public string MemeberName
        {
            get
            {
                return _MemeberName;
            }
            set
            {
                _MemeberName = value;
            }
        }

        public TestSubjectMemeberAttribute(string memeberName)
        {
            _MemeberName = memeberName;
        }
    }
}
