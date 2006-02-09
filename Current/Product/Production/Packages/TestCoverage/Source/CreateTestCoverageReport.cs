using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using NAnt.Core;
using NAnt.Core.Attributes;
using System.IO;
using System.Xml;
using System.Reflection;

namespace TestCoverage.Tasks
{
    [TaskName("createtestcoveragereport")]
    public class CreateTestCoverageReport :Task
    {
        
#region Enums

        public enum OutPutFormat
        {
            Xml,
            Html
        }

#endregion
        
#region Fields

        private OutPutFormat _Format;
        private FileInfo _File;
        private NAnt.Core.Types.FileSet _ProductionSet;
        private NAnt.Core.Types.FileSet _TestSet;
        private string _ProjectName;
        private FixtureTable _TestFixtures;
        private static StringCollection HintDirectories = new StringCollection();

#endregion
        
#region Properties

        [TaskAttribute("projectname", Required=true)]
        public string ProjectName
        {
            get
            {
                return _ProjectName;
            }
            set
            {
                _ProjectName = value;
            }
        }

        [TaskAttribute("format", Required = true)]
        public OutPutFormat Format
        {
            get
            {
                return _Format;
            }
            set
            {
                _Format = value;
            }
        }

        [TaskAttribute("reportfile", Required = true)]
        public FileInfo File
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

        [BuildElement("productionassemblies", Required = true)]
        public NAnt.Core.Types.FileSet ProductionSet
        {
            get
            {
                return _ProductionSet;
            }
            set
            {
                _ProductionSet = value;
            }
        }

        [BuildElement("testasseblies", Required = true)]
        public NAnt.Core.Types.FileSet TestSet
        {
            get
            {
                return _TestSet;
            }
            set
            {
                _TestSet = value;
            }
        }

        public FixtureTable TestFixtures
        {
            get
            {
                if (_TestFixtures == null)
                    _TestFixtures = new FixtureTable();
                return _TestFixtures;
            }
            set
            {
                _TestFixtures = value;
            }
        }

        #endregion

