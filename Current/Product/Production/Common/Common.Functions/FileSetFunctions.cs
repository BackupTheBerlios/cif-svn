using System;
using System.Collections;
using System.Text;

using NAnt.Core;
using NAnt.Core.Types;
using NAnt.Core.Attributes;
using NAnt.Core.Functions;

namespace Common.Functions
{
    [FunctionSet("fileset", "DataTypes")]
    public class FileSetFunctions : FunctionSetBase
    {

        public FileSetFunctions(Project project, PropertyDictionary properties)
            : base(project, properties)
        {
        }

        [Function("include-add")]
        public void AddInclude(String refID, String name)
        {
            if (!this.Project.DataTypeReferences.Contains(refID))
                throw new BuildException(String.Format("The refid {0} is not defined.", refID));

            FileSet RefFileSet = (FileSet)this.Project.DataTypeReferences[refID];
            RefFileSet.Includes.Add(name);
        }

        [Function("include-remove")]
        public void RemoveInclude(String refID, String name)
        {
            if (!this.Project.DataTypeReferences.Contains(refID))
                throw new BuildException(String.Format("The refid {0} is not defined.", refID));

            FileSet RefFileSet = (FileSet)this.Project.DataTypeReferences[refID];
            RefFileSet.Includes.Remove(name);
        }

        [Function("include-contains")]
        public Boolean ContainsInclude(String refID, String name)
        {
            if (!this.Project.DataTypeReferences.Contains(refID))
                throw new BuildException(String.Format("The refid {0} is not defined.", refID));

            FileSet RefFileSet = (FileSet)this.Project.DataTypeReferences[refID];
            return RefFileSet.Includes.Contains(name);
        }

        [Function("exclude-add")]
        public void AddExclude(String refID, String name)
        {
            if (!this.Project.DataTypeReferences.Contains(refID))
                throw new BuildException(String.Format("The refid {0} is not defined.", refID));

            FileSet RefFileSet = (FileSet)this.Project.DataTypeReferences[refID];
            RefFileSet.Excludes.Add(name);
        }

        [Function("exclude-remove")]
        public void RemoveExclude(String refID, String name)
        {
            if (!this.Project.DataTypeReferences.Contains(refID))
                throw new BuildException(String.Format("The refid {0} is not defined.", refID));

            FileSet RefFileSet = (FileSet)this.Project.DataTypeReferences[refID];
            RefFileSet.Excludes.Remove(name);
        }

        [Function("exclude-contains")]
        public Boolean ContainsExclude(String refID, String name)
        {
            if (!this.Project.DataTypeReferences.Contains(refID))
                throw new BuildException(String.Format("The refid {0} is not defined.", refID));

            FileSet RefFileSet = (FileSet)this.Project.DataTypeReferences[refID];
            return RefFileSet.Excludes.Contains(name);
        }

    }
}
