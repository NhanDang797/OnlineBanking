$(document).ready(function () {
    
    $("#btn-save").click(function () {
        var numcheque = $('#numberofCheque option:selected').val();
        var bankAcc = $('#bankacc option:selected').val();
        //alert(num)
        $.ajax({
            url: "/Account/CreateCheque",
            method: "POST",
            data: {
                accBank: bankAcc,
                numberCheque : numcheque
            },
            success: function () {
                alert("Request Cheque Book Successfull");
                loadData();
            }
        })
    })

    $("#btn-actived").click(function () {
        var id = $(this).attr("data-id");
        var result = confirm("Are you want to Inactive this account");
        if (result == true) {

            //call function change status 
            $.ajax({
                url: "/Account/BlockBankAccount",
                method: "POST",
                data: {
                    id: id
                },
                success: function () {
                    alert("Update Successfull");
                    window.location.reload();
                }
            })
        } else {
            return;
        }
    })


    //event table
    $(document).on("click", "#table-bankacc tr", function () {
        var value = $(this).attr("data-id");
        
        window.location.href = "/Account/Transaction/?accNumber=" + value;
       
    });

});