﻿using System.Collections.Generic;

namespace Alice.Linker
{
    public class ClassDescription
    {
        public string Name
        {
            get { return Name; }
        }

        private string name;
        private string superclass;
        private List<MethodDescription> methods;
        private List<PropertyDescription> properties;

        public ClassDescription(string name)
        {
            this.name = name;
        }
    }
}
