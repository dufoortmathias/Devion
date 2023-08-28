//#region *** DOM references ***********
const baseUrl = "http://localhost:5142/api";
let htmlButton, htmlBestelbonSelect, htmlBedrijfSelect, htmltabel;

//#endregion

//#region *** Callback-Visualisation - show___ ***********

const showBestelbonnen = async function (jsonObject) {
    htmlBestelbonSelect.disabled = false
    let innerhtml = "";
    jsonObject.sort();
    innerhtml += `<option value="0" disabled selected>Selecteer een bestelbon</option>`;
    for (const bestelbon of jsonObject) {
        innerhtml += `<option value="${bestelbon}">${bestelbon}</option>`;
    }
    htmlBestelbonSelect.innerHTML = innerhtml;
};

const showTabel = function (jsonObject) {
    htmltabel.classList.remove("o-hide-accessible");
    let innerhtml = "";
    let leveranciers = [];
    for (const bestelbon of jsonObject.artikels) {
        if (!leveranciers.includes(bestelbon.leverancier)) {
            leveranciers.push(bestelbon.leverancier);
        }
    }
    innerhtml += `<h1>Bestelbon ${jsonObject.bonNummer}</h1>`;

    if (jsonObject.artikels.length == 0) {
        innerhtml += '<h3 class="c-label--error">Er zijn geen artikels bij deze bestelbon!</h3>';
    }

    for (const leverancier of leveranciers) {
        if (leverancier != null) {

            innerhtml += `<div class="div-button-bon-leverancier">`
            innerhtml += `<h2 class="header">${leverancier}</h2>`;
            innerhtml += `<button disabled type="submit" class="download-button o-button-reset c-button .c-button-disabled js-button"> download bestelbon </button>`
            innerhtml += `<div></div>`
            innerhtml += `<button disabled type="submit" class="o-button-reset c-button .c-button-disabled js-button"> order geplaatst </button>`
            innerhtml += `<a target="_blank" href="https://www.routeco.com/nl-be/tools/quick-basket/order-upload" style="text-decoration: none !important; font-size:24px" class="fa">&#xf059;</a>`
            innerhtml += `</div>`

            innerhtml += `<table class='c-tabel'>`;
            innerhtml += `<tr><th>Artikel</th><th>Aantal</th></tr>`;
            for (const bestelbon of jsonObject.artikels) {
                if (bestelbon.leverancier == leverancier) {
                    innerhtml += `<tr><td>${bestelbon.artikelNummer}</td><td>${bestelbon.aantal}</td></tr>`;
                }
            }
            innerhtml += `</table>`;
        }
        else {

            let artikels = [];
            for (const bestelbon of jsonObject.artikels) {
                if (bestelbon.leverancier == null) {
                    artikels.push(bestelbon);
                }
            }
            //check if 'VERZENDING' is in the list and the only one
            for (const artikel of artikels) {
                if (artikel.artikelNummer == 'VERZENDING') {
                    let index = artikels.indexOf(artikel);
                    artikels.pop(index);
                }
            } if (artikels.length >= 1) {
                //get list of articles without supplier
                innerhtml += `<h2>Geen specifieke leverancier</h2>`;
                innerhtml += `<table class='c-tabel'>`;
                innerhtml += `<tr><th>Artikel</th><th>Aantal</th></tr>`;
                for (const bestelbon of jsonObject.artikels) {
                    if (bestelbon.leverancier == leverancier) {
                        if (bestelbon.artikelNummer != 'VERZENDING')
                            innerhtml += `<tr><td>${bestelbon.artikelNummer}</td><td>${bestelbon.aantal}</td></tr>`;
                    }
                }
                innerhtml += `</table>`;
            }
        }}
        htmltabel.innerHTML = innerhtml;

        getBestelbonFile(htmlBestelbonSelect.value).then(data => {
            let files = []

            data.forEach(bon => {
                var decodeString = atob(bon.fileContents);
                var blob = new Blob([decodeString], { type: bon.contentType });
                files.push({
                    "blob": blob,
                    "fileName": bon.fileDownloadName
                })
            });

            let divs = document.getElementsByClassName("div-button-bon-leverancier")
            for (let i = 0; i < divs.length; i++) {
                let result = files.find(content => content["fileName"].includes(divs[i].getElementsByClassName("header")[0].textContent));

                if (result) {
                    let button = divs[i].getElementsByClassName("download-button")[0]
                    button.addEventListener("click", () => {
                        const a = document.createElement("a");
                        a.href = URL.createObjectURL(result["blob"]);
                        a.download = result["fileName"];
                        document.body.appendChild(a);
                        a.click();
                        document.body.removeChild(a);
                    })
                    button.classList.remove("c-button-disabled");
                    button.disabled = false
                }
            }
        })
    };

    const showCompanies = function (jsonObject) {
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
            getBestelbon(htmlBestelbonSelect.value);
        });

        htmlBedrijfSelect.addEventListener("change", function () {
            getBestelbonnen(htmlBedrijfSelect.value)
        });
    };
    //#endregion

    //#region *** Init / DOMContentLoaded ***********
    const init = function () {
        console.log("DOM Content Loaded");
        htmlBestelbonSelect = document.querySelector(".js-bestelbon-select");
        htmlBedrijfSelect = document.querySelector(".js-bedrijf-select");
        htmltabel = document.querySelector(".js-tabel");

        listenToList();
        getCompanies();
    };

    document.addEventListener("DOMContentLoaded", init);
//#endregion
