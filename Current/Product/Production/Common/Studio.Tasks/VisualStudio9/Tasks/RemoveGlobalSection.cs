using System;
using System.Collections;
using System.Text;
using Studio.VisualStudio9.Solution;
using Studio.VisualStudio9.DataTypes;
using NAnt.Core;
using NAnt.Core.Attributes;


namespace Studio.VisualStudio9.Tasks
{
    [TaskName("removeglobalsection9")]
    public class RemoveGlobalSection : Task
    {

        #region Fields

        private string _SolutionFile;
        private GlobalSectionElement _Section;

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

        [BuildElement("section", Required = true)]
        public GlobalSectionElement Section
        {
            get
            {
                return _Section;
            }
            set
            {
                _Section = value;
            }
        }

        #endregion

        protected override void ExecuteTask()
        {
            Solution.Solution VSSolution = Solution.Solution.ReadFile(this.SolutionFile);
            GlobalSection SectionToRemove = this.Section.GetGlobalSection();
            foreach (GlobalSection CurrentSection in VSSolution.Global.Sections)
            {
                if (CurrentSection.Name == SectionToRemove.Name)
                    {
                        SectionToRemove = CurrentSection;
                    	break;
                    }
            }
            VSSolution.Global.Sections.Remove(SectionToRemove);
            VSSolution.WriteFile();
        }

    }
}
