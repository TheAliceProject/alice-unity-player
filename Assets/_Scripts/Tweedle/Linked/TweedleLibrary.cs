/*
using System;
using System.IO;
using System.Collections.Generic;

namespace Alice.Tweedle
{
    public class TweedleLibrary
    {
		private ProjectIdentifier id;
        //private File storageLocation;

        private List<ProjectIdentifier> prerequisites;
        private Dictionary<string, TweedleClass> classes;
        private Dictionary<string, TweedleResource> resources;

        public TweedleClass newClass(string name)
        {
            if (classes.ContainsKey(name))
            {

            }
            TweedleClass classNew = new TweedleClass(name);
            classes.Add(name, classNew);
            return classNew;
        }

        public TweedleType getTypeNamed(string name)
        {
            return classes[name];
        }
    }
}
*/