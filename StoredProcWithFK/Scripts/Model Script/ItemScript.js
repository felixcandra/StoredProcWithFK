var Supplier = []
$(document).ready(function () {
    debugger;
    hideAlert();
    GetItem();
    $('#table').DataTable({
        "ajax": GetItem()
    });
})

function LoadSupplier(element) {
    debugger
    if (Supplier.length == 0) {
        $.ajax({
            type: "GET",
            url: "GetSupplier",
            datatype: "json",
            success: function (supplier) {
                Supplier = supplier;
                renderSupplier(element);
            }
        })
    } else {
        renderSupplier(element);
    }
}

function renderSupplier(element) {
    var $ele = $(element);
    $ele.empty(); //kosongkan element
    $ele.append($('<option/>').val('0').text('Select')); //tambahkan item kedalam dropdown
    var supplier = JSON.parse(Supplier);
    $.each(supplier, function (i, val) { // tambahkan item baru kedalam dropdown 
        $ele.append($('<option/>').val(val.Id).text(val.Name)); //id sama nama
    })
}

function validationInsert() {
    debugger;
    hideAlert(); // setiap kali ngeklik tombol save ilangkan dulu errornya baru cek lagi satu satu
    var isAllValid = true; //asumsi semua textbox sudah terisi
    //cek textbox nama
    if ($('#Name').val() == "" || ($('#Name').val() == " ")) {
        isAllValid = false; //kalau textbox nama kosong maka
        $('#Name').siblings('span.error').css('visibility', 'visible'); //ini notifikasi buat ngasi tau field belum diisi pas mencet save 
    }

    if ($('#Stock').val() == 0) {
        isAllValid = false; //kalau textbox nama kosong maka
        $('#Stock').siblings('span.error').css('visibility', 'visible'); //ini notifikasi buat ngasi tau field belum diisi pas mencet save 
    }
    //cek dropdown Province
    if ($('#Supplier').val() == "Select" || $('#Supplier').val() == 0) {
        isAllValid = false;
        $('#Supplier').siblings('span.error').css('visibility', 'visible');
    }
    // kalau semua field sudah terisi
    if (isAllValid) {
        Save();
    }
}

function Save() {
    debugger;
    var item = new Object();
    item.Name = $('#Name').val();
    item.Stock = $('#Stock').val();
    item.Suppliers_Id = $('#Supplier').val();
    $.ajax({
        type: "POST",
        data: item,
        url: "Create",
        datatype: "json",
        success: function () {
            $('#myModal').modal('hide');
            GetItem();
            nuke();
        }
    });
}

function validationUpdate() {
    debugger;
    hideAlert(); // setiap kali ngeklik tombol save ilangkan dulu errornya baru cek lagi satu satu
    var isAllValid = true; //asumsi semua textbox sudah terisi
    //cek textbox nama
    if ($('#Name').val() == "" || ($('#Name').val() == " ")) {
        isAllValid = false; //kalau textbox nama kosong maka
        $('#Name').siblings('span.error').css('visibility', 'visible'); //ini notifikasi buat ngasi tau field belum diisi pas mencet save 
    }

    if ($('#Stock').val() == 0) {
        isAllValid = false; //kalau textbox nama kosong maka
        $('#Stock').siblings('span.error').css('visibility', 'visible'); //ini notifikasi buat ngasi tau field belum diisi pas mencet save 
    }
    //cek dropdown Province
    if ($('#Supplier').val() == "Select" || $('#Supplier').val() == 0) {
        isAllValid = false;
        $('#Supplier').siblings('span.error').css('visibility', 'visible');
    }
    // kalau semua field sudah terisi
    if (isAllValid) {
        Edit();
    }
}

function Edit() {
    var item = new Object();
    item.Id = $('#Id').val();
    item.Name = $('#Name').val();
    item.Stock = $('#Stock').val();
    item.Suppliers_Id = $('#Supplier').val();
    $.ajax({
        type: "POST",
        data: item,
        url: "Edit/" + $('#Id').val(),
        datatype: "json",
        success: function () {
            $('#myModal').modal('hide');
            GetItem();
            nuke();
        }
    })
}

function GetItem() {
    debugger;
    $.ajax({
        type: "GET",
        url: "GetItem",
        datatype: "json",
        success: function (data) {
            var html = '';
            var data2 = JSON.parse(data);
            $.each(data2, function (index, val) {
                html += '<tr>';
                html += '<td>' + val.Name + '</td>';
                html += '<td>' + val.Stock + '</td>';
                html += '<td>' + val.Suppliers.Name + '</td>'; //ini untuk tampilkan foreign key
                html += '<td> <a href="#" onclick="return GetById(' + val.Id + ')"> Edit </a>';
                html += '| <a href="#" onclick="return Delete(' + val.Id + ')"> Delete </a>  </td>';
                html += '</tr>';
            });
            $('.tbody').html(html);
        }
    });
    swal("Loading Item...", {
        timer: 2000
    });
}

function GetById(Id) {
    $.ajax({
        url: "GetItemById/" + Id,
        type: "GET",
        datatype: "json",
        success: function (data) {
            debugger;
            data = JSON.parse(data);
            $('#Id').val(data.Id);
            $('#Name').val(data.Name);
            $('#Stock').val(data.Stock);
            $('#Supplier').val(data.Suppliers_Id);
            $('#myModal').modal('show');
            $('#Save').hide();
            $('#Update').show();
        }
    });
}

function Delete(Id) {
    debugger;
    swal({ // popup konfirmasi delete swal 
        title: "Are you sure want to delete this?",
        text: "You will not be able to recover this!",
        icon: "warning",
        buttons: true,
        dangerMode: true,
        closeModal: false
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                url: "Delete/" + Id,
                type: "POST",
                datatype: "json",
                success: function (response) {
                    window.location.href = "Index";
                },
                error: function (response) {
                    swal("Oops", "Delete failed!", "error");
                }
            })
        } else {
            swal("Canceled!", "Delete canceled!", "info");
        }
    });
}

function hideAlert() {
    $('#Name').siblings('span.error').css('visibility', 'hidden');
    $('#Stock').siblings('span.error').css('visibility', 'hidden');
    $('#Supplier').siblings('span.error').css('visibility', 'hidden');
}

function nuke() {
    $('#Name').val('');
    $('#Stock').val(0);
    $('#Supplier').val(0);
    hideAlert();
}

LoadSupplier($("#Supplier"));