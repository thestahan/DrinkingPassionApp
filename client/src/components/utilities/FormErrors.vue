<template>
  <div class="flex justify-start mt-3 ml-4 p-1" v-if="errors.length">
    <ul>
      <li
        class="flex items-center py-1"
        v-bind:key="error"
        v-for="error in errors"
      >
        <XCircleIcon class="h-5 w-5 text-red-700"></XCircleIcon>
        <span class="text-red-700 font-medium text-sm ml-3">{{ error }}</span>
      </li>
    </ul>
  </div>
</template>

<script>
import { XCircleIcon } from "@heroicons/vue/solid";

export default {
  components: {
    XCircleIcon,
  },
  props: {
    errors: {
      type: Array,
      required: true,
    },
  },
};
</script>
