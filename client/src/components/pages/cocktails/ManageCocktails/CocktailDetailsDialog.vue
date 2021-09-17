<template>
  <Dialog
    :style="{ width: '550px' }"
    header="Szczegóły koktajlu"
    :modal="true"
    class="p-fluid"
  >
    <div class="p-field" v-if="pictureUrl">
      <label for="name" class="p-mb-2">Aktualne zdjęcie</label>
      <img :src="pictureUrl" alt="Zdjęcie koktajlu" class="cocktail-image" />
    </div>
    <img
      :src="pictureUrl"
      alt="Zdjęcie koktajlu"
      class="cocktail-image"
      v-if="pictureUrl"
    />

    <div class="p-field">
      <label for="name" class="p-mb-2">Nazwa</label>
      <InputText id="name" v-model.trim="name"></InputText>
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
      ></Textarea>
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
          <div class="p-inputgroup">
            <span class="p-inputgroup-addon">Nazwa</span>
            <InputText v-model="newIngredient.name" />
          </div>
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

    <template #footer>
      <Button
        label="Anuluj"
        icon="pi pi-times"
        class="p-button-text"
        @click="closeDialog"
      />
      <Button label="Zapisz" icon="pi pi-check" class="p-button-text" />
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
  },
  emits: ["close-modal"],
  props: {
    cocktail: {
      type: Object,
      required: true,
    },
  },
  data() {
    return {
      name: null,
      picture: null,
      pictureUrl: null,
      description: null,
      preparationInstruction: null,
      ingredients: [],
      newIngredient: {},
    };
  },
  created() {
    this.name = this.cocktail.name;
    this.pictureUrl = this.cocktail.picture;
    this.description = this.cocktail.description;
    this.preparationInstruction = this.cocktail.preparationInstruction;
    //this.ingredients = this.cocktail.ingredients;
    this.ingredients.push({ name: "Campari", amount: 30, productId: 5, id: 1 });
    console.log(this.ingredients);
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
      this.ingredients.push(this.newIngredient);
      this.newIngredient = {};
    },
  },
};
</script>
