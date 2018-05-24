using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountSystem.Models
{
    public class SortViewModel
    {
        public SortState RecordDateSort { get; set; }
        public SortState ProjectSort { get; set; }
        public SortState Current { get; set; }
        public bool Up { get; set; }

        public SortViewModel(SortState sortOrder)
        { 
            RecordDateSort = SortState.RecordDateAsc;
            ProjectSort = SortState.ProjectAsc;
            Up = true;

            if (sortOrder == SortState.RecordDateDesc || sortOrder == SortState.ProjectDesc)
            {
                Up = false;
            }

            switch (sortOrder)
            {
                case SortState.RecordDateDesc:
                    Current = RecordDateSort = SortState.RecordDateAsc;
                    break;
                case SortState.ProjectAsc:
                    Current = ProjectSort = SortState.ProjectDesc;
                    break;
                case SortState.ProjectDesc:
                    Current = ProjectSort = SortState.ProjectAsc;
                    break;
                default:
                    Current = RecordDateSort = SortState.RecordDateDesc;
                    break;
            }
        }
    }
}
