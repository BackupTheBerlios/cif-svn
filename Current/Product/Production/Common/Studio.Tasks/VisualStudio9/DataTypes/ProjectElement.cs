using System;
using System.Collections;
using System.Text;
using Studio.VisualStudio9.Solution;
using NAnt.Core;
using NAnt.Core.Attributes;

namespace Studio.VisualStudio9.DataTypes
{
    [ElementName("project9")]
    public class ProjectElement :DataTypeBase
    {
        
#region Fields

        private ProjectSectionElement _ProjectSections;
        private Guid _ProjectId;
        private Guid _ParentId;
        private string _ProjectName;
        private string _Location;
        private Solution.Project _Project;
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

        private Solution.Project Project
        {
            get
            {
                if (!this.InSync || this._Project == null)
                    this.MakeProject();
                return _Project;
            }
            set
            {
                _Project = value;
            }
        }

        [TaskAttribute("projectid", Required = true)]
        public Guid ProjectId
        {
            get
            {
                return _ProjectId;
            }
            set
            {
                _ProjectId = value;
                this.InSync = false;
            }
        }

        [TaskAttribute("parentid", Required = true)]
        public Guid ParentId
        {
            get
            {
                return _ParentId;
            }
            set
            {
                _ParentId = value;
                this.InSync = false;
            }
        }

        [TaskAttribute("project", Required = true)]
        public string ProjectName
        {
            get
            {
                return _ProjectName;
            }
            set
            {
                if (_ProjectName == value)
                    return;
                _ProjectName = value;
                this.InSync = false;
            }
        }

        [TaskAttribute("location", Required = true)]
        public string Location
        {
            get
            {
                return _Location;
            }
            set
            {
                if (_Location == value)
                    return;
                _Location = value;
                this.InSync = false;
            }
        }

        [BuildElement("projectsection")]
        public ProjectSectionElement ProjectSections
        {
            get
            {
                return _ProjectSections;
            }
            set
            {
                _ProjectSections = value;
                this.InSync = false;
            }
        }

#endregion

        private void MakeProject()
        {
            if (this._Project == null)
                this._Project = new Solution.Project();
            this._Project.Id = this.ProjectId;
            this._Project.ParentId = this.ParentId;
            this._Project.Name = this.ProjectName;
            this._Project.Location = this.Location;
            this._Project.Section = this.ProjectSections.GetProjectSection();
            this.InSync = true;
        }

        public Solution.Project GetProject()
        {
            return this.Project;
        }
    }
}
