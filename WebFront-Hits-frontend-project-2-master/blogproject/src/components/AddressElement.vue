<template>
    <div class="form-group">
        <div v-if="addressLevel != ''">{{ addressLevel }}</div>
        <input type="text" name="address" :list="parentId" :id="parentId + 'address'" v-model="stringAddress" class="mb-2">
            <datalist :id="parentId" :key="stringAddress">
                <option v-for="address in filteredAddresses" :key="address.objectId" :value="address.text">{{ address.text }}</option>
            </datalist>
        <AddressElement :parentId="selectedOption" :key="selectedOption" v-if="filteredChildAddresses.length > 0"/>
    </div>
</template>

<script>
import { store } from '@/store/address_store.js';

  export default {
    name: 'AddressElement',
    props: {
        number: {
            type: Number,
            required: false
        },
        parentId:{
            type: Number,
            required: true
        },
        query:{
            type: String,
            required: false
        }
    },
    setup(){
        return{
            store
        }
    },
    data(){
        return{
            addresses: [],
            selectedOption: -1,
            selectedGuid: '',
            stringAddress: '',
            addressLevel: '',
            childAddresses: []
        }
    },
    mounted(){
        this.getResults();
    },
    computed: {
        filteredAddresses() {
            return this.addresses;
        },
        filteredChildAddresses(){
            return this.childAddresses;
        }
    },
    watch:{
        stringAddress(newValue, oldValue) {
            if (newValue !== oldValue) {
                this.getResults();
                this.selectedOption = -1;
                this.selectOption(newValue);
                this.getChildResults()
            }
        }
    },
    methods:{
        async getResults(){
            try {
                const response = await fetch('https://blog.kreosoft.space/api/address/search?parentObjectId='+this.parentId + (this.stringAddress != '' ? '&query=' + this.stringAddress : ''), {
                    method: 'GET',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                });

                if (!response.ok) {
                    throw new Error('Ошибка HTTP: ' + response.status);
                }

                console.log('https://blog.kreosoft.space/api/address/search?parentObjectId='+this.parentId + (this.stringAddress != '' ? '&query=' + this.stringAddress : ''));

                const data = await response.json();
                this.addresses = data;
                console.log(this.addresses)
            } catch (error) {
                console.error('Ошибка:', error);
            }
        },
        selectOption(selectedText){
            console.log(this.addresses);
            const selectedAddress = this.addresses.find(address => address.text === selectedText);
            this.selectedOption = selectedAddress ? selectedAddress.objectId : -1;
            this.selectedGuid = selectedAddress ? selectedAddress.objectGuid : '';
            this.addressLevel = selectedAddress ? selectedAddress.objectLevelText: '';

            this.store.addAddress(this.selectedGuid);
            console.log('xdd')
        },
        updateAddressList(){
            const dataList = this.$refs.addressList;
            while (dataList.firstChild) {
                dataList.removeChild(dataList.firstChild);
            }
            this.addresses.forEach(address => {
                const option = document.createElement('option');
                option.value = address.text;
                dataList.appendChild(option);
            });
        },
        async getChildResults(){
            try {
                const response = await fetch('https://blog.kreosoft.space/api/address/search?parentObjectId='+this.selectedOption, {
                    method: 'GET',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                });

                if (!response.ok) {
                    throw new Error('Ошибка HTTP: ' + response.status);
                }

                const data = await response.json();
                this.childAddresses = data;
            } catch (error) {
                console.error('Ошибка:', error);
            }
        },
    }
  }
</script>