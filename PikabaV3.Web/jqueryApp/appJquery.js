
$(document).ready(function () {

    /************************* General ***************************/
    var cookieUuid = $.cookie('PikabaV3');
    GetUser(cookieUuid);


    /************************* View Account ***************************/
    $('#menuAccount').on('click', '#linkViewAccountBuyer', function (event) {
        event.preventDefault();
        GetUser(cookieUuid);
    });

    $('#menuAccount').on('click', '#linkViewAccountSeller', function (event) {
        event.preventDefault();
        GetUser(cookieUuid);
    });


    /************************* Update Account ***************************/
    $('#menuAccount').on("click", '#linkEditAccountBuyer', function () {
        GetUserToUpdate(cookieUuid);
    });

    $('#viewUserAction').on("click", '#saveAccountBuyer', function () {
        if ($('.editForm').valid() == true) {
            SaveEditAccountBuyer();
        }
    });

    $('#menuAccount').on("click", '#linkEditAccountSeller', function () {
        GetUserToUpdate(cookieUuid);
    });

    $('#viewUserAction').on("click", '#saveAccountSeller', function () {
        if ($('.editForm').valid() == true) {
            SaveEditAccountSeller();
        }
    });


    /************************* Deleted Account ***************************/
    $('#menuAccount').on("click", '#linkDeleteAccountBuyer', function () {
        DeleteAccountеConfirm();
    });

    $('#menuAccount').on("click", '#linkDeleteAccountSeller', function () {
        DeleteAccountеConfirm();
    });

    $('#viewUserAction').on("click", '#confirmDeleteAccYes', function () {
        DeleteUser(cookieUuid);
    });

    $('#viewUserAction').on("click", '#confirmDeleteAccNo', function () {
        GetUser(cookieUuid);
    });


    /************************* Create Product ***************************/
    $('#menuAccount').on("click", '#linkAddProduct', function () {
        $("#viewUserAction").load("pages/part-pages/formAddProduct.html");
    });

    $('#viewUserAction').on("click", '#buttonCreateProduct', function (event) {
        event.preventDefault();
        ValidateCreateFormProduct();
        if ($('.formCreatedProduct').valid() == true) {
            AddProduct(cookieUuid);
        }
    });


    /************************* View published Products ***************************/
    $('#menuAccount').on("click", '#linkPublishedProducts', function () {
        GetProductsSeller(cookieUuid);
    });

});


/********************************* Client jquery general *********************************/
function GetUser(cookieUuid) {
    $.ajax({
        url: 'http://localhost:49909/account/getAccount/' + cookieUuid,
        type: 'GET',
        contentType: "application/json;charset=utf-8",
        success: function (data) {
            if (data.Role == 0) {
                $('#menuAccount').load('pages/part-pages/menuBuyer.html');
                ViewBuyer(data);
            } else {
                $('#menuAccount').load('pages/part-pages/menuSeller.html');
                ViewSeller(data);
            }
        },
        error: function () {
            $('#menuAccount').html('<h3>Please log in or register!</h3>');
        }
    });
}

function GetUserToUpdate(cookieUuid) {
    $.ajax({
        url: 'http://localhost:49909/account/getAccount/' + cookieUuid,
        type: 'GET',
        contentType: "application/json;charset=utf-8",
        success: function (data) {
            if (data.Role == 0) {
                ViewBuyerToUpdate(data);
            } else {
                ViewSellerToUpdate(data);
            }
        },
        error: function (x, y, z) {
            alert(x + '\n' + y + '\n' + z);
        }
    });
}

function DeleteAccountеConfirm() {
    var result = '<p>Are you sure? <a id="confirmDeleteAccYes">YES</a> | <a id="confirmDeleteAccNo">NO</a> </p>';
    $('#viewUserAction').html(result);
}

function DeleteUser(cookieUuid) {
    $.ajax({
        url: 'http://localhost:49909/account/getAccount/' + cookieUuid,
        type: 'GET',
        contentType: "application/json;charset=utf-8",
        success: function (data) {
            DeleteUserById(data.Id);
        },
        error: function (x, y, z) {
            alert(x + '\n' + y + '\n' + z);
        }
    });
}

