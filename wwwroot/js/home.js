// this document contains the client side functions for the home view

$.fn.dataTable.moment( 'MM/DD/YYYY');
$(document).ready(function(){
    var table = $("#dt").DataTable({
        pageLength : 100,
        searching: true,
        ordering: true,
        paging: true,
        bLengthChange: false,
        columnDefs: [
            { orderable: false, targets: [-1,-2] }
        ]
    });
    if( /Android|webOS|iPhone|iPad|iPod|BlackBerry/i.test(navigator.userAgent) ) {
    $('.selectpicker').selectpicker('mobile');
    }
    $('#search').on( 'keyup', function () {
        table.search( this.value ).draw();
    } );
    $('#dropdown').on( 'change' , function () {
        table.search( this.value ).draw();
        $('#search').val('');
    } );
    $(".dataTable").on('change','#called', function () { 
        var called = $(this).val();
        if (called == "Yes")
        {
            $(this).parent().parent().css("background-color", "rgba(185, 171, 139, 0.3)");
            $(this).parent().parent().find( "td.accepted" ).children().prop("disabled",false);
        }
        else
        {
            $(this).parent().parent().removeAttr('style');
            $(this).parent().parent().find( "td.accepted" ).children().prop("disabled",true);
        }
    });
    $(".dataTable").on('change','#accepted', function () { 
        $(this).parent().parent().css("background-color", "rgba(185, 171, 139, 0.3)");
        var accepted = $(this).val();
        var called = $(this).parent().parent().find( "td.called" ).children().val();
        var OID = $(this).parent().parent().find( "#oid" ).val();
        if ((accepted == "Declined" | accepted == "Accepted" | accepted == "Excused") & (called == "Yes" )) 
        {
            $.ajax(
                {
                    url: "/Home/PostCallout",
                    type: 'POST',
                    data: { 'OID' : OID, 'Accepted' : accepted },
                    error: function(result) {
                        alert("Failed to post.  Please try again.");
                    }
                }
            );
            table.rows('.parent').nodes().to$().find('td:first-child').trigger('click');
            $(this).parent().parent().css("background-color", "rgba(57, 172, 205, 0.2)");
            $(this).parent().parent().fadeOut(600, function(){ $(this).remove();});
        }
        else
        {
            if (accepted == "Declined" | accepted == "Accepted" | accepted == "Excused")
            {
                $(this).parent().parent().css("background-color", "rgba(185, 171, 139, 0.2)");
            }
            else
            {
                $(this).parent().parent().removeAttr('style');
            }
        }
    });
    $("#form").show();
    new $.fn.dataTable.Responsive( table, {} );
});