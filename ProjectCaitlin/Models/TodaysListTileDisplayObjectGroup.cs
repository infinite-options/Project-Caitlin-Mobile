using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ProjectCaitlin.Models
{
    public class TodaysListTileDisplayObjectGroup : ObservableCollection<TodaysListTileDisplayObject>
    {
        public string Name { get; set; }

        public string GroupIcon { get; set; }

        public TodaysListTileDisplayObjectGroup(string Name, ObservableCollection<TodaysListTileDisplayObject> Tiles) : base(Tiles)
        {
            this.Name = Name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
