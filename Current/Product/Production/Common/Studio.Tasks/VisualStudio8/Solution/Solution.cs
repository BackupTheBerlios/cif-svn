using System;
using System.Collections;
using System.Text;
using System.IO;

namespace Studio.VisualStudio8.Solution
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
        private SolutionFile _SolutionFile;

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
                this.SolutionFile.File = value;
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

        internal SolutionFile SolutionFile
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

        #endregion
        
#region Constructors

        public Solution()
        {

        }

        public Solution(string file)
        {
            _File = new FileInfo(file);
            this.SolutionFile = new SolutionFile(this.File, this);
        }

        public Solution(FileInfo file)
        {
            _File = file;
            this.SolutionFile = new SolutionFile(this.File, this);
        }

        internal Solution(FileInfo file, SolutionFile solutionFile)
        {
            _File = file;
            this.SolutionFile = solutionFile;
        }

#endregion
        
#region ReadWrite

        public void WriteFile()
        {
            this.SolutionFile.WriteFile();
        }

        public static Solution ReadFile(FileInfo file)
        {
            SolutionFile FileReader = new SolutionFile(file);
            return FileReader.ReadFile();
        }

        public static Solution ReadFile(string file)
        {
            SolutionFile FileReader = new SolutionFile(file);
            return FileReader.ReadFile();
        }

#endregion
        
#region Test

        internal void test()
        {
            Solution TestSolution;
            try
            {
                TestSolution = Solution.ReadFile(new FileInfo("C:\\Temp\\test.sln.txt"));
                string Title = TestSolution.Title;
                TestSolution.File = new FileInfo("C:\\Temp\\testwrite.sln.txt");
                TestSolution.WriteFile();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
        }

#endregion

    }
}
