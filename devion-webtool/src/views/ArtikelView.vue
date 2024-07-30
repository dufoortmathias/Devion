<template>
  <h1>artikel</h1>
  <textInput :id="textInputArtikelSearch.id" :label="textInputArtikelSearch.label" :error="textInputArtikelSearch.error"
    :errorText="textInputArtikelSearch.errorText" :placeholder="textInputArtikelSearch.label"
    @option-selected="handleTextInputArtikelSearch" />
  <buttonDevion :label="searchButton.label" :isDisabled="searchButton.isButtonDisabled" @click="ArtikelZoeken"
    :showButton="searchButton.showButton" class="c-button-search" />
  <div v-if="loading.showLoad" class="c-load">
    <LoadingAnimation :showLoad="loading.showLoad" />
  </div>
  <div class="c-artikel-form">
    <artikelForm :showform="artikelForm.showform" :data="artikelForm.data" @object-artikel="handleArtikel" class="c-form"
      :check="artikelForm.check" ref="article" />
    <div class="c-artikel-button--save" :class="{ 'o-hide-accessible': !save.showButton }">
      <buttonDevion :label="save.label" :isDisabled="save.isButtonDisabled" :showButton="save.showButton"
        @click="handleSaveButtonClick" class="c-button-artikel--next" />
    </div>
    <div class="c-artikel-button--next" :class="{ 'o-hide-accessible': !next.showButton }">
      <buttonDevion :label="next.label" :isDisabled="next.isButtonDisabled" :showButton="next.showButton"
        @click="handleNextButtonClick" class="c-button-artikel--next" />
    </div>
    <div class="c-artikel-button--prev" :class="{ 'o-hide-accessible': !prev.showButton }">
      <buttonDevion :label="prev.label" :isDisabled="prev.isButtonDisabled" :showButton="prev.showButton"
        @click="handlePrevButtonClick" class="c-button-artikel--prev" />
    </div>
    <div class="c-artikel-progress">
      <labelDevion :label="artikelProgress.label" :showLabel="artikelProgress.showLabel"
        class="c-artikel-progress__text" />
    </div>
  </div>
</template>

<script>
import textInput from '../components/componenten/textInput.vue';
import buttonDevion from '../components/componenten/ButtonDevion.vue';
import artikelForm from '../components/ArtikelForm.vue';
import labelDevion from '../components/componenten/LabelDevion.vue';
import LoadingAnimation from '../components/componenten/LoadingAnimation.vue';
import { GetData, PostDataWithBody } from '../global/global.js';

let artikelSearch = ""
let endpoint = ""
let artikelNrs = []
let index = 0
let artikel = null
let index2 = 0
let artikels = []
let save = false

