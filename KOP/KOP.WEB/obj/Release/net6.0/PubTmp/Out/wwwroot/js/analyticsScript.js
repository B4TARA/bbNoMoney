
//async function getGradeAnalytics() {
//    try {
//        // Выполняем fetch запрос
//        let response = await fetch(`/Analytics/GetGradeAnalytics`);

//        // Получаем текстовый HTML-контент из ответа
//        let htmlContent = await response.text();

//        // Вставляем HTML-контент в нужный элемент
//        document.getElementById('gradeAnalytics').innerHTML = htmlContent;
//    } catch (error) {
//        console.error('Произошла ошибка:', error);
//        alert('Не удалось выполнить действие. Попробуйте снова.');
//    }
//}

//async function getMarkAnalytics() {
//    try {
//        // Выполняем fetch запрос
//        let response = await fetch(`/Analytics/GetMarkAnalytics`);

//        // Получаем текстовый HTML-контент из ответа
//        let htmlContent = await response.text();

//        // Вставляем HTML-контент в нужный элемент
//        document.getElementById('markAnalytics').innerHTML = htmlContent;
//    } catch (error) {
//        console.error('Произошла ошибка:', error);
//        alert('Не удалось выполнить действие. Попробуйте снова.');
//    }
//}

//async function getAssessmentAnalytics() {
//    try {
//        // Выполняем fetch запрос
//        let response = await fetch(`/Analytics/GetAssessmentAnalytics`);

//        // Получаем текстовый HTML-контент из ответа
//        let htmlContent = await response.text();

//        // Вставляем HTML-контент в нужный элемент
//        document.getElementById('assessmentAnalytics').innerHTML = htmlContent;
//    } catch (error) {
//        console.error('Произошла ошибка:', error);
//        alert('Не удалось выполнить действие. Попробуйте снова.');
//    }
//}

document.querySelectorAll('.custom-select-politics').forEach(select => {

    const selectBox = select.querySelector('.select-box-politics');
    const options = select.querySelector('.select-options-politics');
    const label = select.querySelector('.select-label-politics');

    selectBox.addEventListener('click', function (event) {
        event.stopPropagation();

        const isOptionsVisible = options.style.display === 'block';

        document.querySelectorAll('.select-options-politics').forEach(opt => {
            opt.style.display = 'none';
        });

        if (!isOptionsVisible) {
            options.style.display = 'block';
        }
    });

    options.querySelectorAll('.option-politics').forEach(option => {
        option.addEventListener('click', function () {
            label.textContent = this.textContent;
            options.style.display = 'none';
        });
    });
});

const test = [70,30]

//// Activity Chart
//const activityCtx = document.getElementById('activityChart').getContext('2d');

//const activityChart = new Chart(activityCtx, {
//    type: 'bar',
//    data: {
//        labels: ['Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб', 'Вс'],
//        datasets: [{
//            data: [10, 20, 15, 7, 30, 18, 5], // Пример данных
//            backgroundColor: [
//                '#ebedf2', '#ebedf2', '#ebedf2', '#ebedf2', '#1b74fd', '#ebedf2', '#ebedf2'
//            ],
//            borderRadius: 8, // Радиус закругления столбцов
//            barThickness: 30 // Толщина столбцов
//        }]
//    },
//    options: {
//        scales: {
//            x: {
//                grid: {
//                    display: false
//                },
//                ticks: {
//                    color: '#A0A0A0',
//                    font: {
//                        size: 12
//                    }
//                }
//            },
//            y: {
//                beginAtZero: true,
//                display: false, // Скрываем ось Y
//                grid: {
//                    display: false
//                }
//            }
//        },
//        plugins: {
//            legend: {
//                display: false // Скрываем легенду
//            }
//        }
//    }
//});

//// Chart
//const chart4Ctx = document.getElementById('chart4').getContext('2d');


//const chart4Chart = new Chart(chart4Ctx, {
//    type: 'polarArea',
//    data: {
//        labels: ['Тип 1', 'Тип 2', 'Тип 3', 'Тип 4', 'Тип 5'],
//        datasets: [
//            {
//                label: 'Dataset 1',
//                data: [11,16,7,14,3],
//                backgroundColor: [
//                    '#ffc107',
//                    '#f69110',
//                    '#5dac50',
//                    '#1b74fd',
//                    '#7b68ee'
//                ]
//            }
//        ]
//    },
//    options: {
//        responsive: true,
//        plugins: {
//            legend: {
//                position: 'top',
//            },
//            title: {
//                display: false,
//                text: 'Chart.js Polar Area Chart'
//            }
//        }
//    }
//});

// Steps Charts
//const stepsCtx = document.getElementById('stepsChart').getContext('2d');

//// Создаем градиент для заливки под графиком
//const gradient = stepsCtx.createLinearGradient(0, 0, 0, 300);
//gradient.addColorStop(0, 'rgba(74, 108, 247, 0.4)');
//gradient.addColorStop(1, 'rgba(255, 255, 255, 0)');

//const stepsChart = new Chart(stepsCtx, {
//    type: 'line',
//    data: {
//        labels: ['08', '10', '12', '14', '16', '18', '20'],
//        datasets: [{
//            data: [300, 600, 1026, 700, 400, 800, 500], // Пример данных
//            borderColor: '#4A6CF7',
//            backgroundColor: gradient, // Применяем градиент
//            fill: true, // Заполнение под линией
//            tension: 0.4, // Плавные линии
//            pointRadius: 4, // Радиус точек на линии
//            pointBackgroundColor: '#fff',
//            pointBorderColor: '#4A6CF7',
//            pointHoverRadius: 5
//        }]
//    },
//    options: {
//        layout: {
//            // padding: {
//            //     left: 10 // Отступ слева
//            // }
//        },
//        scales: {
//            x: {
//                grid: {
//                    display: false,
//                },
//                border: {
//                    display: false, // Скрываем нижнюю линию оси X
//                },
//                ticks: {
//                    color: '#A0A0A0',
//                    font: {
//                        size: 12
//                    },

