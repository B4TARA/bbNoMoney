﻿
// Grade script
async function getGradeLayout(employeeId) {
    try {
        // Выполняем fetch запрос
        let response = await fetch(`/Employee/GetGradeLayout?employeeId=${encodeURIComponent(employeeId)}`);

        // Получаем текстовый HTML-контент из ответа
        let htmlContent = await response.text();

        // Вставляем HTML-контент в элемент с id 'gradeLayout'
        document.getElementById('gradeLayout').innerHTML = htmlContent;

        // Получаем типы количественных оценок
        let response2 = await fetch(`/Grade/GetGradeTypes?employeeId=${encodeURIComponent(employeeId)}`);

        // Получаем JSON-ответ
        let jsonResponse = await response2.json();

        // Проверка на результат ответа
        if (!jsonResponse.success) {
            alert(jsonResponse.message);
            return;
        }

        // Извлекаем данные из ответа
        const gradeTypes = jsonResponse.data;

        // Если есть типы оценок, то подгружаем самый первый
        if (gradeTypes && gradeTypes.length > 0) {
            await getGradeType(employeeId, gradeTypes[0].id);
        }

    } catch (error) {
        console.error('Произошла ошибка:', error);
        alert('Не удалось выполнить действие. Попробуйте снова.');
    }
}

async function getGradeType(employeeId, gradeTypeId) {
    try {
        // Выполняем fetch запрос
        let response = await fetch(`/Employee/GetGradeType?employeeId=${encodeURIComponent(employeeId)}&gradeTypeId=${encodeURIComponent(gradeTypeId)}`);

        // Получаем текстовый HTML-контент из ответа
        let htmlContent = await response.text();

        // Вставляем HTML-контент в элемент с id 'gradeType'
        document.getElementById('gradeType').innerHTML = htmlContent;

        // Получаем последнюю количественную оценку данного типа
        let response2 = await fetch(`/Grade/GetLastGradeId?employeeId=${encodeURIComponent(employeeId)}&gradeTypeId=${encodeURIComponent(gradeTypeId)}`);

        // Получаем JSON-ответ
        let jsonResponse = await response2.json();

        // Проверка на результат ответа
        if (!jsonResponse.success) {
            alert(jsonResponse.message);
            return;
        }
        else if (jsonResponse.statusCode == 0) {
            return;
        }

        // Извлекаем данные из ответа
        const lastGradeId = jsonResponse.data;

        await getGradeInfo(lastGradeId, true);

    } catch (error) {
        console.error('Произошла ошибка:', error);
        alert('Не удалось выполнить действие. Попробуйте снова.');
    }
}

async function getGradeInfo(id, isClickable) {
    try {
        let url;

        // Определяем, какой URL использовать в зависимости от значения isClickable
        if (isClickable) {
            url = `/Employee/GetGrade?id=${encodeURIComponent(id)}`;
        } else {
            url = '/image/EmptyState.png'; // путь к изображению
        }

        // Выполняем fetch запрос
        let response = await fetch(url);

        // Если это SVG или изображение, обрабатываем его как Blob
        if (!isClickable && url.endsWith('.svg') || url.endsWith('.png')) {
            let blob = await response.blob();
            let objectURL = URL.createObjectURL(blob);
            document.getElementById('gradeInfo').innerHTML = `
            <div class= "empty-grade-wrapper">
            <img src="${objectURL}" alt="undefined page" class="empty-grade">
            </div>
            `;
        } else {
            // Если это HTML-контент
            let htmlContent = await response.text();
            document.getElementById('gradeInfo').innerHTML = htmlContent;
        }
    } catch (error) {
        console.error('Произошла ошибка:', error);
        alert('Не удалось выполнить действие. Попробуйте снова.');
    }
}



// Assessment script
async function getAssessmentLayout(employeeId) {
    try {
        // Выполняем fetch запрос
        let response = await fetch(`/Employee/GetAssessmentLayout?employeeId=${encodeURIComponent(employeeId)}`);

        // Получаем текстовый HTML-контент из ответа
        let htmlContent = await response.text();

        // Вставляем HTML-контент в элемент с id 'assessmentLayout'
        document.getElementById('assessmentLayout').innerHTML = htmlContent;

        await getColleaguesAssessment(employeeId);
    } catch (error) {
        console.error('Произошла ошибка:', error);
        alert('Не удалось выполнить действие. Попробуйте снова.');
    }
}

async function getColleaguesAssessment(employeeId) {
    try {
        // Устанавливаем активные классы
        let colleaguesAssessmentLinkItem = document.getElementById('colleagues_assessment_link_item');
        let selfAssessmentLinkItem = document.getElementById('self_assessment_link_item');

        selfAssessmentLinkItem.classList.remove("active");
        colleaguesAssessmentLinkItem.classList.add("active");

        // Выполняем fetch запрос
        let response = await fetch(`/Employee/GetColleaguesAssessment?employeeId=${encodeURIComponent(employeeId)}`);

        // Получаем текстовый HTML-контент из ответа
        let htmlContent = await response.text();

        // Вставляем HTML-контент в элемент с id 'infoblock_main_container'
        document.getElementById('infoblock_main_container').innerHTML = htmlContent;
    } catch (error) {
        console.error('Произошла ошибка:', error);
        alert('Не удалось выполнить действие. Попробуйте снова.');
    }
}

