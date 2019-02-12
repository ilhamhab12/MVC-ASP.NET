var Suppliers = []
//fetch categories from database
function LoadSupplier(element) {
    if (Suppliers.length == 0) {
        //ajax function for fetch data
        $.ajax({
            type: "GET",
            url: 'http://localhost:1873/Transactions/getItemSuppliers/',
            success: function (data) {
                Suppliers = data;
                //render catagory
                renderSupplier(element);
            }
        })
    }
    else {
        //render catagory to the element
        renderSupplier(element);
    }
}

function renderSupplier(element) {
    var $ele = $(element);
    $ele.empty();
    $ele.append($('<option/>').val('0').text('Select'));
    $.each(Suppliers, function (i, val) {
        $ele.append($('<option/>').val(val.Id).text(val.Name));
    })
}

//fetch products
function LoadItem(supplierDD) {
    $.ajax({
        type: "GET",
        url: 'http://localhost:1873/Transactions/getItems/',
        data: { 'Id': $(supplierDD).val() },
        success: function (data) {
            //render products to appropriate dropdown
            renderItem($(supplierDD).parents('.mycontainer').find('select.item'), data);
        },
        error: function (error) {
            console.log(error);
        }
    })
}

function renderItem(element, data) {
    //render product
    var $ele = $(element);
    $ele.empty();
    $ele.append($('<option/>').val('0').text('Select'));
    $.each(data, function (i, val) {
        $ele.append($('<option/>').val(val.Id).text(val.Name));
    })
}

$(document).ready(function () {

    //menampilkan orderDate sesuai dengan tanggal sekarang
    var date = new Date();

    var day = date.getDate();
    var month = date.getMonth() + 1;
    var year = date.getFullYear();

    if (month < 10) month = "0" + month;
    if (day < 10) day = "0" + day;

    //mengeirim data ke orderDate
    var today = year + "-" + month + "-" + day;
    $("#orderDate").attr("value", today);

    //Add button click event
    $('#add').click(function () {
        //validation and add order items
        var isAllValid = true;
        if ($('#itemSupplier').val() == "0") {
            isAllValid = false;
            $('#itemSupplier').siblings('span.error').css('visibility', 'visible');
        }
        else {
            $('#itemSupplier').siblings('span.error').css('visibility', 'hidden');
        }

        if ($('#item').val() == "0") {
            isAllValid = false;
            $('#item').siblings('span.error').css('visibility', 'visible');
        }
        else {
            $('#item').siblings('span.error').css('visibility', 'hidden');
        }

        if (!($('#quantity').val().trim() != '' && (parseInt($('#quantity').val()) || 0))) {
            isAllValid = false;
            $('#quantity').siblings('span.error').css('visibility', 'visible');
        }
        else {
            $('#quantity').siblings('span.error').css('visibility', 'hidden');
        }

        //if (!($('#rate').val().trim() != '' && !isNaN($('#rate').val().trim()))) {
        //    isAllValid = false;
        //    $('#rate').siblings('span.error').css('visibility', 'visible');
        //}
        //else {
        //    $('#rate').siblings('span.error').css('visibility', 'hidden');
        //}

        if (isAllValid) {
            var $newRow = $('#mainrow').clone().removeAttr('id');
            $('.pc', $newRow).val($('#itemSupplier').val());
            $('.item', $newRow).val($('#item').val());

            //Replace add button with remove button
            $('#add', $newRow).addClass('remove').val('Remove').removeClass('btn-success').addClass('btn-danger');

            //remove id attribute from new clone row
            //$('#itemSupplier,#item,#quantity,#rate,#add', $newRow).removeAttr('id');
            $('#itemSupplier,#item,#quantity,#add', $newRow).removeAttr('id');
            $('span.error', $newRow).remove();
            //append clone row
            $('#orderdetailsItems').append($newRow);

            //clear select data
            $('#itemSupplier,#item').val('0');
            //$('#quantity,#rate').val('');
            $('#quantity').val('');
            $('#orderItemError').empty();
        }

    })

    //remove button click event
    $('#orderdetailsItems').on('click', '.remove', function () {
        $(this).parents('tr').remove();
    });

    $('#submit').click(function () {
        var isAllValid = true;
        debugger;

        //validate order items
        $('#orderItemError').text('');
        var list = [];
        var errorItemCount = 0;
        $('#orderdetailsItems tbody tr').each(function (index, ele) {
            if (
                $('select.item', this).val() == "0" ||
                (parseInt($('.quantity', this).val()) || 0) == 0
                //||
                //$('.rate', this).val() == "" ||
                //isNaN($('.rate', this).val())
                ) {
                errorItemCount++;
                $(this).addClass('error');
            } else {
                var orderItem = {
                    Id: $('select.item', this).val(),
                    Quantity: parseInt($('.quantity', this).val()),
                    //Rate: parseFloat($('.rate', this).val())
                }
                list.push(orderItem);
            }
        })

        if (errorItemCount > 0) {
            $('#orderItemError').text(errorItemCount + " invalid entry in order item list.");
            isAllValid = false;
        }

        if (list.length == 0) {
            $('#orderItemError').text('At least 1 order item required.');
            isAllValid = false;
        }

        //if ($('#orderNo').val().trim() == '') {
        //    $('#orderNo').siblings('span.error').css('visibility', 'visible');
        //    isAllValid = false;
        //}
        //else {
        //    $('#orderNo').siblings('span.error').css('visibility', 'hidden');
        //}

        if ($('#orderDate').val().trim() == '') {
            $('#orderDate').siblings('span.error').css('visibility', 'visible');
            isAllValid = false;
        }
        else {
            $('#orderDate').siblings('span.error').css('visibility', 'hidden');
        }

        if (isAllValid) {
            var data = {
                //OrderNo: $('#orderNo').val().trim(),
                TransactionDate: $('#orderDate').val().trim(),
                //Description: $('#description').val().trim(),
                //OrderDetails: list
                TransactionItem: list
            }

            $(this).val('Please wait...');

            $.ajax({
                type: 'POST',
                url: 'http://localhost:1873/Transactions/save/',
                data: JSON.stringify(data),
                contentType: 'application/json',
                success: function (data) {
                    if (data.status) {
                        //alert('Successfully saved');
                        //pake swal untuk menampilkan alert mengunakan sweet allet js
                        swal('Successfully saved');
                        //here we will clear the form
                        list = [];
                        //$('#orderNo,#orderDate,#description').val('');
                        $('#orderDate').val('');
                        $('#orderdetailsItems').empty();
                    }
                    else {
                        //alert('Error');
                        swal('Error');
                    }
                    $('#submit').val('Save');
                },
                error: function (error) {
                    console.log(error);
                    $('#submit').val('Save');
                }
            });
        }

    });

});

LoadSupplier($('#itemSupplier'));