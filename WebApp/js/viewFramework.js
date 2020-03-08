
const supplierListTemplate = "<div class='supplierList'><dl onclick='loadSupplierView({{id}});'><dt>{{name}}</dt><dd>Code:{{supplierCode}}<br/>Created Date:{{createdDate}}<br/></dd></dl></div>";
const productListRowTemplate = "<tr><td>{{productCode}}</td><td>{{description}}</td><td>{{price}}</td></tr>";
function showModal(el) {
    document.getElementById(el).style.display = "block";
}

function closeModal(el) {
    document.getElementById(el).style.display = "none";
}
function loadSupplierView(supplierId) {
    var req = new XMLHttpRequest();
    req.open("GET", "http://localhost:24397/Product/GetSupplierProducts?supplierId=" + supplierId);
    req.onload = function (e) {
        var el = document.getElementById('supplierProductListRows');
        buildProductList(req.responseText, el, supplierId);
        showModal('productModal');
        document.getElementById('addProductButton').onclick = function () {
            loadAddProductView(supplierId);
        };
        document.getElementById('createPOButton').onclick = function () {
            loadCreatePurchaseOrderView(supplierId);
        };
    };
    req.send();
}

function loadAddProductView(supplierId) {
    document.getElementById('hdnAddProductSupplierId').value = supplierId;
    document.getElementById('txtProductName').value = '';
    document.getElementById('txtProductDescription').value = '';
    document.getElementById('txtProductPrice').value = '';
    showModal('addProductModal');
}

function loadCreatePurchaseOrderView(supplierId) {

}

function buildSupplierListDisplay(responseJson, el) {
    var res = JSON.parse(responseJson);
    el.innerHTML = '';
    res.forEach(e => {
        var supDiv = supplierListTemplate.replace('{{name}}', e.name);

        supDiv = supDiv.replace('{{supplierCode}}', e.supplierCode);
        supDiv = supDiv.replace('{{createdDate}}', e.createdDate);
        supDiv = supDiv.replace('{{id}}', '"' + e.id + '"');
        el.innerHTML += supDiv;
    });
}

function buildProductList(responseJson, el, supplierId) {
    var res = JSON.parse(responseJson);
    var tblString = '';
    el.innerHTML = '';
    res.forEach(e => {
        var supDiv = productListRowTemplate.replace('{{productCode}}', e.productCode);

        supDiv = supDiv.replace('{{description}}', e.description);
        supDiv = supDiv.replace('{{price}}', e.price);
        supDiv = supDiv.replace('{{id}}', '"' + e.id + '"');
        
        tblString += supDiv;
    });

    el.innerHTML += tblString;
    console.log(el.innerHTML);
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

function addProduct() {
    let supplierId = document.getElementById('hdnProductSupplierId').value;
    let productCode = document.getElementById('txtProductCode').value;
    let productDescription = document.getElementById('txtProductDescription').value;
    let productPrice = document.getElementById('txtProductPrice').value;
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