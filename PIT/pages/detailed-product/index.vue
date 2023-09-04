<template>
  <div style="background-color: #FFDFCC; height: 100% !important;">

    <div v-if="product" style="max-width: 1000px; margin: auto; padding-top: 40px;">
      <div class="ml-3">
        <v-btn @click="$router.push('/home')" icon>
          <v-icon color="primary">
            mdi-arrow-left
          </v-icon>
        </v-btn>
      </div>
      <v-row class="ma-0">
        <v-col cols="12" sm="4">
          <v-img style="border-radius: 10px;" max-width="100%" height="200" aspect-ratio="1"
            :src="product.type == 0 ? require('../../static/cafeQuente.jpg') : require('../../static/cafeGelado.jpg')"></v-img>
        </v-col>
        <v-col cols="12" sm="8">
          <div style="font-size: 20px; ">
            <div>
              {{ product.name }}
            </div>
            <div>
              R$ {{ product.price }}
            </div>
            <div>
              <div style="ont-size: 20px; font-weight: 600;">
                Descrição
              </div>
              <div>
                {{ product.description }}
              </div>
            </div>
          </div>
        </v-col>
        <v-col cols="12">
          <div class="d-flex" style="height: 100%; align-items: center; justify-content: flex-end;">
            <v-btn width="300" height="40" color="primary">
              Comprar
            </v-btn>
          </div>
        </v-col>
      </v-row>
    </div>
  </div>
</template>
<script>
export default {
  name: "FiquePorDentro",
  components: {},
  data: () => ({
    product: null,
    productId: null
  }),

  created() {
    this.productId = this.$route.query.id
    this.getProduct()
  },

  methods: {
    async getProduct() {
      await this.$axios
        .$get(`product/${this.productId}`)
        .then((res) => {
          this.product = res

        })
        .catch((res) => {
        });
    },
  },
};
</script>
<style>
.product:hover {
  -webkit-box-shadow: 7px 7px 35px 10px rgba(80, 41, 127, 0.27);
  -moz-box-shadow: 7px 7px 35px 10px rgba(80, 41, 127, 0.27);
  box-shadow: 7px 7px 35px 10px rgba(80, 41, 127, 0.27);
  transition: 0.2s;
}

.imgHome {
  display: flex;
  height: 400px;
  width: 100% !important;
  background-image: url(../../static/imgHome.jpg);
  color: white;
  justify-content: center;
  align-items: center;
  font-size: 35px;
  font-weight: 600;
  background-size: cover;
  background-position: center bottom;
}
</style>

    