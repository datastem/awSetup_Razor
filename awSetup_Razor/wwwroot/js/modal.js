$(function () {
    var placeholderElement = $('#modal-placeholder');

    $(document).on('click', '[data-toggle="ajax-modal"]', function (event) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            placeholderElement.html(data);
            placeholderElement.find('[data-dismiss="modal"]').show();
            placeholderElement.find('[data-save="modal"]').show();
            placeholderElement.find('[data-saving="modal"]').hide();
            placeholderElement.find('.modal').modal('show');
        });
    });

    placeholderElement.on('click', '[data-save="modal"]', function (event) {
        event.preventDefault();

        placeholderElement.find('[data-dismiss="modal"]').hide();
        placeholderElement.find('[data-save="modal"]').hide();
        placeholderElement.find('[data-saving="modal"]').show();

        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var dataToSend = form.serialize();

        $.post(actionUrl, dataToSend).done(function (data) {
            var newBody = $('.modal-body', data);
            placeholderElement.find('.modal-body').replaceWith(newBody);

            var isValid = newBody.find('[name="IsValid"]').val() == 'True';
            if (isValid) {
                var notificationsPlaceholder = $('#notification');
                var notificationsUrl = notificationsPlaceholder.data('url');
                $.get(notificationsUrl).done(function (notifications) {
                    notificationsPlaceholder.html(notifications);
                });

                var tableElement = $('table[data-url]');
                var tableUrl = tableElement.data('url');
                $.get(tableUrl).done(function (table) {
                    tableElement.replaceWith(table);
                });

                placeholderElement.find('.modal').modal('hide');
            }
            else {
                placeholderElement.find('[data-dismiss="modal"]').show();
                placeholderElement.find('[data-save="modal"]').show();
                placeholderElement.find('[data-saving="modal"]').hide();
            }
        });
    });
});