using System;
using Newtonsoft.Json;

namespace EvolveTODO.Models
{
    public class ToDoItem
    {
        [Newtonsoft.Json.JsonProperty("Id")]
        public string Id { get; set; }

        [Microsoft.WindowsAzure.MobileServices.Version]
        public string AzureVersion { get; set; }

        public string Text { get; set;}
        public bool Complete { get; set;}
    }
}

