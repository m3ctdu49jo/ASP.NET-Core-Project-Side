
function checkoutCartInit(){
    buildAllItemsNumBtns();
    detectNumScopeBlurEvent(".purch_num");
    setCartAllItemsRemoveBtnEvent();
}

function buildAllItemsNumBtns(){
    document.querySelectorAll(".purch_num").forEach(item => {
        inputNumBuildBtnsClickEvent(item, () => {
            const productId = item.id.split("_")[1];
            const purchCount = item.value;

            shoppingCartFetch(new Request("/ShoppingCart/UpdateShoppingCart", {
                method: 'POST',
                headers: fetchHeaders(),
                body: JSON.stringify({
                    Product: {
                        ProductID: productId
                    },
                    PurchCount: purchCount
                })
            }));
        });
    });
}



function setCartAllItemsRemoveBtnEvent(){
    document.querySelectorAll(".remove-cart-item").forEach(btn => {
        btn.addEventListener("click", function(event) {
            const form = this.closest("form");
            const productId = form.querySelector("input[name='productId']").value;

            shoppingCartFetch(new Request("/ShoppingCart/RemoveFromCart",{
                method: 'POST',
                headers: fetchHeaders({
                        'Content-Type': 'application/x-www-form-urlencoded'
                    }),
                body: new URLSearchParams({ productId })
            }), "✔ 商品已從購物車中移除");

        });
    });
}



function shoppingCartFetch(request, successMsg = "✔ 購物車已更新") {
    fetch(request)
    .then(async res => {
        switch (res.status) {
            case 200:
                return await res.text();
            case 400:
                showAlert("無效的請求，請稍後再試。" + await res.text());
                return;
            case 401:
                showAlert("⚠️ 請登入會員或註冊會員帳號");
                return;
            default :
                showAlert("✖ 發生未知錯誤，請稍後再試");
                console.log(res.status, res.statusText);
                return;
        }
    })
    .then(html => {
        if (html) {
            document.querySelector("#checkout-cart-items").innerHTML = "<p>Loading...</p>";
            showMsg(successMsg);
            document.querySelector("#checkout-cart-items").innerHTML = html;
            checkoutCartInit();
            // setTimeout(() => {
                
            // }, 200);
        }
    })
    .then(() => {
        console.log(123);
    });
}