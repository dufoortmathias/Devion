//#region *** DOM references ***********
const baseUrl = "http://192.168.100.237:5000/api";
let htmlButtonDownload,
  htmlBestelbonSelect,
  htmlBedrijfSelect,
  htmltabel,
  htmlArtikelNrSearch,
  htmlArtikelSearch,
  htmlArtikeNrSearchError,
  htmlArtikelNr,
  htmlReflevNr,
  htmlHoofdlev,
  htmlOmschrijving,
  htmlPrijs,
  htmlFamilie,
  htmlSubFamilie,
  htmlBtwcode,
  htmlRekver,
  htmlLengte,
  htmlBreedte,
  htmlHoogte,
  htmlAkPrijsEh,
  htmlVerEh,
  htmlOmRekFac,
  htmlTypFac,
  htmlMuntcode,
  htmlMerk,
  htmlArtikelForm,
  htmlTariefPrijs,
  htmlVerkoopPrijs,
  htmlWinstpercentage,
  htmlLink,
  htmlLinkIcon,
  htmlStdKorting,
  htmlArtikelToeveogen,
  htmlFileInput;

//#endregion

//#region *** Callback-Visualisation - show___ ***********
const showButtons = function () {
  htmlButtonDownload.classList.remove("c-button-disabled");
  htmlButtonDownload.disabled = false;
};

const showBestelbonnen = async function (jsonObject) {
  let innerhtml = "";
  jsonObject.sort();
  jsonObject.reverse();
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
    innerhtml +=
      '<h3 class="c-label--error">Er zijn geen artikels bij deze bestelbon!</h3>';
  }

  for (const leverancier of leveranciers) {
    if (leverancier != null) {
      innerhtml += `<h2>${leverancier}</h2>`;
      innerhtml += `<table class='c-tabel'>`;
      innerhtml += `<tr><th>Artikel</th><th>Aantal</th></tr>`;
      for (const bestelbon of jsonObject.artikels) {
        if (bestelbon.leverancier == leverancier) {
          innerhtml += `<tr><td>${bestelbon.artikelNummer}</td><td>${bestelbon.aantal}</td></tr>`;
        }
      }
      innerhtml += `</table>`;
    } else {
      let artikels = [];
      for (const bestelbon of jsonObject.artikels) {
        if (bestelbon.leverancier == null) {
          artikels.push(bestelbon);
        }
      }
      //check if 'VERZENDING' is in the list and the only one
      for (const artikel of artikels) {
        if (artikel.artikelNummer == "VERZENDING") {
          let index = artikels.indexOf(artikel);
          artikels.pop(index);
        }
      }
      if (artikels.length >= 1) {
        //get list of articles without supplier
        innerhtml += `<h2>Geen specifieke leverancier</h2>`;
        innerhtml += `<table class='c-tabel'>`;
        innerhtml += `<tr><th>Artikel</th><th>Aantal</th></tr>`;
        for (const bestelbon of jsonObject.artikels) {
          if (bestelbon.leverancier == leverancier) {
            if (bestelbon.artikelNummer != "VERZENDING")
              innerhtml += `<tr><td>${bestelbon.artikelNummer}</td><td>${bestelbon.aantal}</td></tr>`;
          }
        }
        innerhtml += `</table>`;
      }
    }
  }
  htmltabel.innerHTML = innerhtml;
};

const showCompanies = function (jsonObject) {
  let innerhtml = "";
  innerhtml += `<option value="0" disabled selected>Selecteer een bedrijf</option>`;
  for (const company of jsonObject) {
    innerhtml += `<option value="${company.toLowerCase()}">${company}</option>`;
  }
  htmlBedrijfSelect.innerHTML = innerhtml;
};

const showArtikel = function (jsonObject) {
  console.log(jsonObject);
  htmlArtikeNrSearchError.classList.add("o-hide-accessible");
  htmlArtikelNrSearch.classList.remove("c-input--error");
  htmlOmschrijving.value = jsonObject.description;
  htmlStdKorting.value = parseFloat("0").toFixed(2);
  htmlTariefPrijs.value = parseFloat(jsonObject.tarifPrice).toFixed(4);
  htmlWinstpercentage.value = parseFloat("33.3333").toFixed(4);
  htmlPrijs.value = parseFloat(
    jsonObject.nettoPrice * (1 - htmlStdKorting.value / 100)
  ).toFixed(4);
  htmlVerkoopPrijs.value = parseFloat(htmlPrijs.value * (1 + 1 / 3)).toFixed(4);
  htmlReflevNr.value = jsonObject.reference;
  htmlArtikelNr.value = jsonObject.reference;
  htmlOmRekFac.value = jsonObject.salesPackQuantity;
  htmlMerk.value = jsonObject.brand;
  htmlLink.value = jsonObject.url;
  htmlLinkIcon.href = jsonObject.url;
  getFromInfo();
};

