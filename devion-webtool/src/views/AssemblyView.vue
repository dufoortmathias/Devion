<template>
    <excelFileInput :label="file.label" :error="file.error" :filename="file.filename" :showFile="file.showFile"
        @file-updated="handleFileUpdate" />
    <ButtonDevion :label="button.label" :isDisabled="button.isDisabled" :showButton="button.showButton"
        @click="handleButton" class="c-button" />
    <div v-if="loading.showLoad" class="c-load">
        <LoadingAnimation :showLoad="loading.showLoad"/>
    </div>
    <labelDevion :label="missing.label" :showLabel="missing.showlabel" class="c-artikel-missing" />
    <TreeView :jsonData="treeView.jsonData" :showTree="treeView.showTree" />
    <div v-if="treeView.showTree" class="c-buttons">
        <ButtonDevion :label="buttonSave.label" :isDisabled="buttonSave.isDisabled" :showButton="buttonSave.showButton"
            @click="handleButtonSave" class="c-button c-button-tree" />
        <ButtonDevion :label="buttonInsert.label" :isDisabled="buttonInsert.isDisabled"
            :showButton="buttonInsert.showButton" @click="handleButtonInsert" class="c-button c-button-tree" />
    </div>
    <div class="c-artikel-form">
        <artikelForm :showform="artikelForm.showform" :data="artikelForm.data" @object-artikel="handleArtikel"
            class="c-form" :check="artikelForm.check" ref="article" />
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
import ExcelFileInput from '../components/componenten/excelFileInput.vue';
import ButtonDevion from '../components/componenten/ButtonDevion.vue';
import TreeView from '../components/componenten/TreeView.vue';
import ArtikelForm from '../components/ArtikelForm.vue';
import LabelDevion from '../components/componenten/LabelDevion.vue';
import LoadingAnimation from '../components/componenten/LoadingAnimation.vue';

import { PostDataWithBody, PutDataWithBody } from '../global/global';

let FileContents = null
let FileName = null
let partsNotFound = []
let notFound = 0
let totalParts = 0
let index, index2, save
let artikels = []

