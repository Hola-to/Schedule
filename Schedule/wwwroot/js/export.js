// Показ всплывающего окна
document.getElementById('exportBtn').addEventListener('click', function () {
    showPopup();
});

// Нажатие на кнопку "Да"
document.getElementById('yesBtn').addEventListener('click', function () {
    fetch('/api/export', { method: 'POST' })
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

// Закрытие всплывающего окна
document.getElementById('noBtn').addEventListener('click', closePopup);
document.getElementById('overlay').addEventListener('click', closePopup);

// Функции управления всплывающим окном
function showPopup() {
    document.getElementById('popup').style.display = 'block';
    document.getElementById('overlay').style.display = 'block';
}

function closePopup() {
    document.getElementById('popup').style.display = 'none';
    document.getElementById('overlay').style.display = 'none';
}