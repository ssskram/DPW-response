$(document).ready(function() {
    $('#example').DataTable();
} );

function CheckField (){
    var called = $('#called').val();
    var accepted = $('#accepted').val();
    if (called != null & accepted != false) {
         alert("hey");
    }
}

