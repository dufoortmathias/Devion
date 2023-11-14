<template>
  <div>
    <Dropdown :id="dropdownCompanies.id" :label="dropdownCompanies.label" :options="dropdownCompanies.options"
      :error="dropdownCompanies.error" @option-selected="handledropdownCompaniesSelected" class="c-dropdown"
      :selected="dropdownCompanies.selected" />
    <Dropdown :id="dropdownBestelbon.id" :label="dropdownBestelbon.label" :options="dropdownBestelbon.options"
      :error="dropdownBestelbon.error" @option-selected="handledropdownBestelbonSelected" class="c-dropdown" />
    <textInput :id="seperator.id" :label="seperator.label" :error="seperator.error" :placeholder="seperator.label"
      :errorText="seperator.errorText" @option-selected="handletextInputSelected" class="c-text-input" />
    <checkboxInput :id="checkboxInput.id" :label="checkboxInput.label" :placeholder="checkboxInput.label"
      @option-selected="handlecheckboxInputSelected" class="c-checkbox-input" />
    <ButtonDevion :label="buttonDevion.label" :isDisabled="buttonDevion.isButtonDisabled" @click="BestelbonDownload"
      class="c-button-artikel-search" :showButton="buttonDevion.showButton" />
    <div v-if="loading.showLoad" class="c-load">
      <LoadingAnimation :showLoad="loading.showLoad" />
    </div>
    <labelDevion :label="error.label" :showLabel="error.showLabel" class="c-artikel-error" />
    <TabelBestelbon :showTabel="tabelBestelbon.showTabel" :bestelbonNr="tabelBestelbon.bestelbonNr"
      :showError="tabelBestelbon.showError" :showInfo="tabelBestelbon.showInfo" :artikels="tabelBestelbon.artikels" />
  </div>
</template>

<script>
import Dropdown from '../components/componenten/DropdownMenu.vue';
import ButtonDevion from '../components/componenten/ButtonDevion.vue';
import TabelBestelbon from '../components/componenten/TabelBestelbonDevion.vue';
import { GetData } from '../global/global.js'
import textInput from '../components/componenten/textInput.vue'
import LoadingAnimation from '../components/componenten/LoadingAnimation.vue'
import labelDevion from '../components/componenten/LabelDevion.vue'
import checkboxInput from '../components/componenten/CheckboxInput.vue'

let options = [];
let endpoint = 'companies'
let company = "";
let bestelbonNr = "";
let seperator = "";
let forceCSV = false;

