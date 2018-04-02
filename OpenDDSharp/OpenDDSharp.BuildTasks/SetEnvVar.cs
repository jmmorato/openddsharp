using System;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace OpenDDSharp.BuildTasks
{
    public class SetEnvVar : Task
    {
        private string _variable;
        private string _value;

        [Required]
        public string Variable
        {
            get { return _variable; }
            set { _variable = value; }
        }

        [Required]
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public override bool Execute()
        {
            Environment.SetEnvironmentVariable(_variable, _value);
            return true;
        }
    }
}
