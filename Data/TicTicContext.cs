using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicTic.Data
{
    public class TicTicContext : DbContext
    {
        public TicTicContext(DbContextOptions<TicTicContext> options)
            : base(options)
        {
        }
        public DbSet<RoomData> RoomData { get; set; }
        public DbSet<GameData> GameData { get; set; }
    }
}
