getDataRecentBooking()
 
function getDataRecentBooking() {
 
    $.get('/Dashboard/GetBookingRecent', data => {
        PostRecent_Booking(data)
    })
}
function PostRecent_Booking(data) {

    var bgPaid = ""
    $('#recent_Book').empty()
    data.clsBookings.forEach((value, key) => {
        bgPaid = value.ispaid ? "bg-success" : "bg-danger"
        const element =
            `
             <tr>
                 <th scope="row"><a href="#">${key+1}</a></th>
                 <td>${value.studentid}</td>
                 <td><a href="#" class="text-primary">${value.roomname}</a></td>
                 <td>$ ${value.tuitiondue}</td>
                 <td><span class="badge ${bgPaid}">${value.str_ispaid}</span></td>
             </tr>
        `
        $('#recent_Book').append(element)
    })
}

 
$('.listdate').click(function () {
    var dataValue = $(this).data("value");
    ShowDataTenant(dataValue)
    $('#titleTenant_Date').text('|' + $(this).text())
});
ShowDataTenant(1)
function ShowDataTenant(type) {
    console.log(type)
    var currentDate = new Date();
    var formattedDate = currentDate.toLocaleString()
    $.get('/Dashboard/GetDataDB_Tenant', { type: type, date: formattedDate.split(',')[0] }, data => {
        console.log(data)
        let tenant = data.length > 1 ? data[1].tenant : data[0].tenant
        $('#numTenant').text(tenant)
        $('#percentTenant').text(data[0].percent_tenant)
        $('#titleRoomName').text(data[0].roomType)
    })
}
