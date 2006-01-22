using System;
using System.Collections;
using System.Text;

namespace Studio.Tasks.VisualStudio8.Solution
{
    public class ProjectList : CollectionBase
    {

        public void Add(Project project)
        {
            this.InnerList.Add(project);
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

        public void Remove(Project project)
        {
            try
            {
                this.InnerList.Remove(project);
            }
            catch
            {
            }
        }

        public Project this[int index]
        {
            get
            {
                return (Project)this.InnerList[index];
            }
            set
            {
                this.InnerList[index] = value;
            }
        }

    }
}
