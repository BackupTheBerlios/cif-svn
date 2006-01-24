using System;
using System.Collections;
using System.Text;

namespace Studio.VisualStudio9.Solution
{
    public class Project
    {

        #region Fields

        private Guid _ParentId;
        private string _Name;
        private string _Location;
        private Guid _Id;
        private ProjectSection _Section;

        #endregion

        #region Properties

        public Guid ParentId
        {
            get
            {
                return _ParentId;
            }
            set
            {
                if (_ParentId == value)
                    return;
                _ParentId = value;
            }
        }

        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (_Name == value)
                    return;
                _Name = value;
            }
        }

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
            }
        }

        public Guid Id
        {
            get
            {
                return _Id;
            }
            set
            {
                if (_Id == value)
                    return;
                _Id = value;
            }
        }

        public ProjectSection Section
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
    }
}
