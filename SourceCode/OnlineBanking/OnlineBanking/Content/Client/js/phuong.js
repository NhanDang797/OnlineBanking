$(document).ready(function () {
    $("#fund-transfer").show();
    $("#transfer-confirm").hide();
    $("#password-confirm").hide();
    $("#success-notify").hide();
});

var OriginatorAccount, BenificialAccount, Amount, Content, CustomerId, BenificialName;
var TransactionPassword;
$("#btn-fund-transfer").click(function () {
    OriginatorAccount = $("#originator-account").val();
    BenificialAccount = $("#benificial-account").val();
    Amount = $("#amount").val();
    Content = $("#content").val();
    CustomerId = 2;
    var data = JSON.stringify({
        'OriginatorAccount': OriginatorAccount,
        'BenificialAccount': BenificialAccount,
        'Amount': Amount,
        'Content': Content,
        'CustomerId': CustomerId
    });
    $.ajax({
        type: "POST",
        url: '/Transactions/TransferConfirm',
        contentType: 'application/json',
        data: data,
        success: function (data, status) {
            if (data.message == null) {
                $("#fund-transfer").remove();
                $("#transfer-confirm").show("fast");

                $("#originator-account").html(data.oA);
                BenificialName = data.bAName.toUpperCase();
                $("#benificial-account").html(data.bA + " - " + BenificialName);
                $("#amount").html(data.am);
                $("#content").html(data.content);
            }
            else {
                $("#error-message").html(data.message);
            }           
        }
    });
});

$("#btn-confirm-transfer").click(function () {
    console.log("Da vao duoc day 1");
    $("#transfer-confirm").remove();
    $("#password-confirm").show("fast");
    console.log("Da vao duoc day 2");
});

$("#transaction-password").focusout(function () {
    TransactionPassword = $(this).val();
    //$(this).after(txt);
});

$("#btn-confirm-password").click(function () {

    var data = JSON.stringify({
        'OriginatorAccount': OriginatorAccount,
        'BenificialAccount': BenificialAccount,
        'Amount': Amount,
        'Content': Content,
        'TransactionPassword': TransactionPassword,
        'CustomerId': CustomerId
    });
    $.ajax({
        type: "POST",
        url: '/Transactions/Transfer',
        contentType: 'application/json',
        data: data,
        success: function (data, status) {
            if (data.message2 == null) {
                $("#password-confirm").remove();
                $("#success-notify").show("fast");

                $("#benificial-name").html(BenificialName);
                $("#benificial-account").html(data.bA);
                $("#amount").html(data.am + " USD");
            }
            else {
                $("#error-message").html(data.message2);
            }
            
        }
    });
});

$("#dropdown-statement-type").change(function () {
    if ($(this).val() == "2") {
        $("#month-select").show();
    }
    else {
        $("#month-select").hide();
    }
});

