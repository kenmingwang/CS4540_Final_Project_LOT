var names;
var address;
function search_function() {
    var check_input = document.getElementById("search-button").value;
    if (check_input != "") {
        $.ajax({
            //   url: "/Home/searchingPro",
            //   data:{

            //   input: check_input
            //    },
            //}
            //   method: e.srcElement.method

        }).done(function (res) {
            console.log("action taken: " + res);
            if (check_input == 4400)
                window.location.href = "/Courses/PastCourses/4400";
            else if (check_input == 3500)
                window.location.href = "/Courses/PastCourses/3500";
            else if (check_input == 3505)
                window.location.href = "/Courses/PastCourses/3505";
            else if (check_input == 2100)
                window.location.href = "/Courses/PastCourses/2100";
            else if (check_input == 4540)
                window.location.href = "/Courses/PastCourses/4540";
            else if (check_input == 2420)
                window.location.href = "/Courses/PastCourses/2420";
            else {
                alert("No such this course");
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            console.log("failed: ");
            console.log(jqXHR);
            console.log(textStatus);
            console.log(errorThrown);
            alert("Warning");

        });
    }

}
$.ajax({
    url: "/Home/Update"
}).done(function (res) {
    names = res.names;
    address = res.address;

});
$('#search-button').autoComplete({
    minChars: 1,
    source: function (item, hint) {
        item = item.toUpperCase();
        var c = names;
        var match = [];
        for (i = 0; i < c.length; i++)
            if (~c[i].toUpperCase().indexOf(item))
                match.push(c[i]);
        match.sort();
        hint(match);
    },
    onSelect: function (e, term, item) {
        $.ajax({


        }).done(function (res) {
            console.log("action take: " + res);
            window.location.href = address[names.indexOf(term)];


        }).fail(function (jqXHR, textStatus, errorThrown) {
            console.log("failed: ");
            console.log(jqXHR);
            console.log(textStatus);
            console.log(errorThrown);
            alert("Warning");

        });


    }


});