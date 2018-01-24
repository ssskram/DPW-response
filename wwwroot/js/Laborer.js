function CheckField (){
    var accepted = $('#accepted').val();
    if ( (accepted == "Yes" | accepted == "No") & ($('#called').is(':checked')) ) {

        $("#button").prop("disabled",false);
        $("#checkmark").css("display", "block");

    } else {

        $("#button").prop("disabled",true);
        $("#checkmark").css("display", "none");
    }
}