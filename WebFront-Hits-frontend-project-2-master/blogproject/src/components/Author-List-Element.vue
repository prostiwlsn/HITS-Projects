<template>
    <div class="card">
        <div class="container text-start">
            <div class="row">
                <div class="col-4">
                    <div class="position-relative">
                        <img v-if="author.gender == 'Male'" src="https://cdn2.iconfinder.com/data/icons/ios-7-icons/50/user_male2-512.png" class="img-fluid">
                        <img v-else src="https://cdn2.iconfinder.com/data/icons/ios-7-icons/50/user_female2-512.png" class="img-fluid">
                        <div class="position-absolute top-0 start-0" v-if="position<3">
                            <font-awesome-icon :icon="['fas', 'crown']" size="xl" style="color: #d4af37" v-if="position == 0"/>
                            <font-awesome-icon :icon="['fas', 'crown']" size="xl" style="color: #c0c0c0" v-else-if="position == 1"/>
                            <font-awesome-icon :icon="['fas', 'crown']" size="xl" style="color: #cd7f32" v-else/>
                        </div>
                    </div>
                </div>
                <div class="col-8">
                    <h5 class="card-title"><router-link :to="'/?author='+author.fullName">{{ author.fullName }}</router-link></h5>
                    <h6 class="card-subtitle mb-2 text-muted">Создан: {{ author.created }}</h6>
                    <p class="card-text">{{ new Date(author.birthDate) }}</p>
                    <a href="#" class="card-link">Лайков: {{ author.likes }} Постов: {{ author.posts }}</a>
                </div>
            </div>
        </div>
    </div>
</template>
  
  <script>
  export default {
    name: 'Author-List-Element',
    props: {
        author: {
            type: Object,
            required: true
        },
        position: {
            type: Number,
            required: false
        }
    }
  }
  </script>