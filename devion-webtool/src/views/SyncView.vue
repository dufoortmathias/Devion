<template>
    <div>
        <h1>Sync View</h1>
        <div class="buttons">
            <ButtonDevion :label="buttonSyncDev.label" :isDisabled="buttonSyncDev.isDisabled"
                :showButton="buttonSyncDev.showButton" @click="handleButtonSyncDev" />
            <ButtonDevion :label="buttonSyncMet.label" :isDisabled="buttonSyncMet.isDisabled"
                :showButton="buttonSyncMet.showButton" @click="handleButtonSyncMet" />
        </div>
    </div>
</template>

<script>
import ButtonDevion from '../components/componenten/ButtonDevion.vue';
import { GetData } from '../global/global';

export default {
    name: "SyncView",
    props: {
        count: Number
    },
    components: {
        ButtonDevion
    },
    data() {
        return {
            buttonSyncDev: {
                components: {
                    ButtonDevion
                },
                label: 'Sync Devion',
                isDisabled: false,
                showButton: true
            },
            buttonSyncMet: {
                components: {
                    ButtonDevion
                },
                label: 'Sync Metetabil',
                isDisabled: false,
                showButton: true
            }
        };
    },
    methods: {
        handleButtonSyncDev() {
            GetData("devion/sync").then(response => {
                console.log(response)
                return response;
            })
                .then(data => {
                    // Handle the data
                    console.log("Data:", data);
                })
                .catch(error => {
                    console.error('Error during fetch:', error);
                });
        },
        handleButtonSyncMet() {
            GetData("metabil/sync").then(response => {
                console.log(response)
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
                .then(data => {
                    // Handle the data
                    console.log("Data:", data);
                })
                .catch(error => {
                    console.error('Error during fetch:', error);
                });
        },
        update: function () {
            //set function for sync
            console.log("test")
            setTimeout(this.update, 10000);
        }
    },
    mounted: function () {
        this.update()
    },
    beforeUnmount: function () {
        console.log("unmounted")
        clearTimeout(this.update)
    }
}
</script>

<style>
.buttons {
    display: flex;
    justify-content: space-between;
    width: 100%;
    margin-top: 20px;
    gap: 5rem;
}
</style>