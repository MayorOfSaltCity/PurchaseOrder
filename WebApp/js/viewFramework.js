
const supplierListTemplate = "<div class='supplierList'><dl onclick='loadSupplierView({{id}});'><dt>{{name}}</dt><dd>Code:{{supplierCode}}<br/>Created Date:{{createdDate}}<br/></dd></dl></div>";
const productListRowTemplate = "<tr id='{{id}}' class='{{class}}'><td>{{productCode}}</td><td>{{description}}</td><td>{{price}}</td><td><button id='update_{{id}}' {{disabled}} onclick=loadUpdateProductView({{id}})>Update</button></td><td><button id='del_{{id}}' {{disabled}}  onclick=loadDeleteProductView({{id}})>Delete</button></td></tr>";
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

function loadUpdateProductView(productId) {
    var req = new XMLHttpRequest();
    productId = sanitizeId(productId);
    req.open("GET", serverUrl + "Product/GetProduct?productId=" + productId);
    req.onload = function (e) {
        var product = JSON.parse(req.responseText);
        document.getElementById('txtUpdateProductDescription').value = product.description;
        document.getElementById('txtUpdateProductPrice').value = product.price;
        document.getElementById('hdnUpdateProductId').value = product.id;
        showModal('updateProductModal');
    };

    req.send();
}

function updateProduct() {
    let productId = document.getElementById('hdnUpdateProductId').value;
    let productDescription = document.getElementById('txtUpdateProductDescription').value;
    let productPrice = document.getElementById('txtUpdateProductPrice').value;

    let productFormData = {
        "description": productDescription,
        "price": productPrice,
        "productId": productId
    };

    let jsonString = JSON.stringify(productFormData);

    var req = new XMLHttpRequest();

    req.open("PUT", serverUrl + "Product/UpdateProduct");
    req.onload = function (e) {
        closeModal('updateProductModal');
    };

    req.setRequestHeader('CONTENT-TYPE', 'application/json');
    req.setRequestHeader('Accept', '*/*');
    req.send(jsonString);
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
        var productRow = productListRowTemplate.replace('{{productCode}}', e.productCode);

        productRow = replaceAll(productRow, '{{description}}', e.description);
        productRow = replaceAll(productRow, '{{price}}', e.price);
        productRow = replaceAll(productRow, '{{id}}', '"' + e.id + '"');
        productRow = replaceAll(productRow, '{{class}}', e.isDeleted ? 'deleted' : 'regular');
        productRow = replaceAll(productRow, '{{disabled}}', e.isDeleted ? 'disabled' : '');
        tblString += productRow;
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
function replaceAll(s, m, v) {
    while (s.indexOf(m) > -1) {
        s = s.replace(m, v);
    }

    return s;
}
function updateProductList(product) {
    var productRow = productListRowTemplate.replace('{{productCode}}', product.productCode);

    productRow = replaceAll(productRow, '{{description}}', product.description);
    productRow = replaceAll(productRow, '{{price}}', product.price);
    productRow = replaceAll(productRow, '{{id}}', '"' + product.id + '"');
    productRow = replaceAll(productRow, '{{class}}', product.isDeleted ? 'deleted' : 'regular');
    productRow = replaceAll(productRow, '{{disabled}}', product.isDeleted ? 'disabled' : '');
    document.getElementById('supplierProductListRows').innerHTML += productRow;
}
function sanitizeId(id) {
    let sId = id;
    while (sId.indexOf('"') > -1) {
        sId = sId.replace('"', '');
    }

    return sId;
}
function fetchProductData(productId) {
    var req = new XMLHttpRequest();
    // work around to remove quotes that are added by JS
    productId = sanitizeId(productId);

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