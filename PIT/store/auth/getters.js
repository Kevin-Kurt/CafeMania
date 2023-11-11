import moment from 'moment'

export default {
  getUserInfo(state) {
    return {
      user: state.user,
      token: state.token,
      validTo: state.validTo,
    }
  },

  user(state) {
    return state.user.name
      ? state.user
      : JSON.parse(localStorage.getItem('user'))
  },

  permissions(state) {
    if (state.user && state.user.permissions) {
      const permissions = state.user.permissions.split(',')
      return permissions
    } else {
      return []
    }
  },

  authenticated(state) {
    const token = state.token || localStorage.getItem('token')
    const validTo = state.expires || localStorage.getItem('expires')

    if (token !== '' && token !== null) {
      if (moment().isBefore(moment(validTo))) {
        return true
      } else {
        return false
      }
    } else {
      return false
    }
  },
}