export default {
    components: {
        ExcelFileInput,
        ButtonDevion,
        TreeView,
        ArtikelForm,
        LabelDevion,
        LoadingAnimation
    },
    data() {
        return {
            file: {
                components: {
                    ExcelFileInput
                },
                id: 'file',
                label: 'assembly file',
                error: false,
                showLabel: true,
                filename: 'Kies een bestand',
                showFile: true,
            },
            button: {
                components: {
                    ButtonDevion
                },
                label: 'IMPORTEREN',
                isDisabled: false,
                showButton: true
            },
            treeView: {
                components: {
                    TreeView
                },
                showTree: false,
                jsonData: null
            },
            buttonSave: {
                components: {
                    ButtonDevion
                },
                label: 'ARTIKELEN OPSLAAN',
                isDisabled: false,
                showButton: true
            },
            buttonInsert: {
                components: {
                    ButtonDevion
                },
                label: 'LINKEN',
                isDisabled: false,
                showButton: true
            },
            artikelForm: {
                components: {
                    ArtikelForm
                },
                showform: false,
                data: null,
                check: false
            },
            save: {
                components: {
                    ButtonDevion
                },
                label: 'OPSLAAN',
                isButtonDisabled: false,
                showButton: false
            },
            next: {
                components: {
                    ButtonDevion
                },
                label: 'VOLGENDE',
                isButtonDisabled: false,
                showButton: false
            },
            prev: {
                components: {
                    ButtonDevion
                },
                label: 'VORIGE',
                isButtonDisabled: false,
                showButton: false
            },
            artikelProgress: {
                components: {
                    LabelDevion
                },
                label: 'Artikel 1 van 1',
                showLabel: false
            },
            missing: {
                components: {
                    LabelDevion
                },
                label: 'Artikel niet gevonden 0/0',
                showlabel: false
            },
            loading: {
                components: {
                    LoadingAnimation
                },
                showLoad: false
            }
        }
    },
    methods: {
        handleFileUpdate(file) {
            this.file.error = false;
            this.file.showLabel = false;
            this.file.showButton = true;
            this.file.filename = file.name;
            this.file.file = file;

        },
        async handleButton() {
            this.loading.showLoad = true
            let endpoint = 'devion/ets/transformbomexcel'
            FileContents = this.file.file.previewBase64.replace('data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,', '')
            FileName = this.file.filename
            var data = {
                "FileContents": FileContents
            }
            endpoint += "?fileName=" + FileName
            PostDataWithBody(endpoint, data).then((response) => {
                let artikels = JSON.parse(response)
                this.loading.showLoad = false
                this.treeView.showTree = true
                this.treeView.jsonData = artikels
                notFound = 0
                totalParts = 0
                for (let artikel of artikels) {
                    this.checkArtikel(artikel)
                }
                if (notFound > 0) {
                    this.missing.showlabel = true
                } else {
                    this.missing.showlabel = false
                }
                this.missing.label = 'Artikels niet gevonden ' + notFound + '/' + totalParts
            })
        },
        handleButtonSave() {
            if (notFound > 0) {
                this.artikelForm.showform = true
                index = 89
                this.artikelForm.data = partsNotFound[index]
                this.save.showButton = true
                this.next.showButton = true
                this.prev.showButton = true
                this.artikelProgress.showLabel = true
                this.artikelProgress.label = 'Artikel 1/' + partsNotFound.length
                this.file.showFile = false
                this.button.showButton = false
                this.treeView.showTree = false
                this.buttonSave.showButton = false
                this.buttonInsert.showButton = false
                this.missing.showlabel = false
            }
        },
        checkArtikel(artikel) {
            totalParts++
            if (artikel.parts) {
                for (let part of artikel.parts) {
                    this.checkArtikel(part)
                }
            }
            if (artikel.existsETS == false) {
                if (partsNotFound.includes(artikel)) {
                    console.log('already in array')
                } else {
                    partsNotFound.push(artikel)
                    notFound++
                }
            }
        },
        async handleButtonInsert() {
            if (notFound > 0) {
                this.artikelForm.showform = true
                index = 0
                this.artikelForm.data = partsNotFound[index]
                this.save.showButton = false
                this.next.showButton = true
                this.prev.showButton = false
                this.artikelProgress.showLabel = true
                this.artikelProgress.label = 'Artikel 1/' + partsNotFound.length
                this.file.showFile = false
                this.button.showButton = false
                this.treeView.showTree = false
                this.buttonSave.showButton = false
                this.buttonInsert.showButton = false
                this.missing.showlabel = false
            } else {
                this.loading.showLoad = true
                PutDataWithBody('devion/ets/updatalinkedarticles', artikels).then((response) => {
                    console.log(response)
                    this.loading.showLoad = false
                })
            }
        },
        handleArtikel(object) {
            let artikel = object
            artikels[index] = artikel
            if (save == true) {
                // let endpoint = `metabil/cebeo/createarticle`
                // artikels.forEach(artikel => {
                //     PostDataWithBody(endpoint, artikel).then((data) => {
                //         console.log(data)
                //     })
                // });
                console.log("saved")
                this.artikelForm.showform = false
                this.save.showButton = false
                this.next.showButton = false
                this.prev.showButton = false
                this.artikelProgress.showLabel = false
                this.file.showFile = true
                this.button.showButton = true
                this.treeView.showTree = true
                this.buttonSave.showButton = true
                this.buttonInsert.showButton = true
                this.handleButton()
            } else {
                save = false
                index = index2
                if (index == partsNotFound.length - 1) {
                    this.next.showButton = false;
                    this.save.showButton = true;
                    this.artikelProgress.label = `Artikel ${index + 1}/${partsNotFound.length}`
                } else if (index == 0) {
                    this.next.showButton = true;
                    this.save.showButton = false;
                    this.prev.showButton = false;
                    this.artikelProgress.label = `Artikel ${index + 1}/${partsNotFound.length}`
                } else {
                    this.next.showButton = true;
                    this.save.showButton = false;
                    this.prev.showButton = true;
                    this.artikelProgress.label = `Artikel ${index + 1}/${partsNotFound.length}`
                }
                this.ArtikelZoeken()
            }
        },
        ArtikelZoeken() {
            this.artikelForm.showform = true;
            this.artikelForm.data = partsNotFound[index]
            this.artikelForm.check = true
            this.file.showFile = false
            this.button.showButton = false
            this.treeView.showTree = false
            this.buttonSave.showButton = false
            this.buttonInsert.showButton = false
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
    }
}
</script>

<style scoped>
.c-button {
    margin-top: var(--global-whitespace-lg);
    margin-bottom: var(--global-whitespace-lg);
    cursor: pointer;
}

.c-buttons {
    display: flex;
    justify-content: space-between;
}

.c-button-tree {
    width: 48%;
}

.c-artikel-missing {
    color: var(--global-color-error);
    font-size: var(--global-font-size);
    font-weight: var(--global-font-weight-bold);
    margin-bottom: var(--global-baseline);
    padding-top: 11px;
}

.c-load {
    display: flex;
    justify-content: center;
}
</style>