
<template>
    <button type="button" :disabled="isDisabled" v-show="showButton" class="o-button-reset c-button">
        {{label}}
    </button>
</template>

<script>
export default {
    props: {
        label: String,
        isDisabled: Boolean,
        showButton: Boolean,
    }
}

</script>

<style scoped>
.c-button {
  font-size: 16px;
  font-weight: 700;
  border-radius: var(--global-borderRadius);
  border: var(--global-borderWidth) solid var(--global-color-alpha-dark);
  background-color: var(--global-color-alpha);
  color: #fff;
  padding: calc(var(--global-baseline) * 1.5 - var(--global-borderWidth))
    var(--global-whitespace-md);
  line-height: calc(var(--global-baseline) * 3);
  width: 100%;
  outline: none;
  transition: all var(--global-transitionTiming-alpha);
  cursor: pointer;
}

.c-button:hover {
  background-color: var(--global-color-alpha-light);
}

.c-button:focus {
  box-shadow: 0 0 0 3px var(--global-color-alpha-x-transparent);
}

.c-button:active {
  background-color: var(--global-color-alpha-dark);
  box-shadow: none;
}

.c-button:disabled {
  background-color: var(--global-color-alpha-x-light);
  border-color: var(--global-color-alpha-x-light);
  color: var(--global-color-neutral-dark);
  cursor: not-allowed;
}
</style>
