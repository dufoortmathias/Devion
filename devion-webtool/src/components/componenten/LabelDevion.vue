<template>
    <div v-show="showLabel">
        {{ label }}
    </div>
</template>

<script>
export default {
    props: {
        label: String,
        showLabel: Boolean,
    },
};
</script>