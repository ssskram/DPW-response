function CheckField () {
    var accepted = $('#accepted').val();
    var called = $('#called').val();
    if ( (accepted == "Yes" | accepted == "No") & (called == "Yes" ) ) {
        $('#trigger').parent().parent().remove();
    }
}