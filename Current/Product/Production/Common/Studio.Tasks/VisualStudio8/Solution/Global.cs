using System;
using System.Collections;
using System.Text;

namespace Studio.Tasks.VisualStudio8.Solution
{
    public class Global
    {
        private GlobalSectionList _Sections;

        public GlobalSectionList Sections
        {
            get
            {
                if (_Sections == null)
                    _Sections = new GlobalSectionList();
                return _Sections;
            }
            set
            {
                _Sections = value;
            }
        }
    }
}
