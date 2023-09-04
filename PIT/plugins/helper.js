import 'moment/locale/pt-br'

export default ({ $moment }, inject) => {
  inject('randomIntFromInterval', (min, max) =>
    Math.floor(Math.random() * (max - min + 1) + min)
  )
  inject('formatDate', (date) =>
    $moment(date).locale('pt-BR').format('DD/MM/YYYY hh:mm:ss')
  )
  inject('formatMonth', (date) =>
    $moment(date).locale('pt-BR').format('MMMM / YYYY')
  )
  inject('formatDateWithoutHour', (date) =>
    date ? $moment(date).locale('pt-BR').format('DD/MM/YYYY') : date
  )
  inject(
    'formatCurrency',
    (value) =>
      'R$ ' + value.toLocaleString('pt-BR', { minimumFractionDigits: 2 })
  )
  inject('formatHour', (hour) => {
    if (hour.toString().split('.')[0].length === 1) {
      return '0' + hour.toFixed(2).toString().replace('.', ':')
    } else {
      return hour.toFixed(2).toString().replace('.', ':')
    }
  })
}
