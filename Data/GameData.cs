using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicTic.Data
{
    public class GameData
    {

        public int Id { get; set; }

        public int roomDataId { get; set; }

        public RoomData roomData { get; set; }

        public int cellCode { get; set; }

        public string? cellFiller { get; set; }
    }
}
