using ProjectCaitlin.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectCaitlin.Services.Rds
{
    class GratisResponse
    {
        public string message { get; set; }
        public List<GratisDto> result { get; set; }

        public List<GratisObject> ToGratisList()
        {
            List<GratisObject> gratisObjects = new List<GratisObject>();
            if (result == null || result.Count == 0) return gratisObjects;
            foreach(GratisDto dto in result)
            {
                gratisObjects.Add(dto.ToGratisObject());
            }
            return gratisObjects;
        }
    }
}
