<template>
  <Dialog
    :style="{ width: '600px' }"
    header="Szczegóły składnika"
    :modal="true"
    class="p-fluid"
    @hide="closeDialog"
  >
    <form id="ingredient-details-form" @submit.prevent="submitForm()">
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
        <label for="type" class="green-caption p-text-bold p-mb-2">Typ</label>
        <Dropdown
          v-model="type"
          :options="types"
          optionLabel="name"
          placeholder="Wybierz typ składnika"
          :filter="true"
          filterPlaceholder="Znajdź typ"
          emptyFilterMessage="Nie znaleziono typu"
          dataKey="id"
          :class="{ 'p-invalid': v$.type.$error }"
        >
        </Dropdown>
        <p v-for="error of v$.type.$errors" :key="error.$uid">
          <small class="p-error">{{ error.$message }}</small>
        </p>
      </div>

      <div class="p-field">
        <label for="unit" class="green-caption p-text-bold p-mb-2"
          >Jednostka</label
        >
        <Dropdown
          v-model="unit"
          :options="units"
          optionLabel="name"
          placeholder="Wybierz jednostkę składnika"
          :filter="true"
          filterPlaceholder="Znajdź jednostkę"
          emptyFilterMessage="Nie znaleziono jednostki"
          dataKey="id"
          :class="{ 'p-invalid': v$.unit.$error }"
        >
        </Dropdown>
        <p v-for="error of v$.unit.$errors" :key="error.$uid">
          <small class="p-error">{{ error.$message }}</small>
        </p>
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
import useVuelidate from "@vuelidate/core";
import InputText from "primevue/inputtext";
import Dropdown from "primevue/dropdown";
import Button from "primevue/button";
import { required, helpers, minLength } from "@vuelidate/validators";

const objectNameNotEmpty = (obj) => obj.name;

export default {
  components: {
    Dialog,
    InputText,
    Dropdown,
    Button,
  },
  emits: ["close-modal", "manage-ingredient"],
  props: {
    product: {
      type: Object,
      required: true,
    },
    types: {
      type: Array,
      required: true,
    },
    units: {
      type: Array,
      required: true,
    },
  },
  data() {
    return {
      name: this.product.name,
      type: { id: this.product.productTypeId, name: this.product.productType },
      typeName: this.product.productType,
      unit: { id: this.product.productUnitId, name: this.product.productUnit },
    };
  },
  setup() {
    return { v$: useVuelidate() };
  },
  methods: {
    closeDialog() {
      this.$emit("close-modal");
    },
    submitForm() {
      this.v$.$touch();

      if (this.v$.$error) {
        return;
      }

      const selectedUnit = this.units.find((u) => u.id == this.unit.id);

      const product = {
        id: this.product.id ?? 0,
        name: this.name,
        productUnitId: this.unit.id,
        productUnit: selectedUnit.abbreviation ?? selectedUnit.name,
        productTypeId: this.type.id,
        productType: this.type.name,
      };

      this.$emit("manage-ingredient", product);

      return;
    },
  },
  validations() {
    return {
      name: {
        required: helpers.withMessage("To pole jest wymagane", required),
        minLength: helpers.withMessage(
          "Nazwa musi zawierać co najmniej 2 znaki",
          minLength(2)
        ),
      },
      unit: {
        objectNameNotEmpty: helpers.withMessage(
          "To pole jest wymagane",
          objectNameNotEmpty
        ),
      },
      type: {
        objectNameNotEmpty: helpers.withMessage(
          "To pole jest wymagane",
          objectNameNotEmpty
        ),
      },
    };
  },
};
</script>
