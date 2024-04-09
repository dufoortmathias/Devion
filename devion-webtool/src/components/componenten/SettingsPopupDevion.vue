<template>
    <div class="popup">
        <div class="popup-inner">
            <div class="close">
                <svg xmlns="http://www.w3.org/2000/svg" height="24" viewBox="0 -960 960 960" width="24"
                    v-on:click="TogglePopup()">
                    <path
                        d="m256-200-56-56 224-224-224-224 56-56 224 224 224-224 56 56-224 224 224 224-56 56-224-224-224 224Z" />
                </svg>
            </div>
            <h2>Settings</h2>
            <div class="spacer"></div>
            <div v-for="(item, index1) in data" :key="index1">
                <h3>{{ index1 }}</h3>
                <div v-for="(subItem, index2) in item" :key="index2" class="input">
                    <TextInput :v-model:value="data[index1][index2]" :label="index2" :placeholder="subItem.toString()"
                        @option-selected="handleChange($event, index1, index2)" />
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import { GetData, PostDataWithBody } from '../../global/global';
import TextInput from './textInput.vue';

export default {
    name: "SettingsPopupDevion",
    props: {
        TogglePopup: Function
    },
    data() {
        return {
            data: null
        };
    },
    created: function () {
        this.priceSettings();
    },
    components: { TextInput },
    methods: {
        priceSettings() {
            GetData("pricesettings").then(Response => {
            console.log(Response);
            return Response;
        })
            .then(data => {
                console.log(data);
                this.data = data;
            })
            .catch(error => {
                console.error('Error during fetch:', error);
            });
        },
        handleChange(event, index1, index2) {
            this.data[index1][index2] = event;
            let jsonData = this.data;
            for (const key in jsonData) {
                // eslint-disable-next-line no-prototype-builtins
                if (jsonData.hasOwnProperty(key)) {
                    const object = jsonData[key];
                    for (const key2 in object) {
                        // eslint-disable-next-line no-prototype-builtins
                        if (object.hasOwnProperty(key2)) {
                            const value = object[key2];
                            const parsedValue = typeof value === 'number' ? value : parseFloat(value);
                            object[key2] = parsedValue;
                        }
                    }
                }
            }
            console.log(jsonData);
            PostDataWithBody("pricesettings", jsonData).then(Response => {
                console.log(Response);
                this.priceSettings();
            })
                .catch(error => {
                    console.error('Error during fetch:', error);
                });
        },
    }
}
</script>

<style>
.popup {
    position: absolute;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    z-index: 99;
    background-color: rgba(0, 0, 0, 0.8);

    display: flex;
    align-items: center;
    justify-content: center;
    scroll-behavior: auto;

    .popup-inner {
        position: relative;
        background: #FFF;
        padding: 32px;
        overflow-y: auto;
        max-height: 75%;
        min-width: 30vw;

        .close {
            position: absolute;
            top: 0;
            right: 0;
            cursor: pointer;
            padding-top: 12px;
            padding-right: 12px;
        }
    }
}

h3 {
    margin-bottom: 1rem;
    margin-top: 1rem;
}

.input {
    margin-bottom: 1rem;
}
</style>