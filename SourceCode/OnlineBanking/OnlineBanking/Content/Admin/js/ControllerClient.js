

var pageSize = 4;
var pageindex = 0;


var HomeController = {
    init: function () {
        HomeController.registerEvent();
    },
    registerEvent: function () {
        HomeController.loadData();
        HomeController.loadFAQs();
        // search data
        $("#btn-search").click(function () {
            HomeController.loadData(true);
        })
        $("#txtname").keypress(function (e) {
            if (e.which == 13) {
                HomeController.loadData(true);
            }
        })

        // add new customer

        $("#btn-loginpass").click(function () {
            var pass = Math.floor(Math.random() * 100000000) + 100000000;
            $("#loginpass").val(pass);
        })

        $("#btn-logintran").click(function () {
            var pass = Math.floor(Math.random() * 100000000) + 100000000;
            $("#tranpass").val(pass);
        })
        $("#icon-model-close").click(function () {
            HomeController.resetfiled();
        });


        $("#btn-create-cus").click(function () {
            $("#myModal").modal("show");
        })

        // create customer

        $("#submit").click(function () {
            //first name

            if ($("#fname").val().length == 0) {
                $("#alert-fname").text("You can't leave this empty.");
                return;

            }
            if ($("#fname").val().length > 20) {
                $("#alert-fname").text(" First Name cannot be greater than 20 characters");
                return;
            }
            else {
                $("#alert-fname").text("");
            }

            //last name

            if ($("#lname").val().length == 0) {
                $("#alert-lname").text("You can't leave this empty.");
                return;

            }
            if ($("#lname").val().length > 40) {
                $("#alert-lname").text(" Last Name cannot be greater than 40 characters");
                return;
            }
            else {

                $("#alert-lname").text("");
            }
            //email
            function validateEmail($email) {
                var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
                return emailReg.test($email);
            }


            if ($("#email").val().length == 0) {
                $("#alert-email").text("You can't leave this empty.");
                return;

            }
            if (!validateEmail($("#email").val())) {
                $("#alert-email").text("Email Address invalid .");
                return;
            }
            else {
                $("#alert-email").text("");
            }


            // loginpass


            if ($("#loginpass").val().length == 0) {
                $("#alert-loginpass").text("Please generate login password.");
                return;
            }

            else {
                $("#alert-loginpass").text("");
            }

            // tranpass

            if ($("#tranpass").val().length == 0) {
                $("#alert-tranpass").text("Please generate transaction password.");
                return;

            }
            else {
                $("#alert-tranpass").text("");
            }

            // phone

            if ($("#phone").val().length == 0) {
                $("#alert-phone").text("You can't leave this empty.");
                return;
            }

            var filter = /^[0-9-+]+$/;
            if (!filter.test($("#phone").val())) {
                $("#alert-phone").text("Phone number must be 10 digits.");
                return;
            }

            else {
                $("#alert-phone").text("");
            }


            //address

            if ($("#address").val().length == 0) {
                $("#alert-address").text("You can't leave this empty.");
                return;

            }
            if ($("#address").val().length > 255) {
                $("#alert-address").text(" Address cannot be greater than 255 characters");
                return;
            }
            if ($("#address").val().length < 15) {
                $("#alert-address").text(" Address Invalid");
                return;
            }
            else {
                $("#alert-address").text("");
            }

            //---------------------------------
            var firstname = $("#fname").val();
            var lastname = $("#lname").val();
            var email = $("#email").val();
            var logpass = $("#loginpass").val();
            var tranpass = $("#tranpass").val();
            var phone = $("#phone").val();
            var address = $("#address").val();
            var gender = $("input[name='gender']:checked").val()


            HomeController.hideModel();
            HomeController.resetfiled();
            HomeController.loadData(true);
            // show / hide message
            setTimeout(function () {
                $("#alert-box").removeClass("alert-display");
            }, 700);

            setTimeout(function () {
                $("#alert-box").addClass("alert-display");
            }, 1500);
            setTimeout(function () {
                location.reload();
            }, 2000);

            $.ajax({
                url: "/Admin/Admin/CreateCustomer",
                method: "POST",
                data: {
                    fname: firstname,
                    lname: lastname,
                    email: email,
                    loginpass: logpass,
                    tranpass: tranpass,
                    phone: phone,
                    address: address,
                    gender: gender
                },
                success: function () {

                }
            });

        });
        // end create customer

        // event click table

        $(document).on("click", "#tbl-data tr", function () {
            var value = $(this).attr("data-id");

            window.location.href = "/Admin/Admin/Detail/" + value;
        });

        // edit FAQs
        var idTr = "";
        $(document).on("click", "#btn-edit", function () {
            $("#modelEdit").modal("show");
            idTr = $(this).closest("tr").attr("data-id");
            var question = $(this).closest("tr").find("#content-question").text()

            var answer = $(this).closest("tr").find("#content-answer").text();
            $("#container-question").text(question);
            $("#container-answer").text(answer);

        });


        //update faqs
        $("#btn-submit").click(function () {
            if ($("#container-question").val().length == 0) {
                $("#alert-ques").text("Content can't empty.");
                return;

            }
            if ($("#container-answer").val().length == 0) {
                $("#alert-ans").text("Content can't empty.");
                return;

            }
            var textQue = $("#container-question").val();
            var textAns = $("#container-answer").val();

            $.ajax({
                url: "/Admin/Admin/EditFaqs",
                method: "POST",
                data: {
                    id: idTr,
                    question: textQue,
                    answer: textAns
                },
                success: function () {
                    $('#modelEdit').modal('hide');
                    $("#msg").text("Update");
                    setTimeout(function () {
                        $("#alert-faqs").removeClass("alert-faqs");
                    }, 700);

                    setTimeout(function () {
                        $("#alert-faqs").addClass("alert-faqs");
                    }, 1500);

                    HomeController.loadFAQs();
                }
            })
        })

        // delete faqs
        $(document).on("click", "#btn-delete", function () {
            var id = $(this).closest("tr").attr("data-id");
            $.ajax({
                url: "/Admin/Admin/DeleteFaqs",
                method: "POST",
                data: {
                    id: id
                },
                success: function () {
                    $("#msg").text("Delete");
                    setTimeout(function () {
                        $("#alert-faqs").removeClass("alert-faqs");
                    }, 700);

                    setTimeout(function () {
                        $("#alert-faqs").addClass("alert-faqs");
                    }, 1500);
                    HomeController.loadFAQs();
                }
            })
        });

        // Create faqs
        $(document).on("click", "#btn-change", function () {

            if ($("#txt-question").val().length == 0) {
                $("#alert-question").text("Content can't empty.");
                return;

            }
            if ($("#txt-answer").val().length == 0) {
                $("#alert-answer").text("Content can't empty.");
                return;

            }
            var textQue = $("#txt-question").val();
            var textAns = $("#txt-answer").val();
            $.ajax({
                url: "/Admin/Admin/CreateFaqs",
                method: "POST",
                data: {
                    question: textQue,
                    answer: textAns
                },
                success: function () {
                    $('#modelcreate').modal('hide');
                    $("#msg").text("Create new ");
                    setTimeout(function () {
                        $("#alert-faqs").removeClass("alert-faqs");
                    }, 700);

                    setTimeout(function () {
                        $("#alert-faqs").addClass("alert-faqs");
                    }, 1500);
                    HomeController.loadFAQs();
                }
            })
        });

    },
    hideModel: function () {
        $('#myModal').modal('hide');
    },
    resetfiled: function () {
        $("#fname").val("");
        $("#lname").val("");
        $("#email").val("");
        $("#email").val("");
        $("#loginpass").val("");
        $("#tranpass").val("");
        $("#phone").val("");
        $("#address").val("");

        //alert
        $("#alert-fname").text("");
        $("#alert-lname").text("");
        $("#alert-email").text("");
        $("#alert-loginpass").text("");
        $("#alert-tranpass").text("");
        $("#alert-phone").text("");
        $("#alert-address").text("");

    },
    loadData: function (changePageSize) {
        var nameSearch = $("#txtname").val();
        var ckstatus = $("#status").val();

        $.ajax({
            url: "/Admin/Admin/LoadData",
            method: "GET",
            dataType: "json",
            data: {
                page: pageindex,
                size: pageSize,
                name: nameSearch,
                status: ckstatus
            },
            success: function (response) {
                if (response.status) {
                    var data = response.data;
                    var html = "";
                    var temlpate = $("#data-template").html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(temlpate, {
                            ID: item.CustomerId,
                            Name: item.FirstName + " " + item.LastName,
                            Gender: item.Gender == true ? "Male" : "Female",
                            Status: item.LockedStatus == true ? "<span class=\"label label-success\">Actived</span>" : "<span class=\"label label-danger\">Blocked</span>"
                        });
                    })
                    $("#tbl-data").html(html);
                    HomeController.pagignation(response.totalRow, function () {
                        HomeController.loadData();
                    }, changePageSize);

                }
            }
        })
    },
    loadFAQs: function () {

        $.ajax({
            url: "/Admin/Admin/LoadFAQs",
            method: "POST",
            dataType: "json",
            success: function (response) {
                if (response.status) {
                    var data = response.data;
                    var html = "";
                    var temlpate = $("#data-template-faqs").html();
                    var stt = 0;
                    $.each(data, function (i, item) {
                        stt++;
                        html += Mustache.render(temlpate, {
                            stt: stt,
                            id: item.Id,
                            question: item.Question,
                            answer: item.Answer
                        });
                    })
                    $("#tbl-FAQs").html(html);

                }
            }
        })

    },
    pagignation: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / pageSize);
        //reset pagignation
        if ($("#paging").length == 0 || changePageSize === true) {
            $("#paging").empty();
            $("#paging").removeData("twbs-pagination");
            $("#paging").unbind("page");

        }

        $('#paging').twbsPagination({
            totalPages: totalPage,
            visiblePages: 5, // show 5 button . orther will be hidden

            onPageClick: function (event, page) {
                pageindex = page, // reset page current

                    setTimeout(callback, 200);
            }
        });
    }
}
HomeController.init();