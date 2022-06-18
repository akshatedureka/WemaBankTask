using System;
using System.Collections.Generic;
using System.Text;

namespace WemaBankTask.Entities.DTO
{
    public class LocationsDto
    {
        public List<State> States { get; set; }
    }

    public class State
    {
        public int StateId { get; set; }
        public string StateName { get; set; }
        public List<LGA> LGAs { get; set; }
    }

    public class LGA
    {
        public int LGAId { get; set; }
        public string LGAName { get; set; }
    }
}
