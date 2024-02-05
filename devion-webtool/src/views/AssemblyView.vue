<template>
    <excelFileInput :label="file.label" :error="file.error" :filename="file.filename" :showFile="file.showFile"
        @file-updated="handleFileUpdate" />
    <ButtonDevion :label="button.label" :isDisabled="button.isDisabled" :showButton="button.showButton"
        @click="handleButton" class="c-button" />
    <div v-if="loading.showLoad" class="c-load">
        <LoadingAnimation :showLoad="loading.showLoad" />
    </div>
    <div v-show="treeView.showTree" class="c-dev">
        <h2 class="c-dev-title">Devion</h2>
        <textInput :id="mass.id" :label="mass.label" :error="mass.error" :placeholder="mass.placeholder"
            :errorText="mass.errorText" class="c-mass" @option-selected="handleMass" />
        <ButtonDevion :label="changeDev.label" :isDisabled="changeDev.isDisabled" :showButton="changeDev.showButton"
            @click="() => TogglePopup('changeDevion')" class="c-button c-change-dev" />
    </div>
    <labelDevion :label="missing.label" :showLabel="missing.showlabel" class="c-artikel-missing" />
    <PopupDevion :TogglePopup="() => TogglePopup('changeDevion')" :data="changeDev.data" v-show="popupTriggers.changeDevion"
        @change-items="updateItemsDev" />
    <TreeView :jsonData="treeView.jsonData" :showTree="treeView.showTree" />
    <div v-show="treeViewMet.showTree" class="c-metabil">
        <h2 class="c-title">Metabil</h2>
        <labelDevion :label="link.label" :showLabel="link.showlabel" class="c-link" />
        <BasicToggleSwitch v-model="LinkModel" class="c-toggle" />
        <buttonDevion :label="changeMet.label" :isDisabled="changeMet.isDisabled" :showButton="changeMet.showButton"
            @click="() => TogglePopup('changeMetabil')" class="c-button c-change-met" />
    </div>
    <PopupDevion :TogglePopup="() => TogglePopup('changeMetabil')" :data="changeMet.data"
        v-show="popupTriggers.changeMetabil" @change-items="updateItemsMet" />
    <labelDevion :label="missingMet.label" :showLabel="missingMet.showlabel && LinkModel" class="c-artikel-missing" />
    <TreeViewMet :jsonData="treeViewMet.jsonData" :showTree="treeViewMet.showTree && LinkModel" />
    <div v-if="treeView.showTree || treeViewMet.showTree" class="c-buttons">
        <ButtonDevion :label="buttonSave.label" :isDisabled="buttonSave.isDisabled" :showButton="buttonSave.showButton"
            @click="handleButtonSave" class="c-button c-button-tree" />
        <ButtonDevion :label="buttonInsert.label" :isDisabled="buttonInsert.isDisabled"
            :showButton="buttonInsert.showButton" @click="handleButtonInsert" class="c-button c-button-tree" />
    </div>
    <LabelDevion :label="linked.label" :showLabel="linked.showlabel" class="c-linked" />
    <TitleDevion :title="titleDevion.title" v-show="artikelForm.showform" class="c-form-title" />
    <div class="c-artikel-form">

        <artikelForm :showform="artikelForm.showform" :data="artikelForm.data" @object-artikel="handleArtikel"
            :mass="artikelForm.mass" class="c-form" :check="artikelForm.check" ref="article" />
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
import PopupDevion from '../components/componenten/PopupDevion.vue';
import TitleDevion from '../components/componenten/TitleDevion.vue';
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
let index, index2, save, metIndex, metIndex2
let metSave = false
let metArtikel = false
let artikelen = []
let artikelenMet = []
let artikels
let test = "merk"
const LinkModel = ref(true)
let changesDev = []
let changesMet = []
const popupTriggers = ref({
    changeDevion: false,
    changeMetabil: false
})
let logs = []
let mass = parseFloat(0).toFixed(2)

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
        textInput,
        PopupDevion,
        TitleDevion
    },
    data() {
        return {
            artikelenMet: Array(),
            popupTriggers,
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
                isDisabled: true,
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
            titleDevion: {
                components: {
                    TitleDevion
                },
                title: 'Devion',
                showTitle: false
            },
            artikelForm: {
                components: {
                    ArtikelForm
                },
                showform: false,
                data: null,
                check: false,
                mass: 0
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
            changeDev: {
                components: {
                    ButtonDevion,
                    PopupDevion
                },
                label: 'Changes',
                isButtonDisabled: false,
                showButton: false,
                data: changesDev
            },
            changeMet: {
                components: {
                    ButtonDevion,
                    PopupDevion
                },
                label: 'Changes',
                isButtonDisabled: false,
                showButton: false,
                data: changesMet
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
    watch: {
        artikelenMet: function (newValue) {
            for (let artikel of newValue) {
                this.removeArtikel(this.treeView.jsonData, artikel)
                this.$forceUpdate()
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
            this.button.isDisabled = false;
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
                this.button.isDisabled = true;
                let artikelMetDev = JSON.parse(response)
                let artikelDev = artikelMetDev[0][0]
                artikelenMet = []
                for (let item of artikelMetDev[1]) {
                    artikelenMet.push(item)
                }
                this.loading.showLoad = false
                artikelen = []
                artikelen.push(artikelDev)
                this.treeView.showTree = true
                this.treeView.jsonData = Array.from(artikelen)
                changesDev = []
                changesMet = []
                notFound = 0
                totalParts = 0
                for (let artikel of artikelen) {
                    if (artikel) {
                        this.checkArtikel(artikel)
                    }
                }
                if (artikelenMet.length > 0) {
                    this.treeViewMet.showTree = true
                    this.treeViewMet.jsonData = Array.from(artikelenMet)
                    notFoundMet = 0
                    totalPartsMet = 0
                    for (let artikel of artikelenMet) {
                        if (artikel) {
                            this.checkArtikelMet(artikel)
                        }
                    }
                } else {
                    this.treeViewMet.showTree = false
                }
            })
        },
        handleButtonSave() {
            if (partsNotFound == 0){
                metArtikel = true
            } else {
                metArtikel = false
            }
            if (!metArtikel) {
                this.artikelForm.showform = true
                this.save.showButton = false
                this.next.showButton = true
                this.prev.showButton = true
                index = 0
                this.artikelProgress.showLabel = true
                this.artikelForm.data = partsNotFound[0]
                this.artikelProgress.label = 'Artikel 1/' + partsNotFound.length
                this.file.showFile = false
                this.button.showButton = false
                this.treeView.showTree = false
                this.treeViewMet.showTree = false
                this.buttonSave.showButton = false
                this.buttonInsert.showButton = false
                this.missing.showlabel = false
                this.missingMet.showlabel = false
                this.titleDevion.title = 'Devion'
            } else if (metArtikel) {
                this.artikelForm.showform = true
                this.save.showButton = false
                this.next.showButton = true
                this.prev.showButton = true
                metIndex = 0
                this.artikelProgress.showLabel = true
                this.artikelForm.data = partsNotFoundMet[0]
                this.artikelForm.mass = mass
                this.artikelProgress.label = 'Artikel 1/' + partsNotFoundMet.length
                this.file.showFile = false
                this.button.showButton = false
                this.treeView.showTree = false
                this.treeViewMet.showTree = false
                this.buttonSave.showButton = false
                this.buttonInsert.showButton = false
                this.missing.showlabel = false
                this.missingMet.showlabel = false
                this.titleDevion.title = 'Metabil'
            }
        },
        async checkArtikel(artikel) {
            totalParts++
            if (artikel) {
                if (artikel.number.charAt(artikel.number.length - 1) == 'W') {
                    artikel.parts = null
                }

                if (artikel.parts) {
                    for (let part of artikel.parts) {
                        this.checkArtikel(part)
                    }
                }
                GetData('Devion/ets/articleexists?ArticleNumber=' + artikel.number).then((result) => result).then((data) => {
                    let log = { "artikelNumber": artikel.number, "action": "get", "extra": "Devion" }
                    logs.push(log)
                    artikel.existsDev = data
                    if (artikel.existsDev == false) {
                        if (!partsNotFound.includes(artikel)) {
                            partsNotFound.push(artikel)
                            notFound++
                        }
                    } else {
                        PostDataWithBody('Devion/ets/articledifference', artikel).then((response) => {
                            if (response != "{}") {
                                artikel.changedDev = true
                                let res = { artikel: artikel.number, changes: JSON.parse(response) }
                                changesDev.push(res)
                                this.changeDev.data = changesDev
                            }

                            if (changesDev.length != 0) {
                                this.changeDev.showButton = true
                            } else {
                                this.changeDev.showButton = false
                            }

                        })
                    }

                    if (notFound > 0) {
                        this.missing.showlabel = true
                    } else {
                        this.missing.showlabel = false
                    }
                    this.missing.label = 'Artikels niet gevonden ' + notFound + '/' + totalParts

                })
            }
        },
        async checkArtikelMet(artikel) {
            totalPartsMet++
            if (artikel) {
                if (artikel.parts) {
                    for (let part of artikel.parts) {
                        this.checkArtikelMet(part)
                    }
                }
                GetData('Metabil/ets/articleexists?ArticleNumber=' + artikel.number).then((result) => result).then((data) => {
                    let log = { "artikelNumber": artikel.number, "action": "get", "extra": "Metabil" }
                    logs.push(log)
                    artikel.existsMet = data
                    if (artikel.existsMet == false) {
                        if (!partsNotFoundMet.includes(artikel)) {
                            partsNotFoundMet.push(artikel)
                            notFoundMet++

                        }
                    } else {
                        PostDataWithBody('Metabil/ets/articledifference', artikel).then((response) => {
                            if (response != "{}") {
                                artikel.changed = true
                                let res = { artikel: artikel.number, changes: JSON.parse(response) }
                                changesMet.push(res)
                                this.changeMet.data = changesMet
                            }

                            if (changesMet.length != 0) {
                                this.changeMet.showButton = true
                            } else {
                                this.changeMet.showButton = false
                            }


                        })
                    }

                    if (notFoundMet > 0) {
                        this.missingMet.showlabel = true
                    } else {
                        this.missingMet.showlabel = false
                    }
                    this.missingMet.label = 'Artikels niet gevonden ' + notFoundMet + '/' + totalPartsMet

                })
            }
        },
        removeArtikel(main, artikel) {
            if (main.parts != []) {
                let index = main.parts.findIndex(part => part.number === artikel.number)
                if (index !== -1) {
                    main.parts[index].parts = []
                }
                main.parts.forEach(part => {
                    if (part.parts != []) {
                        this.removeArtikel(part, artikel)
                    }
                })
            }
        },
        async handleButtonInsert() {
            if (notFound > 0) {
                // this.artikelForm.showform = true
                // index = 0
                // this.artikelForm.data = partsNotFound[index]
                // this.save.showButton = false
                // this.next.showButton = true
                // this.prev.showButton = false
                // this.artikelProgress.showLabel = true
                // this.artikelProgress.label = 'Artikel 1/' + partsNotFound.length
                // this.file.showFile = false
                // this.button.showButton = false
                // this.treeView.showTree = false
                // this.treeView.showTree = false
                // this.buttonSave.showButton = false
                // this.buttonInsert.showButton = false
                // this.missing.showlabel = false
                // this.missingMet.showlabel = false
            } else {
                this.loading.showLoad = true
                PutDataWithBody('devion/ets/updatelinkedarticles', artikelen).then((response) => {
                    this.loading.showLoad = false
                    this.linked.showlabel = true
                    let log = JSON.parse(response)
                    const lengthlogs = logs.length
                    for (let i in log) {
                        logs[lengthlogs + i] = log[i]
                    }

                    this.createCSV()
                }).catch((error) => {
                    console.error(error)
                })
            }

            if (LinkModel.value) {
                console.log("test")
            }
        },
        createCSV() {
            // create a csv from my logs objects and download it
            let cleanjson = logs.filter(item => Object.values(item).every(value => value !== ''))
            const replacer = (key, value) => value === null ? '' : value // specify how you want to handle null values here
            const header = Object.keys(cleanjson[0])
            let csv = cleanjson.map(row => header.map(fieldName => JSON.stringify(row[fieldName], replacer)).join(','))
            csv.unshift(header.join(','))
            csv = csv.join('\r\n')
            var blob = new Blob([csv], { type: "text/plain;charset utf-8" });
            var link = document.createElement("a");
            link.href = URL.createObjectURL(blob);
            link.download = "logs.csv";
            link.click();
        },
        handleArtikel(object) {
            let artikel = object
            artikels[index] = artikel
            if (metArtikel) {

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
                    metIndex = metIndex2
                    if (metIndex == partsNotFoundMet.length - 1) {
                        this.next.showButton = false;
                        this.save.showButton = true;
                        this.artikelProgress.label = `Artikel ${metIndex + 1}/${partsNotFoundMet.length}`
                    } else if (metIndex == 0) {
                        this.next.showButton = true;
                        this.save.showButton = false;
                        this.prev.showButton = false;
                        this.artikelProgress.label = `Artikel ${metIndex + 1}/${partsNotFoundMet.length}`
                    } else {
                        this.next.showButton = true;
                        this.save.showButton = false;
                        this.prev.showButton = true;
                        this.artikelProgress.label = `Artikel ${metIndex + 1}/${partsNotFoundMet.length}`
                    }
                    this.ArtikelZoeken()
                }
            } else {
                if (save == true) {
                    // let endpoint = `devion/cebeo/createarticle`
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
            }
        },
        ArtikelZoeken() {
            if (metArtikel) {
                this.artikelForm.showform = true;
                this.artikelForm.data = partsNotFoundMet[metIndex]
                this.artikelForm.check = true
                this.file.showFile = false
                this.button.showButton = false
                this.treeView.showTree = false
                this.buttonSave.showButton = false
                this.buttonInsert.showButton = false
            } else {
                this.artikelForm.showform = true;
                this.artikelForm.data = partsNotFound[index]
                this.artikelForm.check = true
                this.file.showFile = false
                this.button.showButton = false
                this.treeView.showTree = false
                this.buttonSave.showButton = false
                this.buttonInsert.showButton = false
            }
        },
        handleNextButtonClick() {
            if (metArtikel) {
                metIndex2 = metIndex + 1
                this.$refs.article.createInfoObject()
            } else {
                index2 = index + 1
                this.$refs.article.createInfoObject()
            }
        },
        handlePrevButtonClick() {
            if (metArtikel) {
                metIndex2 = metIndex - 1
                this.$refs.article.createInfoObject()
            } else {
                index2 = index - 1
                this.$refs.article.createInfoObject()
            }
        },
        handleSaveButtonClick() {
            if (metArtikel) {
                metSave = true
                this.$refs.article.createInfoObject()
            } else {
                save = true
                this.$refs.article.createInfoObject()
            }
        },
        handleTest() { },
        handleMass(object) {
            mass = parseFloat(object).toFixed(2)
        },
        TogglePopup(trigger) {
            popupTriggers.value[trigger] = !popupTriggers.value[trigger]
        },
        updateItemsDev(object) {
            if (object) {
                for (let change of object) {
                    const endpoint = "devion/ets/updateitem"
                    PutDataWithBody(endpoint, change).then((response) => {
                        const log = JSON.parse(response)
                        logs.push(log)
                    }
                    )
                }
                this.handleButton();
            }
        },
        updateItemsMet(object) {
            if (object) {
                for (let change of object) {
                    const endpoint = "metabil/ets/updateitem"
                    PutDataWithBody(endpoint, change).then((response) => {
                        const log = JSON.parse(response)
                        logs.push(log)
                    }
                    )
                }
                this.handleButton();
            }
        }
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
    grid-template-areas: "title link toggle mass button";
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


.c-change-met {
    grid-area: "button";
    max-width: 50%;
    justify-self: end;
}

.c-dev {
    display: grid;
    grid-template-areas: "title mass button";
    grid-template-columns: 6fr 2fr 4fr;
    align-items: baseline;
}

.c-dev-title {
    grid-area: "title"
}

.c-change-dev {
    grid-area: "button";
    max-width: 50%;
    justify-self: end;
}

.c-form-title {
    display: flex;
    justify-content: center;
    align-items: center;
    margin: 0;
    padding: 0;
    width: 70vw;
    height: 100px;
}
</style>