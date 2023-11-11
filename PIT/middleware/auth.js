export default function ({ store, redirect, route }) {
  route.meta.map((meta) => {
    if (meta.requiresAuth && !store.getters['auth/authenticated'])
      return redirect('/')
    return 0
  })
}
