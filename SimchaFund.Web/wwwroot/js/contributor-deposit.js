$(() => {
    $('.deposit-button').on('click', function () {
        const contribId = $(this).data('contribid');
        $('[name="contributorId"]').val(contribId);
        const button = $(this);
        const tr = button.closest('tr');
        const name = tr.data('person');
        $('#deposit-name').text(name);
        $('.deposit').modal();
    })

    $('#new-contributor').on('click', function () {
        $('.new-contrib').modal();
    })

    $('.edit-contributor').on('click', function () {
        const id = $(this).data('id');
        const firstName = $(this).data('firstName');
        const lastName = $(this).data('lastName');
        const cellNumber = $(this).data('cell');
        const createdDate = $(this).data('date');
        const alwaysInclude = $(this).data('alwaysInclude');
        console.log(alwaysInclude);
        $('#contributor_id').val(id);
        $('#contributor_first_name').val(firstName);
        $('#contributor_last_name').prop('value', lastName);
        $('#contributor_cell_number').val(cellNumber);
        $('#contributor_created_at').val(createdDate);
        $('#contributor_always_include').prop('checked', alwaysInclude == "True")


        $('.edit-contrib').modal();        
    })
});