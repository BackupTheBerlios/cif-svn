using System;
using System.Collections;
using System.Text;
using Studio.VisualStudio9.Solution;
using Studio.VisualStudio9.DataTypes;
using NAnt.Core;
using NAnt.Core.Attributes;

namespace Studio.VisualStudio9.Tasks
{
    [TaskName("addprojectsection9")]
    public class AddProjectSection : Task
    {
        #region Fields

        private string _SolutionFile;
        private ProjectElement _StudioProject;

        #endregion

        #region Properties

        [TaskAttribute("solutionfile", Required = true)]
        public string SolutionFile
        {
            get
            {
                return _SolutionFile;
            }
            set
            {
                _SolutionFile = value;
            }
        }

        [BuildElement("project", Required = true)]
        public ProjectElement StudioProject
        {
            get
            {
                return _StudioProject;
            }
            set
            {
                _StudioProject = value;
            }
        }

        #endregion

        protected override void ExecuteTask()
        {
            if (this.StudioProject.ProjectSections == null)
                throw new BuildException("There was no project section defined to add to the project.");
            Solution.Solution VSSolution = Solution.Solution.ReadFile(this.SolutionFile);
            Solution.Project VSProject = VSSolution.Projects.GetProject(this.StudioProject.ID);
            VSProject.Section = this.StudioProject.ProjectSections.GetProjectSection();
            VSSolution.WriteFile();
        }
    }
}
