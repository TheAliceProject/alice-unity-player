using System.Collections.Generic;

namespace Alice.Linker
{
    public class PropertyDescription
    {
        private string name;
        private string type;
        private List<StatementDescription> initializer; // TODO: revisit
    }
}
