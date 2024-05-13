<template>
  <div>
    <h4>Current Call Center Status:</h4>
    <table>
      <thead>
        <tr>
          <th>Agent</th>
          <th class="clickable" @click="sort('state')">State</th>
          <th class="clickable" @click="sort('timeStampUtc')">Timestamp </th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="item in items" :key="item.id">
          <td>{{ item.name }}</td>
          <td>{{ item.state }}</td>
          <td>{{ formatDate(item.timeStampUtc) }}</td>
        </tr>
      </tbody>
    </table>
    <DummyButton text="Refresh search" @click="() => fetchData()"></DummyButton>
  </div>
</template>

<script>
    import { format } from 'date-fns';

    export default {
      data() {
        return {
          items: [],
          orderBy: '',
          direction: 'asc'
        };
      },
      mounted() {
        this.fetchData();
      },
      methods: {
        async fetchData() {
          try {
            const response = await $fetch('http://localhost:8080/agents');
            this.items = response;
          } catch (error) {
            console.error('Erro while fetching agents:', error);
          }
        },

        sort(column) {
          if(column === this.orderBy) {
            this.direction = this.direction === 'asc' ? 'desc' : 'asc';
          } else {
            this.orderBy = column;
            this.direction = 'asc';
          }

          this.items.sort((curr, next) => {
            const currValue = curr[column];
            const nextValue = next[column];

            if (currValue < nextValue) 
              return this.direction === 'asc' ? -1 : 1;
            if (currValue > nextValue) 
              return this.direction === 'asc' ? 1 : -1;
            
            return 0;
          });
        },

        formatDate(_date) {
          return format(_date, 'eeee, LLL do, yyyy - HH:mm:ss');
        }
      }
    };
</script>

<style scoped>
  .clickable {
    cursor: pointer;
  }

  div {
    display: flex;
    justify-content: center;
    align-content: center;
    flex-direction: column;
  }

  table {
      padding: 5rem;
      border-collapse: collapse;
      width: 100%;
      box-shadow: 0px 0px 5px 1px rgba(0,0,0,0.75);
  }

  th, td {
      text-align: left;
      padding: 1rem;
      min-width: 150px;
  }

  th {
    background-color: #2c3e50;
    color: #ecf0f1;
  }

  tr:nth-child(even){
    background-color: #ecf0f1
  }

  tr:nth-child(odd){
    background-color: #bdc3c7
  }
</style>
