<template>
    <div class="c-artikel-link">
        <label :for="id" class="c-label c-label-link">{{ label }}</label>
        <a :href="computedLink" class="c-link c-link-artikel js-link-icon" target="_blank" >?</a>
        <div v-if="error">
            <input type="url" class="c-input c-input-link c-input--error" :placeholder="computedLink" v-model="selectedOption"
                @change="emitSelectedOption" />
            <label class="c-label--error">link is verplicht!</label>
        </div>
        <div v-else>
            <input type="url" :id="id" class="c-input c-input-link" :placeholder="computedLink" v-model="selectedOption"
                @change="emitSelectedOption" />
        </div>
    </div>
</template>

<script>
export default {
    props: {
        id: String,
        label: String,
        error: Boolean,
        link: String,
    },
    data() {
        return {
            selectedOption: null,
        };
    },
    computed: {
        computedLink() {
            return this.link;
        }
    },
    watch: {
        computedLink(value) {
            console.log(value);
            this.computedLink = value;
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
.c-artikel-link {
    display: grid;
    grid-template-areas: "label icon input"
        "none none error";
    grid-template-columns: 1fr 1fr 4fr;
}

.c-label-link {
    padding-top: 11px;
    grid-area: label;
}

.c-input-link {
    grid-area: input;
}

.c-link-artikel {
    grid-area: icon;
    color: #fff;
    justify-self: end;
    align-self: center;
    background-color: var(--global-color-alpha);
    width: var(--global-whitespace-md);
    height: var(--global-whitespace-md);
    display: inline-block;
    border-radius: 100%;
    font-size: 10px;
    text-align: center;
    text-decoration: none;
    -webkit-box-shadow: inset -1px -1px 1px 0px rgba(0, 0, 0, 0.25);
    -moz-box-shadow: inset -1px -1px 1px 0px rgba(0, 0, 0, 0.25);
    box-shadow: inset -1px -1px 1px 0px rgba(0, 0, 0, 0.25);
    margin-right: var(--global-whitespace-md);
}

.c-link-artikel:hover,
.c-link-artikel:visited,
.c-link-artikel:active {
    color: var(--global-color-neutral-xxxx-light)
}
</style>