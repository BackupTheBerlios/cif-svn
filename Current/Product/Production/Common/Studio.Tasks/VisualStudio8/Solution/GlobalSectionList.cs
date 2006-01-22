using System;
using System.Collections;
using System.Text;

namespace Studio.Tasks.VisualStudio8.Solution
{
    public class GlobalSectionList : CollectionBase
    {

        public void Add(GlobalSection globalSection)
        {
            this.InnerList.Add(globalSection);
        }

        public new void Clear()
        {
            try
            {
                this.InnerList.Clear();
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
