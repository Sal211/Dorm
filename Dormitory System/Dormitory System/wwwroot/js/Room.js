 
var Roomdata = []
$('#tbroom').DataTable({
    data: Roomdata,
    columns: [
        {
            render: function (data, type, full, meta) { return meta.row + 1; }
        },
        { data: "roomtype" },
        { data: "roomname" },
        { data: "floor" },
        { data: "bed" },
        { data: "capacity" },
        {
            render: function (data, type, full) {
                return '<a href="#" onclick="remove(' + full.roomid + ')"><i class="fa fa-trash" style="color: red;"></i></a>' +
                    '<a href="#" class="edit"><i class="fa-solid fa-pen-to-square" style="color: green; margin-left: 8px;"></i></a>' +
                    '<a href="#" class="detail"><img src="/img/detail.png" style="width: 17px;margin-left: 8px; "/></a>';
            }
        },
    ]
});
var table = $('#tbroom').DataTable();
var id = 0;
var roomid = 0;
close
function close(name) {
    $('.close-modal').each(function () {
        $(this).click(() => {
            id = 0;
            $(name).modal('hide');
        });
    });
}
// Get Data CBO roomtype
$('#add').click(() => {
    alert(1)
    dataroomtypecbo(1, 1);
});

function dataroomtypecbo(num, name) {
    $.get('/MasterData/Getcboroomtype', data => {

        if (num == 1) {
            $('#roomtype').empty();

            for (var i = 0; i < data.clsRoomTypes.length; i++) {
                $('#roomtype').append(new Option(data.clsRoomTypes[i].roomtype, data.clsRoomTypes[i].roomtypeid));
            }
        } else if (num == 0) {
            $('#uroomtype').empty();
            alert(name)
            for (var i = 0; i < data.clsRoomTypes.length; i++) {
                $('#uroomtype').append(new Option(data.clsRoomTypes[i].roomtype, data.clsRoomTypes[i].roomtypeid));
            }
            $('#uroomtype').val(name);
        }
    });
}

// Get Data

getdata();
function getdata() {
    $.ajax({
        url: '/Room/GetRoom',
        type: "GET",
        dataType: "JSON",
        success: function (data) {

            console.log(data);
            Roomdata = data.clsRooms;
            table.clear().rows.add(Roomdata).draw();

        }
    })

}
// Delete Data

function remove(roomid) {
    $('#msg-delete').modal('show');
    id = roomid
    close('#msg-delete');
    $('#delete').click(() => {
        $.post('/Room/DeleteRoom', { id: id }, data => {
            if (data.errcode == 1) {
                $('#cancel').trigger("click")
                getdata();
            }
        });
    });
}
// click bg
$('#tbroom').on('click', 'tr', function () {
    $(this).toggleClass('selected');
    $('#tbroom tr').not(this).removeClass('selected');
});
// Detail
$('#tbroom').on('click', '.detail', function () {
    var a = $(this).parent().parent();
    console.log(a)
    var index = table.row(a).index()
    var data = table.row(index).data();
    close('#idfrmroom_detail')
    $('#idfrmroom_detail').modal('show');

    $('#droomname').val(data.roomname);
    $('#droomtype').val(data.roomtype);
    $('#dfloor').val(data.floor);
    $('#dcapacity').val(data.capacity);
    $('#dbed').val(data.bed);
    $('#dnote').val(data.note);
    $('#cuid').val(data.createuid);
    $('#cdate').val(data.formatdate);
    roomid = 0;
    roomid = data.roomid;
})
// Edit

$('#tbroom').on('click', '.edit', function () {
    var a = $(this).parent().parent();
    console.log(a)
    var index = table.row(a).index()
    var data = table.row(index).data();
    close('#idfrmroom_update')
    $('#idfrmroom_update').modal('show');

    $('#uroomname').val(data.roomname);
    console.log(data.roomtypeid)
    dataroomtypecbo(0, data.roomtypeid)
    $('#ufloor').val(data.floor);
    $('#ucapacity').val(data.capacity);
    $('#ubed').val(data.bed);
    $('#unote').val(data.note);
    roomid = 0;
    roomid = data.roomid;
})

// Post
var objroom = {}
$('.needs-validate').submit(function (event) {
    event.preventDefault();
    if (this.checkValidity() === false) {
        event.stopPropagation();
    } else {
        objroom = {}
        objroom.roomtypeid = $('#roomtype').val();
        objroom.roomname = $('#roomname').val();
        objroom.floor = $('#floor').val();
        objroom.capacity = $('#capacity').val();
        objroom.note = $('#note').val();
        objroom.bed = $('#bed').val();
  
        $.post('/Room/PostRoom', { obj: objroom },
            data => {
                if (data && data.errcode === 1) {
                    $('#close').trigger('click')
                    $('#msg-success').modal('show');
                    getdata();
                    close('#msg-success');
                    $('.needs-validate')[0].reset();
                }
            });
    }
    $(this).addClass('was-validated');
});
//update
$('.needs-validate-update').submit(function (event) {
    event.preventDefault();
    if (this.checkValidity() === false) {
        event.stopPropagation();
    } else {
        objroom = {}
        objroom.roomtypeid = $('#uroomtype').val();
        objroom.roomname = $('#uroomname').val();
        objroom.floor = $('#ufloor').val();
        objroom.capacity = $('#ucapacity').val();
        objroom.note = $('#unote').val();
        objroom.bed = $('#ubed').val();
        objroom.roomid = roomid
        $.post('/Room/UpdateRoom ', { obj: objroom }, data => {
            if (data.errcode == 1) {
                $('#idfrmroom_update').modal('hide')
                $('#msg-edit').modal('show');
                getdata();
                close('#msg-edit');
            }
        }
        );
    }

    $(this).addClass('was-validated');
});

