<template>
    <div>
        <h1>Sync View</h1>
        <div class="buttons">
            <ButtonDevion :label="buttonSyncDev.label" :isDisabled="buttonSyncDev.isDisabled"
                :showButton="buttonSyncDev.showButton" @click="handleButtonSyncDev" />
            <ButtonDevion :label="buttonSyncMet.label" :isDisabled="buttonSyncMet.isDisabled"
                :showButton="buttonSyncMet.showButton" @click="handleButtonSyncMet" />
        </div>
        <div class="sync">
            <div :style="{ backgroundColor: syncStatus.syncDev ? 'red' : 'green' }" class="metrics">
                Devion: {{ syncStatus.syncDev ? 'Syncing' : 'Not syncing' }}
            </div>
            <div :style="{ backgroundColor: syncStatus.syncMet ? 'red' : 'green' }" class="metrics">
                Metabil: {{ syncStatus.syncMet ? 'Syncing' : 'Not syncing' }}
            </div>
        </div>
    </div>
</template>

<script>
import ButtonDevion from '../components/componenten/ButtonDevion.vue';
import { GetData } from '../global/global';

let loop;

export default {
    name: "SyncView",
    props: {
        count: Number,
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
            },
            syncStatus:
            {
                syncDev: false,
                syncMet: false
            }
        };
    },
    methods: {
        handleButtonSyncDev() {
            this.syncStatus.syncDev = 1;
            GetData("devion/sync").then(response => {
                this.syncStatus.syncDev = 0;
                return response;
            })
                .catch(error => {
                    console.error('Error during fetch:', error);
                });
        },
        handleButtonSyncMet() {
            this.syncStatus.syncMet = 1;
            GetData("metabil/sync").then(response => {
                this.syncStatus.syncMet = 0;
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
                .catch(error => {
                    console.error('Error during fetch:', error);
                });
        },
        update: function () {
            //set function for sync
            GetData("devion/sync/status").then(response => {
                return response;
            }).then(data => {
                this.syncStatus.syncDev = data.sync;
            })

            GetData("metabil/sync/status").then(response => {
                return response;
            }).then(data => {
                this.syncStatus.syncMet = data.sync;
            })
            loop = setTimeout(this.update, 1000);
        }
    },
    mounted: function () {
        loop = setTimeout(this.update, 1000);
    },
    beforeUnmount: function () {
        console.log("unmounted")
        clearTimeout(loop)
    }
}
</script>

<style>
.buttons, .sync {
    display: flex;
    justify-content: space-between;
    width: 100%;
    margin-top: 20px;
    gap: 5rem;
}

.metrics {
    padding: 10px;
    margin-top: 10px;
    width: 100%;
    text-align: center;
    font-size: 1.5rem;
    font-weight: bold;
}
</style>