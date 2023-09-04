<template>
  <v-app>
    <div class="wrapper">
      <Nuxt />
    </div>
  </v-app>
</template>

<script>
export default {
  name: 'DefaultLayout',
  created() {
  },
  data() {
    return {
      drawer: false,
    }
  },
  watch: {
  },
  methods: {
    changeDrawer(value) {
      this.drawer = value
    },

    logout() {
      this.$auth.logout(/* .... */)
    },
  },
}
</script>
<style lang="scss" scoped>
@import '@/assets/main.scss';

.wrapper {
  height: 100%;
}
</style>
