
using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;
using NAnt;
using NAnt.Core;
using NAnt.Core.Attributes;
using NAnt.Core.Tasks;

namespace Nant.Contrib.BuildInstallShield.Tasks
{
	public abstract class BuildInstallShieldBase : ExternalProgramBase
	{
		private const string UNINSTALL_KEY = "Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\";

		private string m_sIsmFile;
		private string m_sRelease;
		private string m_sStandAloneBuildExe;
		private string m_sBuildLocation;
		private string m_sUpdateProductVersion;
		private bool m_bBuildSilently = false;
		private bool m_bStopOnError = false;
		private bool m_bWarningAsError = false;
		private bool m_bNoCompile = false;

		[TaskAttribute("p", Required=true)]
		public string p
		{
			get { return m_sIsmFile; }
			set { m_sIsmFile = value; }
		}

		[TaskAttribute("r", Required=false)]
		public string r
		{
			get { return m_sRelease; }
			set { m_sRelease = value; }
		}

		[TaskAttribute("standalonebuildexe", Required=true)]
		public string standalonebuildexe
		{
			get { return m_sStandAloneBuildExe; }
			set { m_sStandAloneBuildExe = value; }
		}

		[TaskAttribute("b", Required=false)]
		public string b
		{
			get { return m_sBuildLocation; }
			set { m_sBuildLocation = value; }
		}

		[TaskAttribute("updateproductversion", Required=false)]
		public string updateproductversion
		{
			get { return m_sUpdateProductVersion; }
			set { m_sUpdateProductVersion = value; }
		}

		[TaskAttribute("s", Required=false)]
		[BooleanValidator()]
		public bool s
		{
			get { return m_bBuildSilently; }
			set { m_bBuildSilently = value; }
		}

		[TaskAttribute("x", Required=false)]
		[BooleanValidator()]
		public bool x
		{
			get { return m_bStopOnError; }
			set { m_bStopOnError = value; }
		}

		[TaskAttribute("w", Required=false)]
		[BooleanValidator()]
		public bool w
		{
			get { return m_bWarningAsError; }
			set { m_bWarningAsError = value; }
		}

		[TaskAttribute("n", Required=false)]
		[BooleanValidator()]
		public bool n
		{
			get { return m_bNoCompile; }
			set { m_bNoCompile = value; }
		}

		public override string ProgramFileName 
		{
			// The path and file name to the InstallShield stand alone build.
			get { return GetISSAExe(); }
		}

		// The base class calls this to build the command-line string.
		public abstract override string ProgramArguments 
		{
			get;
		}

		protected override void ExecuteTask() 
		{
			// we’ll let the base task do all the work.
			base.ExecuteTask();
		}

		protected string GetBaseArguments()
		{
			if (updateproductversion != null)
			{
				ChangeProductVersion();
			}

			string sCmdLine;
			sCmdLine = "-p " + "\"" + ismfile + "\"";

			if (release != null)
			{
				sCmdLine += " -r " + "\"" + release + "\"";
			}

			if (buildlocation != null)
			{
				sCmdLine += " -b " + "\"" + buildlocation + "\"";
			}

			if (buildsilently)
			{
				sCmdLine += " -s";
			}

			if (warningaserror)
			{
				sCmdLine += " -w";
			}

			if (stoponerror)
			{
				sCmdLine += " -x";
			}

			if (n)
			{
				sCmdLine += " -n";
			}

			return sCmdLine;
		}

		private string GetISSAExe()
		{
			string sExe = m_sStandAloneBuildExe;
			if ('{' == sExe[0])
			{
				string sKey = Path.Combine(UNINSTALL_KEY, sExe);

				RegistryKey reg = Registry.LocalMachine.OpenSubKey(sKey);
				string sValue = (string)reg.GetValue("InstallLocation");

				sExe = Path.Combine(sValue, "IsSABld.exe");
				reg.Close();

				// format it
				sExe = " " + sExe + " ";
			}
			return sExe;
		}

