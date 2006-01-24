using System;
using System.Collections;
using System.Text;
using Studio.VisualStudio9.Solution;
using Studio.VisualStudio9.DataTypes;
using NAnt.Core;
using NAnt.Core.Attributes;

namespace Studio.VisualStudio9.Tasks
{
    [TaskName("removefromglobalsection9")]
    public class RemoveFromGlobalSection : Task
    {

        #region Fields

        private string _SolutionFile;
        private string _SectionName;
        private PairElement[] _Pairs;

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

        [TaskAttribute("sectionname", Required = true)]
        public string SectionName
        {
            get
            {
                return _SectionName;
            }
            set
            {
                _SectionName = value;
            }
        }

        [BuildElementArray("pairs", Required=true)]
        public PairElement[] Pairs
        {
            get
            {
                return _Pairs;
            }
            set
            {
                _Pairs = value;
            }
        }

        #endregion

        protected override void ExecuteTask()
        {
            Solution.Solution VSSolution = Solution.Solution.ReadFile(this.SolutionFile);
            Solution.GlobalSection SectionToAddTo = VSSolution.Global.Sections.GetSection(this.SectionName);
            foreach (PairElement CurrentPair in this.Pairs)
            {
                SectionToAddTo.Pairs.Remove(CurrentPair.Key);
            }
            VSSolution.WriteFile();
        }

    }
}
