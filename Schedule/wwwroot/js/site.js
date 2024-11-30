function handleSelection(select) {
    if (select.value === "Excel") {
        $('#fileSelectionModal').modal('show');
    } else {
        if (select.value) {
            select.form.submit();
        }
    }
}

function submitFiles() {
    var files = document.getElementById('fileInput').files;
    var formData = new FormData();

    for (var i = 0; i < files.length; i++) {
        formData.append('files', files[i]);
    }

    formData.append('selectedAction', document.getElementById('actionSelect').value);

    fetch('Upload', {
        method: 'POST',
        body: formData
    })
        .then(response => {
            if (response.ok) {
                $('#fileSelectionModal').modal('hide'); // скрыть первоначальное модальное окно
                $('#modeSelectionModal').modal('show'); // показать модальное окно выбора режима
            } else {
                alert('Ошибка при загрузке файлов');
            }
        })
        .catch(error => console.error('Ошибка:', error));
}

function submitMode() {
    var selectedMode = document.querySelector('input[name="mode"]:checked');

    if (selectedMode) {
        console.log('Выбранный режим:', selectedMode.value);
        $('#modeSelectionModal').modal('hide'); // скрыть модальное окно выбора режима

        // Покажем окно для ввода имён групп или преподавателей
        if (selectedMode.value === 'Student') {
            $('#nameInputSection').show();
            $('#teacherInputSection').hide();
        } else {
            $('#nameInputSection').hide();
            $('#teacherInputSection').show();
        }
        $('#inputNamesModal').modal('show'); // показать модальное окно для ввода имен
    } else {
        alert('Пожалуйста, выберите режим');
    }
}

function submitNames() {
    var mode = document.querySelector('input[name="mode"]:checked').value;
    var groupName = document.getElementById('groupInput').value;
    var teacherName = document.getElementById('teacherInput').value;

    console.log('Выбранный режим:', mode);
    if (mode === 'Student') {
        console.log('Введенная группа:', groupName);
    } else {
        console.log('Введенный преподаватель:', teacherName);
    }

    // Здесь мы можем отправить данные на сервер
    submitExcel(mode, mode === 'Student' ? groupName : teacherName);

    $('#inputNamesModal').modal('hide'); // скрыть модальное окно
}

function submitExcel(mode, param) {
    if (mode === 'Student') mode = 0;
    if (mode === 'Teacher') mode = 1;
    console.log('Отправляемые данные:', { Mode: mode, Param: param }); // Выводим в консоль

    fetch('Excel', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ Mode: mode, Param: param })
    })
        .then(response => {
            if (!response.ok) {
                return response.text().then(text => {
                    throw new Error(text || 'Ошибка при выполнении импорта');
                });
            }
            // Успешный импорт, перенаправляем на контроллер Export
            window.location.href = '/Export/ExportPreview';
        })
        .catch(error => {
            console.error('Ошибка:', error);
            alert(error.message);
        });
}