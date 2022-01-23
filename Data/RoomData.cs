using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TicTic.Data
{
    public class RoomData
    {
        [Key]
        public int Id { get; set; }

        public string HostId { get; set; }

        public int RoomNo { get; set; }

        public string? JoinerId { get; set; }
        public bool IsHostReady { get; set; }
        public bool IsJoinerReady { get; set; }


        public ICollection<GameData> GameData { get; set; }


    }
}
