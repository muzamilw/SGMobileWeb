$(document).ready(function () {
    $(window).scroll(function () {
        var scroll = $(window).scrollTop();
        if (scroll < 10) {
            $(".desktopMenu a").css("color", "white");
            $("a.navbar-brand").css("color", "white");
            $(".navbar").css("border-bottom", "none");
            $(".navbar").css("box-shadow", "none");
        }

        else {
            $(".desktopMenu a").css("color", "#333333");
            $("a.navbar-brand").css("color", "#333333");
            $(".navbar").css("border-bottom", "1px solid #ccc");
            $(".navbar").css("box-shadow", "2px 1px 10px #888888");
        }
    });

    $('body').css('padding-right', '0');

    $("#Form_Login").validate({
        rules: {
            EmailAddress: {
                required: true,
                email: true
            },
            Password: {
                required: true,
            }
        },
        messages: {
            EmailAddress: {
                required: "Email required"
            },
            Password: {
                required: "Password required"
            }
        }
    });

    $("#Form_ForgetPassword").validate({
        rules: {
            EmailAddress: {
                required: true,
                email: true
            }
        },
        messages: {
            EmailAddress: {
                required: "Email required."
            }
        }
    });

    $("#Form_Signup").validate({
        rules: {
            FirstName: {
                required: true,
            },
            SurName: {
                required: true,
            },
            EmailAddress: {
                required: true,
                email: true,
                remote: {
                    type: "GET",
                    url: "/account/IsEmailExist?" + $("#Modal_SignupPopup input#SignEmailAddress").val(),
                }
            },
            Password: {
                required: true,
            },
            Re_Password: {
                equalTo: "#SignupPassword"
            }
        },
        messages: {
            FirstName: {
                required: "Firstname required."
            },
            SurName: {
                required: "Lastname required."
            },
            EmailAddress: {
                required: "Email required.",
                remote: "Email already exists"
            },
            Password: {
                required: "Password required."
            },
            Re_Password: {
                equalTo: "Password not matched."
            }
        }
    });

    $("a.lnk_ShowLoginModal").on("click", function (e) {
        $("#login-error").hide().html("");
        $("#forgetpassword-error").hide().html("");

        $("#Form_Login").validate().resetForm();

        $("#Form_ForgetPassword").validate().resetForm();
        $(".Modal-LoginStep").removeClass("hide");

        $(".Modal-ForgetPasswordStep").addClass("hide");

        $("#forgetPassowrdMessage").hide();
        $(".forgetPasswordFields").show();
        $("#Form_ForgetPassword").trigger("reset");
        $("#Form_Login").trigger("reset");

        $("#Modal_SignupPopup").modal("hide");
        $("#Modal_LoginPopup").modal("show");

        e.preventDefault();
        e.stopPropagation();
    });

    $("a.lnk_ShowFrogetPasswordModal").on("click", function (e) {
        $(".Modal-LoginStep").addClass("hide");
        $(".Modal-ForgetPasswordStep").removeClass("hide");

        e.preventDefault();
        e.stopPropagation();
    });

    $("a.lnk_ShowSignupModal").on("click", function (e) {
        $("h1.signup-success-message").addClass("hide");
        $("#Form_Signup").show();
        $("#Form_Signup").validate().resetForm();
        $('#Form_Signup').trigger("reset");
        $("#Modal_LoginPopup").modal("hide");
        $("#Modal_SignupPopup").modal("show");
        e.preventDefault();
        e.stopPropagation();
    });

    $(".btn_formLogin").on("click", function (e) {
        $("#login-error").html("");
        if ($("#Form_Login").valid()) {
            $("#LoginSpiner").show();
            var frmData = $("#Form_Login").serialize();
            $.ajax({
                type: "POST",
                url: "/account/Login",
                data: frmData,
                dataType: "json",
                success: function (data) {
                    if (!$.isEmptyObject(data)) {
                        if (data.ResultType == "Success") {
                            window.location = "/TargetPreferences/ModifyTargetPreferences?socialProfileId=" + data.ResultData;
                        } else {
                            $("#LoginSpiner").hide();
                            $("#login-error").show().html(data.message);
                        }
                    }
                },
                failure: function (errMsg) {
                    $("#LoginSpiner").hide();
                }
            });
        }

    });

    $(".btn_forgetPassword").on("click", function (e) {
        $("#forgetpassword-error").html("");
        if ($("#Form_ForgetPassword").valid()) {
            $("#ForgotPassSpiner").show();
            var data = $("#Form_ForgetPassword").serialize();
            $.ajax({
                type: "POST",
                url: "/account/forgetpassword",
                data: data,
                dataType: "json",
                success: function (data) {
                    $("#ForgotPassSpiner").hide();
                    if (!$.isEmptyObject(data)) {
                        $("#ForgotPassSpiner").hide();
                        if (data.ResultType == "Success") {
                            $("#forgetPassowrdMessage").show();
                            $(".forgetPasswordFields").hide();
                        } else {
                            $("#forgetpassword-error").show().html(data.message);
                        }
                    }
                },
                failure: function (errMsg) {
                    $("#ForgotPassSpiner").hide();
                }
            });

        }

    });

    $(".btn_formSignup").on("click", function (e) {
        if ($("#Form_Signup").valid()) {
            $("#SignUpSpinner").show();
            var data = $("#Form_Signup").serialize();
            $.ajax({
                type: "POST",
                url: "/account/SignUp",
                data: data,
                dataType: "json",
                success: function (data) {
                    if (!$.isEmptyObject(data)) {
                        $("#SignUpSpinner").hide();
                        if (data.ResultType == "Success") {
                            $("#Modal_SignupPopup").modal("hide");
                            //$("#Modal_MessageBoxTitle").text("Account Created Succesfully.!");
                            //$("#Modal_MessageBoxBody").text("Congratulation! Your account has successfully created. Please check your inbox for verification.!");
                            //$("#Modal_Messagebox").modal("show");
                            $("#Modal_LoginMessagebox").modal("show")
                        } else {
                            $("#signup-error").show().html(data.message);
                        }
                    }
                },
                failure: function (errMsg) {
                    $("#SignUpSpinner").hide();
                }
            });
        }
    });

    $(".btn_LoginMessageModal").on("click", function (e) {
        $("#Modal_LoginMessagebox").modal("hide");
        $("a.lnk_ShowLoginModal").trigger("click");
    })
})

function myFunction() {
    var x = document.getElementById("myLinks");
    if (x.style.display === "block") {
        x.style.display = "none";
    } else {
        x.style.display = "block";
    }
}

function mymg() {
    var x = document.getElementById("headermg");
    if (x.style.display === "block") {
        x.style.display = "none";
    } else {
        x.style.display = "block";
    }
}

