
/* BEGIN EXTERNAL SOURCE */

        // Auto-hide mensagem após 3 segundos
        setTimeout(function() {
            var alert = document.querySelector('.alert-custom');
            if (alert) {
                alert.style.transition = 'opacity 0.5s';
                alert.style.opacity = '0';
                setTimeout(function() {
                    alert.style.display = 'none';
                }, 500);
            }
        }, 3000);

        // Fechar modal após submit com sucesso
        window.addEventListener('load', function() {
            var modal = bootstrap.Modal.getInstance(document.getElementById('modalNovoProduto'));
            if (modal && document.querySelector('.alert-success')) {
                modal.hide();
            }
        });
    
/* END EXTERNAL SOURCE */
