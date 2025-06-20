/**
 * 綁定指定 input 元素的 blur 事件，
 * 當輸入值小於 1 或不是數字時，自動將值設為 1。
 * @param {Element} inputElem - CSS 選擇器字串，例如 '#inputId' 或 '.inputClass'
 */
function detectNumScopeBlurEvent(inputElem){
    document.querySelectorAll(inputElem).forEach(item => {        
        item.addEventListener("blur", (e) => {
            let elem = e.target
            if(elem && (elem.value < 1 || isNaN(Number(elem.value)))) 
                elem.value = 1;
            let n = Number(elem.value);
            if(elem.max < n)
                elem.value = elem.max;
        });
    });
}

/**
 * 綁定加減數量按鈕的點擊事件
 * @param {string} plusBtnClassName 
 * @param {string} minusBtnClassName 
 */
function inputNumBtnsClickEvent(plusBtnClassName, minusBtnClassName){
    let classes = plusBtnClassName + ", " + minusBtnClassName;
    document.querySelectorAll(classes).forEach(btn => {
        let isMinus = btn.className.indexOf(minusBtnClassName.replace(".", "")) > -1;
        let numElem = isMinus ? btn.previousElementSibling : btn.nextElementSibling;
        btn.addEventListener("click", (e) => {
            if(isMinus){
                if(numElem.value > numElem.min)
                    numElem.value = numElem.value - 1;
            }else{
                let n = Number(numElem.value);
                if(numElem.max > n)
                    numElem.value = n + 1;
                else
                    numElem.value = numElem.max;
            }
        });
    });
}


function builderMsgBoxes(){      
    CreateCoverElem();   
    CreateMessageElem();
    CreateAlertElem();
}
function CreateCoverElem(){
    let coverBox = document.createElement("div");
    coverBox.id = "coverBox";
    document.querySelector("body").append(coverBox);   
}
function CreateMessageElem(){
    let coverBox = document.querySelector("#coverBox");
    let msgBox = document.createElement("div");
    let msgSpan = document.createElement("span");
    msgBox.id = "msgBox";
    msgBox.append(msgSpan);
    coverBox.append(msgBox);
}
function CreateAlertElem(){
    let coverBox = document.querySelector("#coverBox");
    let alertBox = document.createElement("div");
    let closeBtn = document.createElement("div");
    let alertSpan = document.createElement("span");
    alertBox.id = "alertBox";
    closeBtn.id = "closeBtn"
    alertBox.append(alertSpan);
    alertBox.append(closeBtn);
    coverBox.append(alertBox);
    closeBtn.addEventListener("click", () => {
        coverBox.classList.remove("show-alert");
    });
}

function showMsg(msg){
    let coverBox = document.querySelector("#coverBox");
    if(!coverBox){
        builderMsgBoxes();
    }
    
    coverBox = document.querySelector("#coverBox");
    let msgBox = document.querySelector("#msgBox");
    let msgSpan = msgBox.querySelector("span");

    msgSpan.textContent = msg; 
    coverBox.classList.add("show-msg")
    setTimeout(() => {
        coverBox.classList.remove("show-msg")
    }, 2500)
}

function showAlert(msg){
    let coverBox = document.querySelector("#coverBox");
    if(!coverBox){
        builderMsgBoxes();
    }
    
    coverBox = document.querySelector("#coverBox");
    let alertBox = document.querySelector("#alertBox");
    let alertSpan = alertBox.querySelector("span");
    alertSpan.textContent = msg; 
    
    coverBox.classList.add("show-alert");
}