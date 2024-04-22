<template>
  <div class="container">
    <div class="row justify-content-center">
      <div class="col-lg-3 col-md-12 col-sm-12 mb-3">
        <div class="accordion mb-2">
          <div class="accordion-item">
            <h2 class="accordion-header" id="panelsStayOpen-headingOne">
              <button class="accordion-button" type="button" data-bs-toggle="collapse" data-bs-target="#panelsStayOpen-collapseOne" aria-expanded="true" aria-controls="panelsStayOpen-collapseOne">
                Фильтры
              </button>
            </h2>
            <div id="panelsStayOpen-collapseOne" class="accordion-collapse collapse show" aria-labelledby="panelsStayOpen-headingOne">
              <div class="accordion-body">
                  <ul class="list-group list-group-flush text-start">
                    <li class="list-group-item">Username
                      <input type="text" class="form-control" aria-label="Default" v-model="userName">
                    </li>
                    <li class="list-group-item">Теги
                      <select class="custom-select form-control" multiple v-model="selectedTags">
                        <option v-for="tag in tags" :key="tag.id" :value="tag.id">
                          {{ tag.name }}
                        </option>
                      </select>
                    </li>
                    <li class="list-group-item">Сортировка
                      <select class="custom-select form-control" v-model="sorting">
                        <option value="CreateDesc">По убыванию даты</option>
                        <option value="CreateAsc">По возрастанию даты</option>
                        <option value="LikeDesc">По убыванию лайков</option>
                        <option value="LikeAsc">По возрастанию лайков</option>
                      </select>
                    </li>
                    <li class="list-group-item">
                      Время чтения от
                      <input type="number" class="form-control" aria-label="Default" v-model="readingTimeMin">
                    </li>
                    <li class="list-group-item">
                      Время чтения до
                      <input type="number" class="form-control" aria-label="Default" v-model="readingTimeMax">
                    </li>
                    <li class="list-group-item">
                      Только мои группы
                      <input type="checkbox" v-model="onlyMyGroups">
                    </li>
                    <li class="list-group-item">
                      <button type="button" class="btn btn-primary" @click="getPosts">Применить фильтры</button>
                    </li>
                  </ul>
              </div>
            </div>
          </div>
        </div>
        <router-link to="/post/create" v-if="store.isAuthorized"><button type="button" class="btn btn-primary">Создать пост</button></router-link>
      </div>
      <div class="col-lg-6 col-md-12 col-sm-12">
        <div class="container">
          <div class="row">
            <div class="col-12 order-lg-1 order-md-2 order-sm-2 order-2">
              <div class="container">
                <div class="row">
                  <div class="d-none d-lg-block">
                    <div class="overflow-auto mb-3" style="max-height: 87vh;">
                      <UserPost v-for="post in posts" :key="post.id" :post="post" />
                    </div>
                  </div>
                  <div class="d-block d-lg-none">
                    <div class="overflow-auto mb-3" style="max-height: 70vh;">
                      <UserPost v-for="post in posts" :key="post.id" :post="post" />
                    </div>
                  </div>
                </div>
              </div>
            </div>
            <div class="col-12 order-lg-2 order-md-1 order-sm-1 order-1">
              <div class="container">
                <div class="row">
                  <div class="col-6">
                    <nav aria-label="...">
                      <ul class="pagination pagination-sm">
                        <li class="page-item" @click="changePageBack">
                          <a class="page-link" href="#" aria-label="Previous">
                            <span aria-hidden="true">&laquo;</span>
                          </a>
                        </li>
                        <li class="page-item" v-for="page in computePageNumbers" :key="page" @click="changePage(page)"><a class="page-link" href="#">{{ page }}</a></li>
                        <li class="page-item" @click="changePageForward">
                          <a class="page-link" href="#" aria-label="Next">
                            <span aria-hidden="true">&raquo;</span>
                          </a>
                        </li>
                      </ul>
                    </nav>
                  </div>
                  <div class="col-6">
                    <div class="input-group mb-3">
                      <div class="input-group-prepend">
                        <span class="input-group-text" id="inputGroup-sizing-default">Кол-во постов на странице</span>
                      </div>
                      <input type="number" class="form-control" aria-label="Default" aria-describedby="inputGroup-sizing-default" v-model="postsOnPage">
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div class="col-lg-2 col-md-3 col-sm-12">
      </div>
    </div>
  </div>
