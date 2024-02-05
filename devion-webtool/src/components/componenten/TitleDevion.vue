<template>
    <div class="c-title-devion">
        <h1>{{ title }}</h1>
    </div>
</template>

<script>
export default {
    props: {
        title: String
    }
}
</script>

<style scoped>
    .c-title-devion {
        display: flex;
        justify-content: center;
        align-items: center;
        margin: 0;
        padding: 0;
        width: 100%;
        height: 100px;
    }
    .c-title-devion h1 {
        font-size: 2em;
        color: #000;
    }
</style>