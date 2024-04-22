<template>
    <div class="container">
        <div class="row justify-content-center">
            <div class="col-lg-6 col-md-12">
                <Community v-for="community in communities" :key="community.id" :community="community" class="mb-3"/>
            </div>
        </div>
    </div>
</template>
  
<script>
import Community from './CommunityListElement.vue'

export default {
    name: 'Community-List',
    components:{
        Community
    },
    data(){
        return{
            communities: []
        }
    },
    mounted(){
        this.getCommunities()
    },
    methods:{
        async getCommunities(){
            try {
                const response = await fetch('https://blog.kreosoft.space/api/community', {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                },
                });

                if (!response.ok) {
                    throw new Error('Ошибка HTTP: ' + response.status);
                }

                const data = await response.json();

                this.communities = data;
                
            } catch (error) {
                console.error('Ошибка входа:', error);
            }
        }
    }
}
</script>