<template>
    <excelFileInput :label="file.label" :error="file.error" :filename="file.filename" :showFile="file.showFile"
        @file-updated="handleFileUpdate" />
    <ButtonDevion :label="button.label" :isDisabled="button.isDisabled" :showButton="button.showButton"
        @click="handleButton" class="c-button" />
    <div v-if="loading.showLoad" class="c-load">
        <LoadingAnimation :showLoad="loading.showLoad" />
    </div>
    <ButtonDevion :label="test.label" :isDisabled="test.isDisabled" :showButton="test.showButton" @click="reveal"
        class="c-button" />
    <h2 v-show="treeView.showTree">
        Devion</h2>
    <labelDevion :label="missing.label" :showLabel="missing.showlabel" class="c-artikel-missing" />
    <TreeView :jsonData="treeView.jsonData" :showTree="treeView.showTree" />
    <div v-show="treeViewMet.showTree" class="c-metabil">
        <h2 class="c-title">Metabil</h2>
        <labelDevion :label="link.label" :showLabel="link.showlabel" class="c-link" />
        <BasicToggleSwitch v-model="LinkModel" class="c-toggle" />
        <textInput :id="mass.id" :label="mass.label" :error="mass.error" :placeholder="mass.placeholder"
            :errorText="mass.errorText" class="c-mass" @option-selected="handleMass" />
    </div>
    <labelDevion :label="missingMet.label" :showLabel="missingMet.showlabel" class="c-artikel-missing" />
    <TreeViewMet :jsonData="treeViewMet.jsonData" :showTree="treeViewMet.showTree" />
    <div v-if="treeView.showTree" class="c-buttons">
        <ButtonDevion :label="buttonSave.label" :isDisabled="buttonSave.isDisabled" :showButton="buttonSave.showButton"
            @click="handleButtonSave" class="c-button c-button-tree" />
        <ButtonDevion :label="buttonInsert.label" :isDisabled="buttonInsert.isDisabled"
            :showButton="buttonInsert.showButton" @click="handleButtonInsert" class="c-button c-button-tree" />
    </div>
    <LabelDevion :label="linked.label" :showLabel="linked.showlabel" class="c-linked" />
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
    <DialogsWrapper />
</template>

<script>
import ExcelFileInput from '../components/componenten/excelFileInput.vue';
import ButtonDevion from '../components/componenten/ButtonDevion.vue';
import TreeView from '../components/componenten/TreeView.vue';
import ArtikelForm from '../components/ArtikelForm.vue';
import LabelDevion from '../components/componenten/LabelDevion.vue';
import LoadingAnimation from '../components/componenten/LoadingAnimation.vue';
import DialogDevion from '../components/componenten/DialogDevion.vue';
import { createConfirmDialog } from "vuejs-confirm-dialog";
import TreeViewMet from '../components/componenten/TreeViewMet.vue';
import BasicToggleSwitch from '../components/componenten/ToggleButton.vue';
import textInput from '../components/componenten/textInput.vue';
// import { useConfirmBeforeAction } from '../composables';
import { ref } from 'vue'

import { GetData, PostDataWithBody, PutDataWithBody } from '../global/global';

let FileContents = null
let FileName = null
let partsNotFound = []
let notFound = 0
let totalParts = 0
let partsNotFoundMet = []
let notFoundMet = 0
let totalPartsMet = 0
let index, index2, save
let artikelen = []
let artikelenMet = []
let artikels
let test = "merk"
const LinkModel = ref(true)

