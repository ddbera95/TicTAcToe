"use strict";

function ll() {
    window.location.href = "/";
}
var connection = new signalR.HubConnectionBuilder().withUrl("/connection").build();
 connection.start();


var cellData;
var circle = "circle.svg";
var cross = "cross.svg";
var indicator = false;
var isYourTurn = false;
var Filler;
var nonFiller;
var isFillerChosen = false;
var cont = document.getElementById("cont");
var bothReady = false;
var canvas = `<div class="canvas">
                <div id="cell11" class="canvas-pixel" onclick="clicked('cell11')"></div>
                <div id="cell12" class="canvas-pixel" onclick="clicked('cell12')"></div>
                <div id="cell13" class="canvas-pixel" onclick="clicked('cell13')"></div>
            </div>
            <div class="canvas">
                <div id="cell21" class="canvas-pixel" onclick="clicked('cell21')"></div>
                <div id="cell22" class="canvas-pixel" onclick="clicked('cell22')"></div>
                <div id="cell23" class="canvas-pixel" onclick="clicked('cell23')"></div>
            </div>
            <div class="canvas">
                <div id="cell31" class="canvas-pixel" onclick="clicked('cell31')"></div>
                <div id="cell32" class="canvas-pixel" onclick="clicked('cell32')"></div>
                <div id="cell33" class="canvas-pixel" onclick="clicked('cell33')"></div>
            </div>`;


  
/////////////// for chosing filler and make ready enable 

connection.on("yourTurn", function() {

    
    isYourTurn = true;
    console.log("my turn");
    bothReady = true;
    cont.style.border = "2px solid lightGreen";
});
function clicked(x) {
    console.log("hellll");
   var check =  document.getElementById(x).classList.contains("filled");
    if (bothReady && isYourTurn && !check) {

        connection.invoke("NextTurn");
        connection.invoke("FilledCellData", x);
        document.getElementById(x).innerHTML = Filler;
        document.getElementById(x).classList.add("filled");

        isYourTurn = false;
        cont.style.border = "2px solid white";

    }
}
    
 document.getElementById("circle").addEventListener("click", function () {
     Filler = document.getElementById("circle").innerHTML;
     nonFiller = document.getElementById("cross").innerHTML;
    console.log("you have been chosen");
    fillerhasBeenChosen();
});
document.getElementById("cross").addEventListener("click", function () {
    Filler = document.getElementById("cross").innerHTML;
    nonFiller = document.getElementById("circle").innerHTML;
console.log("you have been chosen");
fillerhasBeenChosen();
});



function NowyourTurn() {

}

function fillerhasBeenChosen() {

    isFillerChosen = true;

    cont.innerHTML = canvas;

}

///////////////





connection.on("ThisIsFilled", function (x) {
    console.log('hellooooo');
    document.getElementById(x).classList.add("filled");
    document.getElementById(x).innerHTML = nonFiller;

})

connection.on("GameDataArrived", function (obj) {

    console.log(obj);

});
connection.on("GameisOver", function () {
    setTimeout(function (){ window.location.href = "/";
}, 5000);
   
});
connection.on("won", function () {
    cont.innerHTML = `<div style="color:lightgreen;"><h3>you have won</h3></div>`;
    connection.invoke("GameIsOver");
    
});
connection.on("lost", function () {
    cont.innerHTML = `<div style="color:red;"><h3>you have lost</h3></div>`;
    connection.invoke("GameIsOver");
    
});
connection.on("draw", function () {
    cont.innerHTML = `<div style="color: white;"><h3>this is draw</h3></div>`;
    connection.invoke("GameIsOver");
});

/*document.getElementById("circle").addEventListener("click", function () {
    Filler = fillerElementTag(circle);
});
document.getElementById("cross").addEventListener("click", function () {
    Filler = fillerElementTag(cross);
})*/



function fillerElementTag(x) {

    return `<div class="canvas-pixel-content"><img src="~/images/${x}" alt=""></div>`;
}

var connectionId;
var roomno;
var Name;
var joinroomno = returnRoomno();
var conectedBoth = false;
connection.on("setConnectionId", function (Id) {
    connectionId = Id;
    if (joinroomno == 0) {
        MakeRoom();
    }
    else {
        joinroom(joinroomno);
    }
    
});

