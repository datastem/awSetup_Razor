$(document).ready(function () {
    $("input[type=checkbox]").click(function (event) {
        var dowcheckbox = event.target;
        var dowid = dowcheckbox.id.split("__");
        var starttimeid = dowid[0] + "__StartTime";
        var endtimeid = dowid[0] + "__EndTime";
        var starttimeobj = document.getElementById(starttimeid);
        var endtimeobj = document.getElementById(endtimeid);
        if (dowcheckbox.checked) {
            var d = new Date();
            d.setHours(8, 0);
            starttimeobj.value = ("0" + d.getHours()).slice(-2) + ":" + ("0" + d.getMinutes()).slice(-2);
            d.setHours(17, 0);
            endtimeobj.value = ("0" + d.getHours()).slice(-2) + ":" + ("0" + d.getMinutes()).slice(-2);
        }
        else {
            starttimeobj.value = "";
            endtimeobj.value = "";
        }
        starttimeobj.disabled = !dowcheckbox.checked;
        endtimeobj.disabled = !dowcheckbox.checked;
    });
});

function VoiceScheduleCopy(defaultStartTime, defaultEndTime) {
    for (i = 0; i < 7; i++) {
        var dowswitch = document.getElementById("z" + i.toString() + "__DowSwitch").checked;
        if (dowswitch) {
            var starttimeobj = document.getElementById("z" + i.toString() + "__StartTime");
            starttimeobj.value = defaultStartTime;

            var endtimeobj = document.getElementById("z" + i.toString() + "__EndTime");
            endtimeobj.value = defaultEndTime;
        }
    }
}