export default {
    components: {
        ExcelFileInput,
        ButtonDevion,
        TreeView,
        ArtikelForm,
        LabelDevion,
        LoadingAnimation,
        TreeViewMet,
        BasicToggleSwitch,
        textInput
    },
    data() {
        return {
            LinkModel: true,
            mass: {
                components: {
                    textInput,
                },
                id: 'mass',
                label: 'prijs/kg',
                error: false,
                placeholder: 'euro',
                errorText: 'mass prijs is verplicht'
            },
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
            test: {
                components: {
                    ButtonDevion
                },
                id: 'test',
                label: "test",
                isDisabled: false,
                showButton: false
            },
            treeView: {
                components: {
                    TreeView
                },
                showTree: false,
                jsonData: []
            },
            treeViewMet: {
                components: {
                    TreeViewMet
                },
                showTree: false,
                jsonData: []
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
            missingMet: {
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
            },
            linked: {
                components: {
                    LabelDevion
                },
                label: 'artikelen gekoppeld',
                showlabel: false
            },
            link: {
                components: {
                    LabelDevion
                },
                label: 'Link Metabil: ',
                showlabel: true
            },
        }
    },
    setup() {
        const { reveal, onConfirm } = createConfirmDialog(DialogDevion, {
            question: "Are you sure you want to change " + test + "?",
        })

        onConfirm(() => {
            console.log('confirm')
        })
        return { reveal }
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
                artikelen.push(artikels)
                console.log(artikelen)
                this.loading.showLoad = false
                this.treeView.showTree = true
                this.treeView.jsonData = Array.from(artikelen)
                console.log(this.treeView.jsonData)
                this.treeViewMet.showTree = true
                notFound = 0
                totalParts = 0
                for (let artikel of artikelen) {
                    if (artikel) {
                        this.checkArtikel(artikel, false)
                    }
                }
            })
        },
        handleButtonSave() {
            if (notFound > 0) {
                this.artikelForm.showform = true
                this.save.showButton = false
                this.next.showButton = true
                this.prev.showButton = true
                this.artikelProgress.showLabel = true
                this.artikelProgress.label = 'Artikel 1/' + partsNotFound.length
                this.file.showFile = false
                this.button.showButton = false
                this.treeView.showTree = false
                this.treeViewMet.showTree = false
                this.buttonSave.showButton = false
                this.buttonInsert.showButton = false
                this.missing.showlabel = false
                this.missingMet.showlabel = false
            }
        },
        checkArtikel(artikel, metabil) {
            totalParts++
            if (artikel) {
                if (artikel.number.charAt(artikel.number.length - 1) == 'W') {
                    metabil = true
                }
                if (artikel.parts) {
                    for (let part of artikel.parts) {
                        this.checkArtikel(part, metabil)
                    }
                }
                if (artikel.bewerking1.toUpperCase() == "LASSEN" || artikel.bewerking2.toUpperCase() == "LASSEN" || artikel.bewerking3.toUpperCase() == "LASSEN" || artikel.bewerking4.toUpperCase() == "LASSEN") {
                    metabil = true
                } else if (metabil == true) {
                    metabil = true
                } else {
                    metabil = false
                }
                
                if (metabil == false || artikel.number.charAt(artikel.number.length - 1) == 'W')
                    GetData(`devion/ets/articleexists?ArticleNumber=` + artikel.number).then((result) => result).then((data) => {
                        artikel.existsDev = data
                        if (artikel.existsDev == false) {
                            if (partsNotFound.includes(artikel)) {
                                console.log('already in array')
                            } else {
                                partsNotFound.push(artikel)
                                notFound++
                            }
                        }

                        if (notFound > 0) {
                            this.missing.showlabel = true
                        } else {
                            this.missing.showlabel = false
                        }
                        this.missing.label = 'Artikels niet gevonden ' + notFound + '/' + totalParts
                    })

                if (metabil == true) {
                    GetData('metabil/ets/articleexists?ArticleNumber=' + artikel.number).then((result) => result).then((data) => {
                        totalPartsMet++
                        artikel.existsMet = data
                        if (artikel.existsMet == false) {
                            if (partsNotFoundMet.includes(artikel)) {
                                console.log('already in array')
                            } else {
                                partsNotFoundMet.push(artikel)
                                notFoundMet++
                            }
                        }
                        if (artikel.number.charAt(artikel.number.length - 1) == 'W') {
                            artikelenMet.push(artikel)
                            // Find the index of the item in artikelen based on the number property
                            // Find the index of the item in artikelen based on the number property
                            let i = artikelen[0].parts.findIndex(item => item.number === artikel.number);
                            console.log("Index:", i);

                            console.log(this.treeView.jsonData[0].parts[i])

                            // Check if the item exists at the found index in this.treeView.jsonData
                            if (i !== -1 && this.treeView.jsonData[i]) {
                                // Create a deep copy of the item you're about to remove
                                const retainedItem = JSON.parse(JSON.stringify(this.treeView.jsonData[0].parts[i]));

                                // Set the parts array of the item to an empty array
                                this.treeView.jsonData[0].parts[i].parts = [];

                                console.log("Updated this.treeView.jsonData:", this.treeView.jsonData);

                                // Now, you can add the deep-copied retainedItem to this.treeViewMet.jsonData
                                this.treeViewMet.jsonData.push(retainedItem);

                                console.log("Updated this.treeViewMet.jsonData:", this.treeViewMet.jsonData);
                            } else {
                                console.log("Item not found or already removed.");
                            }
                        }

                        if (notFoundMet > 0) {
                            this.missingMet.showlabel = true
                        } else {
                            this.missingMet.showlabel = false
                        }
                        this.missingMet.label = 'Artikels niet gevonden ' + notFoundMet + '/' + totalPartsMet
                    })
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
                this.treeView.showTree = false
                this.buttonSave.showButton = false
                this.buttonInsert.showButton = false
                this.missing.showlabel = false
                this.missingMet.showlabel = false
            } else {
                this.loading.showLoad = true
                PutDataWithBody('devion/ets/updatelinkedarticles', artikelen).then((response) => {
                    console.log(response)
                    this.loading.showLoad = false
                    this.linked.showlabel = true
                }).catch((error) => {
                    console.error(error)
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
        handleTest() { },
        handleMass() { }
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

.c-linked {
    display: flex;
    justify-content: flex-end;
}

.c-metabil {
    display: grid;
    grid-template-areas: "title link toggle mass";
    grid-template-columns: 2fr 1fr 1fr 2fr 4fr;
    align-items: baseline;
}

.c-title {
    grid-area: "title"
}

.c-link {
    grid-area: "link";
}

.c-toggle {
    grid-area: "toggle"
}

.c-mass {
    grid-area: "mass"
}
</style>