<template>
  <div>
    <Dropdown :id="dropdownCompanies.id" :label="dropdownCompanies.label" :options="dropdownCompanies.options"
      :error="dropdownCompanies.error" @option-selected="handledropdownCompaniesSelected" class="c-dropdown" />
    <Dropdown :id="dropdownBestelbon.id" :label="dropdownBestelbon.label" :options="dropdownBestelbon.options"
      :error="dropdownBestelbon.error" @option-selected="handledropdownBestelbonSelected" class="c-dropdown" />
    <textInput :id="seperator.id" :label="seperator.label" :error="seperator.error" :placeholder="seperator.label"
      @option-selected="handletextInputSelected" class="c-text-input" />
    <ButtonDevion :label="buttonDevion.label" :isDisabled="buttonDevion.isButtonDisabled" @click="BestelbonDownload"
      class="c-button-artikel-search" />
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

let options = [];
let endpoint = 'companies'
let company = "";
let bestelbonNr = "";
let seperator = "";

export default {
  components: {
    Dropdown,
    ButtonDevion,
    TabelBestelbon,
    textInput
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
        methods: {
          handleButtonClick() {
            // Handle button click event here
            console.log('Button clicked');
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
      },
    };
  },
  created() {
    // Fetch data from the API here
    GetData(endpoint).then((data) => {
      return data
    }).then((data) => {
      console.log(data)
      const options = []
      for (var element of data) {
        options.push({ value: element, label: element })
      }
      this.dropdownCompanies.options = options
    })
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
      GetData(endpoint).then((data) => {
        return data
      }).then((data) => {
        console.log(data)
        if (data != null) {
          this.tabelBestelbon.showTabel = true
          this.tabelBestelbon.bestelbonNr = data.bonNummer
          if (data.artikels.length == 0) {
            this.tabelBestelbon.showError = true
            this.tabelBestelbon.showInfo = false
          } else {
            console.log(data)
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
      console.log(selectedOption)
      if (selectedOption == null) {
        seperator = ' '
      } else if (selectedOption.includes('\\t')) {
        seperator = selectedOption.replace('\\t', '\t')
      } else {
        seperator = selectedOption
      }
    },
    async BestelbonDownload() {
      const params = {
        id: bestelbonNr,
        seperator: seperator
      }
      endpoint = `${company.toLocaleLowerCase()}/ets/createpurchasefile?${new URLSearchParams(params)}`
      console.log(endpoint)
      GetData(endpoint).then((bon) => {
        console.log(bon)
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
</style>