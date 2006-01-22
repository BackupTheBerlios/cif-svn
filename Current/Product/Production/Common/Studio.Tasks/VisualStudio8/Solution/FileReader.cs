using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace Studio.Tasks.VisualStudio8.Solution
{
    internal class FileReader
    {
        private const string GlobalDeclaration = "Global";
        private const string EndGlobalDeclaration = "EndGlobal";
        private const string ProjectDeclaration = "Project";
        private const string EndProjectDeclaration = "EndProject";
        private const string GlobalSectionDeclaration = "GlobalSection";
                                            

#region Fields

        private FileInfo _File;
        private Solution _Solution;
        
#endregion
        
#region Properties

        internal FileInfo File
        {
            get
            {
                return _File;
            }
            set
            {
                _File = value;
            }
        }

        internal Solution Solution
        {
            get
            {
                return _Solution;
            }
            set
            {
                _Solution = value;
            }
        }

#endregion
        
#region Constructors

        public FileReader()
        {
            
        }


        internal FileReader(FileInfo file)
        {
            _File = file;
        }

        internal FileReader(FileInfo file, Solution soltion)
        {
            _File = file;
            _Solution = soltion;
        }

#endregion
    
        internal Solution ReadFile()
    {
        if (!this.File.Exists)
        {
            throw new FileNotFoundException("Could not file the solution file.", this.File.Name);
        }
        TextReader Reader;
        using(Reader = this.File.OpenText())
        {
        	this.Solution = new Solution(this.File);
            string Line = string.Empty;
            int LineCount = 0;
            StringCollection RawSection = null;
            while (true)
            {
                Line = Reader.ReadLine();
                LineCount += 1;
                if (Line == null)
                    break;
                if (Line == string.Empty)
                {
                    LineCount -= 1;
                }
                else if (LineCount == 1)
                {
                    Regex Rx = new Regex("^\\s{0,}([\\w+\\s?]+),([\\w+\\s?]+ (\\d+\\.\\d+))");
                    Match Matched = Rx.Match(Line);
                    this.Solution.Title = Matched.Groups[1].Value;
                    this.Solution.FormatVersion = Matched.Groups[2].Value;
                    this.Solution.Version = float.Parse(Matched.Groups[3].Value);
                }
                else if (LineCount == 2)
                {
                	this.Solution.ShortDeclaration = Line.Trim();
                }
                else if (Line.StartsWith(ProjectDeclaration))
                {
                    RawSection = new StringCollection();
                    RawSection.Add(Line);
                }
                else if (Line.StartsWith(EndProjectDeclaration))
                {
                    Project NewProject = this.MakeProject(RawSection);
                	this.Solution.Projects.Add(NewProject);
                }
                else if (Line.StartsWith(GlobalDeclaration))
                {
                    //nothing
                }
                else if (Line.StartsWith(EndGlobalDeclaration))
                {
                    //nothing
                }
                else if (Regex.IsMatch(Line, "^\\s{0,}GlobalSection\\("))
                {
                    RawSection = new StringCollection();
                    RawSection.Add(Line);
                }
                else if (Regex.IsMatch(Line, "^\\s{0,}EndGlobalSection\\s?$"))
                {
                    GlobalSection NewGlobalSection = this.MakeGlobalSection(RawSection);
                	this.Solution.Global.Sections.Add(NewGlobalSection);
                }
                else
                {
                    if (RawSection == null)
                        throw new NullReferenceException("Have not entered a section yet.");
                    RawSection.Add(Line);
                }
            }
            return this.Solution;
        }
    }

        private GlobalSection MakeGlobalSection(StringCollection RawSection)
        {
            GlobalSection NewGlobalSection = new GlobalSection();
            Pair CurrentPair;
            foreach (String SectionLine in RawSection)
            {
                if (Regex.IsMatch(SectionLine, "^\\s{0,}GlobalSection\\("))
                {
                    Match Matched = Regex.Match(SectionLine, "^\\s{0,}GlobalSection\\((\\w+)\\)\\s+=\\s+(\\w+)");
                    NewGlobalSection = new GlobalSection();
                    NewGlobalSection.Name = Matched.Groups[1].Value;
                    NewGlobalSection.ApplicationTime = Matched.Groups[2].Value;
                }
                else if (Regex.IsMatch(SectionLine, "\\s+=\\s+"))
                {
                    Match Matched = Regex.Match(SectionLine, "^\\s{0,}([\\{?\\}?\\|?\\w+?\\\\?\\.?\\-?\\s?]+)\\s+=\\s+([\\{?\\}?\\|?\\w+?\\\\?\\.?\\-?\\s?]+)");
                    CurrentPair = new Pair();
                    CurrentPair.Key = Matched.Groups[1].Value;
                    CurrentPair.Value = Matched.Groups[2].Value;
                    NewGlobalSection.Pairs.Add(CurrentPair);
                }
                else if (Regex.IsMatch(SectionLine, "^\\s{0,}EndGlobalSection\\s?$"))
                {
                    //nothing
                }
                else
                {
                    throw new InvalidOperationException("Unexpected text in the Project.");
                }
            }
            return NewGlobalSection;
        }

        private Project MakeProject(StringCollection RawSection)
        {
            Project NewProject = new Project();
            ProjectSection NewProjectSection = null;
            Pair CurrentPair;
            foreach (String ProjectLine in RawSection)
            {
                if (Regex.IsMatch(ProjectLine, "^\\s{0,}Project\\("))
                {
                    Match Matched = Regex.Match(ProjectLine, "^\\s{0,}Project\\(\"(\\{[\\w+\\-?]+})\"\\)\\s+=\\s+\"([\\w+\\\\?\\.?\\-?\\s?]+)\",\\s+\"([\\w+\\\\?\\.?\\-?\\s?]+)\",\\s+\"(\\{[\\w+\\-?]+})\"");
                    NewProject.ParentId = new Guid(Matched.Groups[1].Value);
                    NewProject.Name = Matched.Groups[2].Value;
                    NewProject.Location = Matched.Groups[3].Value;
                    NewProject.Id = new Guid(Matched.Groups[4].Value);
                }
                else if (Regex.IsMatch(ProjectLine, "^\\s{0,}ProjectSection\\("))
                {
                    Match Matched = Regex.Match(ProjectLine, "^\\s{0,}ProjectSection\\((\\w+)\\)\\s+=\\s+(\\w+)");
                    NewProjectSection = new ProjectSection();
                    NewProjectSection.Name = Matched.Groups[1].Value;
                    NewProjectSection.ApplicationTime = Matched.Groups[2].Value;
                    NewProject.Section = NewProjectSection;
                }
                else if (Regex.IsMatch(ProjectLine, "\\s+=\\s+"))
                {
                    Match Matched = Regex.Match(ProjectLine, "^\\s{0,}([\\w+\\\\?\\.?\\-?\\s?]+)\\s+=\\s+([\\w+\\\\?\\.?\\-?\\s?]+)");
                    CurrentPair = new Pair();
                    CurrentPair.Key = Matched.Groups[1].Value;
                    CurrentPair.Value = Matched.Groups[2].Value;
                    NewProjectSection.Pairs.Add(CurrentPair);
                }
                else if (Regex.IsMatch(ProjectLine, "^\\s{0,}EndProjectSection\\s?$"))
                {
                    //nothing
                }
                else
                {
                    throw new InvalidOperationException("Unexpected text in the Project.");
                }
            }
            return NewProject;
        }


        internal void WriteFile()
        {
            TextWriter Writer;
            using(Writer = this.File.CreateText())
            {
                Writer.WriteLine(string.Format("{0}, {1}", this.Solution.Title, this.Solution.FormatVersion));
                Writer.WriteLine(this.Solution.ShortDeclaration);

                foreach (Project CurrentProject in this.Solution.Projects)
                {
                    
                }
                if (this.Solution.Global.Sections.Count != 0)
                {
                    Writer.WriteLine("")
                    foreach (GlobalSection globalSection in this.Solution.Global.Sections)
                    {
                        
                    }
                }
            }
        }

        private void WriteGlobalSection(GlobalSection globalSection, TextWriter writer)
        {
            
        }

        private void WriteProject(Project project, TextWriter writer)
        {
            
        }

        public void test()
        {
            try
            {
                this.File = new FileInfo("C:\\Temp\\test.sln.txt");
                this.ReadFile();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }

            string Title = this.Solution.Title;
        }
    }
}
