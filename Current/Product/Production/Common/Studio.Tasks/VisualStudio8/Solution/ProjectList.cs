using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;

namespace Studio.VisualStudio8.Solution
{
    public class ProjectList : CollectionBase
    {

        private Hashtable _Projects = new Hashtable();

        private Hashtable Projects
        {
            get
            {
                return _Projects;
            }
            set
            {
                _Projects = value;
            }
        }

        public Project GetProject(String projectId)
        {
            return (Project) this.Projects[projectId];
        }

        public void Add(Project project)
        {
            if (this.Projects.Contains(project.Id.ToString()))
                throw new InvalidOperationException(string.Format("The project id {0} is already in this solution.", project.Id));
            this.InnerList.Add(project);
            this.Projects.Add(project.Id.ToString(), project);
        }

        public new void Clear()
        {
            try
            {
                this.InnerList.Clear();
                this.Projects = new Hashtable();
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
                this.Projects.Remove(project.Id.ToString());
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
