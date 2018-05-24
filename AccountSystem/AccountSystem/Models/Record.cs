using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountSystem.Models
{
    public class Record
    {
        public int Id { get; set; }

        public DateTime RecordDate { get; set; }

        public string Text { get; set; }

        public int ProjectId { get; set; }

        public Project Project { get; set; }
    }
}
