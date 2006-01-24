using System;
using System.Collections;
using System.Text;
using Studio.VisualStudio9.Solution;
using NAnt.Core;
using NAnt.Core.Attributes;

namespace Studio.VisualStudio9.DataTypes
{
    [ElementName("projectsection9")]
    public class ProjectSectionElement :DataTypeBase
    {
        
#region Enums

        public enum ApplicationTime
        {
            Pre,
            Post
        }

#endregion
        
#region Fields

        private string _SectionName;
        private ApplicationTime _TimeToApply;
        private PairElement[] _Pairs;
        private ProjectSection _Section;
        private bool _InSync = false;

#endregion
        
#region Properties

        private bool InSync
        {
            get
            {
                return _InSync;
            }
            set
            {
                _InSync = value;
            }
        }

        private ProjectSection Section
        {
            get
            {
                if (!this.InSync || this._Section == null)
                    this.MakeSection();
                return _Section;
            }
            set
            {
                _Section = value;
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
                if (_SectionName == value)
                    return;
                _SectionName = value;
                this.InSync = false;
            }
        }

        [TaskAttribute("timetoapply", Required = true)]
        public ApplicationTime TimeToApply
        {
            get
            {
                return _TimeToApply;
            }
            set
            {
                _TimeToApply = value;
                this.InSync = false;
            }
        }

        [BuildElementArray("pair")]
        public PairElement[] Pairs
        {
            get
            {
                return _Pairs;
            }
            set
            {
                _Pairs = value;
                this.InSync = false;
            }
        }

#endregion

        private void MakeSection()
        {
            if (this._Section == null)
                this._Section = new ProjectSection();
            this._Section.Name = this.SectionName;
            this._Section.ApplicationTime = this.TimeToApply.ToString();
            foreach (PairElement CurrentPair in this.Pairs)
            {
                this._Section.Pairs.Add(CurrentPair.Key, CurrentPair.Value);
            }
            this.InSync = true;
        }

        public ProjectSection GetProjectSection()
        {
            return this.Section;
        }

    }
}
