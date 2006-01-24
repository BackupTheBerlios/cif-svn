using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;

namespace Studio.VisualStudio9.Solution
{
    public class GlobalSectionList : CollectionBase
    {

        private Hashtable _Sections = new Hashtable();

        private Hashtable Sections
        {
            get
            {
                return _Sections;
            }
            set
            {
                _Sections = value;
            }
        }

        public GlobalSection GetSection(String name)
        {
            return (GlobalSection)this.Sections[name];
        }

        public void Add(GlobalSection globalSection)
        {
            if (this.Sections.Contains(globalSection.Name))
                throw new InvalidOperationException(string.Format("The section name {0} is already in this solution.", globalSection.Name));
            this.InnerList.Add(globalSection);
            this.Sections.Add(globalSection.Name, globalSection);
        }

        public new void Clear()
        {
            try
            {
                this.InnerList.Clear();
                this.Sections = new Hashtable();
            }
            catch
            {
            }
        }

        public void Remove(GlobalSection globalSection)
        {
            try
            {
                this.InnerList.Remove(globalSection);
                this.Sections.Remove(globalSection.Name);
            }
            catch
            {
            }
        }

        public GlobalSection this[int index]
        {
            get
            {
                return (GlobalSection)this.InnerList[index];
            }
            set
            {
                this.InnerList[index] = value;
            }

        }
    }
}
