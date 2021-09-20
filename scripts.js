$(document).ready(function () {
    TextareaMaxLengthHelper();


    if (typeof $.validator !== 'undefined') {
        AspnetRequiredCheckBoxValidation();
        AspnetFileTypeValidation();
    }

    //bootstrap
    BindBootstrapDatePicker();
    BindBootstrapTooltip();
    BindBootstrapFileInput();
});


//calculate the characters remaining in a textarea
function TextareaMaxLengthHelper() {
    $('textarea').keyup(function () {
        var $this = $(this);
        if ($this.attr('maxlength') === '')
            return;

        var maxLength = parseInt($this.attr('maxlength'));
        var contentLength = $this.val().length;
        if (contentLength > maxLength) {
            $this.val($this.val().substring(0, maxLength));
        }

        $this.closest('div').find('.textarea-remain-value').html(maxLength - contentLength);
    });
}


//the jquery validation extension for rquired checkbox
function AspnetRequiredCheckBoxValidation() {
    defaultRangeValidator = $.validator.methods.range;
    $.validator.methods.range = function (value, element, param) {
        if (element.type === 'checkbox') {
            return element.checked;
        }

        return defaultRangeValidator.call(this, value, element, param);
    };
}


//the jquery validation extension for custom file type properties
function AspnetFileTypeValidation() {
    $.validator.unobtrusive.adapters.add('filetype', ['validtypes'], function (options) {
        options.rules['filetype'] = { validtypes: options.params.validtypes.split(',') };
        options.messages['filetype'] = options.message;
    });

    $.validator.addMethod("filetype", function (value, element, param) {
        for (var i = 0; i < element.files.length; i++) {
            var extension = GetFileExtension(element.files[i].name);
            if ($.inArray(extension, param.validtypes) === -1) {

                setTimeout(function () {
                    var $element = $('#' + element.id);
                    var $span = $element.closest('.form-group').find('.field-validation-error span');
                    $span.html($span.html().replace('EXT', extension));
                }, 5);

                return false;
            }
        }
        return true;
    });
}

function GetFileExtension(fileName) {
    if (/[.]/.exec(fileName)) {
        return /[^.]+$/.exec(fileName)[0].toLowerCase();
    }

    return null;
}


//=== bootstrap specific ==============================================


//bind the bootstrap datepicker
function BindBootstrapDatePicker() {
    $('.bootstrap-datepicker').datepicker({
        dateFormat: 'dd-mm-yyyy',
        autoclose: true,
        todayHighlight: true
    });
}


//bind the bootstrap tooltips
function BindBootstrapTooltip() {
    $('body').tooltip({
        selector: '[data-toggle=tooltip]',
        boundary: 'window',
        trigger: 'hover',
        html: true,
        delay: {
            show: 250,
            hide: 0
        }
    });
}


//show the selected file(s) in the file input
function BindBootstrapFileInput() {
    $('.custom-file-input').change(function () {
        var $this = $(this);
        var files = [];
        for (var i = 0; i < $this[0].files.length; i++) {
            files.push($this[0].files[i].name);
        }

        $this.next('.custom-file-label').html(files.join(', '));
    });
}