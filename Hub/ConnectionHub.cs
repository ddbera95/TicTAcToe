using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TicTic.Data;
using TicTic.services;

namespace TicTic.Connections
{
    public class ConnectionHub : Hub 
    {
        public TicTicContext _ticTicContext;
        public IServices _services;
        public ConnectionHub(TicTicContext ticTicContext, IServices services)
        {
            _ticTicContext = ticTicContext;
            _services = services;
        }

        
        public async Task SendMessegeToAll()
        {
            await Clients.All.SendAsync("receivedMessege");
        }

        public override async Task OnConnectedAsync()
        {
           await Clients.Client(Context.ConnectionId).SendAsync("setConnectionId", Context.ConnectionId);
        }
        public override async Task OnDisconnectedAsync(Exception stopCalled)
        {


            var x = _ticTicContext.RoomData.Where(x => x.HostId == Context.ConnectionId || x.JoinerId == Context.ConnectionId).SingleOrDefault();
            if (x == null)
            {
                return;
            }
            else if (x.HostId == Context.ConnectionId && x.JoinerId != null)
            {
                await Clients.Client(x.JoinerId).SendAsync("ConectionCut");
                await Clients.Clients(x.JoinerId).SendAsync("connectionStop");
                _ticTicContext.RoomData.Remove(x);
                await _ticTicContext.SaveChangesAsync();
            }
            else if (x.JoinerId == Context.ConnectionId && x.HostId != null)
            {
                await Clients.Client(x.HostId).SendAsync("ConectionCut");
                await Clients.Clients(x.HostId).SendAsync("connectionStop");
                _ticTicContext.RoomData.Remove(x);
                await _ticTicContext.SaveChangesAsync();
            }
            else 
            {
                _ticTicContext.RoomData.Remove(x);
                await _ticTicContext.SaveChangesAsync();
            }
            


            }



        public async Task MakeRoom()
        {
            var x = "hoster";
                Random rnd = new Random();
                var Room = new RoomData
                {
                    HostId = Context.ConnectionId,
                    RoomNo = rnd.Next(100000,999999)
                };
                await _ticTicContext.RoomData.AddAsync(Room);
                await _ticTicContext.SaveChangesAsync();

                var res = _ticTicContext.RoomData.Where(x => x.HostId == Context.ConnectionId).SingleOrDefault();
                await Clients.Client(Context.ConnectionId).SendAsync("isRoonCreated", res.RoomNo,x);
            
        }
        public async Task JoinRoom(int roomno)
        {

            var room =  _ticTicContext.RoomData.Where(x=> x.RoomNo == roomno).SingleOrDefault();
            if(room == null)
            {
                await Clients.Client(Context.ConnectionId).SendAsync("noRoomForYou");
            }
            else if(room.JoinerId == null)
            {
                var x = "joiner";
                room.JoinerId = Context.ConnectionId;

                await _ticTicContext.SaveChangesAsync();

                var res = _ticTicContext.RoomData.Where(x => x.JoinerId == Context.ConnectionId).SingleOrDefault();
                await Clients.Client(Context.ConnectionId).SendAsync("isRoonCreated", res.RoomNo,x);
                
                    
                


            }
            else
            {
                await Clients.Client(Context.ConnectionId).SendAsync("alreadyfull", room.RoomNo);
            }
            

        }

        public async Task oponentBar()
        {
            var res = _ticTicContext.RoomData.Where(x => x.JoinerId == Context.ConnectionId).SingleOrDefault();
            await Clients.Client(Context.ConnectionId).SendAsync("openentbarset");
            await Clients.Client(res.HostId).SendAsync("openentbarset");

        }
        public async Task reciveName(string f,string l)
        {
            var res = _ticTicContext.RoomData.Where(x => x.JoinerId == Context.ConnectionId || x.HostId==Context.ConnectionId).SingleOrDefault();
            if (res == null)
            {
                return;
            }
            else if(Context.ConnectionId == res.HostId)
            {
                await Clients.Client(res.JoinerId).SendAsync("receiveName",f,l);
            }
            else
            {
                await Clients.Client(res.HostId).SendAsync("receiveName",f,l);
            }
        }

        public async Task makeGrid()
        {

            var x = _ticTicContext.RoomData.Where(x => x.JoinerId == Context.ConnectionId || x.HostId == Context.ConnectionId).SingleOrDefault();

            var tempData = new List<GameData>();
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


                     tempData.Add(tp);
                   




                }
            }

