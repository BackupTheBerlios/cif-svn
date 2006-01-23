using System;
using System.Collections;
using System.Text;
using Studio.VisualStudio8.Solution;
using Studio.VisualStudio8.DataTypes;
using NAnt.Core;
using NAnt.Core.Attributes;

namespace Studio.VisualStudio8.Tasks
{
    [TaskName("removeprojectsection8")]
    public class RemoveProjectSection : Task
    {
        #region Fields

        private string _SolutionFile;
        private string _ProjectId;

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

        [TaskAttribute("projectid", Required = true)]
        public string ProjectId
        {
            get
            {
                return _ProjectId;
            }
            set
            {
                _ProjectId = value;
            }
        }

        #endregion

        protected override void ExecuteTask()
        {
            Solution.Solution VSSolution = Solution.Solution.ReadFile(this.SolutionFile);
            Solution.Project VSProject = VSSolution.Projects.GetProject(this.ProjectId);
            VSProject.Section = null;
            VSSolution.WriteFile();
        }
    }
}
