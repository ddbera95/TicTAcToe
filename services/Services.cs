using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicTic.Data;

namespace TicTic.services
{
    public class Services : IServices
    {
        public TicTicContext _ticTicContext;
        public Services(TicTicContext ticTicContext)
        {
            _ticTicContext = ticTicContext;
        }





        public async Task GameStart(int roomno)
        {
            var x = _ticTicContext.RoomData.Where(x => x.RoomNo == roomno).SingleOrDefault();

            await CreateGrid(roomno);



        }

        public async Task CreateGrid(int roomno)
        {
            var x = _ticTicContext.RoomData.Where(x => x.RoomNo == roomno).SingleOrDefault();

            for (var i = 1; i <= 3; i++)
            {
                for (var j = 1; j <= 3; j++)
                {
                    var temp = Convert.ToString(i) + Convert.ToString(j);
                    var cell = Convert.ToInt32(temp);

                    var tp = new GameData()
                    {
                        roomDataId = x.RoomNo,
                        cellCode = cell
                    };


                    _ticTicContext.GameData.Add(tp);
                    await _ticTicContext.SaveChangesAsync();



                }
            }


        }

        public async Task DropGrid(int roomno)
        {
            var x = _ticTicContext.RoomData.Where(x => x.RoomNo == roomno).SingleOrDefault();

            var Temp = x.GameData;
            foreach (var item in Temp)
            {
                _ticTicContext.GameData.Remove(item);
                await _ticTicContext.SaveChangesAsync();
            }


        }
        public async Task GameEnd(int roomno)
        {
            var x = _ticTicContext.RoomData.Where(x => x.RoomNo == roomno).SingleOrDefault();

            await DropGrid(roomno);

        }


        public string IsGameEnd(List<GameData> data)
        {
            int[,] arr = new int[3, 3];
            string[,] strr = new string[3, 3];
            string winner = "not yet";
            bool isAnyRemain = false;
            foreach (var item in data)
            {
                int x;
                int y;
                string tmp;


                tmp = Convert.ToString(item.cellCode);
                x = Convert.ToInt32(tmp.Substring(0, 1));
                y = Convert.ToInt32(tmp.Substring(1, 1));
                arr[x - 1, y - 1] = item.cellCode;
                strr[x - 1, y - 1] = item.cellFiller;



                if (item.cellFiller == "")
                {

                    isAnyRemain = true;
                }

            }

            //horizontal Lines
            if (strr[0, 0] != "" && strr[0, 1] != "" && strr[0, 2] != "")
            {
                if (strr[0, 0] == strr[0, 1] && strr[0, 1] == strr[0, 2] && strr[0, 0] == strr[0, 2])
                {

                    winner = strr[0, 0];

                    return winner;
                }
            }
            if (strr[1, 0] != "" && strr[1, 1] != "" && strr[1, 2] != "")
            {
                if (strr[1, 0] == strr[1, 1] && strr[1, 1] == strr[1, 2] && strr[1, 0] == strr[1, 2])
                {
                    winner = strr[1, 0];

                    return winner;

                }
            }
            if (strr[2, 0] != "" && strr[2, 1] != "" && strr[2, 2] != "")
            {
                if (strr[2, 0] == strr[2, 1] && strr[2, 1] == strr[2, 2] && strr[2, 0] == strr[2, 2])
                {
                    winner = strr[2, 0];

                    return winner;
                }
            }
            //verticle lines
            if (strr[0, 0] != "" && strr[1, 0] != "" && strr[2, 0] != "")
            {
                if (strr[0, 0] == strr[1, 0] && strr[1, 0] == strr[2, 0] && strr[0, 0] == strr[2, 0])
                {
                    winner = strr[0, 0];

                    return winner;
                }
            }
            if (strr[0, 1] != "" && strr[1, 1] != "" && strr[2, 1] != "")
            {
                if (strr[0, 1] == strr[1, 1] && strr[1, 1] == strr[2, 1] && strr[0, 1] == strr[2, 1])
                {
                    winner = strr[0, 1];

                    return winner;
                }
            }
            if (strr[0, 2] != "" && strr[1, 2] != "" && strr[2, 2] != "")
            {
                if (strr[0, 2] == strr[1, 2] && strr[1, 2] == strr[2, 2] && strr[0, 2] == strr[2, 2])
                {
                    winner = strr[0, 2];

                    return winner;
                }
            }
            //cross lines
            if (strr[0, 0] != "" && strr[1, 1] != "" && strr[2, 2] != "")
            {
                if (strr[0, 0] == strr[1, 1] && strr[1, 1] == strr[2, 2] && strr[0, 0] == strr[2, 2])
                {
                    winner = strr[0, 0];

                    return winner;
                }
            }
            if (strr[1, 2] != "" && strr[1, 1] != "" && strr[0, 2] != "")
            {
                if (strr[1, 2] == strr[1, 1] && strr[1, 1] == strr[0, 2] && strr[1, 2] == strr[0, 2])
                {
                    winner = strr[1, 2];

                    return winner;
                }
            }


            if (!isAnyRemain)
            {
                winner = "draw";
            }

            return winner;
        }

        public string Toss()
        {
            var rnd = new Random();

            if (rnd.Next(0, 2) == 0)
            {
                return "host";
            }
            else
            {
                return "joiner";
            }
        }

       
    }

    

   
}
