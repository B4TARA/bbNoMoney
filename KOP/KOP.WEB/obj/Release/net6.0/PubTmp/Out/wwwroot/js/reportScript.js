
async function saveGradesReport() {
    try {
        popupAlert('Отчёт успешно выгружен', false)

        // На время теста закоментить для блокировки появления окна с сохранением
        // Перенаправляем запрос
        //window.location.href = `/Report/SaveGradesReport`;
    } catch (error) {
        console.error('Произошла ошибка:', error);
        popupAlert('Не удалось выполнить действие. Попробуйте снова.', false)
    }
}