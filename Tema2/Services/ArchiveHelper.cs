using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tema2.Model;

namespace Tema2.Services
{
    [Serializable]
    class ArchiveHelper
    {
        public List<TDL> ToDoLists { get; set; }
        public List<string> Categories { get; set; }
        public ArchiveHelper(List<TDL> toDoLists, List<string> categories)
        {
            this.ToDoLists = toDoLists;
            this.Categories = categories;
        }
    }
}