connection.on("isRoonCreated", function (e,x) {

        alert("room is created")
        roomno = e;
    console.log("room is created here is room id " + roomno);
    makeCanvas();
    if (x == "joiner") {
        opp();
        
    }
    
    

});



connection.on("ConectionCut", function (e) {
    conectedBoth = false;
    document.getElementsByClassName("ready")[0].style.display = "null";

    var x = document.getElementsByClassName("cnt");

    x[1].innerHTML = "";
    x[0].innerHTML = `<h1 style="color:red;"> connection has been cut </h1>`;
})

connection.on("alreadyfull", function (e) {

    var x = document.getElementsByClassName("cnt");

    x[1].innerHTML = "";
    x[0].innerHTML = `<h3 style="color:red;"> room no ${e} already full </h3>`;
})
connection.on("openentbarset", function () {

    
    
    console.log("oponent is joined");
    conectedBoth = true;
    var temp = `<div class="user-profile-joiner">

            <div class="connected"></div>
            <div id="op-name">
                <p>anounymous <span></span></p>
            </div>
            <div id="iamready"></div>
        </div>
        <div class="comment">
            <div id="comment-container"class="inner-comment">

            </div>
           </div>`;
    var x = document.getElementById("oppenent-bar");
    x.innerHTML = temp;
    console.log("hjjhjhjh");
    sendName();
    
    
})

connection.on("console", function (e) {
    console.log(e);
})
function MakeRoom() {

    if (connection.state != null) {
        connection.invoke("MakeRoom");
    }
}
function opp() {
    console.log("jdkjsd");
    if (connection.state != null) {
        connection.invoke("oponentBar");
        makeGrid();
    }
}

function makeCanvas() {
    document.getElementById("roomno").innerText = roomno;
    var dis = document.getElementsByClassName("dis");
    Array.from(dis).forEach(function (item, index) {

        
        item.classList.remove("dis");
        
    })
}

function joinroom(roomno) {
    if (connection.state != null) {
        console.log("i going to join " + roomno);
        connection.invoke("JoinRoom", roomno);
    }
}

function sendName() {
    console.log("hello sendname");
    var fName = document.getElementById("FirstName").value;
    var lName = document.getElementById("LastName").value;
    if (fName == null || lName == null) {
        fName = "anonymous";
        lName = "-";
    }
    connection.invoke("reciveName", fName, lName);
}


connection.on("receiveName", function (f, l) {
    console.log(f + l);
    var x = document.getElementById("op-name");
    x.innerHTML = `<p>${f} <span>${l}</span></p>`;
}
);

function makeGrid() {
    connection.invoke("makeGrid");
}

connection.on("connectionStop", function () {
    connection = false;
    document.getElementsByClassName("ready")[0].style.display = "null";
    connection.stop();
})

connection.on("notAvailable", function () {

    var x = document.getElementsByClassName("cnt");

    x[1].innerHTML = "";
    x[0].innerHTML = `<h3 style="color:red;"> room no  is not available</h3>`;
});
connection.on("noRoomForYou", function () {
    console.log("no room for you");
    var x = document.getElementsByClassName("cnt");

    x[1].innerHTML = "";
    x[0].innerHTML = `<h3 style="color:red;"> room not available</h3>`;
    x[0].classList.remove("dis");
})

function ready() {
    if (isFillerChosen && conectedBoth) {
        connection.invoke("IamReady");
        document.getElementsByClassName("ready")[0].style.display = "none";

    }
    
}
connection.on("oponentIsReady", function () {


    document.getElementById("iamready").innerHTML = `<p style="color:lightgreen;">I am ready</P>`;
});
function sendComment() {
    var messege = document.getElementById("comment").value;

    if (conectedBoth && messege!= "") {
        
        
        document.getElementById("comment").value = "";
        connection.invoke("receiveComment",messege)
    }
}
connection.on("receiveComment", function (e) {

    var messege = `<div class="comment-child"><div>===================</div>
                   <div>!! comment !!</div>
                   <div>===================</div>
                   <div>${e}</div></div>`;
    document.getElementById("comment-container").innerHTML += messege;

    var x = document.getElementsByClassName("comment-child");

    x[x.length - 1].scrollIntoView();

})

