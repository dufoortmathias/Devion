<template>
    <div class="c-checkbox">
        <input type="checkbox" id="checkbox" v-model="selectedOption" @change="emitSelectedOption" name="checkbox" />
        <label for="checkbox">{{ placeholder }}</label>
    </div>
</template>

<script>

export default {
    props: {
        label: Object,
        placeholder: String,
    },
    data() {
        return {
            selectedOption: null,
        };
    },
    watch: {
        placeholder(value) {
            this.selectedOption = value;
        },
    },
    methods: {
        emitSelectedOption() {
            // Emit the selected option when the value changes
            this.$emit('option-selected', this.selectedOption);
        },
    }
};
</script>

<style scoped>
.c-checkbox {
    display: flex;
    align-items: center;
    margin-bottom: 1rem;
}
</style>