export default {
  components: {
    Dropdown,
    ButtonDevion,
    TabelBestelbon,
    textInput,
    LoadingAnimation,
    labelDevion,
    checkboxInput
  },
  data() {
    return {
      dropdownCompanies: {
        components: {
          Dropdown,
        },
        id: 'dropdownCompanies',
        label: 'Bedrijf',
        options: options,
        error: false,
      },
      dropdownBestelbon: {
        components: {
          Dropdown,
        },
        id: 'dropdownBestelbon',
        label: 'Bestelbon',
        options: [],
        error: false,
      },
      buttonDevion: {
        components: {
          ButtonDevion,
        },
        label: 'Download bestelbon',
        isButtonDisabled: true,
        showButton: true,
        methods: {
          handleButtonClick() {
            // Handle button click event here
          },
          toggleButtonState() {
            this.isButtonDisabled = !this.isButtonDisabled;
          },
        },
      },
      tabelBestelbon: {
        components: {
          TabelBestelbon,
        },
        showTabel: false,
        bestelbonNr: "",
        showError: false,
        showInfo: true,
        artikels: [],
      },
      seperator: {
        components: {
          textInput,
        },
        id: 'seperator',
        label: 'Seperator',
        options: [],
        error: false,
        errorText: 'Seperator is verplicht',
      },
      loading: {
        components: {
          LoadingAnimation,
        },
        showLoad: false,
      },
      error: {
        components: {
          labelDevion,
        },
        label: 'Bestelbon niet gevonden',
        showLabel: false,
      },
      checkboxInput: {
        components: {
          checkboxInput,
        },
        id: 'checkboxInput',
        label: 'Force csv',
      },
    };
  },
  created() {
    // Fetch data from the API here
    GetData(endpoint).then((data) => {
      return data
    }).then((data) => {
      const options = []
      for (var element of data) {
        options.push({ value: element, label: element })
      }
      this.dropdownCompanies.options = options
      this.dropdownCompanies.selected = this.dropdownCompanies.options.find((x) => x.label.toLowerCase() == "devion").value
      company = this.dropdownCompanies.selected
      endpoint = `${company}/ets/openpurchaseorderids`
      GetData(endpoint).then((data) => {
        return data
      }).then((data) => {
        data.sort()
        data.reverse()
        for (var element of data) {
          this.dropdownBestelbon.options.push({ value: element, label: element })
        }
      })
    })
  },
  beforeUnmount() {
    // Clean up before component is destroyed
    options = [];
    endpoint = 'companies'
    company = "";
    bestelbonNr = "";
    seperator = "";
  },
  methods: {
    async handledropdownCompaniesSelected(selectedOption) {
      // Update options for the second dropdown based on the selection in the first dropdown
      company = selectedOption
      endpoint = `${company}/ets/openpurchaseorderids`
      GetData(endpoint).then((data) => {
        return data
      }).then((data) => {
        data.sort()
        data.reverse()
        for (var element of data) {
          this.dropdownBestelbon.options.push({ value: element, label: element })
        }
      })
    },
    async handledropdownBestelbonSelected(selectedOption) {
      bestelbonNr = selectedOption
      this.buttonDevion.isButtonDisabled = false
      const params = {
        id: selectedOption
      }
      endpoint = `${company}/ets/purchaseorder?${new URLSearchParams(params)}`
      this.tabelBestelbon.showTabel = false
      this.loading.showLoad = true
      GetData(endpoint).then((data) => {
        this.loading.showLoad = false
        return data
      }).then((data) => {
        if (data != null) {
          this.tabelBestelbon.showTabel = true
          this.tabelBestelbon.bestelbonNr = data.bonNummer
          if (data.artikels.length == 0) {
            this.tabelBestelbon.showError = true
            this.tabelBestelbon.showInfo = false
          } else {
            this.tabelBestelbon.showError = false
            this.tabelBestelbon.showInfo = true
            let artikels = []
            let leveranciers = []
            for (const bestelbon of data.artikels) {
              if (!leveranciers.includes(bestelbon.leverancier)) {
                leveranciers.push(bestelbon.leverancier);
              }
            }

            for (const leverancier of leveranciers) {
              let artikelsLeverancier = []
              for (const bestelbon of data.artikels) {
                if (bestelbon.leverancier == leverancier) {
                  if (bestelbon.artikelNummer != 'VERZENDING' || bestelbon.omschrijving != 'VERZENDING') {
                    artikelsLeverancier.push({ artikelNummer: bestelbon.artikelNummer, aantal: bestelbon.aantal, omschrijving: bestelbon.omschrijving })
                  }
                }
              }
              artikels.push({ leverancier: { name: leverancier, artikels: artikelsLeverancier } })
            }
            this.tabelBestelbon.artikels = artikels
          }
        } else {
          this.tabelBestelbon.showTabel = false
        }
      })

    },
    handletextInputSelected(selectedOption) {
      if (selectedOption == null) {
        seperator = ' '
      } else if (selectedOption.includes('\\t')) {
        seperator = selectedOption.replace('\\t', '\t')
      } else {
        seperator = selectedOption
      }
    },
    async BestelbonDownload() {
      if (seperator == "") {
        this.seperator.error = true
        this.seperator.errorText = "Seperator is verplicht"
      } else {
        this.seperator.error = false
        const params = {
          id: bestelbonNr,
          seperator: seperator,
          forceCSV: forceCSV
        }
        endpoint = `${company.toLocaleLowerCase()}/ets/createpurchasefile?${new URLSearchParams(params)}`
        GetData(endpoint).then((bon) => {
          console.log(bon)
          if (bon.status) {
            this.error.showLabel = true
            this.error.label = bon.detail
            return
          } else {
            this.error.showLabel = false
          }
          var decodeString = atob(bon.fileContents);
          var blob = new Blob([decodeString], { type: bon.contentType });
          const a = document.createElement("a");
          a.href = URL.createObjectURL(blob);
          a.download = bon.fileDownloadName;
          document.body.appendChild(a);
          a.click();
          document.body.removeChild(a);
        })
      }
    },
    handlecheckboxInputSelected(selectedOption) {
      console.log(selectedOption)
      forceCSV = selectedOption
    },
  },
};
</script>

<style scoped>
.c-dropdown {
  margin-bottom: var(--global-baseline);
}

.c-button-artikel-search {
  margin-top: var(--global-whitespace-lg);
  margin-bottom: var(--global-whitespace-lg);
  cursor: pointer;
}

.c-load {
    display: flex;
    justify-content: center;
}

.c-artikel-error {
  color: var(--global-color-error);
  font-size: 32px;
}
</style>