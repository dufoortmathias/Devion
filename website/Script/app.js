//#region *** DOM references ***********
const baseUrl = "http://192.168.100.237:5001/api";
let htmlButton, htmlBestelbonSelect, htmlBedrijfSelect, htmltabel;

//#endregion

//#region *** Callback-Visualisation - show___ ***********
const showButtons = function () {
    console.log("showButtons");
    htmlButton.classList.remove("c-button-disabled");
    htmlButton.disabled = false;
};

const showBestelbonnen = async function (jsonObject) {
    console.log("showBestelbonnen");
    let innerhtml = "";
    innerhtml += `<option value="0" disabled selected>Selecteer een bestelbon</option>`;
    for (const bestelbon of jsonObject) {
        innerhtml += `<option value="${bestelbon}">${bestelbon}</option>`;
    }
    htmlBestelbonSelect.innerHTML = innerhtml;
};

const showTabel = function (jsonObject) {
    console.log("showTabel");
    console.log(jsonObject);
    htmltabel.classList.remove("o-hide-accessible");
    let innerhtml = "";
    let leveranciers = [];
    for (const bestelbon of jsonObject.artikels) {
        if (!leveranciers.includes(bestelbon.leverancier)) {
            leveranciers.push(bestelbon.leverancier);
        }
    }
    console.log(leveranciers);
    innerhtml += `<h1>Bestelbon ${jsonObject.bonNummer}</h1>`;
    for (const leverancier of leveranciers) {
        innerhtml += `<h2>${leverancier}</h2>`;
        innerhtml += `<table class='c-tabel'>`;
        innerhtml += `<tr><th>Artikel</th><th>Aantal</th></tr>`;
        for (const bestelbon of jsonObject.artikels) {
            if (bestelbon.leverancier == leverancier) {
                innerhtml += `<tr><td>${bestelbon.artikelNummer}</td><td>${bestelbon.aantal}</td></tr>`;
            }
        }
        innerhtml += `</table>`;
    }
    htmltabel.innerHTML = innerhtml;
};

const showCompanies = function (jsonObject) {
    console.log("showCompanies");
    let innerhtml = "";
    innerhtml += `<option value="0" disabled selected>Selecteer een bedrijf</option>`;
    for (const company of jsonObject) {
        innerhtml += `<option value="${company.toLowerCase()}">${company}</option>`;
    }
    htmlBedrijfSelect.innerHTML = innerhtml;
};
//#endregion

//#region *** Callback-No Visualisation - callback___ ***********
//#endregion

//#region *** Data Access - get___ ***********
const getData = async function (endpoint) {
    console.log(endpoint);

    try {
        const response = await fetch(endpoint);

        if (!response.ok) {
            throw new Error(`API request failed with status: ${response.status}`);
        }

        const data = await response.json();
        return data;
    } catch (error) {
        alert("geen data van api call: " + endpoint + "\n" + error + "\n");
        throw error; // Re-throw the error to handle it in the calling code if needed
    }
};

const getBestelbonnen = async function (bedrijfId) {
    const data = await getData(`${baseUrl}/${bedrijfId}/ets/openpurchaseorderids`);
    if (data != null) {
        showBestelbonnen(data);
    } else {
        alert("no data from api call with endpoint: " + `${baseUrl}/${bedrijfId}/ets/openpurchaseorderids`);
    }
};

const getBestelbon = async function (bestelbonId) {
    const data = await getData(`${baseUrl}/${htmlBedrijfSelect.value}/ets/purchaseorder?id=${bestelbonId}`);
    if (data != null) {
       showTabel(data);
    } else {
        alert("no data from api call with endpoint: " + `${baseUrl}/${htmlBedrijfSelect.value}/ets/purchaseorder?id=${bestelbonId}`);
    }
};

const getBestelbonFile = async function (bestelbonId) {
    const data = await getData(`${baseUrl}/${htmlBedrijfSelect.value}/ets/createpurchasefile?id=${bestelbonId}`);
    if (data != null) {
        return data;
    } else {
        alert("no data from api call with endpoint: " + `${baseUrl}/${htmlBedrijfSelect.value}/ets/createpurchasefile?id=${bestelbonId}`);
    }
};

const getCompanies = async function () {
    const data = await getData(`${baseUrl}/companies`);
    if (data != null) {
        showCompanies(data);
    } else {
        alert("no data from api call with endpoint: " + `${baseUrl}/ets/companies`);
    }
};
//#endregion

//#region *** Event Listeners - listenTo___ ***********
const listenToList = function () {
    htmlBestelbonSelect.addEventListener("change", function () {
        console.log("select");
        showButtons();
        getBestelbon(htmlBestelbonSelect.value);
    });

    htmlBedrijfSelect.addEventListener("change", function () {
        console.log("bedrijf select");
        getBestelbonnen(htmlBedrijfSelect.value)
    });
};

const listenToButtons = function () {
    htmlButton.addEventListener("click", function () {
        console.log("button");
        getBestelbonFile(htmlBestelbonSelect.value).then(data => {
            console.log(data);
            data.forEach(bon => {
                console.log(bon)
                var decodeString = atob(bon.fileContents);
                console.log(decodeString)
                var blob = new Blob([decodeString], { type: bon.contentType });
                const a = document.createElement("a");
                a.href = URL.createObjectURL(blob);
                a.download = bon.fileDownloadName;
                document.body.appendChild(a);
                a.click();
                console.log("downloaded")
                document.body.removeChild(a);
            });
        });
    });
};
//#endregion

//#region *** Init / DOMContentLoaded ***********
const init = function () {
    console.log("DOM Content Loaded");
    htmlButton = document.querySelector(".js-button");
    htmlBestelbonSelect = document.querySelector(".js-bestelbon-select");
    htmlBedrijfSelect = document.querySelector(".js-bedrijf-select");
    htmltabel = document.querySelector(".js-tabel");

    listenToList();
    listenToButtons();
    getCompanies();
};

document.addEventListener("DOMContentLoaded", init);
//#endregion
