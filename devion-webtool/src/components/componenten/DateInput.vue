<template>
    <VueDatePicker v-model="date" :format="format">label</VueDatePicker>
</template>

<script>
import VueDatePicker from '@vuepic/vue-datepicker'
import '@vuepic/vue-datepicker/dist/main.css'
import { ref } from 'vue'

const date = ref(new Date())

const format = (date) => {
    const day = date.getDate();
    const month = date.getMonth() + 1;
    const year = date.getFullYear();

return `${day}/${month}/${year}`
}

export default {
    props: {
        label: String,
        Date: Date
    },
    components: {
        VueDatePicker
    },
    setup() {
        return {
            date
        }
    }
}
</script>

<style>

</style>