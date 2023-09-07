<template>
    <div class="c-input-placement">
        <label :for="id" class="c-label">{{ label }}:</label>
        <div v-if="error">
            <input type="text" :id="id" :label="label" v-model="selectedOption" @change="emitSelectedOption" :placeholder="placeholder"
                class="c-input c-input--error" />
            <label class="c-label--error"> {{ label }} is verplicht!</label>
        </div>
        <div v-else>
            <input type="text" :id="id" v-model="selectedOption" @change="emitSelectedOption" class="c-input" :placeholder="placeholder"/>
        </div>
    </div>
</template>

<script>
export default {
    props: {
        id: String,
        label: String,
        error: Boolean,
        placeholder: String,
    },
    data() {
        return {
            selectedOption: null,
        };
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
/* Add your custom styles for the dropdown here */
.c-custom-select {
    grid-area: text;
    display: block;
    position: relative;
    width: 100%;
    cursor: pointer;
}

.c-label {
    grid-area: label;
    display: block;
    font-size: var(--global-font-size);
    font-weight: var(--global-font-weight-bold);
    margin-bottom: var(--global-baseline);
    padding-top: 11px;
}

.c-label--error {
    grid-area: error;
    color: var(--global-color-error);
}

.c-input--error {
    border-color: var(--global-color-error);
}

.c-input-placement {
    position: relative;
    display: grid;
    grid-template-areas: "label text" "none error";
    grid-template-columns: 1fr 2fr;
}

.c-input {
    white-space: pre;
}
</style>