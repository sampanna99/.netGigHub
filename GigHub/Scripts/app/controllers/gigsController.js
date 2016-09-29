var GigsController = function (attendanceService) {
    var button;
    var init = function (container) {
        $(container).on("click", ".js-toggle-attendance", toggleAttendance);
        //$(".js-toggle-attendance").click(toggleAttendance); only works for element in the dom. if implement sth like load more isn't good
    };
    var toggleAttendance = function (e) {
        button = $(e.target);
        gigId = button.attr("data-gig-id")

        if (button.hasClass("btn-default"))
            attendanceService.createAttendance(gigId, done, fail);
        else
            attendanceService.deleteAttendance(gigId, done, fail);
    };



    var done = function () {
        var text = (button.text() == "Going") ? "Going?" : "Going";

        button.toggleClass("btn-info").toggleClass("btn-default").text(text);
    };


    var fail = function () {
        alert("something failed");
    };

    return {
        init: init
    }

}(AttendanceService);