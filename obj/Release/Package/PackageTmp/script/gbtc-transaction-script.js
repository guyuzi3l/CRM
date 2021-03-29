
$('document').ready(function () {
    
    $('.datepicker').datepicker({
        format: 'yyyy-mm-dd'
    });

    $('.table-responsive').on('show.bs.dropdown', function () {
        $('.table-responsive').css("overflow", "inherit");
    });

    $('.table-responsive').on('hide.bs.dropdown', function () {
        $('.table-responsive').css("overflow", "auto");
    });

    $('.change-transaction-status').click(function () {
        $(this).prop('disabled', true);
        var d = { tid: $(this).data('transaction-id') };
        var cid = { cid: $(this).data('client-id') };
        console.log(cid);
        $("#pspModal").modal();
        $('#myModal').on('hidden.bs.modal', function (e) {
            $(this).prop('disabled', false);
        });

        $('#selectPspStatusBtn').click(function () {

            var postdata = Object.assign({}, d, { selectedStatus: $('#selectPspStatus').val() }, cid);
            $("#pspModal").modal('toggle');
            $("#loader").show();
            $.ajax({
                type: "POST",
                url: '/transaction-lists.aspx/ChangeTransactionStatus',
                data: JSON.stringify(postdata),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    processResponse(response);

                },
                failure: function (response) {
                    console.log(response);

                }
            });
        });

    });

    $('.change-credited-transaction-status').click(function () {

        var d = { tid: $(this).data('transaction-id') };
        $("#credTransactionModal").modal();


        $('#selectCredStatusBtn').click(function () {

            var postdata = Object.assign({}, d, { selectedStatus: $('#creditedStatusSelected').val() });
            $("#credTransactionModal").modal('toggle');

            $("#loader").show();

            $.ajax({
                type: "POST",
                url: '/transaction-lists.aspx/ChangeCreditedTransactionStatus',
                data: JSON.stringify(postdata),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    processResponse(response);
                },
                failure: function (response) {
                    console.log(response);
                }
            });
        });

    });

    $('.change-transaction-note-ref').click(function () {

        var d = { tid: $(this).data('transaction-id') };
        $("#txEditNoteRefModal").modal();
        console.log(d);
        $('#EditNoteRefBtn').click(function () {

            var postdata = Object.assign({}, d, { note: $('#note').val(), ref_hash: $('#reff').val() });
            $("#txEditNoteRefModal").modal('toggle');

            $("#loader").show();

            $.ajax({
                type: "POST",
                url: '/transaction-lists.aspx/ChangeNoteAndReff',
                data: JSON.stringify(postdata),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    processResponse(response);
                },
                failure: function (response) {
                    console.log(response);
                }
            });
        });

    });

    $('.class-change-ref').click(function () {

        var d = { id: $(this).data('wd-id') };
        $("#wdEditPayRefModal").modal();
        console.log(d);
        $('#UpdateWDReferrence').click(function () {
            var postdata = Object.assign({}, d, { ref_hash: $('#reference_hash').val() });
            $("#wdEditPayRefModal").modal('toggle');

            $("#loader").show();

            $.ajax({
                type: "POST",
                url: '/client-detail.aspx/EditReference',
                data: JSON.stringify(postdata),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    processResponse(response);
                },
                failure: function (response) {
                    console.log(response);
                }
            });
        });

    });

});

function processResponse(res) {
    if (res.d.Success == true) {
        location.reload();
        return;
    }
    $.alert('An error occurred while processing your request.');
}