            x.GameData = tempData;
            await _ticTicContext.SaveChangesAsync();

        }
        
       
        
        public async Task GameEnd()
        {
            var x = _ticTicContext.RoomData.Where(x => x.JoinerId == Context.ConnectionId || x.HostId == Context.ConnectionId).SingleOrDefault();

            var Temp = x.GameData;
            foreach (var item in Temp)
            {
                _ticTicContext.GameData.Remove(item);
                await _ticTicContext.SaveChangesAsync();
            }

        }

        public async Task IsPlayerReady()
        {
            var x = _ticTicContext.RoomData.Where(x => x.JoinerId == Context.ConnectionId || x.HostId == Context.ConnectionId).SingleOrDefault();

            if(Context.ConnectionId == x.HostId)
            {
                x.IsHostReady = true;
                await _ticTicContext.AddRangeAsync();
            }
            if(Context.ConnectionId == x.JoinerId)
            {
                x.IsJoinerReady = false;
                await _ticTicContext.AddRangeAsync();
            }
        }
        public async Task IamReady()
        {
            var x = _ticTicContext.RoomData.Where(x => x.JoinerId == Context.ConnectionId || x.HostId == Context.ConnectionId).SingleOrDefault();
            var obb = _ticTicContext.GameData.Where(y => y.roomDataId == x.Id).ToList();
            if(!(x==null))
            {
                if(x.HostId == Context.ConnectionId)
                {
                    x.IsHostReady = true;
                    await Clients.Client(x.JoinerId).SendAsync("oponentIsReady");
                    await _ticTicContext.SaveChangesAsync();
                }
                if(x.JoinerId == Context.ConnectionId)
                {
                    x.IsJoinerReady = true;
                    await Clients.Client(x.HostId).SendAsync("oponentIsReady");
                    await _ticTicContext.SaveChangesAsync();
                }

                if(x.IsHostReady == true && x.IsJoinerReady == true)
                {
                    
                   
                    //var obj = JsonConvert.SerializeObject(x.GameData);
                   
                    var toss = _services.Toss();
                    if (toss == "host")
                    {
                        
                        await Clients.Client(x.HostId).SendAsync("yourTurn");
                        
                    }
                    else if(toss == "joiner")
                    {
                        
                        await Clients.Client(x.JoinerId).SendAsync("yourTurn");
                        
                    }

                    
                }
                



            }
            
        }

        public async Task NextTurn()
        {
            var x = _ticTicContext.RoomData.Where(x => x.JoinerId == Context.ConnectionId || x.HostId == Context.ConnectionId).SingleOrDefault();
            if(!(x==null))
            {
                if (Context.ConnectionId == x.HostId)
                {
                    await Clients.Client(x.JoinerId).SendAsync("yourTurn");
                }
                else if (Context.ConnectionId == x.JoinerId)
                {
                    await Clients.Client(x.HostId).SendAsync("yourTurn");
                }
            }
            
        }
        public async Task receiveComment(string messege)
        {
            var x = _ticTicContext.RoomData.Where(x => x.JoinerId == Context.ConnectionId || x.HostId == Context.ConnectionId).SingleOrDefault();
            if(!(x==null))
            {
                if(Context.ConnectionId == x.HostId)
                {
                    await Clients.Client(x.JoinerId).SendAsync("receiveComment", messege);
                }
                if (Context.ConnectionId == x.JoinerId)
                {
                    await Clients.Client(x.HostId).SendAsync("receiveComment", messege);
                }
            }
        }
        public async Task Gamestart()
        {

        }
        public async Task FilledCellData(string celldata)
        {
            
            var x = _ticTicContext.RoomData.Where(x => x.JoinerId == Context.ConnectionId || x.HostId == Context.ConnectionId).SingleOrDefault();
            var obb = _ticTicContext.GameData.Where(y => y.roomDataId == x.Id).ToList();

           var result = IsGameEnd(obb);

            var tmp = Convert.ToString(celldata);
            var p = Convert.ToInt32(tmp.Substring(4, 1));
            var j = Convert.ToInt32(tmp.Substring(5, 1));
            var check = Convert.ToInt32(tmp.Substring(4, 1)+ tmp.Substring(5, 1));
            
            if(result == "not yet")
            {
                for (var i = 0; i < 9; i++)

                    if (obb[i].cellCode == check)
                    {
                        if (Context.ConnectionId == x.HostId)
                        {
                            obb[i].cellFiller = "host";
                            await Clients.Client(x.JoinerId).SendAsync("ThisIsFilled", celldata);
                        }
                        if (Context.ConnectionId == x.JoinerId)
                        {
                            obb[i].cellFiller = "joiner";
                            await Clients.Client(x.HostId).SendAsync("ThisIsFilled", celldata);
                        }
                        await _ticTicContext.SaveChangesAsync();
                    }

            }

            result = IsGameEnd(obb);
            if (result == "draw")
            {
                await Clients.Client(x.HostId).SendAsync("draw", celldata);
                await Clients.Client(x.JoinerId).SendAsync("draw", celldata);

            }
            else if(result == "host" || result == "joiner")
            {
                if(result =="host")
                {
                    await Clients.Client(x.HostId).SendAsync("won", celldata);
                    await Clients.Client(x.JoinerId).SendAsync("lost", celldata);
                }
                if(result == "joiner")
                {
                    await Clients.Client(x.HostId).SendAsync("lost", celldata);
                    await Clients.Client(x.JoinerId).SendAsync("won", celldata);
                }
            }


            string IsGameEnd(List<GameData> data)
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



                    if (item.cellFiller == null)
                    {

                        isAnyRemain = true;
                    }

                }

                //horizontal Lines
                if (strr[0, 0] != null && strr[0, 1] != null && strr[0, 2] != null)
                {
                    if (strr[0, 0] == strr[0, 1] && strr[0, 1] == strr[0, 2] && strr[0, 0] == strr[0, 2])
                    {

                        winner = strr[0, 0];

                        return winner;
                    }
                }
                if (strr[1, 0] != null && strr[1, 1] != null && strr[1, 2] != null)
                {
                    if (strr[1, 0] == strr[1, 1] && strr[1, 1] == strr[1, 2] && strr[1, 0] == strr[1, 2])
                    {
                        winner = strr[1, 0];

                        return winner;

                    }
                }
                if (strr[2, 0] != null && strr[2, 1] != null && strr[2, 2] != null)
                {
                    if (strr[2, 0] == strr[2, 1] && strr[2, 1] == strr[2, 2] && strr[2, 0] == strr[2, 2])
                    {
                        winner = strr[2, 0];

                        return winner;
                    }
                }
                //verticle lines
                if (strr[0, 0] != null && strr[1, 0] != null && strr[2, 0] != null)
                {
                    if (strr[0, 0] == strr[1, 0] && strr[1, 0] == strr[2, 0] && strr[0, 0] == strr[2, 0])
                    {
                        winner = strr[0, 0];

                        return winner;
                    }
                }
                if (strr[0, 1] != null && strr[1, 1] != null && strr[2, 1] != null)
                {
                    if (strr[0, 1] == strr[1, 1] && strr[1, 1] == strr[2, 1] && strr[0, 1] == strr[2, 1])
                    {
                        winner = strr[0, 1];

                        return winner;
                    }
                }
                if (strr[0, 2] != null && strr[1, 2] != null && strr[2, 2] != null)
                {
                    if (strr[0, 2] == strr[1, 2] && strr[1, 2] == strr[2, 2] && strr[0, 2] == strr[2, 2])
                    {
                        winner = strr[0, 2];

                        return winner;
                    }
                }
                //cross lines
                if (strr[0, 0] != null && strr[1, 1] != null && strr[2, 2] != null)
                {
                    if (strr[0, 0] == strr[1, 1] && strr[1, 1] == strr[2, 2] && strr[0, 0] == strr[2, 2])
                    {
                        winner = strr[0, 0];

                        return winner;
                    }
                }
                if (strr[1, 2] != null && strr[1, 1] != null && strr[0, 2] != null)
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
        }

        public async Task GameIsOver()
        {
            var x = _ticTicContext.RoomData.Where(x => x.JoinerId == Context.ConnectionId || x.HostId == Context.ConnectionId).SingleOrDefault();
            if (!(x == null))
            {
                _ticTicContext.RoomData.Remove(x);
                await _ticTicContext.SaveChangesAsync();

            }
           await Clients.Client(Context.ConnectionId).SendAsync("GameisOver");
            
        }

          



    }
}
