<template>
  <section class="p-m-4 p-d-flex p-jc-between">
    <div>
      <Button label="Filtry" icon="pi pi-filter" iconPos="left"></Button>
    </div>
    <div>
      <Dropdown
        v-model="currentSort"
        :options="sortOptions"
        optionLabel="name"
        placeholder="Sortuj według"
        @change="setFilters"
      ></Dropdown>
    </div>
  </section>
</template>

<script>
import Button from "primevue/button";
import Dropdown from "primevue/dropdown";

export default {
  components: { Button, Dropdown },
  emits: ["set-filters"],
  data() {
    return {
      sortOptions: [
        { name: "Nazwa rosnąco", code: "NAME_ASC" },
        { name: "Nazwa malejąco", code: "NAME_DESC" },
        { name: "Ilość składników rosnąco", code: "INGREDIENTS_COUNT_ASC" },
        { name: "Ilość składników malejąco", code: "INGREDIENTS_COUNT_DESC" },
      ],
      currentSort: null,
    };
  },
  created() {
    this.currentSort = this.sortOptions.find((opt) => opt.code == "NAME_ASC");
  },
  methods: {
    setFilters() {
      const filters = {
        sort: this.currentSort.code,
      };

      this.$emit("set-filters", filters);
    },
  },
};
</script>
