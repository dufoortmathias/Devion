<template>
    <div>
        <label :for="id" class="c-label">{{ label }}:</label>
        <select :id="id" v-model="selectedOption" @change="emitSelectedOption" class="c-input c-custom-select">
            <option value="null" disabled selected>Selecteer een {{ label }}</option>
            <option v-for="(option, index) in options" :key="index" :value="option.value">{{ option.label }}</option>
        </select>
        <svg class="c-custom-select__symbol" xmlns="http://www.w3.org/2000/svg"
            viewBox="0 0 24 24">
            <path d="M7,10l5,5,5-5Z" />
        </svg>
    </div>
</template>
  
<script>
export default {
    props: {
        id: String,
        label: String,
        options: Array,
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
    grid-area: select;
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

.c-custom-select__symbol {
  width: calc(var(--global-baseline) * 3);
  height: calc(var(--global-baseline) * 3);
  fill: var(--global-color-neutral-x-light);
  position: absolute;
  right: 12px;
  top: 12px;
  pointer-events: none;
}

div {
    position: relative;
    display: grid;
    grid-template-areas: "label select";
    grid-template-columns: 1fr 2fr;
}

</style>