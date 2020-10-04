using ProjectCaitlin.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectCaitlin.Services.Rds
{
    public class ActionTaskResponse
    {
        public string message { get; set; }
        public List<ActionTasksDto> result { get; set; }

        internal List<atObject> ToActionTaskList()
        {
            List<atObject> ats = new List<atObject>();
            if (result == null || result.Count == 0) return ats;
            foreach (ActionTasksDto dto in result)
            {
                ats.Add(dto.ToATObjects());
            }
            return ats;
        }

        internal List<atObject> ToActionTaskList(string type)
        {

            List<atObject> ats = new List<atObject>();
            
            if (result == null || result.Count == 0) return ats;
            foreach (ActionTasksDto dto in result)
            {
                
                if (type=="goal")
                {
                    ats.Add(dto.ToAction());
                }
                else
                {
                    ats.Add(dto.ToTask());
                }
            }
            return ats;
        }
    }
}
