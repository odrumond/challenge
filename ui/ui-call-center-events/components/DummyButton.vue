<template>
    <button @click="handleClick">{{ text }}</button>
</template>
  
<script>
  export default {
    props: {
      text: {
        type: String,
        default: 'Ok'
      },
    },
    methods: {
      handleClick() {
        this.$emit('click');
      }
    }
  };
</script>
  
<style scoped>
    button {
        margin: 50px auto;
        display: block;
        background: #34495e;
        border: none;
        padding: 10px 20px;
        color: white;
        font-size: 16px;
        border-radius: 3px;
        cursor: pointer;
    }

    .icon-adv {
        margin-left: 10px;
        font-size: 18px !important;
    }
</style>