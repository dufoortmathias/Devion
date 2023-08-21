//#region *** DOM references ***********
const baseUrl = "https://192.168.100.237:7223/api";
let htmlButton, htmlBestelbonSelect, htmlBedrijfSelect;
//#endregion

//#region *** Callback-Visualisation - show___ ***********
const showButtons = function () {
    console.log("showButtons");
    htmlButton.classList.remove("c-button-disabled");
    htmlButton.disabled = false;
};

const showBestelbonnen = function (jsonObject) {
    console.log("showBestelbonnen");
    let innerhtml = "";
    for (const bestelbon of jsonObject) {
        innerhtml += `<option value="${bestelbon.id}">${bestelbon.id}</option>`;
    }
    htmlBestelbonSelect.innerHTML = innerhtml;
};
//#endregion

//#region *** Callback-No Visualisation - callback___ ***********
//#endregion

//#region *** Data Access - get___ ***********
const getData = async function (endpoint) {
    console.log(endpoint)
    const data = await fetch(endpoint).then(r => r.json()).catch(err => console.error(err));
    return data;
};

const getBestelbonnen = async function (bedrijfId) {
    const data = await getData(`${baseUrl}/${bedrijfId}/ets/openpurchaseorderids`);
    showBestelbonnen(data);
};
//#endregion

//#region *** Event Listeners - listenTo___ ***********
const listenToList = function () {
    htmlBestelbonSelect.addEventListener("change", function () {
        console.log("select");
        showButtons();
    });
    htmlBedrijfSelect.addEventListener("change", function () {
        console.log("bedrijf select");
        getBestelbonnen(htmlBedrijfSelect.value);
    });
};

const listenToButtons = function () {
    htmlButton.addEventListener("click", function () {
        console.log("button");
    });
};
//#endregion

//#region *** Init / DOMContentLoaded ***********
const init = function () {
    console.log("DOM Content Loaded");
    htmlButton = document.querySelector(".js-button");
    htmlBestelbonSelect = document.querySelector(".js-bestelbon-select");
    htmlBedrijfSelect = document.querySelector(".js-bedrijf-select");

    listenToList();
    listenToButtons();
};

document.addEventListener("DOMContentLoaded", init);
//#endregion
