
<template>
  <div class="container">
    <div class="row justify-content-center">
      <div class="col-lg-3 col-md-12 col-sm-12 mb-3 order-lg-1 order-md-1 order-sm-1">
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
                      <button type="button" class="btn btn-primary" @click="getPosts">Применить фильтры</button>
                    </li>
                  </ul>
              </div>
            </div>
          </div>
        </div>
        <router-link to="/createPost" v-if="store.isAuthorized && isAdmin"><button type="button" class="btn btn-primary">Создать пост</button></router-link>
      </div>
      <div class="col-lg-6 col-md-12 col-sm-12 order-lg-2 order-md-3 order-sm-3 order-3">
        <div class="d-none d-lg-block">
          <div class="overflow-auto mb-3" style="max-height: 87vh;">
            <CommunityPost v-for="post in posts" :key="post.id" :post="post" />
          </div>
        </div>
        <div class="d-block d-lg-none">
          <div class="overflow-auto mb-3" style="max-height: 65vh;">
            <CommunityPost v-for="post in posts" :key="post.id" :post="post" />
          </div>
        </div>
        <div class="container" v-if="posts.length != 0">
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
      <div class="col-lg-3 col-md-12 col-sm-12 order-lg-3 order-md-2 order-sm-2 order-2">
        <div class="accordion" id="accordionExample">
          <div class="accordion-item">
            <h2 class="accordion-header" id="headingOne">
              <button class="accordion-button" type="button" data-bs-toggle="collapse" data-bs-target="#collapseTwo" aria-expanded="true" aria-controls="collapseTwo">
                Сообщество
              </button>
            </h2>
            <div id="collapseTwo" class="accordion-collapse collapse show" aria-labelledby="headingOne" data-bs-parent="#accordionExample">
              <div class="accordion-body">
                <CommunityInfo v-if="infoRecieved" :community="info" :communityId="id" class="mb-2" @roleEvent="handleRole"/>
                <Administrator v-for="admin in info.administrators" :key="admin.id" :author="admin" class="mb-2"/>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
  
<script>
import CommunityPost from './UserPost.vue'
import CommunityInfo from './CommunityListElement.vue'
import Administrator from './Author-List-Element.vue'
import { store } from '@/store/store.js';
 
export default {
  name: 'CommunityPage',
  components: {
    CommunityPost,
    Administrator,
    CommunityInfo
  },
  setup(){
    return{
      store
    }
  },
  mounted(){
    this.getInfo();
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
      pages: 1,
      currentPage: 1,
      pageNumbers: [1, 2, 3],
      info: {},
      postsOnPage: 5,
      id: this.$route.params.id,
      infoRecieved: false,
      isAdmin: false
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
    async getInfo(){
      try {
        const response = await fetch('https://blog.kreosoft.space/api/community/' + this.id, {
          method: 'GET',
          headers: {
            'Content-Type': 'application/json',
          },
        });
 
      if (!response.ok) {
        throw new Error('Ошибка HTTP: ' + response.status);
      }
 
      const data = await response.json();
 
      this.info = data;
      this.infoRecieved = true;
      } catch (error) {
        console.error('Ошибка HTTP:', error);
      }
    },
    async getPosts(){
      try {
        const token = localStorage.getItem('token');
 
        let params = {
          sorting: this.sorting,
          page: this.currentPage,
          size: this.postsOnPage,
        };
 
        if(this.selectedTags.length > 0){
          params.tags = this.selectedTags;
        }
 
        const query = new URLSearchParams(params).toString();
 
        console.log(query);
 
        const response = await fetch('https://blog.kreosoft.space/api/community/'+ this.id +'/post?'+query, {
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
        
        const currentRoute = { ...this.$route };
        currentRoute.query = Object.assign({}, params)
        this.$router.push(currentRoute);

 
        console.log(data.posts)     
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
    },
    handleRole(data){
      this.isAdmin = data;
    }
  }
}
</script>
 
<style>

</style>