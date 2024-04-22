<script setup>
const props = defineProps(['annotations'])
</script>

<template>
<div v-html="annotations"></div>
</template>