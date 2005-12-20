using System;
using System.Text;
using System.Collections;
using System.IO;
using System.Xml;

using NAnt.Core;
using NAnt.Core.Attributes;

namespace Tracker.Tasks
 {
     
    [TaskName("trackerquery")]
    public class TrackerQuery : BaseTrackerTask
    {
        // Methods
        public TrackerQuery()
        {
        }

        protected override void ExecuteTask()
        {
            if ((this.ScrIdsProperty == null) && (this.ScrCountProperty == null))
            {
                throw new BuildException("trackerquery was called and not output was specified, should you have specified: scridsproperty or scrcountproperty?");
            }
            this.Login();
            int[] numArray1 = this.TrackerServer.GetSCRIDListFromQuery(this.Query);
            if (this.ScrCountProperty != null)
            {
                this.Properties[this.ScrCountProperty] = numArray1.Length.ToString();
            }
            if (this.ScrIdsProperty != null)
            {
                this.Properties[this.ScrIdsProperty] = this.FormatIdList(numArray1);
            }
            this.Logout();
        }

        private string FormatIdList(int[] IDs)
        {
            StringBuilder builder1 = new StringBuilder();
            builder1.Append(IDs[0]);
            int num2 = IDs.Length - 1;
            for (int num1 = 1; num1 <= num2; num1++)
            {
                builder1.Append(",");
                builder1.Append(IDs[num1]);
            }
            return builder1.ToString();
        }

        public void Test()
        {
            this.Query = "For Me";
            this.ConnectionInformation = new ConnectionInformation();
            ConnectionInformation information1 = this.ConnectionInformation;
            information1.DBMSLoginMode = 2;
            information1.DBMSServer = "Jupiter";
            information1.DBMSType = "Tracker SQL Server Sys";
            information1.DBMSPassword = "tracker12";
            information1.DBMSUserName = "tracker";
            information1.ProjectName = "EF";
            information1.UserName = "jflowers";
            information1.UserPWD = "password";
            try
            {
                this.ExecuteTask();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                throw;
            }
        }


        // Properties
        [TaskAttribute("query", Required=true)]
        public string Query
        {
            get
            {
                return this._Query;
            }
            set
            {
                this._Query = value;
            }
        }

        [TaskAttribute("countproperty", Required=false)]
        public string ScrCountProperty
        {
            get
            {
                return this._ScrCountProperty;
            }
            set
            {
                this._ScrCountProperty = value;
            }
        }

        [TaskAttribute("idsproperty", Required=false)]
        public string ScrIdsProperty
        {
            get
            {
                return this._ScrIdsProperty;
            }
            set
            {
                this._ScrIdsProperty = value;
            }
        }


        // Fields
        private string _Query;
        private string _ScrCountProperty;
        private string _ScrIdsProperty;
    }
     }