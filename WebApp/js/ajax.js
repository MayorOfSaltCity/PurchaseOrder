var dataSource = "http://localhost:23497/";

function loadData(method, source, target) {
    var req = new XMLHttpRequest();
    req.open(method,dataSource + source,false);
    req.onload = callback;
    req.send();
    var el = document.getElementById(target);
    el.innerHTML = req.responseText;
}