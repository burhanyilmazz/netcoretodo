using System.Collections.Generic;

namespace Todo.Core.Entities
{
    public class ProjectInfo 
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string DefaultLanguage { get; set; }
        public List<string> SupportedLanguages { get; set; }
        public string HostName { get; set; }
        public Contact Contact { get; set; }
        public Api Api { get; set; }
    }

    public class Api
    {
        public string DeprecatedText { get; set; }
    }

    public class Contact
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Web { get; set; }
    }
}
