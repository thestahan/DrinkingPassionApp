<template>
  <div class="hello">
    <h1>{{ msg }}</h1>
  </div>
  <div class="container">
    <button class="btn btn-blue">Info</button>
  </div>
</template>

<script>
export default {
  name: "HelloWorld",
  props: {
    msg: String,
  },
};
</script>
