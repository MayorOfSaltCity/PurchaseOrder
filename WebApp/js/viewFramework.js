function loadPage(url, target) {
    var element = document.getElementById(target);
    var req = new XMLHttpRequest();
    req.onload = displayPage;
    req.open("GET", url, false);
    req.send();
    element.innerHTML = req.responseText;
}

function displayPage(e) {
    alert(e);
}

// need a better way to load the default page
loadPage('views/supplier/supplierList.htm', 'main');