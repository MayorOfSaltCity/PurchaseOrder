var dataSource = "http://localhost:23497/";

function loadData(method, source, target, template, callback) {
    var req = new XMLHttpRequest();
    req.open(method,dataSource + source);
    req.onload = callback;
}