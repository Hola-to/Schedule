window.onload = function () {
    setTimeout(function () {
        document.getElementById('popup').style.display = 'block';
        document.getElementById('overlay').style.display = 'block';
    }, 500); 
}

document.getElementById('yesBtn').onclick = function () {
    window.location.href = '/Export/Index';
    document.getElementById('popup').style.display = 'none';
    document.getElementById('overlay').style.display = 'none';
}

document.getElementById('noBtn').onclick = function () {
    console.log("Кнопка 'Нет' нажата.");
    document.getElementById('popup').style.display = 'none';
    document.getElementById('overlay').style.display = 'none';
}

document.getElementById('googleForm').onsubmit = function (event) {
    event.preventDefault(); // Предотвращаем стандартное поведение формы

    fetch('Google', {
        method: 'POST'
    }).then(response => {
        if (response.ok) {
            alert('Данные успешно экспортированы');
            // Вы также можете перенаправить на другую страницу или выполнить другие действия
        } else {
            // Обработка ошибок
            return response.text().then(text => { throw new Error(text); });
        }
    }).catch(error => {
        alert('Ошибка: ' + error.message);
    });
};