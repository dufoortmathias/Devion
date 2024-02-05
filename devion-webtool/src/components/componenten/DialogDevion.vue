<template>
    <div class="modal-container">
        <div class="modal-body">
            <span class="modal-close" @click="$emit('cancel')">x</span>
            <h2>{{ question }}</h2>
            <div class="modal-action">
                <Button class="c-button modal-button" @click="$emit('confirm')" >Confirm</Button>
                <Button class="c-button modal-button" @click="$emit('cancel')"> Cancel</Button>
            </div>
        </div>
    </div>
</template>

<script>
import { defineComponent } from 'vue';

export default defineComponent({
    props: ["question"],
    emits: ["confirm", "cancel"],
})
</script>

<style>
.modal-container {
    display: flex;
    justify-content: center;
    align-items: center;
    position: absolute;
    top: 0;
    left: 0;
    right: 0;
    width: 100%;
    height: 100%;
    background-color: #cececeb5;
}

.modal-body {
    background-color: #fff;
    border: 2px solid #74a2cf;
    border-radius: 10px;
    text-align: center;
    padding: 20px 40px;
    min-width: 250px;
    display: flex;
    flex-direction: column;
}

.modal-action {
    display: flex;
    flex-direction: row;
    gap: 40px;
    justify-content: center;
}

.modal-button {
    cursor: pointer;
    height: 30px;
    padding: 0 25px;
    border: 2px solid #74a2cf;
    border-radius: 5px;
    background-color: var(--global-color-alpha);
    color: #fff;
}

.modal-close {
    cursor: pointer;
    position: relative;
    align-self: end;
    right: -33px;
    top: -17px;
}</style>