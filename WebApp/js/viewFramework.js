
const supplierListTemplate = "<div class='supplierList'><dl onclick='loadSupplierView({{id}});'><dt>{{name}}</dt><dd>Code:{{supplierCode}}<br/>Created Date:{{createdDate}}<br/></dd></dl></div>";
const productListRowTemplate = "<tr id='{{id}}' class='{{class}}'><td>{{productCode}}</td><td>{{description}}</td><td>{{price}}</td><td><button id='update_{{id}}' {{disabled}}>Update</button></td><td><button id='del_{{id}}' {{disabled}}>Delete</button></td></tr>";
const serverUrl = "http://localhost:24397/";
function showModal(el) {
    document.getElementById(el).style.display = "block";
}

function closeModal(el) {
    document.getElementById(el).style.display = "none";
}
function loadSupplierView(supplierId) {
    var req = new XMLHttpRequest();
    req.open("GET", serverUrl + "Product/GetSupplierProducts?supplierId=" + supplierId);
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
    document.getElementById('txtProductCode').value = '';
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
        var productDiv = productListRowTemplate.replace('{{productCode}}', e.productCode);

        productDiv = productDiv.replace('{{description}}', e.description);
        productDiv = productDiv.replace('{{price}}', e.price);
        productDiv = productDiv.replace('{{id}}', '"' + e.id + '"');
        productDiv = productDiv.replace('{{class}}', e.isDeleted ? 'deleted' : 'regular');
        while (productDiv.indexOf('{{disabled}}') > -1)
            productDiv = productDiv.replace('{{disabled}}', e.isDeleted ? 'disabled' : '');
        tblString += productDiv;
    });

    el.innerHTML += tblString;
    console.log(el.innerHTML);
}

function listSuppliers() {
    var req = new XMLHttpRequest();
    req.open("GET", serverUrl + "Supplier/Search");
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

    req.open("POST", serverUrl + "Supplier/Add?name=" + name + "&supplierCode=" + supplierCode);
    try {
        req.send();
    } catch (err) {
        errDiv.innerHTML += '<span>' + err.message + '</span>';
    }

}

function addProduct() {
    let supplierId = document.getElementById('hdnAddProductSupplierId').value;
    let productCode = document.getElementById('txtProductCode').value;
    let productDescription = document.getElementById('txtProductDescription').value;
    let productPrice = document.getElementById('txtProductPrice').value;

    let productFormData = {
        "productCode": productCode,
        "description": productDescription,
        "price": productPrice,
        "supplierId": supplierId
    };
    let jsonString = JSON.stringify(productFormData);

    var req = new XMLHttpRequest();

    req.open("POST", serverUrl + "Product/AddProductToSupplier");
    req.onload = function (e) {
        fetchProductData(req.responseText);
        closeModal('addProductModal');
    };
    req.setRequestHeader('CONTENT-TYPE', 'application/json');
    req.setRequestHeader('Accept', '*/*');
    req.send(jsonString);
}

function updateProductList(product) {
    var productDiv = productListRowTemplate.replace('{{productCode}}', product.productCode);

    productDiv = productDiv.replace('{{description}}', product.description);
    productDiv = productDiv.replace('{{price}}', product.price);
    productDiv = productDiv.replace('{{id}}', '"' + product.id + '"');
    productDiv = productDiv.replace('{{class}}', product.isDeleted ? 'deleted' : 'regular');
    while (productDiv.indexOf('{{disabled}}') > -1)
        productDiv = productDiv.replace('{{disabled}}', product.isDeleted ? 'disabled' : '');
    document.getElementById('supplierProductListRows').innerHTML += productDiv;
}

function fetchProductData(productId) {
    var req = new XMLHttpRequest();
    // work around to remove quotes that are added by JS
    while (productId.indexOf('"') > -1) {
        productId = productId.replace('"', '');
    }

    req.open("GET", serverUrl + "Product/GetProduct?productId=" + productId);
    req.onload = function (e) {
        if (req.status === 200) {
            let product = JSON.parse(req.responseText);
            updateProductList(product);
        } else {
            // do some error handling
        }
    };

    req.send();
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