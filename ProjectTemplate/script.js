var contentPanels = ['logonPanel', 'homePanel'];
var testName = "test";
var testPass = "test";

var sessionUsername = "";
var profilesArray;
var uid = "";


// This function will be used when using database credentials
function LogOn(username, pass) {
    var webMethod = "ProjectServices.asmx/LogOn";
    var parameters = "{\"uid\":\"" + encodeURI(username) + "\",\"pass\":\"" + encodeURI(pass) + "\"}";

    $.ajax({
        type: "POST",
        url: webMethod,
        data: parameters,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            //document.getElementById("error").innerHTML = "";
            var responseFromServer = msg.d;
            if (responseFromServer == true) {
                sessionUsername = document.getElementById("logonId").value;
                setCookie("username", sessionUsername, 1);
                alert("success");
                alert(sessionUsername);
                window.open("home.html", "_self");
            }
           else {
                //document.getElementById("error").innerHTML = "Incorrect Credentials";
                alert("wrong credentials")
            }
        },
        error: function () {
            alert("Error");
        }
    })

}

function showPanel(panelId) {
    for (var i = 0; i < contentPanels.length; i++) {
        if (panelId == contentPanels[i]) {
            $("#" + contentPanels[i].css('visibility', 'visible'));
        }
        else {
            $("#" + contentPanels[i].css('visibility', 'hidden'));
        }
    }
}

function welcome(name) {

    document.getElementById(id).innerHTML = "Welcome, " + name + "!";
}


function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

function checkCookie() {
    var user = getCookie("username");
    if (user != "") {
        alert("Welcome again " + user);
    } else {
        user = prompt("Please enter your name:", "");
        if (user != "" && user != null) {
            setCookie("username", user, 365);
        }
    }
} 

function test() {
    var sessionUsername = getCookie('username');
    document.getElementById('welcome').innerHTML = "Welcome, " + sessionUsername + "!";
}

//this function grabs accounts and loads our account window
function getProfiles() {
    var webMethod = "ProjectServices.asmx/getProfiles";
    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            if (msg.d.length > 0) {
                profilesArray = msg.d;
            }
        },
        error: function (e) {
            alert("Error");
        }
    });
}

function logOff() {
    sessionUsername = "";
    window.open("index.html", "_self");
}

function getUID(uName)
{
    var webMethod = "ProjectServices.asmx/GetUserId";
    var parameters = "{\"uName\":\"" + encodeURI(uName) + "\"}"
    alert(parameters);
    $.ajax({
        type: "POST",
        url: webMethod,
        data: parameters,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            if (msg.d.length > 0) {
                uid = msg.d;
            }
        },
        error: function (e) {
            alert("Error");
        }
    });
}


function displayPic() {
    var sessionUsername = getCookie('username');
    var element = document.getElementById("PicUrl");
    getUID(sessionUsername);
    getProfiles();
    for (var i = 0; i < profilesArray.length; i++) {
        if (uid == profilesArray[i].uid) {
            element.src = profilesArray[i].url;
        }
    }
}

function updateProfile() {
    var sessionUsername = getCookie('username');
    getUID(sessionUsername);
    var webMethod = "ProjectServices.asmx/updateProfile";
    var parameters = "{\"description\":\"" + encodeURI(document.getElementById("description").innerText) + "\",\"company\":\"" + encodeURI(document.getElementById("companyName").innerHTML) +
        "\",\"education\":\"" + encodeURI(document.getElementById("education").innerHTML) + "\",\"url\":\"" + encodeURI(document.getElementById("picture").innerText) + "\",\"uid\":\"" + encodeURI(uid) + "\"}";
    alert(parameters);

    $.ajax({

        type: "POST",        url: webMethod,
        data: parameters,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function () {
            alert("Updated Profile");
            window.open("home.html", "_self");
        },
        error: function (e) {
            alert("failed to update profile.")
        }
    });
}
      
   









