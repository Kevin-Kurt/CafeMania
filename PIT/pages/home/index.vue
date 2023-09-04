<template>
  <div style="background-color: #FFDFCC; height: 100% !important;">
    <div class="imgHome">
      <div style="background-color: #FFDFCC; padding-block: 7px; padding-inline: 70px;">
        CaféMania
      </div>
    </div>
    <div style="max-width: 1000px; margin: auto;">
      <div>
        <div class="d-flex mt-5" style="padding-right: 12px;justify-content: flex-end;">
          <v-select class="mr-2" label="Filtrar" @change="getProducts(1)" hide-details style="max-width: 300px;" outlined
            v-model="filter.filter" :items="filterProducts" item-text="text" item-value="value"
            :menu-props="{ offsetY: true }"></v-select>

          <v-select label="Ordenar" @change="getProducts(1)" hide-details style="max-width: 300px;" outlined
            v-model="filter.order" :items="types" item-text="text" item-value="text"
            :menu-props="{ offsetY: true }"></v-select>
        </div>
      </div>
      <v-row style="padding-top: 10px; margin: auto; justify-content: center;">
        <v-col cols="12" sm="4" md="3" class="pb-2" v-for="(product, index) in products" :key="index"
          style="min-width: 250px;  max-width: 250px;">
          <div @click="detailed(product)" class="product"
            style="cursor: pointer; background-color: var(--v-primary-base); padding: 10px; border-radius: 8px">
            <div>
              <v-img style="border-radius: 10px;" max-width="230" height="200" aspect-ratio="1"
                :src="product.type == 0 ? require('../../static/cafeQuente.jpg') : require('../../static/cafeGelado.jpg')"></v-img>
            </div>
            <div class="mt-2" style="color: white;">
              <div class="d-flex" style="justify-content: space-between;">
                <div style="font-size: 14px;">
                  {{ product.name }}
                </div>
                <div>
                  {{ product.type == 1 ? 'Gelado' : 'Quente' }}
                </div>
              </div>
              <div class="mt-2" style="text-align: end;">
                Preço: {{ product.price }}
              </div>
            </div>
          </div>
        </v-col>
      </v-row>
      <div class="pt-2 pb-4" v-if="products != null">
        <div class="pt-5" style="text-align: end;">
          <span> 1 - {{ products.length }} de {{ totalItems }}</span>
        </div>
        <v-pagination @input="getProducts" @next="getProducts" @previous="getProducts" v-model="filter.page"
          :length="pageCount" :total-visible="8" style="margin-top: 10px"></v-pagination>
      </div>
    </div>
  </div>
</template>
<script>
export default {
  name: "FiquePorDentro",
  components: {},
  data: () => ({
    types: [
      {
        text: "Preço",
      }, {
        text: "Tipo",
      }
    ],
    filterProducts: [
      {
        text: "Quente",
        value: 0
      }, {
        text: "Gelado",
        value: 1
      }
    ],
    products: null,
    totalItems: null,
    pageCount: null,
    filter: {
      page: 1,
      pagesize: 12,
      order: "",
      filter: ""
    },
  }),

  async created() {
    await this.getProducts()
  },

  methods: {
    detailed(product) {
      this.$router.push(`/detailed-product?id=${product.id}`)
    },
    async getProducts(i) {
      if (i === 1) {
        this.filter.page = 1
      }
      await this.$axios
        .$get(`product/list?page=${this.filter.page}&pagesize=${this.filter.pagesize}&order=${this.filter.order}&filter=${this.filter.filter}`)
        .then((res) => {
          this.products = res.entity
          this.totalItems = res.pager.totalItems
          this.pageCount = res.pager.totalPages
          this.filter.page = res.pager.currentPage
          console.log(this.products)
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

    