function DeleteUserById(userId) {
    $.ajax({
        url: 'http://localhost:49909/account/deleteAccount/' + userId,
        type: 'DELETE',
        contentType: "application/json;charset=utf-8",
        success: function () {
            $('#menuAccount').hide('slow');
            $('#viewUserAction').html('<p>Your profile has been deleted!</p>');
        },
        error: function (x, y, z) {
            alert(x + '\n' + y + '\n' + z);
        }
    });
}


/********************************* Client jquery for Seller *********************************/
function GetProductsSeller(cookieUuid) {
    $.ajax({
        url: 'http://localhost:49909/account/getAccount/' + cookieUuid,
        type: 'GET',
        contentType: "application/json;charset=utf-8",
        success: function (data) {
            GetProducts(data.Id);
        },
        error: function () {
            $('#menuAccount').html('<h3>Please log in or register!</h3>');
        }
    });
}

function GetProducts(sellerId) {
    $.ajax({
        url: 'http://localhost:49909/product/GetProductsSeller/' + sellerId,
        type: 'GET',
        contentType: "application/json;charset=utf-8",
        success: function (data) {
            ShowProducts(data);
        },
        error: function (x, y, z) {
            alert(x + '\n' + y + '\n' + z);
        }
    });
}

function GetProductToEdit(productId) {
    $.ajax({
        url: 'http://localhost:49909/product/' + productId,
        type: 'GET',
        contentType: "application/json;charset=utf-8",
        success: function (data) {
            ShowProductToEdit(data);
            ValidateEditFormProduct();
        },
        error: function (x, y, z) {
            alert(x + '\n' + y + '\n' + z);
        }
    });
}

function ShowProducts(products) {
    var strResult = '';
    $(products).each(function (i, item) {
        strResult += '<div id="marginLinkDiv">' +
                     "<a id='divLink' data-item=" + item.Id + " onclick='ViewProduct(this);return false;'>" +
                     "<div id='viewProduct'>" +
                     '<img src="Content/images/NoImage.jpg" width="70" height="60" />' +
                     "<span>Title: <span>" + item.Title + "</span></span><br />" +
                     "<span>Price: <span>" + item.Price + "$</span></span><br />" +
                     "<span>Added: <span>" + moment(item.DateAdded).format('lll') + "</span></span></div></a></div>";
    });
    $('#viewUserAction').html(strResult);
}

function ViewProduct(productId) {
    var id = $(productId).attr('data-item');
    $.ajax({
        url: 'http://localhost:49909/product/' + id,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            ShowProduct(data);
        },
        error: function (x, y, z) {
            alert(x + '\n' + y + '\n' + z);
        }
    });
}

function ViewProductById(productId) {
    $.ajax({
        url: 'http://localhost:49909/product/' + productId,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            ShowProduct(data);
        },
        error: function (x, y, z) {
            alert(x + '\n' + y + '\n' + z);
        }
    });
}

function ShowProduct(product) {
    var testStr = '<h3>Details product:</h3>' +
        '<span><b>Seller: </b><a>' + product.Owner.DisplayName + '</a></b></span> <br />' +
        '<span><b>Title: </b>' + product.Title + '</span> <br />' +
        '<span><b>Price: </b>' + product.Price + '</b></span> <br />' +
        '<span><b>Description: </b>' + product.Description + '</b></span> <br />' +
        '<span><b>DateAdded: </b>' + moment(product.DateAdded).format('lll') + '</b></span> <br />';
    $(product.Comments).each(function (i, item) {
        testStr += '<div id="viewComment"><b>Author: </b><a>' + item.Owner.DisplayName + '</a></b><br />' +
            '<b>Text: </b>' + item.Text + '</b><br />' +
            '<b>Date Added: </b>' + moment(item.DateCreation).format('lll') + '</b></div>';
    });
    testStr += "<p><a id='editProduct' data-item=" + product.Id + " onclick='EditProduct(this);return false;'>Edit</a> " +
                "| <a id='deleteProduct' data-item=" + product.Id + " onclick='DeleteProduct(this);return false;'>Delete</a></p>";
    $('#viewUserAction').html(testStr);
}