//                }
//            },
//            y: {
//                beginAtZero: true,
//                display: false, // Скрываем ось Y
//                grid: {
//                    display: false
//                }
//            }
//        },
//        plugins: {
//            legend: {
//                display: false // Скрываем легенду
//            },
//            tooltip: {
//                callbacks: {
//                    label: function (context) {
//                        return context.raw + ' steps';
//                    }
//                }
//            }
//        }
//    }
//});

// Calories Chart
//const caloriesCtx = document.getElementById('caloriesChart').getContext('2d');

//const caloriesChart = new Chart(caloriesCtx, {
//    type: 'doughnut',
//    data: {
//        labels: ['Burned till now', 'Left to burn'],
//        datasets: [{
//            data: [70, 30], // Пример данных: 70% сожжено, 30% осталось
//            backgroundColor: ['#4A6CF7', '#E5E5E5'],
//            borderWidth: 2,
//            cutout: '75%', // Размер выреза для создания эффекта кольца
//        }]
//    },
//    options: {
//        plugins: {
//            legend: {
//                display: false // Скрываем легенду
//            },
//            tooltip: {
//                callbacks: {
//                    label: function (context) {
//                        return context.label + ': ' + context.raw + '%';
//                    }
//                }
//            }
//        }
//    }
//});


// Workouts Chart
//const workoutsCtx = document.getElementById('workoutsChart').getContext('2d');

//const workoutsChart = new Chart(workoutsCtx, {
//    type: 'bar',
//    data: {
//        labels: ['январь', 'февраль', 'март', 'апрель', 'май', 'июль', 'сентябрь', 'октябрь', 'ноябрь', 'декабрь'],
//        datasets: [
//            {
//                label: 'Тип1',
//                data: [200000, 50000, 10000, 5000, 8000, 0, 0, 0, 0, 0],
//                backgroundColor: '#FF4C4C',
//            },
//            {
//                label: 'Тип2',
//                data: [2000, 5000, 10000, 3000, 2000, 0, 0, 0, 0, 0],
//                backgroundColor: '#4C6CF7',
//            },
//            {
//                label: 'Тип3',
//                data: [0, 0, 25000, 0, 0, 70000, 0, 0, 0, 0],
//                backgroundColor: '#FFC107',
//            },
//            {
//                label: 'Тип4',
//                data: [0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
//                backgroundColor: '#FF69B4',
//            },
//            {
//                label: 'Тип5',
//                data: [0, 0, 0, 0, 0, 0, 0, 0, 5000, 0],
//                backgroundColor: '#8B8B8B',
//            },
//            {
//                label: 'Тип6',
//                data: [0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
//                backgroundColor: '#8B4513',
//            },
//            {
//                label: 'Другие',
//                data: [0, 0, 0, 0, 0, 0, 0, 0, 0, 700000],
//                backgroundColor: '#28A745',
//            }
//        ],
//        borderRadius: 8, // Радиус закругления столбцов
//    },
//    options: {
//        responsive: true,
//        plugins: {
//            legend: {
//                position: 'top',
//            },
//            tooltip: {
//                callbacks: {
//                    label: function (context) {
//                        return context.dataset.label + ': ' + context.raw.toLocaleString();
//                    }
//                }
//            }
//        },
//        scales: {
//            x: {
//                stacked: true,
//                grid: {
//                    display: false,
//                },
//            },
//            y: {
//                stacked: true,
//                beginAtZero: true,
//                ticks: {
//                    callback: function (value) {
//                        return value.toLocaleString(); // Форматирование чисел
//                    }
//                }
//            }
//        }
//    }
//});


const modal = document.getElementById('chartModal');
const modalContent = modal.querySelector('.modal-content');
const closeBtn = document.querySelector('.modal .close');

// Переменные для хранения исходных местоположений графиков
const originalParents = {};

// Обработчик клика на кнопки "Расширить"
document.querySelectorAll('.expand-btn').forEach(button => {
    button.addEventListener('click', function () {
        const targetChartId = this.getAttribute('data-target');
        const targetChart = document.getElementById(targetChartId);

        // Сохраняем исходное родительское место для возвращения графика
        originalParents[targetChartId] = targetChart.parentNode;

        // Перемещаем выбранный график в модальное окно
        modalContent.innerHTML = ''; // Очищаем содержимое
        modalContent.appendChild(targetChart); // Добавляем график в модальное окно

        // Открываем модальное окно
        modal.style.display = 'flex';
    });
});

// Обработчик клика на кнопку закрытия модального окна
closeBtn.addEventListener('click', function () {
    // Возвращаем график обратно в его изначальное место
    Object.keys(originalParents).forEach(chartId => {
        const chart = document.getElementById(chartId);
        const originalParent = originalParents[chartId];
        if (chart && originalParent) {
            originalParent.appendChild(chart); // Возвращаем график обратно
        }
    });

    modal.style.display = 'none';
});

// Закрытие модального окна при клике вне его
window.addEventListener('click', function (event) {
    if (event.target === modal) {
        closeBtn.click(); // Закрытие при клике вне модального окна
    }
});