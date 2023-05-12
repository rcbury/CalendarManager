<template>
  <v-dialog
    v-model="isTaskOpen"
    persistent
    width="1024"
  >
    <v-card>
      <v-card-title>
        <span class="text-h5">Task</span>
      </v-card-title>
      <v-card-text>
        <v-container>
          <v-row>

          </v-row>
        </v-container>
      </v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn
          color="blue-darken-1"
          variant="text"
          @click="$event => close()"
        >
          Close
        </v-btn>
        <v-btn
          color="blue-darken-1"
          variant="text"
          @click="$event => close()"
        >
          Save
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
  export default {
    props: ["isTaskOpen"],
    methods: {
      close() {
        this.$emit('closeTask')
      },
    }
  }
</script>