function ShowProductToEdit(product) {
    var result = '<h3>Edit product</h3><form class="formCreatedProduct">';
    result += '<span><input type="text" id="editTitleProduct" value="' + product.Title + '" name="Title"/> Title </span><br/>' +
        '<span><input type="text" id="editPriceProduct" value="' + product.Price + '" name="Price"/> Price </span><br/>' +
        '<textarea id="editDescriptionProduct" rows="5" cols="30">' + product.Description + '</textarea>';

    result += "<p><a data-item=" + product.Id + " onclick='SaveEditProduct(this);return false;'>Save</a> " +
               "| <a data-item=" + product.Id + " onclick='ViewProduct(this);return false;'>Back</a></p>";
    $('#viewUserAction').html(result);
}

function SaveEditProduct(id) {
    if ($('.formCreatedProduct').valid() == true) {
        var editedProduct = {
            Title: $('#editTitleProduct').val(),
            Price: $('#editPriceProduct').val(),
            Description: $('#editDescriptionProduct').val(),
            Id: $(id).attr('data-item')
        };

        $.ajax({
            url: 'http://localhost:49909/product/update/' + editedProduct.Id,
            type: 'PUT',
            data: JSON.stringify(editedProduct),
            contentType: "application/json;charset=utf-8",
            success: function () {
                ViewProductById(editedProduct.Id);
            },
            error: function (x, y, z) {
                alert(x + '\n' + y + '\n' + z);
            }
        });
    }
}

function EditProduct(productId) {
    var id = $(productId).attr('data-item');
    GetProductToEdit(id);
}

function DeleteProduct(productId) {
    var id = $(productId).attr('data-item');
    $.ajax({
        url: 'http://localhost:49909/product/' + id,
        type: 'DELETE',
        contentType: "application/json;charset=utf-8",
        success: function () {
            $('#viewUserAction').html('<p>Product has bin Deleted</p>');
        },
        error: function (x, y, z) {
            alert(x + '\n' + y + '\n' + z);
        }
    });
}

function AddProduct(cookieUuid) {
    $.ajax({
        url: 'http://localhost:49909/account/getAccount/' + cookieUuid,
        type: 'GET',
        contentType: "application/json;charset=utf-8",
        success: function (data) {
            var owner = {
                UserId: data.Id,
                DisplayName: data.DisplayName,
                Email: data.Email,
                Phone: data.Phone,
                Location: data.Location
            };
            PostProduct(owner);
        },
        error: function (x, y, z) {
            alert(x + '\n' + y + '\n' + z);
        }
    });
}

function PostProduct(owner) {
    var product = {
        Owner: owner,
        DateAdded: new Date(),
        Title: $('#addTitleProduct').val(),
        Price: $('#addPriceProduct').val(),
        Description: $('#AddDescriptionProduct').val()
    };
    $.ajax({
        url: 'http://localhost:49909/product',
        type: 'POST',
        data: JSON.stringify(product),
        contentType: "application/json;charset=utf-8",
        success: function () {
            $('.formCreatedProduct')[0].reset();
            $('#viewUserAction').html('<p>Product is published</p>');
        },
        error: function (x, y, z) {
            alert(x + '\n' + y + '\n' + z);
        }
    });
}

function ViewSeller(seller) {
    var result = '<h3>My profile</h3>';
    result += '<span><b>Display name: </b>' + seller.DisplayName + '</span> <br />' +
        '<span><b>Email: </b>' + seller.Email + '</span> <br />' +
        '<span><b>Role: </b>Seller</span> <br />' +
        '<span><b>Date Register: </b>' + moment(seller.DateRegister).format('lll') + '</span> <br />' +
        '<span><b>Location: </b>' + seller.Location + '</span> <br />' +
        '<span><b>Phone: </b>' + seller.Phone + '</span> <br />' +
        '<p><b>Reviews about me: </b></p>';
    $(seller.comments).each(function (i, item) {
        result += '<span><b>Author: </b>' + item.Owner.DisplayName + '<br /></span>' +
                  '<span><b>Text: </b>' + item.Text + '<br /></span>' +
                  '<span><b>Date Added: </b>' + moment(item.DateCreation).format('lll') + '</span>';
    });
    $('#viewUserAction').html(result);
}

