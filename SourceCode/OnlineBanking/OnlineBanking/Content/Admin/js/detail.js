$(document).ready(function () {
    $("#btn-actived").click(function () {
        var id = $(this).attr("data-id");
        var result = confirm("Are you want to Block this account");
        if (result == true) {
           
            //call function change status 
            $.ajax({
                url: "/Admin/Admin/BlockCustomer",
                method: "POST",
                data: {
                    id: id
                },
                success: function () {
                    alert("Update Successfull");
                    window.location.href = "/Admin/Admin/Detail/" + id;
                }
            })
        } else {
            return;
        }
    })

    $("#btn-blocked").click(function () {
        var id = $(this).attr("data-id");
        var result = confirm("Are you want to Ative this account");
        if (result == true) {
            var loginpass = Math.floor(Math.random() * 100000000) + 100000000;
            var tranpass = Math.floor(Math.random() * 100000000) + 100000000;

            
                alert("Update Successfull");
                window.location.href = "/Admin/Admin/Detail/" + id;
            
            //call function change status and password
            $.ajax({
                url: "/Admin/Admin/UpdateCustomer",
                method: "POST",
                data: {
                    id: id,
                    loginpass: loginpass,
                    tranpass: tranpass
                },
                success: function () {
                   
                }
                
            })
            
        } else {
            return;
        }
    })

    // create account customer
    $("#btn-create-acc").click(function () {
        var id = $(this).attr("data-id");
        var accNumber = ("OB-" + (Math.floor(Math.random() * 1000000000) + 1000000000));
        $.ajax({
            url: "/Admin/Admin/CreateAccount",
            method: "POST",
            data: {
                id: id,
                accountNumber: accNumber
            },
            success: function () {
                alert("Create new AccountBank Successfull");
                window.location.href = "/Admin/Admin/Detail/" + id;
            }
        })

    });
    $("#btn-funds").click(function () {
        $("#myModal").modal("show");
        $("#alert-money").text("");
    })

    $("#btn-tranfer").click(function () {
        
        var value = $('#dropDownId option:selected').val();
        var numberMoney = $("#money").val();
        if (value.length == 0) {
            $("#alert-money").text("Transaction Failed !!!");
            return;
        }
        if (numberMoney.length == 0) {
            $("#alert-money").text("Transaction Failed !!!");
            return;
        }
        if (numberMoney.length > 10) {
            $("#alert-money").text("Transaction amounts are too large !!!");
            return;
        }
        $.ajax({
            url: "/Admin/Admin/TransferMoney",
            method: "POST",
            data: {
                accNumber: value,
                numberMoney: numberMoney
            },
            success: function () {
                alert("Transfers Money Successfull");
                  $("#myModal").modal("hide");
                location.reload();
            }
        })
    })


    // even click tableAcc
    $(document).on("click", "#table-accountBank tr", function () {
        var bankID = $(this).attr("data-bankId");
       // $("#myModal").modal("show");
    });
});