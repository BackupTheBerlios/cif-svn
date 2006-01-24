using System;
using System.Collections;
using System.Text;
using Studio.VisualStudio9.Solution;
using Studio.VisualStudio9.DataTypes;
using NAnt.Core;
using NAnt.Core.Attributes;

namespace Studio.VisualStudio9.Tasks
{
    [TaskName("addproject9")]
    public class AddProject : Task
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
            VSSolution.Projects.Add(this.StudioProject.GetProject());
            VSSolution.WriteFile();
        }

    }
}
