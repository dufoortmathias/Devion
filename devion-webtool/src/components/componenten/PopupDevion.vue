<template>
    <div class="popup">
        <div class="popup-inner">
            <h2>CHANGES</h2>
            <div class="spacer"></div>
            <ul class="c-list">
                <li v-for="(item, index) in data" :key="index">
                    <h3>{{ item.artikel }}</h3>
                    <ul>
                        <li v-for="(change, key) in item.changes" :key="key" class="change">
                            <input type="checkbox" :id="'checkbox' + index + key" @change="select(item.artikel, change, key)"/>
                            <label :for="'checkbox' + index + key">{{ key }}: {{ change.etsWaarde }} --> {{ change.newWaarde
                            }}</label>
                        </li>
                    </ul>
                </li>
            </ul>
            <ButtonDevion :label="Close.label" :isDisabled="Close.isDisabled" :showButton="Close.showButton"
                @click="TogglePopup()" class="c-button c-change-dev" />
            <ButtonDevion :label="'Save selected'" :isDisabled="false" :showButton="true" @click="SaveSelected"
                class="c-save-selected" />
            <ButtonDevion :label="'Save all'" :isDisabled="false" :showButton="true" @click="SaveAll" class="c-save-all" />
        </div>
    </div>
</template>

<script>
import ButtonDevion from './ButtonDevion.vue'

export default {
    components: {
        ButtonDevion
    },
    props: {
        data: Array,
        TogglePopup: Function
    },
    data() {
        return {
            Close: {
                components: {
                    ButtonDevion
                },
                label: 'Close popup',
                isDisabled: false,
                showButton: true
            },
            selectedItems: []
        }
    },
    methods: {
        select(articlenumber, change, key){
            const object = {articlenumber, key, change}
            if(!this.selectedItems.find(item => item.articlenumber === object.articlenumber && item.key === object.key)){
                this.selectedItems.push(object)
            } else {
                this.selectedItems = this.selectedItems.filter(item => item.articlenumber !== object.articlenumber && item.key !== object.key)
            }
        },
        SaveSelected() {
            this.$emit("change-items", this.selectedItems)
            this.TogglePopup()
        },
        SaveAll() {
            this.$emit("change-items", this.data)
            this.TogglePopup()
        }
    }
}
</script>

<style scoped>
.popup {
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    z-index: 99;
    background-color: rgba(0, 0, 0, 0.8);

    display: flex;
    align-items: center;
    justify-content: center;

    .popup-inner {
        background: #FFF;
        padding: 32px;
        overflow-y: auto;
        max-height: 75%;
    }
}

h2 {
    color: white;
    font-weight: 500;
    line-height: 2rem;
    margin-bottom: 1rem;
    margin-top: 0;
    text-align: center;
    position: fixed;
    transform: translateY(-250%)
}

.c-button {
    position: fixed;
    bottom: 20px;
    left: 50%;
    transform: translateX(-50%);
    width: 20%
}

.c-save-selected {
    transform: translateX(-200%)
}

.c-save-all {
    transform: translateX(100%)
}

.spacer {
    /* height: 58px; */
}

.change {
    display: flex;
    align-items: center;
}
</style>