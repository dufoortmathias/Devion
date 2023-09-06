<template>
  <textInput :id="textInputArtikelSearch.id" :label="textInputArtikelSearch.label" :error="textInputArtikelSearch.error"
    :placeholder="textInputArtikelSearch.label" />
  <buttonDevion :label="searchButton.label" :isDisabled="searchButton.isButtonDisabled" @click="ArtikelZoeken" class="c-button-search"/>
  <artikelFrom :disabled="artikelForm.disabled" />
</template>

<script>
import textInput from '../components/componenten/textInput.vue';
import buttonDevion from '../components/componenten/ButtonDevion.vue';
import artikelFrom from '../components/ArtikelFrom.vue';


export default {
  components: {
    textInput,
    buttonDevion,
    artikelFrom,
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
          artikelFrom,
        },
        disabled: false,
      },
    };
  },
  methods: {
    ArtikelZoeken() {
      if (this.textInputArtikelSearch.value == null) {
        this.textInputArtikelSearch.error = true;
      } else {
        this.textInputArtikelSearch.error = false;
      }
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
</style>