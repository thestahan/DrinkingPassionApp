<template>
  <div class="p-d-flex p-jc-center p-mt-6">
    <div class="p-px-3" style="max-width: 28rem; width: 100%">
      <div>
        <img
          class="p-mx-auto"
          src="https://www.svgrepo.com/show/119676/cocktail-glass.svg"
          alt="DrinkingPassion logo"
          style="height: 3rem"
        />
        <div class="p-text-center p-mt-3">
          <h2 class="p-text-bold" style="font-size: 2rem">Załóż konto</h2>
          <p style="font-size: 0.9rem">
            lub
            <router-link
              to="/login"
              class="p-text-bold"
              style="color: var(--primary-color)"
              >zaloguj się na istniejące konto</router-link
            >
          </p>
        </div>
      </div>
      <form @submit.prevent="submitForm()" class="p-fluid p-mt-5">
        <div class="p-grid p-fluid">
          <div class="p-col p-field">
            <div class="p-float-label">
              <InputText
                type="text"
                v-model="firstName"
                :class="{ 'p-invalid': v$.firstName.$error }"
                style="width: 100%"
              />
              <label for="firstName">Imię</label>
            </div>
            <p v-for="error of v$.firstName.$errors" :key="error.$uid">
              <small class="p-error">{{ error.$message }}</small>
            </p>
          </div>
          <div class="p-col p-field">
            <div class="p-float-label">
              <InputText
                type="text"
                v-model="lastName"
                :class="{ 'p-invalid': v$.lastName.$error }"
                style="width: 100%"
              />
              <label for="lastName">Nazwisko</label>
            </div>
            <p v-for="error of v$.lastName.$errors" :key="error.$uid">
              <small class="p-error">{{ error.$message }}</small>
            </p>
          </div>
        </div>
        <div class="p-field p-pb-2">
          <div class="p-float-label">
            <InputText
              id="displayName"
              v-model="displayName"
              :class="{ 'p-invalid': v$.displayName.$error }"
            ></InputText>
            <label for="displayName">Nazwa wyświetlana</label>
          </div>
          <p v-for="error of v$.displayName.$errors" :key="error.$uid">
            <small class="p-error">{{ error.$message }}</small>
          </p>
        </div>
        <div class="p-field p-pb-2">
          <div class="p-float-label">
            <InputText
              id="email"
              v-model.trim="email"
              :class="{ 'p-invalid': v$.email.$error }"
            ></InputText>
            <label for="email">Adres email</label>
          </div>
          <p v-for="error of v$.email.$errors" :key="error.$uid">
            <small class="p-error">{{ error.$message }}</small>
          </p>
        </div>
        <div class="p-field p-pb-2">
          <div class="p-float-label">
            <InputText
              id="password"
              v-model.trim="password"
              type="password"
              :class="{ 'p-invalid': v$.password.$error }"
            ></InputText>
            <label for="password">Hasło</label>
          </div>
          <p v-for="error of v$.password.$errors" :key="error.$uid">
            <small class="p-error">{{ error.$message }}</small>
          </p>
        </div>
        <div class="p-field">
          <div class="p-float-label">
            <InputText
              id="passwordConfirm"
              v-model="passwordConfirm"
              type="password"
              :class="{ 'p-invalid': v$.passwordConfirm.$error }"
            ></InputText>
            <label for="passwordConfirm">Powtórz hasło</label>
          </div>
          <p v-for="error of v$.passwordConfirm.$errors" :key="error.$uid">
            <small class="p-error">{{ error.$message }}</small>
          </p>
        </div>
        <div class="p-mb-2">
          <p class="p-mb-2">Które zdanie najlepiej Cię określa?</p>
          <div class="p-field-radiobutton">
            <RadioButton
              id="hobbyst"
              name="bartenderType"
              value="hobbyst"
              v-model="bartenderType"
              :class="{ 'p-invalid': v$.bartenderType.$error }"
            />
            <label for="hobbyst"
              ><span class="p-text-bold" style="color: var(--primary-color)"
                >Hobbysta</span
              >
              - tworzę koktajle głównie w domu</label
            >
          </div>
          <div class="p-field-radiobutton">
            <RadioButton
              id="professionalist"
              name="bartenderType"
              value="professionalist"
              v-model="bartenderType"
              :class="{ 'p-invalid': v$.bartenderType.$error }"
            />
            <label for="professionalist"
              ><span class="p-text-bold" style="color: var(--primary-color)"
                >Zawodowiec</span
              >
              - tworzę koktajle głównie w barach</label
            >
          </div>
          <Divider></Divider>
          <div>
            <Button
              type="submit"
              label="Zarejestruj się"
              style="width: 100%"
            ></Button>
          </div>
        </div>
      </form>
    </div>
  </div>

  <spinner v-if="isLoading"></spinner>
  <base-success-modal
    title="Sukces!"
    content="Rejestracja przebiegła pomyślnie. 
      Na Twój adres email została wysłana wiadomość z linkiem potwierdzającym. 
      Bez uprzedniego potwierdzenia adresu, zalogowanie na konto nie będzie możliwe."
    :open="openModal"
    @close-modal="redirectToLogin"
  ></base-success-modal>