		protected void ChangeProductVersion()
		{
			if (!File.Exists(m_sIsmFile))
			{
				return;
			}

			System.IO.FileAttributes fileAttribs = File.GetAttributes(m_sIsmFile);

			const string TEMP_FILE_EXT = ".tmp";
			string sTempFile = m_sIsmFile + TEMP_FILE_EXT;

			if (File.Exists(sTempFile))
			{
				File.SetAttributes(sTempFile, System.IO.FileAttributes.Normal);
				File.Delete(sTempFile);
			}

			bool bReplaced = false;

			try 
			{
				using (StreamWriter sw = new StreamWriter(sTempFile))
				{
					using (StreamReader sr = new StreamReader(m_sIsmFile)) 
					{
						const string BEGIN_PROPERTY_TABLE = "<table name=\"Property\">";
						const string END_TABLE = "</table>";
						const string END_OF_ELEMENT_START_OF_NEW = "</td><td>";
						const string ELEM_VALUE_TO_REPLACE = "ProductVersion";

						bool bFoundPropertySection = false;
						bool bFoundEndOfPropertySection = false;

						int nPos = -1;
						string sLine;
						while ((sLine = sr.ReadLine()) != null)
						{
							if (!bReplaced && !bFoundEndOfPropertySection)
							{
								if (bFoundPropertySection)
								{
									bFoundEndOfPropertySection = (-1 < sLine.LastIndexOf(END_TABLE));
								}
								else
								{
									bFoundPropertySection = (-1 < sLine.LastIndexOf(BEGIN_PROPERTY_TABLE));
								}

								if (!bFoundEndOfPropertySection && bFoundPropertySection && -1 < (nPos = sLine.LastIndexOf(ELEM_VALUE_TO_REPLACE)))
								{
									// Found "ProductVersion"

									nPos += ELEM_VALUE_TO_REPLACE.Length + END_OF_ELEMENT_START_OF_NEW.Length;
									string sBegin = sLine.Substring(0, nPos);

									// Find the end of the ProductVersion value
									// (if the version is "1.0.0.0", find the
									// position of the last '0')
									while (sLine[nPos++] != '<');

									--nPos;
									string sEnd = sLine.Substring(nPos, sLine.Length - nPos);

									
									sLine = sBegin + m_sUpdateProductVersion + sEnd;
									bReplaced = true;
								}
							}
							sw.WriteLine(sLine);
						}
					}
				}
				if (bReplaced)
				{
					File.SetAttributes(m_sIsmFile, System.IO.FileAttributes.Normal);
					File.Delete(m_sIsmFile);
					File.Move(sTempFile, m_sIsmFile);
					File.SetAttributes(m_sIsmFile, fileAttribs);
				}
			}
			catch (Exception ex) 
			{
				Debug.Assert(false, ex.ToString(), ex.Message);
				return;
			}

			Debug.Assert(bReplaced);
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
				return base.GetBaseArguments();
			}
		}
	}

	[TaskName("buildinstallshieldmsi")]
	public class BuildInstallShieldMsi : BuildInstallShieldBase
	{
		private string m_sProductConfiguration;
		private string m_sReleaseConfiguration;
		private string m_sMergeModuleSearchPath;
		private bool m_bSkipUpgrade = false;
		private string m_sDotNetFrameworkPath;
		private string m_sMinimumTargetMsiVersion;
		private string m_sMinimumTargetDotNetFrameworkVersion;
		private bool m_bCreateSetupExe = false;
		private string m_sReleaseFlags;
		private bool m_bQ1 = false;
		private bool m_bQ2 = false;
		private bool m_bQ3 = false;

		[TaskAttribute("a", Required=false)]
		public string a
		{
			get { return m_sProductConfiguration; }
			set { m_sProductConfiguration = value; }
		}

		[TaskAttribute("c", Required=false)]
		public string c
		{
			get { return m_sReleaseConfiguration; }
			set { m_sReleaseConfiguration = value; }
		}

		[TaskAttribute("o", Required=false)]
		public string o
		{
			get { return m_sMergeModuleSearchPath; }
			set { m_sMergeModuleSearchPath = value; }
		}

		[TaskAttribute("h", Required=false)]
		[BooleanValidator()]
		public bool h
		{
			get { return m_bSkipUpgrade; }
			set { m_bSkipUpgrade = value; }
		}

		[TaskAttribute("t", Required=false)]
		public string t
		{
			get { return m_sDotNetFrameworkPath; }
			set { m_sDotNetFrameworkPath = value; }
		}

		[TaskAttribute("g", Required=false)]
		public string g
		{
			get { return m_sMinimumTargetMsiVersion; }
			set { m_sMinimumTargetMsiVersion = value; }
		}

		[TaskAttribute("j", Required=false)]
		public string j
		{
			get { return m_sMinimumTargetDotNetFrameworkVersion; }
			set { m_sMinimumTargetDotNetFrameworkVersion = value; }
		}

		[TaskAttribute("e", Required=false)]
		[BooleanValidator()]
		public bool e
		{
			get { return m_bCreateSetupExe; }
			set { m_bCreateSetupExe = value; }
		}

		[TaskAttribute("f", Required=false)]
		public string f
		{
			get { return m_sReleaseFlags; }
			set { m_sReleaseFlags = value; }
		}

		[TaskAttribute("q1", Required=false)]
		[BooleanValidator()]
		public bool q1
		{
			get { return m_bQ1; }
			set { m_bQ1 = value; }
		}

		[TaskAttribute("q2", Required=false)]
		[BooleanValidator()]
		public bool q2
		{
			get { return m_bQ2; }
			set { m_bQ2 = value; }
		}

		[TaskAttribute("q3", Required=false)]
		[BooleanValidator()]
		public bool q3
		{
			get { return m_bQ3; }
			set { m_bQ3 = value; }
		}

		// The base class calls this to build the command-line string.
		public override string ProgramArguments 
		{
			get 
			{
				string sCmdLine = base.GetBaseArguments();

				if (null != a)
				{
					sCmdLine += " -a " + "\"" + productconfiguration + "\"";
				}

				if (null != o)
				{
					sCmdLine += " -o " + "\"" + mergemodulesearchpath + "\"";
				}

                if (null != c)
				{
					sCmdLine += " -c " + "\"" + releaseconfiguration + "\"";
				}

				if (h)
				{
					sCmdLine += " -h";
				}

                if (null != t)
				{
					sCmdLine += " -t " + "\"" + dotnetframeworkpath + "\"";
				}

                if (null != g)
				{
					sCmdLine += " -g " + "\"" + minimumtargetmsiversion + "\"";
				}

                if (null != j)
				{
					sCmdLine += " -j " + "\"" + minimumtargetdotnetframeworkversion + "\"";
				}

				if (e)
				{
					sCmdLine += " -e y";
				}
				else
				{
					sCmdLine += " -e n";
				}

                if (null != f)
				{
					sCmdLine += " -f " + "\"" + releaseflags + "\"";
				}

				if (q1)
				{
					sCmdLine += " -q1";
				}

				if (q2) 
				{
					sCmdLine += " -q2";
				}

				if (q3) 
				{
					sCmdLine += " -q3";
				}

				return sCmdLine;
			}
		}
	}
}
