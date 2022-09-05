const toolbarOptions = [
    [{ 'header': [1, 2, 3, 4, 5, 6, false] }],
    ['bold', 'italic', 'underline'],        // toggled buttons
    //['blockquote', 'code-block'],

    [{ 'header': 1 }, { 'header': 2 }],               // custom button values
    [{ 'list': 'ordered' }, { 'list': 'bullet' }],
    //[{'script': 'sub'}, {'script': 'super'}],      // superscript/subscript
    //[{'indent': '-1'}, {'indent': '+1'}],          // out-dent/indent
    //[{'direction': 'rtl'}],                         // text direction

    //[{'size': ['small', false, 'large', 'huge']}],  // custom dropdown


    [{ 'color': [] }, { 'background': [] }],          // dropdown with defaults from theme
    //[{'font': []}],
    [{ 'align': [] }],

    ['clean']                                         // remove formatting button
];

const quill = new Quill('#editor-container', {
    modules: {
        toolbar: toolbarOptions
    },
    placeholder: 'Place for your flashcard...',
    theme: 'snow'
});
quill.on('text-change', innerHTML => {
    document.getElementById("quill_html").value = quill.root.innerHTML;
});
