import https from 'https'

export default function ({ $axios, $toast, store, error: nuxtError }) {
  $axios.defaults.httpsAgent = new https.Agent({ rejectUnauthorized: false })
  $axios.setBaseURL(process.env.VUE_APP_BASE_URL)
  $axios.onRequest((config) => {
    // const token = store.state.auth.token;
    // if(token) {
    //   config.headers.Authorization = 'Bearer ' + token;
    // }

    store._vm.$nextTick(() => {
      if (store._vm.$nuxt) {
        store._vm.$nuxt.$loading.start()
        return config
      }
    })
  })

  $axios.onResponse((response) => {
    store._vm.$nextTick(() => {
      if (store._vm.$nuxt) {
        store._vm.$nuxt.$loading.finish()
      }
    })

    return response
  })

  $axios.onError((error) => {
    store._vm.$nextTick(() => {
      if (store._vm.$nuxt) {
        setTimeout(() => {})
        store._vm.$nuxt.$loading.finish()
      }
    })

    if (error.response) {
      const code = error.response.data.statusCode
      const message = error.response.data.content

      $toast.error(message + ' - ' + 'Código: ' + code)
    }
  })
}
