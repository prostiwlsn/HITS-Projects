import { reactive } from 'vue'

export const store = reactive({
    addresGuid: '',
    addAddress(address){
        this.addresGuid = address;
    },
    deleteAddress(){
        this.addresGuid = '';
    }
  })