<style scoped>
@keyframes spin {
  from {
    transform: rotate(0deg);
  }
  to {
    transform: rotate(360deg);
  }
}

.wrapper {
  position: fixed;
  top: 0;
  right: 0;
  height: 100vh;
  width: 100vw;
  z-index: 50;
  display: flex;
  justify-content: center;
  align-items: center;
  opacity: 0.5;
}

.spinner {
  animation: spin 1s linear infinite;
  height: 8rem;
  width: 8rem;
  border-top-width: 2px;
  border-bottom-width: 2px;
  border-radius: 9999px;
}
</style>

<template>
  <div class="wrapper" style="background-color: var(--surface-100)">
    <div class="spinner" style="border-color: var(--primary-color)"></div>
  </div>
</template>
