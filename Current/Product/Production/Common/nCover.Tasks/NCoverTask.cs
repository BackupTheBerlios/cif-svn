using System;
using System.Collections;
using System.Text;
using NAnt.Core;
using NAnt.Core.Tasks;

namespace nCover.Tasks
{
    public class NCoverTask : ExternalProgramBase
    {
        public override string ProgramArguments
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }
    }
}