const showFromInfo = function (jsonObject) {
  let innerhtml = "";
  innerhtml += `<option value="0" disabled selected>Selecteer een familie</option>`;
  for (familie of jsonObject.families) {
    if (familie.CODE == "2") {
      innerhtml += `<option value="${familie.CODE}" selected>${familie.DESCRIPTION}</option>`;
    } else {
      innerhtml += `<option value="${familie.CODE}">${familie.DESCRIPTION}</option>`;
    }
  }
  htmlFamilie.innerHTML = innerhtml;

  innerhtml = "";
  innerhtml += `<option value="0" disabled selected>Selecteer een subfamilie</option>`;
  for (subfamilie of jsonObject.subfamilies) {
    if (subfamilie.CODE == "1D") {
      innerhtml += `<option value="${subfamilie.CODE}" selected>${subfamilie.DESCRIPTION}</option>`;
    } else {
      innerhtml += `<option value="${subfamilie.CODE}">${subfamilie.DESCRIPTION}</option>`;
    }
  }
  htmlSubFamilie.innerHTML = innerhtml;

  innerhtml = "";
  innerhtml += `<option value="0" disabled selected>Selecteer een eenheid</option>`;
  for (eenheid of jsonObject.measureTypes) {
    innerhtml += `<option value="${eenheid.CODE}">${eenheid.DESCRIPTION}</option>`;
  }
  htmlVerEh.innerHTML = innerhtml;
  htmlAkPrijsEh.innerHTML = innerhtml;

  innerhtml = "";
  innerhtml += `<option value="0" disabled selected>Selecteer een rekening</option>`;
  for (rekening of jsonObject.bankAccounts) {
    if (rekening.CODE == "700000") {
      innerhtml += `<option value="${rekening.CODE}" selected>${rekening.CODE} - ${rekening.DESCRIPTION}</option>`;
    } else {
      innerhtml += `<option value="${rekening.CODE}">${rekening.CODE} - ${rekening.DESCRIPTION}</option>`;
    }
  }
  htmlRekver.innerHTML = innerhtml;

  innerhtml = "";
  innerhtml += `<option value="0" disabled selected>Selecteer een muntcode</option>`;
  for (muntcode of jsonObject.coinTypes) {
    if (muntcode.CODE == "EUR") {
      innerhtml += `<option value="${muntcode.CODE}" selected>${muntcode.DESCRIPTION}</option>`;
    } else {
      innerhtml += `<option value="${muntcode.CODE}">${muntcode.DESCRIPTION}</option>`;
    }
  }
  htmlMuntcode.innerHTML = innerhtml;

  innerhtml = "";
  innerhtml += `<option value="0" disabled selected>Selecteer een btwcode</option>`;
  for (btwcode of jsonObject.BTWCodes) {
    if (btwcode.CODE == "9") {
      innerhtml += `<option value="${btwcode.CODE}" selected>${btwcode.DESCRIPTION}</option>`;
    } else if (btwcode.DESCRIPTION == null) {
      continue;
    } else if (
      btwcode.DESCRIPTION.includes("EXTRA CODE") ||
      btwcode.DESCRIPTION.includes("TEKSTLIJN")
    ) {
      continue;
    } else {
      innerhtml += `<option value="${btwcode.CODE}">${btwcode.DESCRIPTION}</option>`;
    }
  }
  htmlBtwcode.innerHTML = innerhtml;

  htmlArtikelForm.classList.remove("o-hide-accessible");
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
    throw error; // Re-throw the error to handle it in the calling code if needed
  }
};

const getBestelbonnen = async function (bedrijfId) {
  const data = await getData(
    `${baseUrl}/${bedrijfId}/ets/openpurchaseorderids`
  );
  if (data != null) {
    showBestelbonnen(data);
  } else {
    alert(
      "no data from api call with endpoint: " +
        `${baseUrl}/${bedrijfId}/ets/openpurchaseorderids`
    );
  }
};

