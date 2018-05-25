
    jQuery('.datapicker').datetimepicker({
        datepicker: false,
        format: 'H:i'
    });

$('.shadule :checkbox').on('change', function () {
    if (this.checked) {
        this.value = true;
        var dispalyclass = this.className;
        $("." + dispalyclass).css("display", "none");
        $(this).css("display", "initial");

    } else {
        this.value = false;
        var dispalyclass = this.className;
        $("." + dispalyclass).css("display", "initial");
    }
});