function ViewSellerToUpdate(data) {
    var result = '<form class="editForm"><br/>';
    result += '<span><input type="text" id="editDisplayName" value="' + data.DisplayName + '" name="ViewName"/> Name </span><br/>' +
        '<span><input type="text" id="editEmail" value="' + data.Email + '" name="Email"/> Email </span><br/>' +
        '<input type="hidden" id="editPassword" value="' + data.Password + '" name="Password"/>' +
        //'<span><input type="password" id="confirmPassword" value="" name="ConfirmPassword"/> Confirm Password </span><br/>' +
        '<span><input type="text" id="editLocation" value="' + data.Location + '" name="Location"/> Location</span><br/>' +
        '<span><input type="text" id="editPhone" value="' + data.Phone + '" name="Phone"/> Phone </span><br/>' +
        '<input type="hidden" id="editId" value="' + data.Id + '"/>' +
        '<input type="hidden" id="editIsActive" value="' + data.IsActive + '"/>' +
        '<input type="hidden" id="editRole" value="' + data.Role + '"/>' +
        '<input type="hidden" id="editDateRegister" value="' + data.DateRegister + '"/>' +
        '<input type="hidden" id="editComments" value="' + data.Comments + '"/>' +
        '<p><input id="saveAccountSeller" type="button" value="Save" /></p></form>';
    $('#viewUserAction').html(result);
    ValidateEditFormSeller();
}

function SaveEditAccountSeller() {
    var editSeller = {
        DisplayName: $('#editDisplayName').val(),
        Email: $('#editEmail').val(),
        Password: $('#editPassword').val(),
        Location: $('#editLocation').val(),
        Phone: $('#editPhone').val(),
        Id: $('#editId').val(),
        IsActive: $('#editIsActive').val(),
        Role: $('#editRole').val(),
        DateRegister: $('#editDateRegister').val(),
        Comments: $('editComments').val()
    };

    $.ajax({
        url: 'http://localhost:49909/account/editSeller/' + editSeller.Id,
        type: 'PUT',
        data: JSON.stringify(editSeller),
        contentType: "application/json;charset=utf-8",
        success: function () {
            var cookieUuid = $.cookie('PikabaV3');
            GetUser(cookieUuid);
        },
        error: function (x, y, z) {
            alert(x + '\n' + y + '\n' + z);
        }
    });
}

function ValidateEditFormSeller() {
    $(".editForm").validate({
        rules: {
            ViewName: {
                required: true,
                minlength: 3,
                maxlength: 20
            },
            Email: {
                required: true,
                email: true,
                minlength: 3,
                maxlength: 20
            },
            Location: {
                required: true,
                minlength: 2,
                maxlength: 20
            },
            Phone: {
                required: true,
                minlength: 3,
                maxlength: 20
            },
            Password: {
                required: true,
                minlength: 3,
                maxlength: 20
            },
            ConfirmPassword: {
                minlength: 3,
                equalTo: "#editPassword"
            },
        },
        messages: {
            ViewName: {
                required: "Enter your first name",
                minlength: "At least 3 characters",
                maxlength: "Max length 20 symbol"
            },
            Email: {
                onfocusout: true,
                required: "Enter Email",
                email: "Email incorrect!",
                minlength: "At least 3 characters",
                maxlength: "Max length 20 symbol"
            },
            Location: {
                required: "Enter your Location",
                minlength: "At least 2 characters",
                maxlength: "Max length 20 symbol"
            },
            Phone: {
                required: "Enter Phone",
                minlength: "At least 3 characters",
                maxlength: "Max length 20 symbol"
            },
            Password: {
                required: "Enter password",
                minlength: "At least 3 characters",
                maxlength: "Max length 20 symbol"
            },
            ConfirmPassword: {
                minlength: "At least 3 characters",
                equalTo: "Password does not match"
            },
        }
    });
}

