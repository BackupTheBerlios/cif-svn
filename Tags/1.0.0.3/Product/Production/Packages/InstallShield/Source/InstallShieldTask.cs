
using System;
using System.IO;
using Microsoft.Win32;
using NAnt;
using NAnt.Core;
using NAnt.Core.Attributes;
using NAnt.Core.Tasks;
using System.Text;

namespace InstallShield.Tasks
{

	public abstract class BuildInstallShieldBase : ExternalProgramBase
	{
        public const string BEGIN_PROPERTY_TABLE = "<table name=\"Property\">";
        public const string END_TABLE = "</table>";
        public const string END_OF_ELEMENT_START_OF_NEW = "</td><td>";
        public const string ELEM_VALUE_TO_REPLACE = "ProductVersion";
        public const string TEMP_FILE_EXT = ".tmp";

        private string _IsmFile;
        private string _Release;
        private string _BuildLocation;
        private string _UpdateProductVersion;
        private string _ProgramFile;

        [TaskAttribute("ismfile", Required = true)]
        public string IsmFile
        {
            get { return _IsmFile; }
            set { _IsmFile = value; }
        }

        [TaskAttribute("release", Required = false)]
        public string Release
        {
            get { return _Release; }
            set { _Release = value; }
        }

        [TaskAttribute("buildlocation", Required = false)]
        public string BuildLocation
        {
            get { return _BuildLocation; }
            set { _BuildLocation = value; }
        }

        [TaskAttribute("updateproductversion", Required = false)]
        public string UpdateProductVersion
        {
            get { return _UpdateProductVersion; }
            set { _UpdateProductVersion = value; }
        }

        [TaskAttribute("programfile", Required = false)]
        public string ProgramFile
        {
            set 
            { 
                this._ProgramFile = value; 
            }
        }

        public override string ProgramFileName
        {
            get
            {
                return this._ProgramFile;
            }
        }

        protected void ChangeProductVersion()
        {
            if (!File.Exists(this.IsmFile))
            {
                return;
            }

            System.IO.FileAttributes FileAttribs = File.GetAttributes(this.IsmFile);

			string TempFile = this.IsmFile + TEMP_FILE_EXT;

			if (File.Exists(TempFile))
			{
				File.SetAttributes(TempFile, System.IO.FileAttributes.Normal);
				File.Delete(TempFile);
			}

			bool Replaced = false;

			using (StreamWriter Writer = new StreamWriter(TempFile))
				{
					using (StreamReader Reader = new StreamReader(this.IsmFile)) 
					{
						bool FoundPropertySection = false;
						bool FoundEndOfPropertySection = false;

						int Position = -1;
						string Line;
						while ((Line = Reader.ReadLine()) != null)
						{
							if (!Replaced && !FoundEndOfPropertySection)
							{
								if (FoundPropertySection)
								{
									FoundEndOfPropertySection = (-1 < Line.LastIndexOf(END_TABLE));
								}
								else
								{
									FoundPropertySection = (-1 < Line.LastIndexOf(BEGIN_PROPERTY_TABLE));
								}

								if (!FoundEndOfPropertySection && FoundPropertySection && -1 < (Position = Line.LastIndexOf(ELEM_VALUE_TO_REPLACE)))
								{
									// Found "ProductVersion"

									Position += ELEM_VALUE_TO_REPLACE.Length + END_OF_ELEMENT_START_OF_NEW.Length;
									string sBegin = Line.Substring(0, Position);

									// Find the end of the ProductVersion value
									// (if the version is "1.0.0.0", find the
									// position of the last '0')
									while (Line[Position++] != '<');

									string End = Line.Substring(--Position, Line.Length - Position);
									
									Line = sBegin + _UpdateProductVersion + End;
									Replaced = true;
								}
							}
							Writer.WriteLine(Line);
						}
					}
				}

			if (Replaced)
			{
				File.SetAttributes(this.IsmFile, System.IO.FileAttributes.Normal);
				File.Delete(this.IsmFile);
				File.Move(TempFile, this.IsmFile);
				File.SetAttributes(this.IsmFile, FileAttribs);
			}
		}

        protected override void ExecuteTask()
        {
            if (this.UpdateProductVersion != null)
			{
                this.ChangeProductVersion();
			}
            base.ExecuteTask();
        }

        public string MakeOptionPair(string option, string value)
        {
        	return String.Format(" {0} \"{1}\" ", option, value);
        }
    
    }

	[TaskName("buildinstallscript")]
	public class BuildInstallScript : BuildInstallShieldBase
	{
		// The base class calls this to build the command-line string.
		public override string ProgramArguments 
		{
			get 
			{
				StringBuilder CmdLineBuilder = new StringBuilder();
				CmdLineBuilder.Append(this.MakeOptionPair("-p", this.IsmFile));

				if (this.Release != null)
				{
					CmdLineBuilder.Append(this.MakeOptionPair("-r", this.Release));
				}
                return CmdLineBuilder.ToString();
			}
		}
	}

