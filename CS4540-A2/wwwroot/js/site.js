var names;
var address;
//This search box function call in JS
//when people start searching 
function search_function() {
    var check_input = document.getElementById("search-button").value;
    if (check_input != "") {
        $.ajax({
            url: "/Home/search_class",
            type: 'GET',
            data: {
                text: check_input
            },
            success: function (res) {
                console.log(res.add + "test");
                window.location.href = res.add;
            },
            error: function () {
                console.log("test111");

            }
        });

        console.log("test1");
    }
}
//Update the name and adddress 
$.ajax({
    url: "/Home/Update"
}).done(function (res) {
    names = res.names;
    address = res.address;

});
//resourse:https://tarekraafat.github.io/autoComplete.js/#/
//auto-complete function
//when people start searching, there is small algorithm for
// auto-complete that information that this professor'courses provide in db
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
    }
});


