<template>
    <div>
        <h1>Historiek</h1>
        <Dropdown :id="dropdownCompanies.id" :label="dropdownCompanies.label" :options="dropdownCompanies.options"
            :error="dropdownCompanies.error" @option-selected="handledropdownCompaniesSelected" class="c-dropdown"
            :selected="dropdownCompanies.selected" />
        <Dropdown :id="dropdownArtikelGroep.id" :label="dropdownArtikelGroep.label" :options="dropdownArtikelGroep.options"
            :error="dropdownArtikelGroep.error" @option-selected="handledropdownArtikelGroepSelected" class="c-dropdown"
            :selected="dropdownArtikelGroep.selected" />
        <div class="c-date">
            <p class="c-label">Start Datum:</p>
            <VueDatePicker v-model="StartDate" :format="formatStartDate" class="c-datepicker" />
        </div>

        <div class="c-date">
            <p class="c-label">Eind Datum:</p>
            <VueDatePicker v-model="EndDate" :format="formatEndDate" class="c-datepicker" />
        </div>
        <ButtonDevion :label="button.label" :isDisabled="button.isDisabled" :showButton="button.showButton"
            @click="handleButton" class="c-button" />
    </div>
</template>

<script>
import VueDatePicker from '@vuepic/vue-datepicker';
import '@vuepic/vue-datepicker/dist/main.css';
import Dropdown from '../components/componenten/DropdownMenu.vue'
import ButtonDevion from '../components/componenten/ButtonDevion.vue'
import { GetData } from '../global/global.js'
import { ref } from 'vue'



let company = ""

export default {
    components: {
        Dropdown,
        VueDatePicker,
        ButtonDevion
    },
    setup() {
        const StartDate = ref(new Date());
        const formatStartDate = (StartDate) => {
            const day = StartDate.getDate();
            const month = StartDate.getMonth() + 1;
            const year = StartDate.getFullYear();

            return `${day}/${month}/${year}`;
        }
        const EndDate = ref(new Date());
        const formatEndDate = (EndDate) => {
            const day = EndDate.getDate();
            const month = EndDate.getMonth() + 1;
            const year = EndDate.getFullYear();

            return `${day}/${month}/${year}`;
        }
        return { StartDate , formatStartDate, EndDate, formatEndDate}
    },
    data() {
        return {
            dropdownCompanies: {
                components: {
                    Dropdown,
                },
                id: 'dropdownCompanies',
                label: 'Bedrijf',
                options: [],
                error: false,
            },
            dropdownArtikelGroep: {
                components: {
                    Dropdown,
                },
                id: 'dropdownArtikelGroep',
                label: 'Artikel Groep',
                options: [],
                error: false,
            },
            button: {
                components: {
                    ButtonDevion
                },
                label: 'Historiek ophalen',
                isDisabled: false,
                showButton: true
            },
        }
    },
    created() {
        GetData("companies").then((data) => {
            return data
        }).then((data) => {
            const options = []
            for (var element of data) {
                options.push({ value: element, label: element })
            }
            this.dropdownCompanies.options = options
            this.dropdownCompanies.selected = this.dropdownCompanies.options.find((x) => x.label.toLowerCase() == "devion").value
            company = this.dropdownCompanies.selected
            console.log(company)
            let endpoint = company + "/artikel/groepen"
            GetData(endpoint).then((data) => {
                return data
            }).then((data) => {
                console.log(data)
                for (var element of data) {
                    console.log(element)
                    this.dropdownArtikelGroep.options.push({ value: element['arG_ID'], label: element['arG_OMSCHRIJVING'] })
                }
            })
        })
    },
    methods: {
        handledropdownCompaniesSelected(value) {
            company = value
            let endpoint = company + "/artikel/groepen"
            GetData(endpoint).then((data) => {
                return data
            }).then((data) => {
                console.log(data)
                this.dropdownArtikelGroep.options = []
                for (var element of data) {
                    console.log(element)
                    this.dropdownArtikelGroep.options.push({ value: element['arG_ID'], label: element['arG_OMSCHRIJVING'] })
                }
            })
        },
        handledropdownArtikelGroepSelected(value) {
            console.log(value)
            this.dropdownArtikelGroep.selected = value
        },
        handleButton() {
            console.log(company)
            console.log(this.StartDate.getDate() + '.'+ this.StartDate.getMonth() + '.'+ this.StartDate.getFullYear())
            console.log(this.EndDate.getDate() + '.'+ this.EndDate.getMonth() + '.'+ this.EndDate.getFullYear())
            let startdate = this.StartDate.getDate() + '-'+ this.StartDate.getMonth() + '-'+ this.StartDate.getFullYear()
            let enddate = this.EndDate.getDate() + '-'+ this.EndDate.getMonth() + '-'+ this.EndDate.getFullYear()

            let endpoint = company + "/historiek/stock?Groep=" + this.dropdownArtikelGroep.selected + "&StartDate=" + startdate + "&EndDate=" + enddate
            console.log(endpoint)
            GetData(endpoint).then((data) => {
                console.log(data)
            })
        }
    }

}
</script>

<style>
.c-dropdown {
    margin: 1rem 0;
}

.c-date {
    margin: 1rem 0;
    display: grid;
    grid-template-columns: 1fr 2fr;
    grid-template-areas: "label datepicker";
}

.c-datepicker {
    margin: 1rem 0;
    display: block;
    position: relative;
    grid-area: "datepicker";
}

.c-label {
    margin: 1rem 0;
    display: block;
    position: relative;
    grid-area: "label";
}
</style>