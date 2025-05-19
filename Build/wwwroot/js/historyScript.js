
async function getEmployeeHistoryLayout(employeeId) {
    try {
        // Перенаправляем запрос
        window.location.href = `/History/GetEmployeeHistoryLayout?employeeId=${encodeURIComponent(employeeId)}`;
    } catch (error) {
        console.error('Произошла ошибка:', error);
        alert('Не удалось выполнить действие. Попробуйте снова.');
    }
}

async function getEmployeeGradeHistoryLayout(employeeId) {
    try {
        // Выполняем fetch запрос
        let response = await fetch(`/History/GetEmployeeGradeHistoryLayout?employeeId=${encodeURIComponent(employeeId)}`);

        // Получаем текстовый HTML-контент из ответа
        let htmlContent = await response.text();

        // Вставляем HTML-контент в элемент с id 'employeeHistoryItem'
        document.getElementById('employeeHistoryItem').innerHTML = htmlContent;
        console.log('')
    } catch (error) {
        console.error('Произошла ошибка:', error);
        alert('Не удалось выполнить действие. Попробуйте снова.');
    }
}

async function getEmployeeAssessmentHistoryLayout(employeeId) {
    try {
        // Выполняем fetch запрос
        let response = await fetch(`/History/GetEmployeeAssessmentHistoryLayout?employeeId=${encodeURIComponent(employeeId)}`);

        // Получаем текстовый HTML-контент из ответа
        let htmlContent = await response.text();

        // Вставляем HTML-контент в элемент с id 'employeeHistoryItem'
        document.getElementById('employeeHistoryItem').innerHTML = htmlContent;
        console.log('')
    } catch (error) {
        console.error('Произошла ошибка:', error);
        alert('Не удалось выполнить действие. Попробуйте снова.');
    }
}

async function getEmployeeGradeTypeHistory(gradeTypeId, employeeId) {
    try {
        // Выполняем fetch запрос
        let response = await fetch(`/History/GetEmployeeGradeTypeHistory?employeeId=${encodeURIComponent(employeeId)}&gradeTypeId=${encodeURIComponent(gradeTypeId)}`);

        // Получаем текстовый HTML-контент из ответа
        let htmlContent = await response.text();

        // Вставляем HTML-контент в элемент с id 'employeeHistoryItemContent'
        document.getElementById('employeeHistoryItemContent').innerHTML = htmlContent;
    } catch (error) {
        console.error('Произошла ошибка:', error);
        alert('Не удалось выполнить действие. Попробуйте снова.');
    }
}

async function getEmployeeAssessmentTypeHistory(assessmentTypeId, employeeId) {
    try {
        // Выполняем fetch запрос
        let response = await fetch(`/History/GetEmployeeAssessmentTypeHistory?employeeId=${encodeURIComponent(employeeId)}&assessmentTypeId=${encodeURIComponent(assessmentTypeId)}`);

        // Получаем текстовый HTML-контент из ответа
        let htmlContent = await response.text();

        // Вставляем HTML-контент в элемент с id 'employeeHistoryItemContent'
        document.getElementById('employeeHistoryItemContent').innerHTML = htmlContent;
    } catch (error) {
        console.error('Произошла ошибка:', error);
        alert('Не удалось выполнить действие. Попробуйте снова.');
    }
}

async function getEmployeeMarkTypeHistory(martTypeId, employeeId) {
    try {
        // Выполняем fetch запрос
        let response = await fetch(`/History/GetEmployeeMarkTypeHistory?employeeId=${encodeURIComponent(employeeId)}&markTypeId=${encodeURIComponent(martTypeId)}`);

        // Получаем текстовый HTML-контент из ответа
        let htmlContent = await response.text();

        // Вставляем HTML-контент в элемент с id 'employeeHistoryItemContent'
        document.getElementById('employeeHistoryItemContent').innerHTML = htmlContent;
    } catch (error) {
        console.error('Произошла ошибка:', error);
        alert('Не удалось выполнить действие. Попробуйте снова.');
    }
}