</template>

<script>
import Spinner from "../../utilities/Spinner.vue";
import BaseSuccessModal from "../../utilities/modals/BaseSuccessModal.vue";
import InputText from "primevue/inputtext";
import Button from "primevue/button";
import RadioButton from "primevue/radiobutton";
import Divider from "primevue/divider";
import useVuelidate from "@vuelidate/core";
import {
  required,
  email,
  helpers,
  sameAs,
  minLength,
  maxLength,
} from "@vuelidate/validators";

export default {
  components: {
    Spinner,
    BaseSuccessModal,
    InputText,
    Button,
    RadioButton,
    Divider,
  },
  setup() {
    return {
      v$: useVuelidate(),
    };
  },
  data() {
    return {
      firstName: "",
      lastName: "",
      email: "",
      password: "",
      passwordConfirm: "",
      displayName: "",
      bartenderType: "",
      isLoading: false,
      openModal: false,
    };
  },
  methods: {
    async submitForm() {
      this.v$.$touch();

      if (this.v$.$error) {
        return;
      }

      this.isLoading = true;

      try {
        await this.$store.dispatch("signUp", {
          firstName: this.firstName,
          lastName: this.lastName,
          email: this.email,
          password: this.password,
          displayName: this.displayName,
          bartenderType: this.bartenderType == "hobbyst" ? 1 : 2,
        });
      } catch (err) {
        console.error("Wystąpił błąd podczas rejestracji");
      }

      this.isLoading = false;

      this.openModal = true;
    },

    redirectToLogin() {
      this.$router.replace("/login");
    },
  },
  validations() {
    return {
      firstName: {
        minLength: helpers.withMessage(
          "Imię musi być dłuższe niż 2 litery",
          minLength(2)
        ),
        maxLength: helpers.withMessage(
          "Imię nie może być dłuższe niż 60 liter",
          maxLength(60)
        ),
      },
      lastName: {
        minLength: helpers.withMessage(
          "Nazwisko musi być dłuższe niż 2 litery",
          minLength(2)
        ),
        maxLength: helpers.withMessage(
          "Nazwisko nie może być dłuższe niż 60 liter",
          maxLength(60)
        ),
      },
      displayName: {
        required: helpers.withMessage("To pole jest wymagane", required),
        minLength: helpers.withMessage(
          "Nazwa musi zawierać więcej niż 3 znaki",
          minLength(3)
        ),
      },
      email: {
        required: helpers.withMessage("To pole jest wymagane", required),
        email: helpers.withMessage("Wprowadź poprawny adres email", email),
      },
      password: {
        required: helpers.withMessage("To pole jest wymagane", required),
        minLength: helpers.withMessage(
          "Hasło musi zawierać minimum 8 znaków",
          minLength(8)
        ),
      },
      passwordConfirm: {
        repeatPassword: helpers.withMessage(
          "Podane hasła muszą być identyczne",
          sameAs(this.password)
        ),
      },
      bartenderType: {
        required,
      },
    };
  },
};
</script>