async function getSelfAssessment(employeeId, assessmentId) {
    try {
        // Обновляем классы для активной/неактивной ссылки
        let colleaguesAssessmentLinkItem = document.getElementById('colleagues_assessment_link_item');
        let selfAssessmentLinkItem = document.getElementById('self_assessment_link_item');

        colleaguesAssessmentLinkItem.classList.remove("active");
        selfAssessmentLinkItem.classList.add("active");

        // Выполняем fetch запрос
        let response = await fetch(`/Employee/GetSelfAssessmentLayout?employeeId=${encodeURIComponent(employeeId)}`);

        // Получаем текстовый HTML-контент из ответа
        let htmlContent = await response.text();

        // Вставляем HTML-контент в элемент с id 'infoblock_main_container'
        document.getElementById('infoblock_main_container').innerHTML = htmlContent;

        // Получаем типы количественных оценок
        let response2 = await fetch(`/Assessment/GetLastAssessments?employeeId=${encodeURIComponent(employeeId)}`);

        // Получаем JSON-ответ
        let jsonResponse = await response2.json();

        // Проверка на результат ответа
        if (!jsonResponse.success) {
            alert(jsonResponse.message);
            return;
        }

        // Извлекаем данные из ответа
        const lastAssessments = jsonResponse.data;

        // Если есть типы оценок, то подгружаем самый первый
        if (lastAssessments && lastAssessments.length > 0) {

            if (assessmentId !== undefined) {
                await getAssessment(employeeId, assessmentId);
            }
            else {
                await getAssessment(employeeId, lastAssessments[0].id);
            }
        }        
    } catch (error) {
        console.error('Произошла ошибка:', error);
        alert('Не удалось выполнить действие. Попробуйте снова.');
    }
}

async function getAssessment(employeeId, assessmentId) {
    try {
        // Формируем URL для запроса
        const url = `/Employee/GetSelfAssessment?employeeId=${encodeURIComponent(employeeId)}&assessmentId=${encodeURIComponent(assessmentId)}`;

        // Выполняем fetch запрос
        let response = await fetch(url);

        // Получаем текстовый HTML-контент из ответа
        let htmlContent = await response.text();

        // Вставляем HTML-контент в элемент с id 'lastAssessment'
        document.getElementById('lastAssessment').innerHTML = htmlContent;
    } catch (error) {
        console.error('Произошла ошибка:', error);
        alert('Не удалось выполнить действие. Попробуйте снова.');
    }
}

async function assessEmployee(elem, assessmentId, assessmentResultId, employeeId, isSelfAssessment) {
    try {
        let assessmentValues = elem.parentNode.querySelectorAll(".input_assessment_value");
        let assessmentContainer = document.getElementById('assessment_container');

        const jsonToSend = {};
        jsonToSend.resultValues = [];
        jsonToSend.assessmentResultId = assessmentResultId;

        assessmentValues.forEach((item) => {
            let itemValue = item.value;
            let itemMax = +item.max;
            let itemMin = +item.min;

            if (itemValue <= itemMax && itemValue >= itemMin) {
                if (itemValue.includes('.')) {
                    jsonToSend.resultValues.push(itemValue.split('.')[0]);
                    validationFormAssessment(item, 'errorInclude');
                } else if (itemValue.includes(',')) {
                    jsonToSend.resultValues.push(itemValue.split(',')[0]);
                    validationFormAssessment(item, 'errorInclude');
                } else {
                    jsonToSend.resultValues.push(itemValue);
                    validationFormAssessment(item, 'success');
                }
            } else {
                validationFormAssessment(item, 'errorValidation');
            }
        });

        let url = '/Employee/AssessEmployee';

        const response = await fetch(url, {
            headers: {
                'Content-Type': 'application/json;charset=utf-8'
            },
            method: 'POST',
            body: JSON.stringify(jsonToSend)
        });

        if (!response.ok) {
            let errorData = await response.json();
            alert(`Ошибка ${response.status}: ${errorData.message}`);
            return;
        }

        //alert('Оценка успешно принята!');
        popupAlert('Оценка успешно принята!', false)

        // Обновление интерфейса после успешной оценки
        await getAssessmentLayout(employeeId);

        if (isSelfAssessment) {
            await getSelfAssessment(employeeId, assessmentId);
        }

    } catch (error) {
        console.error('Произошла ошибка:', error);
        alert('Не удалось выполнить действие. Попробуйте снова.');
    }
}

function validationFormAssessment(item, type) {
    item.style.color = "#f00";
    let errorElem = item.parentNode.parentNode.nextElementSibling.querySelector('.grade_error_description');

    if (type == 'errorInclude') {
        textError = 'Неверное значение';
        errorElem.innerHTML = textError;
        item.parentNode.parentNode.nextElementSibling.style.display = 'flex'
    } else if (type == 'errorValidation') {
        textError = 'Значение должно быть меньше или равно максимального';
        errorElem.innerHTML = textError;
        item.parentNode.parentNode.nextElementSibling.style.display = 'flex'
    } else if (type == 'success') {
        item.style.color = "#000";
        item.parentNode.parentNode.nextElementSibling.style.display = 'none'
    }
}

