using System;
using System.IO;
using System.Collections.Generic;

namespace Alice.Tweedle
{
    public class TweedleLibrary
    {
		private File.ProjectIdentifier id;
        //private File storageLocation;

        private List<File.ProjectIdentifier> prerequisites;
        private Dictionary<string, TweedleClass> classes;
        private Dictionary<string, TweedleResource> resources;

        public TweedleClass NewClass(string name)
        {
			// TODO
			//if (classes.ContainsKey(name))
			//{

			//}
			//TweedleClass classNew = new TweedleClass(name);
			//classes.Add(name, classNew);
			//return classNew;
			return null;
        }

        public TweedleType getTypeNamed(string name)
        {
            return classes[name];
        }
    }
}
