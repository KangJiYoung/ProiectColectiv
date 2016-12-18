
var ConfirmDialog = {

    callback: null,

    init: function (container) {
        var modal = $('#confirm-modal-dialog');

        if (!container)
            container = $('#content');

        container.find('[data-confirm]').each(function () {
            var btn = $(this);
            if (btn.attr("onclick")) {
                btn.data("confirmCallback", btn.attr("onclick"));
                btn.removeAttr("onclick");
            }
        }).click(function (event) {

            event.preventDefault();

            var btn = $(this);
            ConfirmDialog.callback = btn.data("confirmCallback");
            console.log(btn.data("confirmTitle"));
            modal.find('.modal-header .modal-title').text(btn.data("confirm-title"));
            modal.find('.modal-body').text(btn.data("confirm"));
            modal.find('.modal-header small').text(btn.data("confirm-descripiton"));

            modal.modal();
        });

        modal.find('.btn-primary').click(function () {
            modal.modal('hide');
            if (ConfirmDialog.callback) {
                if (typeof ConfirmDialog.callback === "function")
                    ConfirmDialog.callback();
                else if (typeof ConfirmDialog.callback === "string")
                    eval(ConfirmDialog.callback);
            }
            ConfirmDialog.callback = null;
        })
    }
}