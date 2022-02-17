// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//var token = getToken();
//var RequestVerificationToken = $('input[name="__RequestVerificationToken"]').val();

function confirmarExclusaoDoAnexo(id) {
    Swal.fire({
        title: 'Deseja remover o documento ?',
        text: '',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        cancelButtonText: 'Não',
        confirmButtonText: 'Sim'
    }).then((result) => {
        if (result.value) {
            excluirAnexoDeDocumento(id)
        }
    })
}

function NotificarSucesso() {
    Swal.fire({
        icon: 'success',
        title: 'Ação realizada com sucesso!',
        confirmButtonColor: '#3085d6',
        confirmButtonText: 'Ok'
    }).then((result) => {
        if (result.value) {
            window.location.reload()
        }
    })
}

function NotificarErro() {
    Swal.fire({
        icon: 'error',
        title: 'Ocorreu um erro!',
    })
}

