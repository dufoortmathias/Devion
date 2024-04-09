<template>
    <div class="c-input-placement">
        <label :for="id" class="c-label">{{ label }}:</label>
        <div v-if="error">
            <input type="text" :id="id" :label="label" v-model="selectedOption" @change="emitSelectedOption" :placeholder="placeholder"
                class="c-input c-input--error">
            <label class="c-label--error"> {{ errorText }}</label>
        </div>
        <div v-else>
            <input type="text" :id="id" v-model="selectedOption" @change="emitSelectedOption" class="c-input" :placeholder="placeholder" @input="$emit('option-selected', $event.target.value)"/>
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
        errorText: String,
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
            console.log('Emitting selected option:', this.selectedOption);
            this.$emit('option-selected', this.selectedOption);
        },
    }
};
</script>

<style scoped>
/* Add your custom styles for the dropdown here */
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
</style>