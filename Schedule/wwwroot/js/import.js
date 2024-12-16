document.addEventListener('DOMContentLoaded', () => {
    const fileUpload = document.getElementById('fileInput');
    const roleSelection = document.getElementById('role-selection');
    const additionalInfo = document.getElementById('additional-info');
    const studentInfo = document.getElementById('student-info');
    const teacherInfo = document.getElementById('teacher-info');
    const submitButton = document.getElementById('submit-button');

    // Обработчик загрузки файла
    fileUpload.addEventListener('change', () => {
        if (fileUpload.files.length > 0) {
            roleSelection.style.display = 'block';
            additionalInfo.style.display = 'block';
            submitButton.style.display = 'block';

            __UploadFiles__();
        } else {
            roleSelection.style.display = 'none';
            additionalInfo.style.display = 'none';
            submitButton.style.display = 'none';
        }
    });

    async function __UploadFiles__() {
        const files_ = fileUpload.files; // Получаем все выбранные файлы

        if (!files_ || files_.length === 0) {
            alert('Пожалуйста, выберите файлы для загрузки.');
            return;
        }

        // Отправка файлов на сервер
        const formData = new FormData();

        // Добавляем каждый файл в FormData с ключом '_files'
        for (let i = 0; i < files_.length; i++) {
            formData.append('_Files', files_[i]);
        }

        try {
            const uploadResponse = await fetch('/Import/Upload', {
                method: 'POST',
                body: formData
            });

            if (!uploadResponse.ok) {
                throw new Error('Ошибка загрузки файлов');
            }

            Promise.all;
            console.log('Файлы успешно загружены!');

            const excelUploadResponse = await fetch('/Import/ExcelUpload', {
                method: 'POST',
            });

            if (!excelUploadResponse.ok) {
                throw new Error('Ошибка обработки Excel');
            }

            console.log('Excel успешно обработан!');
        } catch (error) {
            console.error('Ошибка:', error);
        }
    }

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
        const role = document.querySelector('input[name="mode"]:checked').value;
        const groupNumber = document.getElementById('groupInput').value;
        const teacherSurname = document.getElementById('teacherInput').value;

        try {
            // Создаем массив промисов для ожидания
            const promises = [];

            // Отправка данных на сервер
            const excelResponse = await fetch('/Import/ExcelFind', {
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
                throw new Error('Ошибка импорта данных');
            }

            promises.push(excelResponse);
            console.log('Данные успешно импортированы!');

            const deleteResponse = await fetch('/Import/DeleteUploadedFiles', {
                method: 'POST'
            });

            // Ожидаем выполнения всех промисов
            await Promise.all(promises);

            // Перенаправление на страницу предварительного просмотра
            window.location.href = '/Export/ExportPreview';

        } catch (error) {
            console.error('Ошибка:', error);
        }
    });
});