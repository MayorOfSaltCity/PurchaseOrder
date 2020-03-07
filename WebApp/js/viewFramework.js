
const supplierTemplate = "<div><dl><dt>{{name}}</dt><dd>Code:{{supplierCode}}<br/>Created Date:{{createdDate}}<br/></dd></dl></div>";


function buildSupplierListDisplay(responseJson, el) {
    var res = JSON.parse(responseJson);
    res.forEach(e => {
        var supDiv = supplierTemplate.replace('{{name}}', e.name);

        supDiv = supDiv.replace('{{supplierCode}}', e.supplierCode);
        supDiv = supDiv.replace('{{createdDate}}', e.createdDate);
        el.innerHTML += supDiv;
    });
}

function listSuppliers() {
    var req = new XMLHttpRequest();
    req.open("GET", "http://localhost:24397/Supplier/Search?searchString=Test");
    req.onload = function (e) {
        var el = document.getElementById('supplierList');
        buildSupplierListDisplay(req.responseText, el);
    };
    req.send();
}

function addSupplier() {

}

function fetchSupplier(supplierCode) {

}

function displayPage(e) {
    //alert(e);
}
