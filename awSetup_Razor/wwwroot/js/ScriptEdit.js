var toolbarOptions = [
    [{ 'font': [] }],
    [{ 'header': [1, 2, 3, 4, 5, 6, false] }],

    ['bold', 'italic', 'underline'],        // toggled buttons

    [{ 'list': 'ordered' }, { 'list': 'bullet' }],
    [{ 'indent': '-1' }, { 'indent': '+1' }],          // outdent/indent

    [{ 'color': [] }, { 'background': [] }],          // dropdown with defaults from theme

    ['clean']                                         // remove formatting button
];

var quill = new Quill('#editor', {
    modules: {
        toolbar: toolbarOptions
    },
    theme: 'snow'
});
document.querySelector('.ql-editor').innerHTML = document.getElementById("Scripts_MessageScript").value;

quill.on('text-change', function (delta, olddelta, source) {
    var container = document.querySelector('.ql-editor');
    var newtext = container.innerHTML;
    document.getElementById("Scripts_MessageScript").value = container.innerHTML;
});
