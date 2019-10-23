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
        var prefix = "ScriptSchedules_" + i.toString();
        var valid = document.getElementById(prefix + "__IsActive");
        var isactive = document.getElementById(prefix + "__IsActive").checked;
        if (isactive) {
            var starttimeobj = document.getElementById(prefix + "__StartTime");
            starttimeobj.value = defaultStartTime;

            var endtimeobj = document.getElementById(prefix + "__EndTime");
            endtimeobj.value = defaultEndTime;
        }
    }
}

function ScheduleCopy(defaultStartTime) {
    for (i = 0; i < 7; i++) {
        var prefix = "ScriptSchedules_" + i.toString();
        var valid = document.getElementById(prefix + "__IsActive");
        var isactive = document.getElementById(prefix + "__IsActive").checked;
        if (isactive) {
            var starttimeobj = document.getElementById(prefix + "__StartTime");
            starttimeobj.value = defaultStartTime;
        }
    }
}