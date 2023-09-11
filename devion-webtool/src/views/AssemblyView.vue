<template>
    <excelFileInput :label="file.label" :error="file.error" :filename="file.filename" @file-updated="handleFileUpdate" />
    <ButtonDevion :label="button.label" :isDisabled="button.isDisabled" :showButton="button.showButton" @click="handleButton" class="c-button" />
    <TreeView :jsonData="treeView.jsonData" :showTree="treeView.showTree"/>
</template>

<script>
import ExcelFileInput from '../components/componenten/excelFileInput.vue';
import ButtonDevion from '../components/componenten/ButtonDevion.vue';
import TreeView from '../components/componenten/TreeView.vue';
import { PostDataWithBody } from '../global/global';

let FileContents = null

export default {
    components: {
        ExcelFileInput,
        ButtonDevion,
        TreeView
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
            let endpoint = 'devion/ets/transformbomexcel'
            FileContents = this.file.file.previewBase64.replace('data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,', '')
            PostDataWithBody(endpoint, FileContents).then((response) => {
                let artikels = JSON.parse(response)
                this.treeView.showTree = true
                this.treeView.jsonData = artikels
            })
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
</style>