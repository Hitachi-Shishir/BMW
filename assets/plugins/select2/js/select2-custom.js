$(function () {
    "use strict";

    // For single-select field
    $('.single-select-field').select2({
        theme: "bootstrap-5",
        width: function () {
            return $(this).data('width') ? $(this).data('width') : $(this).hasClass('w-100') ? '100%' : 'style';
        },
        placeholder: function () {
            return $(this).data('placeholder');
        },
    });

    // For single-select optgroup field
    $('.single-select-optgroup-field').select2({
        theme: "bootstrap-5",
        width: function () {
            return $(this).data('width') ? $(this).data('width') : $(this).hasClass('w-100') ? '100%' : 'style';
        },
        placeholder: function () {
            return $(this).data('placeholder');
        },
    });

    // For single-select clear field
    $('.single-select-clear-field').select2({
        theme: "bootstrap-5",
        width: function () {
            return $(this).data('width') ? $(this).data('width') : $(this).hasClass('w-100') ? '100%' : 'style';
        },
        placeholder: function () {
            return $(this).data('placeholder');
        },
        allowClear: true
    });

    // For multiple-select field
    $('.multiple-select-field').select2({
        theme: "bootstrap-5",
        width: function () {
            return $(this).data('width') ? $(this).data('width') : $(this).hasClass('w-100') ? '100%' : 'style';
        },
        placeholder: function () {
            return $(this).data('placeholder');
        },
        closeOnSelect: false,
    });

    // For multiple-select clear field
    $('.multiple-select-clear-field').select2({
        theme: "bootstrap-5",
        width: function () {
            return $(this).data('width') ? $(this).data('width') : $(this).hasClass('w-100') ? '100%' : 'style';
        },
        placeholder: function () {
            return $(this).data('placeholder');
        },
        closeOnSelect: false,
        allowClear: true,
    });

    // Example for additional custom select elements like prepends, appends, etc.
    $('.prepend-text-single-field').select2({
        theme: "bootstrap-5",
        width: function () {
            return $(this).data('width') ? $(this).data('width') : $(this).hasClass('w-100') ? '100%' : 'style';
        },
        placeholder: function () {
            return $(this).data('placeholder');
        },
    });

    $('.small-bootstrap-class-single-field').select2({
        theme: "bootstrap-5",
        width: function () {
            return $(this).data('width') ? $(this).data('width') : $(this).hasClass('w-100') ? '100%' : 'style';
        },
        placeholder: function () {
            return $(this).data('placeholder');
        },
        dropdownParent: $(this).parent(),
    });

    // Apply the same pattern to other fields...
});
