using System;
using System.Collections;
using System.Text;
using Studio.VisualStudio8.Solution;
using NAnt.Core;
using NAnt.Core.Attributes;

namespace Studio.VisualStudio8.DataTypes
{
    [ElementName("global8")]
    public class GlobalElement : DataTypeBase
    {
        private GlobalSectionElement[] _GlobalSelections;
        private Global _Global;
        private bool _InSync = false;


        private Global Global
        {
            get
            {
                if (!this.InSync || this._Global == null)
                    this.MakeGlobal();
                return _Global;
            }
            set
            {
                _Global = value;
            }
        }

        private bool InSync
        {
            get
            {
                return _InSync;
            }
            set
            {
                _InSync = value;
            }
        }

        [BuildElementArray("globalsection")]
        public GlobalSectionElement[] GlobalSelections
        {
            get
            {
                return _GlobalSelections;
            }
            set
            {
                _GlobalSelections = value;
                this.InSync = false;
            }
        }

        private void MakeGlobal()
        {
            if (this._Global == null)
                this._Global = new Global();
            foreach (GlobalSectionElement CurrentSection in this.GlobalSelections)
            {
                this._Global.Sections.Add(CurrentSection.GetGlobalSection());
            }
            this.InSync = true;
        }

        public Global GetGlobal()
        {
            return this.Global;
        }

    }
}
