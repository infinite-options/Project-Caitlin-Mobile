using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectCaitlin.Services.Firebase
{
    class PeopleDto
    {
        public string pic { get; set; }
        public Boolean have_pic { get; set; }
        public string speaker_id { get; set; }
        public string unique_id { get; set; }
        public Boolean important { get; set; }
        public string name { get; set; }
        public string relation { get; set; }
        public string phone_number { get; set; }
    }
}
