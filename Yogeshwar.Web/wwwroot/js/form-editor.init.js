var ckClassicEditor = document.querySelectorAll(".ckeditor-classic"),
    snowEditor =
        (ckClassicEditor &&
            Array.from(ckClassicEditor).forEach(function () {
                ClassicEditor.create(document.querySelector(".ckeditor-classic"), {
                    removePlugins: ['CKFinderUploadAdapter', 'CKFinder', 'EasyImage', 'Image',
                        'ImageCaption', 'ImageStyle', 'ImageToolbar', 'ImageUpload', 'MediaEmbed']
                })
                    .then(function (e) {
                        console.log(e.plugins._availablePlugins);
                        e.ui.view.editable.element.style.height = "200px";
                    })
                    .catch(function (e) {
                        console.error(e);
                    });
            })
        );
