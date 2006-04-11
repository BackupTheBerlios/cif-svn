using System;
using System.Collections;
using System.Text;

using NAnt.Core;
using NAnt.Core.Types;
using NAnt.Core.Attributes;
using NAnt.Core.Functions;

using Common.Tasks;

namespace Common.Functions
{
    [FunctionSet("stringlist", "DataTypes")]
    public class StringListFunctions : FunctionSetBase
    {

        public StringListFunctions(Project project, PropertyDictionary properties)
            : base(project, properties)
        {
        }

        [Function("add")]
        public void AddInclude(String refID, String name)
        {
            if (!this.Project.DataTypeReferences.Contains(refID))
                throw new BuildException(String.Format("The refid {0} is not defined.", refID));

            StringList RefStringList = (StringList)this.Project.DataTypeReferences[refID];
            RefStringList.StringItems.Add(name, new StringItem(name));
        }

        [Function("remove")]
        public void RemoveInclude(String refID, String name)
        {
            if (!this.Project.DataTypeReferences.Contains(refID))
                throw new BuildException(String.Format("The refid {0} is not defined.", refID));

            StringList RefStringList = (StringList)this.Project.DataTypeReferences[refID];
            RefStringList.StringItems.Remove(name);
        }

        [Function("contains")]
        public Boolean ContainsInclude(String refID, String name)
        {
            if (!this.Project.DataTypeReferences.Contains(refID))
                throw new BuildException(String.Format("The refid {0} is not defined.", refID));

            StringList RefStringList = (StringList)this.Project.DataTypeReferences[refID];
            return RefStringList.StringItems.Contains(name);
        }

        [Function("count")]
        public int Count(String refID)
        {
            if (!this.Project.DataTypeReferences.Contains(refID))
                throw new BuildException(String.Format("The refid {0} is not defined.", refID));

            StringList RefStringList = (StringList)this.Project.DataTypeReferences[refID];
            return RefStringList.StringItems.Count;
        }

    }
}
