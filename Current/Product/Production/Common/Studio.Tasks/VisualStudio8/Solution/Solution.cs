using System;
using System.Collections;
using System.Text;
using System.IO;

namespace Studio.Tasks.VisualStudio8.Solution
{
    public class Solution
    {
        #region Fields

        private FileInfo _File;

        private string _Title;
        private string _FormatVersion;
        private float _Version;
        private string _ShortDeclaration;
        private ProjectList _Projects;
        private Global _Global;

        #endregion

        #region Properties

        public FileInfo File
        {
            get
            {
                return _File;
            }
            set
            {
                if (_File == value)
                    return;
                _File = value;
            }
        }

        public string Title
        {
            get
            {
                return _Title;
            }
            set
            {
                if (_Title == value)
                    return;
                _Title = value;
            }
        }

        public string FormatVersion
        {
            get
            {
                return _FormatVersion;
            }
            set
            {
                if (_FormatVersion == value)
                    return;
                _FormatVersion = value;
            }
        }

        public float Version
        {
            get
            {
                return _Version;
            }
            set
            {
                if (_Version == value)
                    return;
                _Version = value;
            }
        }

        public string ShortDeclaration
        {
            get
            {
                return _ShortDeclaration;
            }
            set
            {
                if (_ShortDeclaration == value)
                    return;
                _ShortDeclaration = value;
            }
        }

        public ProjectList Projects
        {
            get
            {
                if (_Projects == null)
                    _Projects = new ProjectList();
                return _Projects;
            }
            set
            {
                _Projects = value;
            }
        }

        public Global Global
        {
            get
            {
                if (_Global == null)
                    _Global = new Global();
                return _Global;
            }
            set
            {
                _Global = value;
            }
        }

        #endregion

        public Solution(FileInfo file)
        {
            _File = file;
            
        }
    }
}
