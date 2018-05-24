using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AccountSystem.Models
{
    public class FilterViewModel
    {
        public FilterViewModel(List<Project> projects, int? project, DateTime? daterecord) //
        {
            projects.Insert(0, new Project { Name = "все", Id = 0 });
            Projects = new SelectList(projects, "Id", "Name", project);
            SelectedProject = project;
            SelectedDateTime = daterecord;
        }

        public SelectList Projects { get; private set; }
        public DateTime? SelectedDateTime { get; private set; }
        public int? SelectedProject { get; private set; }
    }
}
