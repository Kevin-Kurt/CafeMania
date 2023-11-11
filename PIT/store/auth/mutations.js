export default {
  setUser(state, payload) {
    state.user = payload
    localStorage.setItem('user', JSON.stringify(payload))
  },

  setToken(state, payload) {
    state.token = payload
    localStorage.setItem('token', payload)
  },

  setExpires(state, payload) {
    state.expires = payload
    localStorage.setItem('expires', payload)
  },

  logout(state) {
    this.$router.push('/')
    state.user = null
    state.token = null
    state.expires = null
    state.company = null
    localStorage.removeItem('token')
    localStorage.removeItem('expires')
    localStorage.removeItem('user')
  },
}
