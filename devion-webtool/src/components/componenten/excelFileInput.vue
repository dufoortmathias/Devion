<template>
    <div class="c-input-placement">
        <label class="c-label c-label-file">{{ label }}: </label>
        <div v-if="error" class="c-input-file">
            <input id="file-input" class="o-hide-accessible" type="file" accept=".xlsx, .xls, .csv"
                @change="onFileChange($event)" />
            <label class="file-label c-error" for="file-input"> {{ filename }} </label>
            <label class="c-label--error"> {{ label }} is verplicht!</label>
        </div>
        <div v-else class="c-input-file">
            <input id="file-input" class="o-hide-accessible" type="file" accept=".xlsx, .xls, .csv"
                @change="onFileChange($event)" />
            <label class="file-label" for="file-input"> {{ filename }} </label>
        </div>
    </div>
</template>

<script>

export default {
    props: {
        error: Boolean,
        label: String,
        filename: String,
    },
    components: {
    },
    data() {
        return {
        }
    },
    methods: {
        onFileChange($event) {
            const file = $event.target.files[0]
            const reader = new FileReader()
            if (file) {
                reader.readAsDataURL(file)
                reader.onload = () => {
                    // Set a new property on the captured `file` and set it to the converted base64 image
                    file.previewBase64 = reader.result
                    // Emit the file with the new previewBase64 property on it
                    this.$emit('file-updated', file)
                }
                reader.onerror = (error) => {
                    console.error(error)
                }
            }
        },
    },
}
</script>

<style scoped>
.file-label {
    display: flex;
    color: #fff;
    background-color: var(--global-color-alpha);
    padding: 0.5rem 1rem;
    border-radius: 0.25rem;
    cursor: pointer;
    justify-content: center;
    align-items: center;
}

input[type='file']:focus+.file-label {
    box-shadow: 0 0 0 4px #bae6fd;
}

.c-input-placement {
    position: relative;
    display: grid;
    grid-template-areas: "label select" "none error";
    grid-template-columns: 1fr 2fr;
}

.c-input-file {
    grid-area: select;
    display: block;
    position: relative;
    width: 100%;
    cursor: pointer;
}

.c-label-file {
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

.c-error {
    border-color: var(--global-color-error);
}
</style>