<template>
  <div
    class="fixed top-0 right-0 h-screen w-screen z-50 flex justify-center items-center bg-gray-300 bg-opacity-50"
    v-if="open"
  >
    <div class="md:w-1/3 sm:w-full rounded-lg shadow-lg bg-white my-3">
      <div
        class="flex justify-between items-center border-b border-gray-100 px-5 py-4"
      >
        <div class="flex flex-row items-center">
          <CheckIcon class="h-6 w-6 text-green-500"></CheckIcon>
          <span class="font-bold text-gray-700 text-lg px-">{{ title }}</span>
        </div>
        <div class="flex flex-row items-center">
          <button>
            <XIcon
              class="h-6 w-6 text-red-500 hover:text-red-600 transition duration-150"
              @click="closeModal"
            ></XIcon>
          </button>
        </div>
      </div>

      <div class="px-10 pt-4 text-gray-600">
        {{ content }}
      </div>

      <div class="px-5 py-3 flex justify-end">
        <button
          class="text-sm py-2 px-3 text-gray-500 hover:text-gray-600 transition duration-150"
          @click="closeModal"
        >
          Zamknij
        </button>
      </div>
    </div>
  </div>
</template>

<script>
import { CheckIcon, XIcon } from "@heroicons/vue/outline";

export default {
  props: {
    title: {
      type: String,
      required: true,
    },
    content: {
      type: String,
      required: true,
    },
    open: {
      type: Boolean,
      required: true,
    },
  },
  components: {
    CheckIcon,
    XIcon,
  },
  methods: {
    closeModal() {
      this.$emit("close-modal", false);
    },
  },
};
</script>
