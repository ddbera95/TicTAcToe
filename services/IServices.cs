using System.Collections.Generic;
using System.Threading.Tasks;
using TicTic.Data;

namespace TicTic.services
{
    public interface IServices
    {

        Task GameStart(int roomno);
        Task CreateGrid(int roomno);
        Task DropGrid(int roomno);
        Task GameEnd(int roomno);
        string IsGameEnd(List<GameData> gameData);
        string Toss();

    }
}