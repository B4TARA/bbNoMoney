var arrowShowMenu = document.querySelectorAll(".arrow"),
    arrowShowSubMenu = document.querySelectorAll(".arrow-sub"),
    selectBtn = document.querySelectorAll(".select-btn"),
    sidebar = document.querySelector(".sidebar"),
    sidebarBtn = document.querySelector(".sidebar_close_btn_wrapper");


arrowShowMenu.forEach(item => {
    item.addEventListener("click", (e) => {
        let arrowParent = e.target.parentElement.parentElement;//selecting main parent of arrow
        arrowParent.classList.toggle("showMenu");
    });
})
if (sidebarBtn != null) {
    sidebarBtn.addEventListener("click", () => {
        sidebar.classList.toggle("close_sidebar");
    });
}

arrowShowSubMenu.forEach(item => {
    item.addEventListener("click", (e) => {
        let targetElem = e.target.parentElement;
        let arrowParent = e.target.parentElement.nextElementSibling;//selecting main parent of arrow
        console.log(targetElem)
        arrowParent.classList.toggle("active");
        targetElem.classList.toggle("active");
    });
})



selectBtn.forEach(item => {
    item.addEventListener("click", () => {
        item.classList.toggle("open");
    });

})

if (selectBtn.length != 0) {
    if (selectBtn.length <= 1) {
        selectBtn[0].classList.toggle("open");
    }
}


/*COMPARE BUTTON*/





/******POPUP ALERT*******/
function closeBtnPopup(item, isReload) {
    let popupSection = document.getElementById("section_popup");
    let isCompareBox = document.getElementById('compare_box')
    let isSelectedWrapper = document.getElementById('selected_main_wrapper')
    console.log(isCompareBox)
    popupSection.remove();
    //if (isCompareBox != undefined || isSelectedWrapper != undefined) {
    //    console.log(isCompareBox)
    //} else {
    //    location.reload();
    //}

    if (isReload == true) {
        location.reload();
    }

}

function popupAlert(text, isReload) {
    let alertSection = document.createElement('section');
    let homeSection = document.querySelector('.home-content')
    alertSection.className = "section_popup alert_popup active_popup";
    alertSection.setAttribute('id', 'section_popup')

    alertSection.innerHTML = `<div class="modal-box">
        <div class="close_btn close-btn margin_container_bottom_middle" onclick = "closeBtnPopup(this,${isReload})">
                                    <i class="fa-solid fa-xmark"></i>
                                </div>
        <div class="mid_title">${text}</div>
        </div>
        `;
    homeSection.appendChild(alertSection)

    //setTimeout(closeBtnPopup, 1500);
}

function popupResult(isReload, planVal, markId, employeeId, isPercentage) {
    console.log(isPercentage)
    let alertSection = document.createElement('section');
    let homeSection = document.querySelector('.home-content')
    alertSection.className = "section_popup alert_popup active_popup";
    alertSection.setAttribute('id', 'section_popup')
    
    alertSection.innerHTML = `<div class="modal-box result">
        <div class="close_btn close-btn margin_container_bottom_middle" onclick = "closeBtnPopup(this,${isReload})">
                                    <i class="fa-solid fa-xmark"></i>
                                </div>

                                <div class="modal-box-content">
                                    <div class="container_description margin_container_bottom">
                                         <div class="mid_title">
                                            Рассчет показателя
                                         </div>
                                         <div class="mid_description">
                                            Введите данные для рассчета
                                         </div>
                                    </div>
                                    <div class="form_group_item margin_container_bottom">
                                        <label for="position_name" class="mid_description">Фактическое значение</label>
                                        <input type="number" value="" class="calculate-fact-popup-value" id="calculateFactValue" name="position_name">
                                    </div>

                                    <div class="container_description margin_container_bottom">
                                         <div class="mid_title">
                                            Результат
                                         </div>
                                         <div class="title" id="resultParametr">
                                            0
                                         </div>
                                    </div>

                                     <div onclick="saveResultParameter(${markId}, ${employeeId})" class="action_btn primary_btn">
                                        Сохранить
                                     </div>
                                </div>
        
        </div>
        `;
    homeSection.appendChild(alertSection)
    //setTimeout(closeBtnPopup, 1500);

    const factInput = document.getElementById('calculateFactValue')
    if (factInput) {

        const calculateResult = document.getElementById('resultParametr')
        //Сюда формулу рассчета
        if (isPercentage) {
            factInput.addEventListener('input', function (event) {
                const calculateVal = (factInput.value / Number(planVal) * 100).toFixed(0);
                if (calculateVal >= 100) {
                    calculateResult.classList.remove('red-text')
                    calculateResult.classList.add('green-text')
                    
                } else {
                    calculateResult.classList.remove('green-text')
                    calculateResult.classList.add('red-text')
                }
                calculateResult.textContent = calculateVal + '%';

            })
        } else {
            factInput.addEventListener('input', function (event) {
                

                if (factInput.value >= planVal) {
                    calculateResult.classList.remove('red-text')
                    calculateResult.classList.add('green-text')

                } else {
                    calculateResult.classList.remove('green-text')
                    calculateResult.classList.add('red-text')
                }

                calculateResult.textContent = factInput.value;

            })
        }
        
    }
}