export default {
  components: {
    textInput,
    buttonDevion,
    artikelForm,
    labelDevion,
    LoadingAnimation
  },
  data() {
    return {
      textInputArtikelSearch: {
        components: {
          textInput,
        },
        id: 'artikelSearch',
        label: 'Artikel nummer',
        error: false,
        errorText: 'Artikel niet gevonden',
      },
      searchButton: {
        components: {
          buttonDevion,
        },
        label: 'artikel(s) zoeken',
        isButtonDisabled: false,
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
      artikelForm: {
        components: {
          artikelForm,
        },
        showform: false,
        data: null,
      },
      save: {
        components: {
          buttonDevion,
        },
        label: 'Opslaan',
        isButtonDisabled: false,
        showButton: false,
      },
      next: {
        components: {
          buttonDevion,
        },
        label: 'Volgende',
        isButtonDisabled: false,
        showButton: false,
      },
      prev: {
        components: {
          buttonDevion,
        },
        label: 'Vorige',
        isButtonDisabled: false,
        showButton: false,
      },
      artikelProgress: {
        components: {
          labelDevion,
        },
        label: 'Artikel 1/1',
        showLabel: false,
      },
      loading: {
        components: {
          LoadingAnimation,
        },
        showLoad: false,
      },
    };
  },
  beforeUnmount() {
    artikelSearch = ""
    endpoint = ""
    artikelNrs = []
    index = 0
    artikel = null
    index2 = 0
    artikels = []
  },
  methods: {
    ArtikelZoeken() {
      if (artikelSearch == null) {
        this.textInputArtikelSearch.error = true;
      } else {
        this.textInputArtikelSearch.error = false;
        let artikelString = artikelSearch.toString()
        if (artikelSearch != '') {
          artikelNrs = [...new Set(artikelString.split(', '))]
        }
        if (artikelNrs != null) {
          if (artikels[index] == undefined) {
            this.loading.showLoad = true;
            endpoint = `devion/cebeo/searcharticle?articleReference=${artikelNrs[index]}`
            GetData(endpoint).then((data) => {
              console.log(data)
              if (data.status) {
                this.textInputArtikelSearch.error = true;
                this.textInputArtikelSearch.errorText = data.detail;
              } else {
                this.artikelForm.data = data
                this.artikelForm.showform = true;
                if (artikelNrs.length > 1) {
                  if (index == artikelNrs.length - 1) {
                    this.next.showButton = false;
                    this.save.showButton = true;
                    this.artikelProgress.showLabel = true;
                    this.artikelProgress.label = `Artikel ${index + 1}/${artikelNrs.length}`
                  } else {
                    this.next.showButton = true;
                    this.save.showButton = false;
                    this.prev.showButton = true;
                    this.artikelProgress.showLabel = true;
                    this.artikelProgress.label = `Artikel ${index + 1}/${artikelNrs.length}`
                  }
                } else {
                  this.artikelProgress.showLabel = false;
                  this.save.showButton = true;
                  this.next.showButton = false;
                  this.prev.showButton = false;
                }
              }
            }).catch(error => {
              this.textInputArtikelSearch.error = true;
              this.textInputArtikelSearch.errorText = error.message;
            });
          } else {
            this.artikelForm.data = artikels[index]
            this.artikelForm.showform = true;
            if (artikelNrs.length > 1) {
              if (index == artikelNrs.length - 1) {
                this.next.showButton = false;
                this.save.showButton = true;
                this.artikelProgress.showLabel = true;
                this.artikelProgress.label = `Artikel ${index + 1}/${artikelNrs.length}`
              } else {
                this.next.showButton = true;
                this.save.showButton = false;
                this.prev.showButton = true;
                this.artikelProgress.showLabel = true;
                this.artikelProgress.label = `Artikel ${index + 1}/${artikelNrs.length}`
              }
            } else {
              this.artikelProgress.showLabel = false;
              this.save.showButton = true;
              this.next.showButton = false;
              this.prev.showButton = false;
            }
          }
        }
      }
    },
    handleTextInputArtikelSearch(selectedOption) {
      artikelSearch = selectedOption;
    },
    handleArtikel(object) {
      artikel = object
      artikels[index] = artikel
      if (save == true) {
        endpoint = `devion/ets/validatearticleform`
        artikels.forEach(artikel => {
          PostDataWithBody(endpoint, artikel).then((data) => {
            console.log(data)
          }) 
        });
      } else {
        save = false
        index = index2
        if (index == artikelNrs.length - 1) {
          this.next.showButton = false;
          this.save.showButton = true;
          this.artikelProgress.label = `Artikel ${index + 1}/${artikelNrs.length}`
        } else if (index == 0) {
          this.next.showButton = true;
          this.save.showButton = false;
          this.prev.showButton = false;
          this.artikelProgress.label = `Artikel ${index + 1}/${artikelNrs.length}`
        } else {
          this.next.showButton = true;
          this.save.showButton = false;
          this.prev.showButton = true;
          this.artikelProgress.label = `Artikel ${index + 1}/${artikelNrs.length}`
        }
        this.ArtikelZoeken()
      }
    },
    handleNextButtonClick() {
      index2 = index + 1
      this.$refs.article.createInfoObject()
    },
    handlePrevButtonClick() {
      index2 = index - 1
      this.$refs.article.createInfoObject()
    },
    handleSaveButtonClick() {
      console.log("opslaan")
      save = true
      this.$refs.article.createInfoObject()
    },
  },
};
</script>

<style scoped>
.c-button-search {
  margin-top: var(--global-whitespace-lg);
  margin-bottom: var(--global-whitespace-lg);
  cursor: pointer;
}

.c-load {
    display: flex;
    justify-content: center;
}
</style>