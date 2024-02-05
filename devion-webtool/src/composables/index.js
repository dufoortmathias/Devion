import Simplebox from './../components/componenten/DialogDevion.vue'
import { createConfirmDialog } from 'vuejs-confirm-dialog'

const useConfirmBeforeAction = (action, props) => {
  const { reveal, onConfirm } = createConfirmDialog(Simplebox, props)

  onConfirm(action)

  reveal()
}

export { useConfirmBeforeAction }
