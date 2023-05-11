using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema2.Model
{
    public class Category
    {
        private List<string> types;
        public Category()
        {
            types = new List<string>();
            types.Add("Cooking");
            types.Add("Fitness");
            types.Add("Health");
            types.Add("Home");
            types.Add("Finances");
            types.Add("Cleaning");
            types.Add("Shopping");
            types.Add("Study");
            types.Add("Work");
            types.Add("School");
            types.Add("Travel");
            types.Add("Personal");
            types.Add("Other");
        }
        public List<string> Types
        {
            get
            {
                return types;
            }
        }
    }
}