function ValidateEditFormProduct() {
    $(".formCreatedProduct").validate({
        rules: {
            Title: {
                required: true,
                minlength: 3,
                maxlength: 20
            },
            Price: {
                required: true,
                minlength: 1,
                maxlength: 20,
                number: true
            },
            Description: {
                maxlength: 60
            }
        },
        messages: {
            Title: {
                required: "Enter Title your product",
                minlength: "At least 3 characters",
                maxlength: "Max length 20 symbol"
            },
            Price: {
                required: "Enter Price",
                minlength: "At least 1 characters",
                maxlength: "Max length 20 symbol",
                number: "Only number"
            },
            Description: {
                maxlength: "Max length 60 symbol"
            }
        }
    });
}

function ValidateCreateFormProduct() {
    $(".formCreatedProduct").validate({
        rules: {
            Title: {
                required: true,
                minlength: 3,
                maxlength: 20
            },
            Price: {
                required: true,
                minlength: 1,
                maxlength: 20,
                number: true
            },
            Description: {
                maxlength: 60
            }
        },
        messages: {
            Title: {
                required: "Enter Title your product",
                minlength: "At least 3 characters",
                maxlength: "Max length 20 symbol"
            },
            Price: {
                required: "Enter Price",
                minlength: "At least 1 characters",
                maxlength: "Max length 20 symbol",
                number: "Only number"
            },
            Description: {
                maxlength: "Max length 60 symbol"
            }
        }
    });
}

/********************************* Client jquery for Buyer *********************************/

function ViewBuyer(buyer) {
    var result = '<h3>My profile</h3>';
    result += '<span><b>Display name: </b>' + buyer.DisplayName + '</span> <br />' +
        '<span><b>Email: </b>' + buyer.Email + '</span> <br />' +
        '<span><b>Role: </b>Buyer</span> <br />' +
        '<span><b>Date Register: </b>' + moment(buyer.DateRegister).format('lll') + '</span> <br />' +
        '<p><b>Reviews about me: </b></p>';
    $(buyer.comments).each(function (i, item) {
        result += '<span><b>Author: </b>' + item.Owner.DisplayName + '<br /></span>' +
                  '<span><b>Text: </b>' + item.Text + '<br /></span>' +
                  '<span><b>Date Added: </b>' + moment(item.DateCreation).format('lll') + '</span>';
    });
    $('#viewUserAction').html(result);
}

function ViewBuyerToUpdate(data) {
    var result = '<form class="editForm"><br/>';
    result += '<span><input type="text" id="editDisplayName" value="' + data.DisplayName + '" name="ViewName"/> Name </span><br/>' +
        '<span><input type="text" id="editEmail" value="' + data.Email + '" name="Email"/> Email </span><br/>' +
        '<input type="hidden" id="editPassword" value="' + data.Password + '" name="Password"/>' +
        //'<span><input type="password" id="confirmPassword" value="" name="ConfirmPassword"/> Confirm Password </span><br/>' +
        '<input type="hidden" id="editId" value="' + data.Id + '"/>' +
        '<input type="hidden" id="editIsActive" value="' + data.IsActive + '"/>' +
        '<input type="hidden" id="editRole" value="' + data.Role + '"/>' +
        '<input type="hidden" id="editDateRegister" value="' + data.DateRegister + '"/>' +
        '<input type="hidden" id="editComments" value="' + data.Comments + '"/>' +
        '<p><input id="saveAccountBuyer" type="button" value="Save" /></p></form>';
    $('#viewUserAction').html(result);
    ValidateEditFormSeller();
}

function SaveEditAccountBuyer() {
    var editBuyer = {
        DisplayName: $('#editDisplayName').val(),
        Email: $('#editEmail').val(),
        Password: $('#editPassword').val(),
        Id: $('#editId').val(),
        IsActive: $('#editIsActive').val(),
        Role: $('#editRole').val(),
        DateRegister: $('#editDateRegister').val(),
        Comments: $('editComments').val()
    };

    $.ajax({
        url: 'http://localhost:49909/account/editUser/' + editBuyer.Id,
        type: 'PUT',
        data: JSON.stringify(editBuyer),
        contentType: "application/json;charset=utf-8",
        success: function () {
            var cookieUuid = $.cookie('PikabaV3');
            GetUser(cookieUuid);
        },
        error: function (x, y, z) {
            alert(x + '\n' + y + '\n' + z);
        }
    });
}