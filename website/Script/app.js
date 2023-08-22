//#region *** DOM references ***********
const baseUrl = "https://192.168.100.237:5001/api";
let htmlButton, htmlBestelbonSelect, htmlBedrijfSelect;

const config = {
    credentials: "include"
};
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
    console.log(endpoint);

    try {
        const response = await fetch(endpoint, config);

        if (!response.ok) {
            throw new Error(`API request failed with status: ${response.status}`);
        }

        const data = await response.json();
        console.log(JSON.stringify(data)); // Log the fetched data
        return data;
    } catch (error) {
        console.error('An error occurred during the fetch:', error);
        throw error; // Re-throw the error to handle it in the calling code if needed
    }
};

const getBestelbonnen = async function (bedrijfId) {
    const data = await getData(`${baseUrl}/${bedrijfId}/ets/customerids?datestring=1/01/2022`);
    if (data != null) {
        showBestelbonnen(data);
    } else {
        console.error("no data from api call with endpoint: " + `${baseUrl}/${bedrijfId}/ets/openpurchaseorderids`);
    }
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
