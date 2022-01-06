<template>
  <Dialog
    header="Szczegóły listy koktajli"
    :breakpoints="{ '1920px': '65vw', '1440px': '75vw', '960px': '95vw' }"
    :style="{ width: '45vw' }"
    :modal="true"
    class="p-fluid"
    @hide="closeDialog"
  >
    <form>
      <div class="p-field">
        <label for="name" class="green-caption p-text-bold p-mb-2">Nazwa</label>
        <InputText
          id="name"
          v-model.trim="name"
          :class="{ 'p-invalid': v$.name.$error }"
        ></InputText>
        <p v-for="error of v$.name.$errors" :key="error.$uid">
          <small class="p-error">{{ error.$message }}</small>
        </p>
      </div>

      <div class="p-field">
        <label for="cocktails" class="green-caption p-text-bold p-mb-2"
          >Koktajle</label
        >
        <div v-if="showMinCocktailsError" class="p-mb-2">
          <p>
            <small class="p-error"
              >Lista musi się składać z co najmniej jednego koktajlu</small
            >
          </p>
        </div>
        <Autocomplete
          :multiple="true"
          v-model="cocktails"
          :suggestions="filteredCocktails"
          :dropdown="true"
          @complete="searchCocktail($event)"
          field="name"
        >
        </Autocomplete>
      </div>
    </form>

    <template #footer>
      <Button
        label="Anuluj"
        icon="pi pi-times"
        class="p-button-text"
        @click="closeDialog"
      />
      <Button
        label="Zapisz"
        icon="pi pi-check"
        class="p-button-text"
        form="cocktail-details-form"
        @click="submitForm"
      />
    </template>
  </Dialog>
</template>

<script>
import Dialog from "primevue/dialog";
import InputText from "primevue/inputtext";
import Autocomplete from "primevue/autocomplete";
import Button from "primevue/button";
import useVuelidate from "@vuelidate/core";
import { required, helpers, minLength } from "@vuelidate/validators";

export default {
  components: { Dialog, InputText, Autocomplete, Button },
  emits: ["close-dialog", "manage-list"],
  props: {
    list: {
      type: Object,
      required: true,
    },
  },
  setup() {
    return { v$: useVuelidate() };
  },
  data() {
    return {
      name: this.list.name,
      cocktails: this.list.cocktails,
      filteredCocktails: [],
      minCocktailsError: null,
    };
  },
  computed: {
    availableCocktails() {
      return this.$store.getters.allAvailableCocktails;
    },
    showMinCocktailsError() {
      if (this.minCocktailsError == null) return false;

      if (!this.cocktails) return true;

      return this.cocktails.length < 1;
    },
  },
  methods: {
    closeDialog() {
      this.$emit("close-dialog");
    },
    searchCocktail(event) {
      if (!event.query.trim().length) {
        this.filteredCocktails = [...this.availableCocktails];
      } else {
        this.filteredCocktails = this.availableCocktails.filter(
          (cocktail) =>
            cocktail.name.toLowerCase().includes(event.query.toLowerCase()) &&
            !this.filteredCocktails.includes(cocktail)
        );
      }
    },
    submitForm() {
      this.v$.$touch();

      if (!this.cocktails || this.cocktails.length < 1) {
        this.minCocktailsError = true;
      } else {
        this.minCocktailsError = false;
      }

      if (this.v$.$error || this.minCocktailsError) {
        return;
      }

      const list = {
        id: this.list.id,
        name: this.name,
        cocktails: this.cocktails.map((c) => c.id),
      };

      this.$emit("manage-list", list);
    },
  },
  validations() {
    return {
      name: {
        required: helpers.withMessage("To pole jest wymagane", required),
        minLength: helpers.withMessage(
          "Nazwa musi zawierać co najmniej 3 znaki",
          minLength(3)
        ),
      },
    };
  },
};
</script>
