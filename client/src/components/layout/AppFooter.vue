<template>
  <div class="layout-footer">
    <div class="layout-footer-left">
      <span>DrinkingPassion © 2021, </span>
      <a href="mailto:adm.drinking-passion@outlook.com" class="author"
        >Kacper Korman</a
      >
    </div>

    <div class="layout-footer-right">
      <a href="https://github.com/thestahan">
        <i class="pi pi-github"></i>
      </a>
    </div>
  </div>
</template>

<style scoped>
.layout-footer {
  font-size: 1rem;
  padding: 2rem;
  background-color: var(--surface-a);
  display: flex;
  align-items: center;
  justify-content: space-between;
}

.layout-footer-right a {
  color: var(--text-color);
  font-weight: 600;
}

.layout-footer-left a {
  color: var(--green-600);
  font-weight: 600;
}

.layout-footer .layout-footer-right a i {
  color: var(--text-secondary-color);
  font-size: 1.5rem;
}
</style>
