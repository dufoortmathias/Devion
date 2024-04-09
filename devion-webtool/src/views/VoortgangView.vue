<template>
    <h1>Krijgen van excel file</h1>
    <ButtonDevion :label="ButtonGet.label" :isDisabled="ButtonGet.isDisabled" :showButton="ButtonGet.showButton"
        @click="handleButtonGet" />
    <div class="break"></div>
    <h1>Syncroniseren van projecten</h1>
    <ExcelFileInput :label="file.label" :error="file.error" :filename="file.filename" :showFile="file.showFile"
        @file-updated="handleFileUpdate" />
    <div class="break"></div>
    <ButtonDevion :label="button.label" :isDisabled="button.isDisabled" :showButton="button.showButton"
        @click="handleButton" class="c-button" />
</template>

<script>
import ButtonDevion from '../components/componenten/ButtonDevion.vue';
import ExcelFileInput from '../components/componenten/excelFileInput.vue';

import { GetData, PostDataWithBody } from '../global/global';

let FileContents = null
let FileName = null

export default {
    name: "ProjectenVoortgang",
    components: {
        ButtonDevion,
        ExcelFileInput
    },
    data() {
        return {
            ButtonGet: {
                components: {
                    ButtonDevion
                },
                label: 'Download Excel file',
                isDisabled: false,
                showButton: true
            },
            file: {
                components: {
                    ExcelFileInput
                },
                id: 'file',
                label: 'Projecten file',
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
        };
    },
    methods: {
        handleButtonGet() {
            let endpoint = 'Devion/projecten/voortgang/export'
            GetData(endpoint).then((bon) => {
                console.log(bon)
                // if (bon.status) {
                //     this.error.showLabel = true
                //     this.error.label = bon.detail
                //     return
                // } else {
                //     this.error.showLabel = false
                // }
                // var decodeString = atob(bon.fileContents);
                // var blob = new Blob([decodeString], { type: bon.contentType });
                // const a = document.createElement("a");
                // a.href = URL.createObjectURL(blob);
                // a.download = bon.fileDownloadName;
                // document.body.appendChild(a);
                // a.click();
                // document.body.removeChild(a);
            })
        },
        handleFileUpdate(file) {
            this.file.error = false;
            this.file.showLabel = false;
            this.file.showButton = true;
            this.file.filename = file.name;
            this.file.file = file;
            this.button.isDisabled = false;
        },
        handleButton() {
            let endpoint = 'devion/projecten/voortgang/import'
            FileContents = this.file.file.previewBase64.replace('data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,', '')
            FileName = this.file.filename
            var data = {
                "FileContents": FileContents
            }
            endpoint += "?FileName=" + FileName
            PostDataWithBody(endpoint, data).then((projectenVoortgang) => {
                console.log(JSON.parse(projectenVoortgang))
            })
        }
    }
}
</script>

<style scoped>
.break {
    margin-top: 20px;
}
</style>