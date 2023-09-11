<template>
  <textInput :id="textInputArtikelSearch.id" :label="textInputArtikelSearch.label" :error="textInputArtikelSearch.error"
    :placeholder="textInputArtikelSearch.label" @option-selected="handleTextInputArtikelSearch" />
  <buttonDevion :label="searchButton.label" :isDisabled="searchButton.isButtonDisabled" @click="ArtikelZoeken"
    :showButton="searchButton.showButton" class="c-button-search" />
  <div class="c-artikel-form">
    <artikelForm :showform="artikelForm.showform" :data="artikelForm.data" @object-artikel="handleArtikel" class="c-form"
      :check="artikelForm.check" ref="article" />
    <div class="c-artikel-button--save">
      <buttonDevion :label="save.label" :isDisabled="save.isButtonDisabled" :showButton="save.showButton"
        @click="handleSaveButtonClick" class="c-button-artikel--save " />
    </div>
    <div class="c-artikel-button--next">
      <buttonDevion :label="next.label" :isDisabled="next.isButtonDisabled" :showButton="next.showButton"
        @click="handleNextButtonClick" class="c-button-artikel--next" />
    </div>
    <div class="c-artikel-button--prev">
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
    labelDevion
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
            console.log('Button clicked');
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
          artikelNrs = artikelString.split(', ')
        }
        if (artikelNrs != null) {
          if (artikels[index] == undefined) {
            endpoint = `devion/cebeo/searcharticle?articleReference=${artikelNrs[index]}`
            GetData(endpoint).then((data) => {
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
            })
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
        endpoint = `metabil/cebeo/createarticle`
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

.c-artikel-form {
  display: grid;
  width: 100vw;
  transform: translateX(-15vw);
  grid-template-columns: 1fr 1fr 1fr;
  grid-template-areas: "form form form" "prev progress next";
  grid-gap: var(--global-whitespace-lg);
  margin-top: var(--global-whitespace-lg);
  margin-bottom: var(--global-whitespace-lg);
}

.c-form {
  grid-area: form;
}

.c-artikel-button--save {
  grid-area: next;
  display: flex;
  margin-bottom: var(--global-whitespace-lg);
  justify-content: flex-end;
  margin-right: calc(var(--global-whitespace-lg) * 2.8)
}

.c-artikel-button--prev {
  grid-area: prev;
  display: flex;
  margin-bottom: var(--global-whitespace-lg);
  justify-content: flex-start;
}

.c-artikel-button--next {
  grid-area: next;
  margin-bottom: var(--global-whitespace-lg);
  display: flex;
  justify-content: flex-end;
  margin-right: calc(var(--global-whitespace-lg) * 2.8);
}

.c-button-artikel--prev,
.c-button-artikel--next,
.c-button-artikel--save {
  width: 50%;
}

.c-artikel-progress {
  grid-area: progress;
  display: flex;
  justify-content: center;
  align-items: center;
  margin-bottom: var(--global-whitespace-lg);
}

.c-artikel-progress__text {
  text-align: center;
  width: 100%;
  font-weight: bolder;
}
</style>