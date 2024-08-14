<template>
    <button @click="openPopup">Log In</button>
  </template>
  
  <script>
  export default {
    methods: {
      openPopup() {
        this.$emit('open-popup');
      }
    }
  }
  </script>