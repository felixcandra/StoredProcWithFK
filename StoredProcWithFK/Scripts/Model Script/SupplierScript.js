$(document).ready(function () {
    hideAlert();
    GetSupplier();
    $('#table').DataTable({
        "ajax": GetSupplier()
    });
})

function validationInsert() {
    debugger;
    hideAlert(); // setiap kali ngeklik tombol save ilangkan dulu errornya baru cek lagi satu satu
    var isAllValid = true; //asumsi semua textbox sudah terisi
    //cek textbox nama
    if ($('#Name').val() == "" || ($('#Name').val() == " ")) {
        isAllValid = false; //kalau textbox nama kosong maka
        $('#Name').siblings('span.error').css('visibility', 'visible'); //ini notifikasi buat ngasi tau field belum diisi pas mencet save 
    }
    // kalau semua field sudah terisi
    if (isAllValid) {
        Save();
    }
}

function Save() {
    debugger;
    var supplier = new Object();
    supplier.Name = $('#Name').val();

    $.ajax({
        type: "POST",
        data: supplier,
        url: "Create",
        datatype: "json",
        success: function () {
            $('#myModal').modal('hide');
            GetSupplier();
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
    // kalau semua field sudah terisi
    if (isAllValid) {
        Edit();
    }
}

function Edit() {
    var supplier = new Object();
    supplier.Id = $('#Id').val();
    supplier.Name = $('#Name').val();
    $.ajax({
        type: "POST",
        data: supplier,
        url: "Edit/" + $('#Id').val(),
        datatype: "json",
        success: function () {
            $('#myModal').modal('hide');
            GetSupplier();
            nuke();
        }
    })
}

function GetSupplier() {
    debugger;
    $.ajax({
        type: "GET",
        url: "GetSupplier",
        datatype: "json",
        success: function (data) {
            debugger;
            var html = '';
            var data2 = JSON.parse(data);
            $.each(data2, function (index, val) {
                html += '<tr>';
                html += '<td>' + val.Name + '</td>';
                html += '<td> <a href="#" onclick="return GetById(' + val.Id + ')"> Edit </a>';
                html += '| <a href="#" onclick="return Delete(' + val.Id + ')"> Delete </a>  </td>';
                html += '</tr>';
            });
            $('.tbody').html(html);
        }
    });
    swal("Loading Supplier...", {
        timer: 2000
    });
}

function GetById(Id) {
    $.ajax({
        url: "GetSupplierById/" + Id,
        type: "GET",
        datatype: "json",
        success: function (data) {
            debugger;
            data = JSON.parse(data);
            $('#Id').val(data.Id);
            $('#Name').val(data.Name);
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
                    window.location.href = "Index"
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