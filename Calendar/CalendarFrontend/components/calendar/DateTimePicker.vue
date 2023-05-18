<template>
    <v-flex sm12>
      <v-menu
        ref="menu"
        v-model="menu2"
        :close-on-content-click="false"
        :nudge-right="40"
        :return-value.sync="timedString"
        lazy
        transition="scale-transition"
        offset-y
        location="top"
        max-width="300px"
        full-width
      >
        <template v-slot:activator="{ on }">
          <v-text-field
            v-model="timedString"
            :label="label"
            readonly
            v-on="on"
          ></v-text-field>
        </template>

        <v-time-picker
          v-if="menu2"
          v-model="time"
          format="24hr"
          full-width
        ></v-time-picker>

        <v-date-picker v-if="menu2" v-model="date" no-title scrollable>
          <v-spacer></v-spacer>
          <v-btn flat color="primary" @click="menu2 = false">Cancel</v-btn>
          <v-btn flat color="primary" @click="$refs.menu.save(date) || save()">OK</v-btn>
        </v-date-picker>
      </v-menu>
    </v-flex>
</template>

<script>
  export default {
    props: ["label", "time", "date"],

    data () {
      return {
        date: new Date().toISOString().substr(0, 10),
        time: null,
        menu2: false,
      }
    },

    methods: {
        save() {
            this.$emit('save', new Date(`${this.date} ${this.time}`))
        }
    },

    computed: {
        timedString() {
            return `${this.date} ${this.time == null ? "" : this.time}` 
        }
    }
  }

</script>


<style>

</style>