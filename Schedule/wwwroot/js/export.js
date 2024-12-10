document.addEventListener('DOMContentLoaded', function () {
    // Объявляем функцию showPopup
    function showPopup() {
        document.getElementById('popup').style.display = 'block';
        document.getElementById('overlay').style.display = 'block';
    }

    // Объявляем функцию closePopup
    function closePopup() {
        document.getElementById('popup').style.display = 'none';
        document.getElementById('overlay').style.display = 'none';
    }

    // Назначаем обработчики событий
    document.getElementById('exportBtn').addEventListener('click', showPopup);
    document.getElementById('yesBtn').addEventListener('click', function () {
        closePopup();

        fetch('/Export/Google', { method: 'POST' })
            .then(response => {
                if (response.ok) {
                    alert('Экспорт выполнен успешно!');
                } else {
                    return response.text().then(text => { throw new Error(text); });
                }
            })
            .catch(error => {
                alert('Ошибка: ' + error.message);
            })
            .finally(() => {
                closePopup();
            });
    });
    document.getElementById('noBtn').addEventListener('click', closePopup);
    document.getElementById('overlay').addEventListener('click', closePopup);
});