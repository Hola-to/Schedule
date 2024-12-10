document.addEventListener('DOMContentLoaded', () => {
    const messages = document.querySelectorAll('.message');
    let currentIndex = 0;
    const delayBetweenMessages = 2000;
    const delayAfterLastMessage = 10000;

    function showNextMessage() {
        if (currentIndex < messages.length) {
            const message = messages[currentIndex];
            message.classList.add('visible');
            currentIndex++;
            setTimeout(showNextMessage, delayBetweenMessages);
        } else {
            setTimeout(() => {
                messages.forEach(msg => msg.classList.remove('visible'));
                currentIndex = 0;
                showNextMessage();
            }, delayAfterLastMessage);
        }
    }

    showNextMessage();
});


document.addEventListener('DOMContentLoaded', () => {
    const fileUpload = document.getElementById('fileInput');
    const roleSelection = document.getElementById('role-selection');
    const additionalInfo = document.getElementById('additional-info');
    const studentInfo = document.getElementById('student-info');
    const teacherInfo = document.getElementById('teacher-info');
    const submitButton = document.getElementById('submit-button');
    const previewButton = document.getElementById('preview-button'); // Новая кнопка

    // Обработчик загрузки файла
    fileUpload.addEventListener('change', () => {
        if (fileUpload.files.length > 0) {
            roleSelection.style.display = 'block';
            additionalInfo.style.display = 'block';
            submitButton.style.display = 'block';
            previewButton.style.display = 'block'; // Показываем новую кнопку
        } else {
            roleSelection.style.display = 'none';
            additionalInfo.style.display = 'none';
            submitButton.style.display = 'none';
            previewButton.style.display = 'none'; // Скрываем новую кнопку
        }
    });

    // Обработчик выбора роли (студент или преподаватель)
    const roleRadios = document.querySelectorAll('input[name="mode"]');
    roleRadios.forEach(radio => {
        radio.addEventListener('change', () => {
            if (radio.value === 'Student') {
                studentInfo.style.display = 'block';
                teacherInfo.style.display = 'none';
            } else {
                studentInfo.style.display = 'none';
                teacherInfo.style.display = 'block';
            }
        });
    });

    // Обработчик нажатия кнопки "Загрузить"
    submitButton.addEventListener('click', async () => {
        const file = fileUpload.files[0];
        const role = document.querySelector('input[name="mode"]:checked').value;
        const groupNumber = document.getElementById('groupInput').value;
        const teacherSurname = document.getElementById('teacherInput').value;

        if (!file) {
            alert('Пожалуйста, выберите файл для загрузки.');
            return;
        }

        // Отправка файлов на сервер
        const formData = new FormData();
        formData.append('files', file);

        try {
            // Загрузка файлов
            const uploadResponse = await fetch('/Import/Upload', {
                method: 'POST',
                body: formData
            });

            if (!uploadResponse.ok) {
                const errorText = await uploadResponse.text();
                throw new Error(errorText || 'Ошибка при загрузке файлов');
            }

            // Ожидание завершения всех потоков uploadResponse
            await uploadResponse.text(); // Ожидаем завершения загрузки файлов

            Promise.all;

            console.log('Файл успешно загружен!'); // Логирование в консоль

            // Отправка данных на сервер
            const excelResponse = await fetch('/Import/Excel', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    Mode: role === 'Student' ? 0 : 1,
                    Param: role === 'Student' ? groupNumber : teacherSurname
                })
            });

            if (!excelResponse.ok) {
                const errorText = await excelResponse.text();
                throw new Error(errorText || 'Ошибка при выполнении импорта');
            }

            // Ожидание завершения всех потоков excelResponse
            await excelResponse.text(); // Ожидаем завершения импорта данных

            Promise.all;

            console.log('Данные успешно импортированы!'); // Логирование в консоль

            // Удаление папки UploadedFiles перед переходом на другую страницу
            const deleteResponse = await fetch('/Import/DeleteUploadedFiles', {
                method: 'POST'
            });

            if (!deleteResponse.ok) {
                const errorText = await deleteResponse.text();
                throw new Error(errorText || 'Ошибка при удалении папки UploadedFiles');
            }


            // Перенаправление на страницу предварительного просмотра
            window.location.href = '/Export/ExportPreview';
        } catch (error) {
            console.error('Ошибка:', error);
        }
    });
});