        protected override void ExecuteTask()
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolve);
            this.BuildTestSubjectsCovered();
            this.WriteXmlReport();
            AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(AssemblyResolve);
        }

        private void BuildTestSubjectsCovered()
        {
            foreach (String AsmFileName in this.TestSet.FileNames)
            {
                HintDirectories.Add(Path.GetDirectoryName(AsmFileName));
                Assembly Asm = Assembly.LoadFile(AsmFileName);
                foreach (Type TestType in Asm.GetTypes())
                {
                    this.ProcessTestType(TestType);
                }
            }
        }

        private void ProcessTestType(Type testType)
        {
            if (!testType.IsEnum || !testType.IsInterface)
            {
                foreach (Type NestedType in testType.GetNestedTypes())
                {
                    this.ProcessTestType(NestedType);
                }
                if (this.HasTestCoverage(testType))
                {
                    TestSubjectClassAttribute ClassTestCoverage = this.GetClassTestCoverage(testType);
                    if (!this.TestFixtures.Contains(ClassTestCoverage.TestSubject.FullName))
                    {
                        this.TestFixtures.Add(ClassTestCoverage.TestSubject.FullName, new TestFixture(testType.FullName));
                    }
                    foreach (MethodInfo Method in testType.GetMethods())
                    {
                        if (this.HasTestCoverage(Method))
                        {
                            TestSubjectMemeberAttribute MemberTestCoverage = this.GetMemberTestCoverage(Method);
                            if (!this.TestFixtures[ClassTestCoverage.TestSubject.FullName].Contains(MemberTestCoverage.MemeberName))
                                this.TestFixtures[ClassTestCoverage.TestSubject.FullName].Add(MemberTestCoverage.MemeberName, Method.Name);
                        }
                    }
                }
            }
        }

        private bool HasTestCoverage(Type testType)
        {
            return testType.GetCustomAttributes(typeof(TestSubjectClassAttribute), false).Length > 0;
        }

        private TestSubjectClassAttribute GetClassTestCoverage(Type testType)
        {
            return (TestSubjectClassAttribute) testType.GetCustomAttributes(typeof(TestSubjectClassAttribute), false)[0];
        }

        private bool HasTestCoverage(MethodInfo method)
        {
            return method.GetCustomAttributes(typeof(TestSubjectMemeberAttribute), false).Length > 0;
        }

        private TestSubjectMemeberAttribute GetMemberTestCoverage(MethodInfo method)
        {
            return (TestSubjectMemeberAttribute) method.GetCustomAttributes(typeof(TestSubjectMemeberAttribute), false)[0];
        }

        private void WriteXmlReport()
        {
            using (FileStream Stream = this.File.Open(FileMode.Create))
            {
                XmlTextWriter Writer = new XmlTextWriter(Stream, System.Text.Encoding.UTF8);
                Writer.Formatting = Formatting.Indented;
                Writer.WriteStartDocument();
                Writer.WriteStartElement("TestCoverage");

                Writer.WriteStartAttribute("Time", null);
                Writer.WriteString(System.DateTime.Now.ToString("F"));
                Writer.WriteEndAttribute();

                Writer.WriteStartAttribute("ProjectName", null);
                Writer.WriteString(this.ProjectName);
                Writer.WriteEndAttribute();
                
                foreach (String AsmFileName in this.ProductionSet.FileNames)
                {
                    HintDirectories.Add(Path.GetDirectoryName(AsmFileName));
                    Assembly Asm = Assembly.LoadFile(AsmFileName);

                    Writer.WriteStartElement("Assembly");
                    
                    Writer.WriteStartAttribute("Name", null);
                    Writer.WriteString(Path.GetFileName(Asm.Location));
                    Writer.WriteEndAttribute();
                    
                    foreach (Type ProdType in Asm.GetTypes())
                    {
                        this.ProcessType(ProdType, Writer);
                    }
                    Writer.WriteEndElement();
                }
                Writer.WriteEndElement();
                Writer.WriteEndDocument();
                Writer.Close();
            }
        }

        private void ProcessType(Type prodType, XmlTextWriter writer)
        {
            if (!prodType.IsEnum || !prodType.IsInterface)
            {
                foreach (Type NestedType in prodType.GetNestedTypes())
                {
                    this.ProcessType(NestedType, writer);
                }

                TestFixture CurrentFixture = null;
                if (this.TestFixtures.Contains(prodType.FullName))
                {
                    CurrentFixture = this.TestFixtures[prodType.FullName];
                }
                

                writer.WriteStartElement("Class");
                
                writer.WriteStartAttribute("FullName", null);
                writer.WriteString(prodType.FullName);
                writer.WriteEndAttribute();

                writer.WriteStartAttribute("TestFixture", null);
                if (CurrentFixture == null)
                {
                    writer.WriteString(String.Empty);
                }
                else
                {
                    writer.WriteString(CurrentFixture.FullName);
                }
                
                writer.WriteEndAttribute();

                String MemberType;
                bool IsCoverable;
                foreach (MemberInfo Member in prodType.GetMembers())
                {
                    MemberType = String.Empty;
                    IsCoverable = false;
                    if (Member is ConstructorInfo)
                    {
                        IsCoverable = true;
                        MemberType = "Constructor";
                    }
                    else if (Member is MethodInfo)
                    {
                        if (!Regex.IsMatch(Member.Name, @"get_|set_"))
                        {
                            IsCoverable = true;
                            MemberType = "Method";
                        }
                    }
                    else if (Member is PropertyInfo)
                    {
                        IsCoverable = true;
                        MemberType = "Property";
                    }

                    if (IsCoverable && Member.DeclaringType == Member.ReflectedType)
                    {
                        writer.WriteStartElement("Member");

                        writer.WriteStartAttribute("Name", null);
                        writer.WriteString(Member.Name);
                        writer.WriteEndAttribute();

                        writer.WriteStartAttribute("MemberType", null);
                        writer.WriteString(MemberType);
                        writer.WriteEndAttribute();

                        writer.WriteStartAttribute("TestMethod", null);
                        if (CurrentFixture != null && CurrentFixture.Contains(Member.Name))
                        {
                            writer.WriteString(CurrentFixture[Member.Name]);
                        }
                        else
                        {
                            writer.WriteString(String.Empty);
                        }
                        
                        writer.WriteEndAttribute();

                        writer.WriteEndElement();
                    }
                }
                writer.WriteEndElement();
            }
        }

        private static System.Reflection.Assembly AssemblyResolve(object sender, ResolveEventArgs args)
        {
            char[] Separators = new char[1] { ',' };
            string AsmName = args.Name.Split(Separators)[0];
            foreach (string Directory in HintDirectories){
                string AsmPath = Path.Combine(Directory, string.Format("{0}.dll", AsmName));
                if (System.IO.File.Exists(AsmPath))
                {
                    return Assembly.LoadFrom(AsmPath);
                }
                AsmPath = Path.Combine(Directory, string.Format("{0}.exe", AsmName));
                if (System.IO.File.Exists(AsmPath))
                {
                    return Assembly.LoadFrom(AsmPath);
                }
            }
            return null;
        }
    }
}
