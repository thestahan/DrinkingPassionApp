<template>
  <Dialog
    :style="{ width: '550px' }"
    header="Szczegóły koktajlu"
    :modal="true"
    class="p-fluid"
    @hide="closeDialog"
  >
    <form id="cocktail-details-form" @submit.prevent="submitForm()">
      <div class="p-field" v-if="pictureUrl">
        <label for="name" class="p-mb-2">Aktualne zdjęcie</label>
        <img :src="pictureUrl" alt="Zdjęcie koktajlu" class="cocktail-image" />
      </div>

      <div class="p-field">
        <label for="name" class="p-mb-2">Nazwa</label>
        <InputText
          id="name"
          v-model.trim="name"
          :class="{ 'p-invalid': v$.name.$error }"
        ></InputText>
        <p v-for="error of v$.name.$errors" :key="error.$uid">
          <small class="p-error">{{ error.$message }}</small>
        </p>
      </div>

      <div class="p-field" id="file-upload-field">
        <label for="picture" class="p-mb-2">Zdjęcie</label>
        <FileUpload
          name="picture"
          :customUpload="true"
          @remove="handleDelete"
          @select="handleSelect"
          :showUploadButton="false"
          chooseLabel="Wybierz"
          cancelLabel="Usuń"
          :fileLimit="1"
          accept="image/*"
        />
      </div>

      <div class="p-field">
        <label for="description" class="p-mb-2">Opis</label>
        <Textarea
          id="description"
          v-model.trim="description"
          :autoResize="true"
          rows="3"
          :class="{ 'p-invalid': v$.description.$error }"
        ></Textarea>
        <p v-for="error of v$.description.$errors" :key="error.$uid">
          <small class="p-error">{{ error.$message }}</small>
        </p>
      </div>

      <div class="p-field">
        <label for="preparationInstruction" class="p-mb-2"
          >Instrukcja przygotowania</label
        >
        <Textarea
          id="preparationInstruction"
          v-model.trim="preparationInstruction"
          :autoResize="true"
          rows="3"
        ></Textarea>
      </div>

      <div class="p-field">
        <label for="description" class="p-mb-2">Składniki</label>
        <div v-if="showMinIngredientsError" class="p-mb-2">
          <p>
            <small class="p-error"
              >Koktajl musi składać się z co najmniej 2 składników</small
            >
          </p>
        </div>
        <DataTable
          :value="ingredients"
          editMode="cell"
          class="editable-cells-table"
          responsiveLayout="scroll"
        >
          <Column field="name" header="Nazwa" style="width: 40%">
            <template #editor="slotProps"
              ><InputText
                v-model="slotProps.data[slotProps.column.props.field]"
              ></InputText
            ></template>
          </Column>
          <Column field="amount" header="Ilość" style="width: 40%">
            <template #editor="slotProps"
              ><InputNumber
                v-model="slotProps.data[slotProps.column.props.field]"
                mode="decimal"
                :min="0"
                :max="1000"
                :step="0.1"
                :minFractionDigits="1"
                :maxFractionDigits="1"
              ></InputNumber
            ></template>
          </Column>
          <Column :exportable="false" style="min-width: 2rem">
            <template #body="slotProps">
              <Button
                icon="pi pi-trash"
                class="p-button-rounded p-button-danger"
                @click="deleteIngredient(slotProps.data)"
              />
            </template>
          </Column>
        </DataTable>
      </div>

      <div class="p-field">
        <label>Dodaj składnik</label>
        <div class="p-grid">
          <div class="p-col-5">
            <Dropdown
              v-model="newIngredient"
              :options="filteredProducts"
              placeholder="Wybierz składnik"
              :filter="true"
              optionLabel="name"
              dataKey="id"
              filterPlaceholder="Znajdź składnik"
              emptyFilterMessage="Nie znaleziono składnika"
            />
            <!-- <div class="p-inputgroup">
            <span class="p-inputgroup-addon">Nazwa</span>
            <InputText v-model="newIngredient.name" />
          </div> -->
          </div>
          <div class="p-col-5">
            <div class="p-inputgroup">
              <span class="p-inputgroup-addon">Ilość</span>
              <InputNumber
                v-model="newIngredient.amount"
                mode="decimal"
                :min="0"
                :max="1000"
                :step="0.1"
                :minFractionDigits="1"
                :maxFractionDigits="1"
              ></InputNumber>
            </div>
          </div>
          <div class="p-col-2">
            <Button
              icon="pi pi-plus"
              class="p-button-success"
              @click="addIngredient()"
            ></Button>
          </div>
        </div>
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
import InputNumber from "primevue/inputnumber";
import Textarea from "primevue/textarea";
import FileUpload from "primevue/fileupload";
import Button from "primevue/button";
import DataTable from "primevue/datatable";
import Column from "primevue/column";
import Dropdown from "primevue/dropdown";
import useVuelidate from "@vuelidate/core";
import { required, helpers, minLength } from "@vuelidate/validators";

export default {
  components: {
    Dialog,
    InputText,
    InputNumber,
    Button,
    Textarea,
    FileUpload,
    DataTable,
    Column,
    Dropdown,
  },
  setup() {
    return { v$: useVuelidate() };
  },
  emits: ["close-modal", "manage-cocktail"],
  props: {
    cocktail: {
      type: Object,
      required: true,
    },
    products: {
      type: Array,
      required: true,
    },
  },
  data() {
    return {
      name: this.cocktail.name,
      picture: null,
      pictureUrl: this.cocktail.picture,
      description: this.cocktail.description,
      preparationInstruction: this.cocktail.preparationInstruction,
      ingredients: this.cocktail.ingredients ? this.cocktail.ingredients : [],
      minIngredientsError: null,
      newIngredient: {},
    };
  },
  computed: {
    showMinIngredientsError() {
      if (this.minIngredientsError == null) return false;

      return this.ingredients.length < 2;
    },
    filteredProducts() {
      const ingredientsNames = this.ingredients.map((x) => x.name);

      return this.products.filter((x) => !ingredientsNames.includes(x.name));
    },
  },
  methods: {
    closeDialog() {
      this.$emit("close-modal");
    },
    handleDelete(e) {
      console.log("deleting");
      console.log(e);
      this.picture = null;
    },
    handleSelect(e) {
      this.picture = e.files[0];
      console.log(this.picture);
    },
    deleteIngredient(ingredient) {
      this.ingredients = this.ingredients.filter((x) => x != ingredient);
    },
    addIngredient() {
      if (!this.newIngredient.name || !this.newIngredient.amount) return;

      this.ingredients.push({
        id: 0,
        amount: this.newIngredient.amount,
        name: this.newIngredient.name,
        productId: this.newIngredient.id,
      });

      this.newIngredient = {};
    },
    submitForm() {
      this.v$.$touch();

      if (this.ingredients.length < 2) {
        this.minIngredientsError = true;
      }

      if (this.v$.$error || this.minIngredientsError) {
        return;
      }

      const cocktail = {
        id: this.cocktail.id,
        name: this.name,
        picture: "",
        description: this.description,
        preparationInstruction: this.preparationInstruction,
        ingredients: this.getTransformedIngredients(),
      };

      this.$emit("manage-cocktail", cocktail);
    },
    getTransformedIngredients() {
      return this.ingredients.reduce((accum, obj) => {
        const ingredientToUpload = {
          amount: obj.amount,
          productId: obj.productId,
        };
        accum.push(ingredientToUpload);
        return accum;
      }, []);
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
      description: {
        required: helpers.withMessage("To pole jest wymagane", required),
      },
    };
  },
};
</script>
