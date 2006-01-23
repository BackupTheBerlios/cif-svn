using System;
using System.Collections;
using System.Text;
using Studio.VisualStudio8.Solution;
using Studio.VisualStudio8.DataTypes;
using NAnt.Core;
using NAnt.Core.Attributes;

namespace Studio.VisualStudio8.Tasks
{
    [TaskName("removeproject8")]
    public class RemoveProject : Task
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
            Solution.Solution VSSolution = Solution.Solution.ReadFile(this.SolutionFile);
            Solution.Project ProjectToRemove = this.StudioProject.GetProject();
            foreach (Solution.Project CurrentProject in VSSolution.Projects)
            {
                if (CurrentProject.Id == ProjectToRemove.Id)
                    {
                        ProjectToRemove = CurrentProject;
                    	break;
                    }
            }
            VSSolution.Projects.Remove(ProjectToRemove);
            VSSolution.WriteFile();
        }

    }
}