const getBestelbon = async function (bestelbonId) {
  const data = await getData(
    `${baseUrl}/${htmlBedrijfSelect.value}/ets/purchaseorder?id=${bestelbonId}`
  );
  if (data != null) {
    showTabel(data);
  } else {
    alert(
      "no data from api call with endpoint: " +
        `${baseUrl}/${htmlBedrijfSelect.value}/ets/purchaseorder?id=${bestelbonId}`
    );
  }
};

const getBestelbonFile = async function (bestelbonId) {
  const data = await getData(
    `${baseUrl}/${htmlBedrijfSelect.value}/ets/createpurchasefile?id=${bestelbonId}`
  );
  if (data != null) {
    return data;
  } else {
    alert(
      "no data from api call with endpoint: " +
        `${baseUrl}/${htmlBedrijfSelect.value}/ets/createpurchasefile?id=${bestelbonId}`
    );
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

const getArtikel = async function (artikelNr) {
  let data = null;
  try {
    data = await getData(
      `${baseUrl}/devion/cebeo/searcharticle?articleReference=${artikelNr}`
    );
  } catch (error) {
    if (error.message == "API request failed with status: 500") {
      htmlArtikelNrSearch.classList.add("c-input--error");
      htmlArtikeNrSearchError.classList.remove("o-hide-accessible");
      htmlArtikelForm.classList.add("o-hide-accessible");
    } else {
      alert(
        "no data from api call with endpoint: " +
          `${baseUrl}/devion/cebeo/searcharticle?articleReference=${artikelNr}`
      );
    }
  }
  if (data != null) {
    showArtikel(data);
  }
};

const getFromInfo = async function () {
  try {
    const data = await getData(`${baseUrl}/devion/ets/articleforminfo`).catch(
      (error) => {
        if (error.status == 500) {
          alert("something went wrong with the data");
        } else {
          alert(
            "no data from api call with endpoint: " +
              `${baseUrl}/devion/ets/articleforminfo`
          );
        }
      }
    );
    if (data != null) {
      showFromInfo(data);
    } else {
      alert(
        "no data from api call with endpoint: " +
          `${baseUrl}/devion/ets/articleforminfo`
      );
    }
  } catch (error) {
    if (error.message == "API request failed with status: 500") {
      alert("artikel bestaat al in de database");
    } else if (error.message == "API request failed with status: 404") {
      alert(
        "no data from api call with endpoint: " +
          `${baseUrl}/devion/ets/articleforminfo`
      );
    } else {
      alert("something went wrong");
    }
  }
};
//#endregion

//#region *** Event Listeners - listenTo___ ***********
const listenToList = function () {
  htmlBestelbonSelect.addEventListener("change", function () {
    showButtons();
    getBestelbon(htmlBestelbonSelect.value);
  });

  htmlBedrijfSelect.addEventListener("change", function () {
    getBestelbonnen(htmlBedrijfSelect.value);
  });
};

const listenToButtons = function () {
  htmlButtonDownload.addEventListener("click", function () {
    getBestelbonFile(htmlBestelbonSelect.value).then((bon) => {
      var decodeString = atob(bon.fileContents);
      var blob = new Blob([decodeString], { type: bon.contentType });
      const a = document.createElement("a");
      a.href = URL.createObjectURL(blob);
      a.download = bon.fileDownloadName;
      document.body.appendChild(a);
      a.click();
      document.body.removeChild(a);
    });
  });
};

const listenToButtonsArtikels = function () {
  htmlArtikelSearch.addEventListener("click", function () {
    var reader = new FileReader();
    reader.readAsText(htmlFileInput.files[0]);
    reader.onload = function () {
      var header = reader.result.split("\r\n")[0];
      var artikels = [];
      for (var i = 1; i < reader.result.split("\n").length; i++) {
        var data = reader.result.split("\r\n")[i];
        var obj = {};
        for (var j = 0; j < header.split(", ").length; j++) {
          obj[header.split(", ")[j]] = data.split(", ")[j];
        }
        if (obj["artikelNr."] != "") {
          artikels.push(obj);
        }
      }
      console.log(artikels);
      var artikelNrs = [];
      for (const artikel of artikels) {
        artikelNrs.push(artikel["artikelNr."]);
      }
      console.log(artikelNrs);
      sessionStorage.setItem("artikelNrs", JSON.stringify(artikelNrs));
    };
    //getArtikel(htmlArtikelNrSearch.value);
  });
  htmlArtikelToeveogen.addEventListener("click", function () {
    getArtikel(htmlArtikelNrSearch.value);
  });
};

const listenToChangeWinst = function () {
  htmlWinstpercentage.addEventListener("change", function () {
    if (htmlWinstpercentage.value.includes("33.33")) {
      htmlVerkoopPrijs.value = parseFloat(
        htmlPrijs.value * (1 + 1 / 3)
      ).toFixed(4);
    } else {
      htmlVerkoopPrijs.value = parseFloat(
        htmlPrijs.value * (1 + htmlWinstpercentage.value / 100)
      ).toFixed(4);
    }
  });
};

const listenToChangeStdKorting = function () {
  htmlStdKorting.addEventListener("change", function () {
    htmlPrijs.value = parseFloat(
      htmlTariefPrijs.value * (1 - htmlStdKorting.value / 100)
    ).toFixed(4);
    if (htmlWinstpercentage.value.includes("33.33")) {
      htmlVerkoopPrijs.value = parseFloat(
        htmlPrijs.value * (1 + 1 / 3)
      ).toFixed(4);
    } else {
      htmlVerkoopPrijs.value = parseFloat(
        htmlPrijs.value * (1 + htmlWinstpercentage.value / 100)
      ).toFixed(4);
    }
  });
};

//#endregion

//#region *** Init / DOMContentLoaded ***********
const init = function () {
  console.log("DOM Content Loaded");

  htmlselectors();

  if (htmlButtonDownload) {
    listenToList();
    listenToButtons();
    getCompanies();
  }

  if (htmlArtikelSearch) {
    listenToButtonsArtikels();
    listenToChangeWinst();
    listenToChangeStdKorting();
  }
};

const htmlselectors = function () {
  htmlButtonDownload = document.querySelector(".js-button-download");
  htmlBestelbonSelect = document.querySelector(".js-bestelbon-select");
  htmlBedrijfSelect = document.querySelector(".js-bedrijf-select");
  htmltabel = document.querySelector(".js-tabel");
  htmlArtikelNrSearch = document.querySelector(".js-artikel-nr");
  htmlArtikeNrSearchError = document.querySelector(
    ".js-artikelnr-search-error"
  );
  htmlArtikelSearch = document.querySelector(".js-button-artikel-search");
  htmlArtikelNr = document.querySelector(".js-input-artikelnr");
  htmlReflevNr = document.querySelector(".js-input-reflevnr");
  htmlHoofdlev = document.querySelector(".js-input-hoofdlev");
  htmlOmschrijving = document.querySelector(".js-input-omschrijving");
  htmlPrijs = document.querySelector(".js-input-prijs");
  htmlFamilie = document.querySelector(".js-input-familie");
  htmlSubFamilie = document.querySelector(".js-input-subfamilie");
  htmlBtwcode = document.querySelector(".js-input-btwcode");
  htmlRekver = document.querySelector(".js-input-rekver");
  htmlLengte = document.querySelector(".js-input-lengte");
  htmlBreedte = document.querySelector(".js-input-breedte");
  htmlHoogte = document.querySelector(".js-input-hoogte");
  htmlAkPrijsEh = document.querySelector(".js-input-akprijseh");
  htmlVerEh = document.querySelector(".js-input-verbruikseh");
  htmlOmRekFac = document.querySelector(".js-input-omrekfac");
  htmlTypFac = document.querySelector(".js-input-typfac");
  htmlMuntcode = document.querySelector(".js-input-muntcode");
  htmlMerk = document.querySelector(".js-input-merk");
  htmlArtikelForm = document.querySelector(".js-artikel-form");
  htmlTariefPrijs = document.querySelector(".js-input-tarief");
  htmlVerkoopPrijs = document.querySelector(".js-input-verkoop");
  htmlWinstpercentage = document.querySelector(".js-input-winst");
  htmlLink = document.querySelector(".js-input-link");
  htmlLinkIcon = document.querySelector(".js-link-icon");
  htmlStdKorting = document.querySelector(".js-input-stdkorting");
  htmlArtikelToeveogen = document.querySelector(".js-button-artikel-toevoegen");
  htmlFileInput = document.querySelector(".js-file-input");
};
document.addEventListener("DOMContentLoaded", init);
//#endregion