</template>

<script>
import UserPost from './UserPost.vue'
import { store } from '@/store/store.js';

export default {
  name: 'Main-page',
  components: {
    UserPost
  },
  props:{
    tagsProp: Array,
    author: String,
    min: Number,
    max: Number,
    sortingProp: String,
    onlyMyCommunities: Boolean,
    page: Number,
    size: Number 
  },
  setup(){
    return{
      store
    }
  },
  mounted(){
    this.userName = this.author;
    this.selectedTags = this.tagsProp;
    this.sorting = this.sortingProp;
    this.readingTimeMin = this.min;
    this.readingTimeMax = this.max;
    this.onlyMyGroups = this.onlyMyCommunities;
    this.postsOnPage = this.size;
    this.currentPage = this.page;

    this.getPosts();
    this.getTags();
  },
  data() {
    return {
      posts: [],
      userName: '',
      tags: [],
      selectedTags: [],
      sorting: 'CreateDesc',
      readingTimeMax: Infinity,
      readingTimeMin: 0,
      onlyMyGroups: false,
      postsOnPage: 5,
      pages: 1,
      currentPage: 1,
      pageNumbers: [1, 2, 3]
    };
  },
  watch:{
    postsOnPage(){
      this.getPosts();
    },
    currentPage(){
      this.getPosts();
    }
  },
  computed:{
    computePageNumbers(){
      const page = this.currentPage;

      if(page%3 == 1){
        return [page, page+1, page+2];
      }
      else if(page%3 == 2){
        return [page-1, page, page+1];
      }
      else{
        return [page-2, page-1, page];
      }
    }
  },
  methods:{
    async getPosts(){
      try {
        const token = localStorage.getItem('token');
        
        let params = {
          onlyMyCommunities: this.onlyMyGroups,
          sorting: this.sorting,
          page: this.currentPage,
          size: this.postsOnPage,
          min: this.readingTimeMin
        };

        if(this.selectedTags.length > 0){
          params.tags = this.selectedTags;
        }
        if(this.readingTimeMax < Infinity){
          params.max = this.readingTimeMax;
        }
        if(this.userName != ''){
          params.author = this.userName;
        }

        const query = new URLSearchParams(params).toString();

        console.log(query);

        const response = await fetch('https://blog.kreosoft.space/api/post?'+query, {
          method: 'GET',
          headers: {
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + token 
          },
        });

        if (!response.ok) {
          throw new Error('Ошибка HTTP: ' + response.status);
        }

        const data = await response.json();

        this.posts = data.posts;

        this.pages = data.pagination.count;
        this.currentPage = data.pagination.current;

        console.log(data.posts);

        const currentRoute = { ...this.$route };
        currentRoute.query = Object.assign({}, params)
        this.$router.push(currentRoute);
      } catch (error) {
        console.error('Ошибка HTTP:', error);
      }
    },
    async getTags(){
      try {
        const response = await fetch('https://blog.kreosoft.space/api/tag', {
          method: 'GET',
          headers: {
            'Content-Type': 'application/json',
          },
        });

        if (!response.ok) {
          throw new Error('Ошибка HTTP: ' + response.status);
        }

        const data = await response.json();

        this.tags = data;  
      } catch (error) {
        console.error('Ошибка HTTP:', error);
      }
    },
    changePage(pageNumber){
      this.currentPage = pageNumber;
    },
    changePageForward(){
      const add = this.currentPage % 3 == 0 ? 3 : this.currentPage % 3; 
      const page = this.currentPage + (4 - add)
      this.currentPage = page < this.pages ? page : this.pages;
    },
    changePageBack(){
      const add = this.currentPage % 3 == 0 ? 3 : this.currentPage % 3; 
      const page = this.currentPage - (4 - add)
      this.currentPage = page > 0 ? page : 1;
    }
  }
}
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>