	[TaskName("buildinstallshieldmsi")]
	public class BuildInstallShieldMsi : BuildInstallShieldBase
	{
		private string _ProductConfiguration;
		private string _ReleaseConfiguration;
		private string _MergeModuleSearchPath;
		private bool _SkipUpgrade;
		private string _DotNetFrameworkPath;
		private string _MinimumTargetMsiVersion;
		private string _MinimumTargetDotNetFrameworkVersion;
		private bool _CreateSetupExe = true;
		private string _ReleaseFlags;
		private bool _BuildSilently = false;

		[TaskAttribute("productconfiguration", Required=false)]
		public string ProductConfiguration
		{
			get { return _ProductConfiguration; }
			set { _ProductConfiguration = value; }
		}

		[TaskAttribute("releaseconfiguration", Required=false)]
		public string ReleaseConfiguration
		{
			get { return _ReleaseConfiguration; }
			set { _ReleaseConfiguration = value; }
		}

		[TaskAttribute("mergemodulesearchpath", Required=false)]
		public string MergeModuleSearchPath
		{
			get { return _MergeModuleSearchPath; }
			set { _MergeModuleSearchPath = value; }
		}

		[TaskAttribute("skipupgrade", Required=false)]
		[BooleanValidator()]
		public bool SkipupGrade
		{
			get { return _SkipUpgrade; }
			set { _SkipUpgrade = value; }
		}

		[TaskAttribute("dotnetframeworkpath", Required=false)]
		public string DotNetFrameworkPath
		{
			get { return _DotNetFrameworkPath; }
			set { _DotNetFrameworkPath = value; }
		}

		[TaskAttribute("minimumtargetmsiversion", Required=false)]
		public string MinimumTargetMsiVersion
		{
			get { return _MinimumTargetMsiVersion; }
			set { _MinimumTargetMsiVersion = value; }
		}

		[TaskAttribute("minimumtargetdotnetframeworkversion", Required=false)]
		public string MinimumTargetDotNetFrameworkVersion
		{
			get { return _MinimumTargetDotNetFrameworkVersion; }
			set { _MinimumTargetDotNetFrameworkVersion = value; }
		}

		[TaskAttribute("createsetupexe", Required=false)]
		[BooleanValidator()]
		public bool CreateSetupExe
		{
			get { return _CreateSetupExe; }
			set { _CreateSetupExe = value; }
		}

		[TaskAttribute("releaseflags", Required=false)]
		public string ReleaseFlags
		{
			get { return _ReleaseFlags; }
			set { _ReleaseFlags = value; }
		}

		[TaskAttribute("buildsilently", Required=false)]
		[BooleanValidator()]
		public bool BuildSilently
		{
			get { return _BuildSilently; }
			set { _BuildSilently = value; }
		}

		// The base class calls this to build the command-line string.
		public override string ProgramArguments 
		{
			get 
			{
                StringBuilder CmdLineBuilder = new StringBuilder();
				CmdLineBuilder.Append(this.MakeOptionPair("-p", this.IsmFile));

				if (this.ProductConfiguration != null)
				{
                    CmdLineBuilder.Append(this.MakeOptionPair("-a", this.ProductConfiguration));
				}

                if (this.Release != null)
				{
					CmdLineBuilder.Append(this.MakeOptionPair("-r", this.Release));
				}

				if (this.MergeModuleSearchPath != null)
				{
                    CmdLineBuilder.Append(this.MakeOptionPair("-o", this.MergeModuleSearchPath));
				}

				if (this.ReleaseConfiguration != null)
				{
                    CmdLineBuilder.Append(this.MakeOptionPair("-c", this.ReleaseConfiguration));
				}

				if (this.SkipupGrade)
				{
					CmdLineBuilder.Append(" -h");
				}

				if (this.DotNetFrameworkPath != null)
				{
                    CmdLineBuilder.Append(this.MakeOptionPair("-t", this.DotNetFrameworkPath));
				}

				if (this.MinimumTargetMsiVersion != null)
				{
                    CmdLineBuilder.Append(this.MakeOptionPair("-g", this.MinimumTargetMsiVersion));
				}

				if (this.MinimumTargetDotNetFrameworkVersion != null)
				{
                    CmdLineBuilder.Append(this.MakeOptionPair("-j", this.MinimumTargetDotNetFrameworkVersion));
				}

				if (this.BuildLocation != null)
				{
					CmdLineBuilder.Append(this.MakeOptionPair("-b", this.BuildLocation));
				}

				if (this.CreateSetupExe)
				{
					CmdLineBuilder.Append(" -e y");
				}
				else
				{
					CmdLineBuilder.Append(" -e n");
				}

				if (this.ReleaseFlags != null)
				{
                    CmdLineBuilder.Append(this.MakeOptionPair("-f", this.ReleaseFlags));
				}

				if (this.BuildSilently)
				{
                    CmdLineBuilder.Append(" -s");
				}

                return CmdLineBuilder.ToString();
			}
		}
	}
}
