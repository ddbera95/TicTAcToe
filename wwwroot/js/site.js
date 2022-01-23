
 var join = document.getElementById("join");

//document.getElementById("make-room2").addEventListener("click", link);
function link() {
    alert('hii');
    window.location.href = "/playing/play";
}
    

 var x=0;
 var input = `<div class="input">
 <input id="input" type="text" placeholder="enter room no." >
 <span ><p id="submit" onclick="validateRoom()">Enter</p></span></div>`;
 join.addEventListener("click", ()=>{

    if(x==0)
    {
        document.getElementById("hero").innerHTML += input;
        document.getElementById("script").innerHTML = `var y = document.getElementById("submit");
        y.addEventListener("click", ()=>{
       
            validateRoom();
         });
         `;
    }
    x++;
    

    
    
 });
 
 function validateRoom()
 {
     
     let temp = document.getElementById("input").value;
     let link = `/playing/joinRoom/${temp}`;
        if(temp.length==6)
        {
            location.href = link;
        }
        else{
            alert("enter 6 digit number");
        }
     

}
