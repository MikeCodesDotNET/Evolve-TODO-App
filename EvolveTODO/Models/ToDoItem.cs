using EvolveTODO.Helpers;

namespace EvolveTODO.Models
{
    // EntityData is in ../Helpers - it adds the required
    // fields for Azure App Service Mobile Apps SDK
    public class ToDoItem: EntityData
    {
        public string Text { get; set;}

        public bool Complete { get; set;}
    }
}

