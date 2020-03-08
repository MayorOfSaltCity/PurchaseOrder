
const supplierTemplate = "<div class='supplierList'><dl><dt>{{name}}</dt><dd>Code:{{supplierCode}}<br/>Created Date:{{createdDate}}<br/></dd></dl><button onclick='showAddProduct();'>Add Product</button><button onclick='showAddProduct();'>View Products</button></div>";

function showModal(el) {
    document.getElementById(el).style.display = "block";
}

function closeModal(el) {
    document.getElementById(el).style.display = "none";
}

function buildSupplierListDisplay(responseJson, el) {
    var res = JSON.parse(responseJson);
    el.innerHTML = '';
    res.forEach(e => {
        var supDiv = supplierTemplate.replace('{{name}}', e.name);

        supDiv = supDiv.replace('{{supplierCode}}', e.supplierCode);
        supDiv = supDiv.replace('{{createdDate}}', e.createdDate);
        el.innerHTML += supDiv;
    });
}

function listSuppliers() {
    var req = new XMLHttpRequest();
    req.open("GET", "http://localhost:24397/Supplier/Search");
    req.onload = function (e) {
        var el = document.getElementById('supplierList');
        buildSupplierListDisplay(req.responseText, el);
    };
    req.send();
}

function addSupplier() {
    let name = document.getElementById('txtSupplierName').value;
    document.getElementById('txtSupplierName').value = '';
    let supplierCode = document.getElementById('txtSupplierCode').value;
    document.getElementById('txtSupplierCode').value = '';
    let errDiv = document.getElementById('addSupplierError');
    errDiv.style.display = "none";
    errDiv.innerText = '';
    let err = 0;
    if (name === '') {
        errDiv.innerHTML += '<span>Please enter a supplier name</span>';
        err++;
    }
    if (supplierCode === '') {
        errDiv.innerHTML += '<br /><span>Please enter a supplier code</span>';
        err++;
    }

    if (err > 0) {
        errDiv.style.display = "block";
        return;
    }

    let req = new XMLHttpRequest();
    req.onload = function (e) {
        if (req.status === 200) {
            closeModal('addSupplierModal');
            listSuppliers();
        } else {
            errDiv.innerHTML += '<br /><span>Error occured!<span>';
            errDiv.style.display = "block";
        }
    };
    req.onerror = function (e) {
        errDiv.innerHTML += '<br /><span>Error occured: ' + req.responseText + '<span>';
        errDiv.style.display = "block";
    };

    req.open("POST", "http://localhost:24397/Supplier/Add?name=" + name + "&supplierCode=" + supplierCode);
    try {
        req.send();
    } catch (err) {
        errDiv.innerHTML += '<span>' + err.message + '</span>';
    }
    
}

function fetchSupplier(supplierCode) {

}

function displayPage(e) {
    //alert(e);
}
var forms = document.getElementsByTagName("form");
for (var i = 0; i < forms.length; i++) {
    var f = forms[i];
    f.addEventListener("click", function (e) {
        e.preventDefault();
    });
}