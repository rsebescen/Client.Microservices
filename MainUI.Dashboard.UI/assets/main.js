function loadData() {
    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function() {
        if (this.readyState == 4 && this.status == 200) {
            console.log(JSON.parse(this.responseText));
        document.getElementById("root").innerHTML = "<div>Alo</div>";
            sendData();
        }
    };
    xhttp.open("GET", "brb/api/data", true);
    xhttp.send();
}

loadData();

function sendData() {
    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function() {
        if (this.readyState == 4 && this.status == 200) {
            console.log(JSON.parse(this.responseText));
        document.getElementById("root").innerHTML = "<div>Sent</div>";
        }
    };
    xhttp.open("POST", "brb/api/data", true);
    xhttp.setRequestHeader("Content-type", "application/json");
    xhttp.send('{"firstname":"pera","lastname":"peric"}');
}
