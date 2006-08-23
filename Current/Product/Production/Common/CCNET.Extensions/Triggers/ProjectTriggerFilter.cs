using System;
using System.Collections;
using Exortech.NetReflector;
using ThoughtWorks.CruiseControl.Core;
using ThoughtWorks.CruiseControl.Core.Util;
using ThoughtWorks.CruiseControl.Remote;

namespace CCNET.Extensions.Triggers
{
    [ReflectorType("projectTriggerFilter")]
    public class ProjectTriggerFilter : ITrigger
    {
        
#region Fields

        private ITrigger _InnerTrigger;
        private ProjectFilterList _ProjectFilters;

#endregion
        
#region Properties

        [ReflectorCollection("projectFilters", InstanceType = typeof(ProjectFilterList), Required = true)]
        public ProjectFilterList ProjectFilters
        {
            get
            {
                return _ProjectFilters;
            }
            set
            {
                _ProjectFilters = value;
            }
        }

        [ReflectorProperty("trigger", InstanceTypeKey = "type")]
        public ITrigger InnerTrigger
        {
            get
            {
                return _InnerTrigger;
            }
            set
            {
                _InnerTrigger = value;
            }
        }

#endregion

#region ITrigger Members

        public void IntegrationCompleted()
        {
            this.InnerTrigger.IntegrationCompleted();
        }

        public DateTime NextBuild
        {
            get
            {
                return this.InnerTrigger.NextBuild;
            }
        }

        public BuildCondition ShouldRunIntegration()
        {
            if (this.InnerTrigger.ShouldRunIntegration() == BuildCondition.NoBuild)
                return BuildCondition.NoBuild;

            foreach (ProjectFilter Project in this.ProjectFilters)
            {
                if (!Project.IsAllowed())
                {
                    this.InnerTrigger.IntegrationCompleted();
                    return BuildCondition.NoBuild;
                }
            }
            
            return this.InnerTrigger.ShouldRunIntegration();
        }
        
#endregion

        public void Test()
        {
            this.ProjectFilters = new ProjectFilterList();
            ((IList)this.ProjectFilters).Add(new ProjectFilter());
            this.ProjectFilters[0].ExclusionFilters.Conditions = new IntegrationStatus[1] { IntegrationStatus.Failure };
            this.ProjectFilters[0].ExclusionFilters.Activities = new ProjectActivity[2] { ProjectActivity.Building, ProjectActivity.CheckingModifications };

            this.ProjectFilters[0].Project = "experimental1";
            this.ProjectFilters[0].ServerUri = "tcp://localhost:21247/CruiseManager.rem";

            this.InnerTrigger = new ThoughtWorks.CruiseControl.Core.Triggers.IntervalTrigger();
            ((ThoughtWorks.CruiseControl.Core.Triggers.IntervalTrigger)this.InnerTrigger).IntervalSeconds = 60;

            string Serialized = Zation.Serialize("projectTriggerFilter", this);

            ProjectTriggerFilter Clone = (ProjectTriggerFilter)Zation.Deserialize(Serialized);
            if (Clone.ProjectFilters[0].ExclusionFilters.Activities[0] != ProjectActivity.Building)
                throw new Exception();
        }

        public void Test2()
        {
            Project TestProject = new Project();
            TestProject.Name = "Test Project";

            ArrayList Triggers = new ArrayList();
            Triggers.Add(this);
            TestProject.Triggers = (ITrigger[]) Triggers.ToArray(typeof(ITrigger));

            this.ProjectFilters = new ProjectFilterList();
            ((IList)this.ProjectFilters).Add(new ProjectFilter());
            this.ProjectFilters[0].ExclusionFilters.Conditions = new IntegrationStatus[1] { IntegrationStatus.Failure };
            this.ProjectFilters[0].ExclusionFilters.Activities = new ProjectActivity[1] { ProjectActivity.Building };

            this.ProjectFilters[0].Project = "Project 1";

            this.InnerTrigger = new ThoughtWorks.CruiseControl.Core.Triggers.IntervalTrigger();
            ((ThoughtWorks.CruiseControl.Core.Triggers.IntervalTrigger)this.InnerTrigger).IntervalSeconds = 60;

            string Serialized = Zation.Serialize("project", TestProject);

            Project Clone = (Project)Zation.Deserialize(Serialized);
            if (((ProjectTriggerFilter)Clone.Triggers[0]).ProjectFilters[0].ExclusionFilters.Activities[0] != ProjectActivity.Building)
                throw new Exception();
        }

        public void Test3()
        {
            
            string Serialized = "<project name=\"experimental2\"><state type=\"state\" /><triggers><projectTriggerFilter><trigger type=\"intervalTrigger\" seconds=\"60\" /><projectFilters><projectFilter serverUri=\"tcp://localhost:21247/CruiseManager.rem\" project=\"experimental1\"><exclusionFilters><conditions><condition>Failure</condition></conditions><activities><activity>Building</activity><activity>CheckingModifications</activity></activities></exclusionFilters></projectFilter></projectFilters></projectTriggerFilter></triggers><labeller type=\"defaultlabeller\"><prefix>1.0.0.</prefix></labeller><tasks><nant><executable>C:\\Temp\\TestProjects\\nAnt\\bin\\nant.exe</executable><baseDirectory>C:\\Temp\\TestProjects</baseDirectory><logger>NAnt.Core.XmlLogger </logger><buildFile>scratch.build.xml</buildFile><buildTimeoutSeconds>5400</buildTimeoutSeconds></nant></tasks><publishers><xmllogger /></publishers></project>";

            NetReflectorProjectSerializer Izer = new NetReflectorProjectSerializer();
            Project Clone = Izer.Deserialize(Serialized);
            if (((ProjectTriggerFilter)Clone.Triggers[0]).ProjectFilters[0].ExclusionFilters.Activities[0] != ProjectActivity.Building)
                throw new Exception();
        }
    }
}