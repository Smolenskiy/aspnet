using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountSystem.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Record> Records { get; set; }

        public Project()
        {
            Records = new List<Record>();
        }
    }
}
