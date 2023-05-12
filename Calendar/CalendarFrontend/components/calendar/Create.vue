<template>
  <div class="text-center">
    <v-dialog
      width="500"
      v-model="dialog"
    >
      <v-card>
        <v-card-title class="text-h5 grey lighten-2">
            {{ openDialogType == 'create' ? "Create event" : "Edit event" }}
        </v-card-title>

        <v-card-text>
            <br>
            <div class="application-calendar-time">
                Start: {{ formatedTime(event.start) }}
            </div>

            <div class="application-calendar-time">
                End: {{ formatedTime(event.end) }}
            </div>
            <br><br>

            <v-text-field
                v-model="nameEventsField"
                :counter="32"
                label="Name event"
                required
            ></v-text-field>

            <v-text-field
                v-model="descriptionField"
                :counter="144"
                label="Description"
                required
            ></v-text-field>
        </v-card-text>

        <v-divider></v-divider>

        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn
            color="primary"
            text
            @click="createEvent()"
          >
            {{ openDialogType == 'create' ? "Create" : "Close" }}
          </v-btn>

          <v-btn
            color="error"
            text
            @click="closeDialog(false)"
          >
            {{ openDialogType == 'create' ? "Close" : "Delete" }}
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>
export default {
    props: ["event", "onCreateEvent", "openDialogType"],
    
    data: () => ({
        nameEventsField: "",
        descriptionField: "",
        dialog: true
    }),

    created() {
        this.nameEventsField = this.event.name;
    },

    methods: {
        formatedTime(time) {
            const date = new Date(time);
            const hours = date.getUTCHours().toString().padStart(2, "0");
            const minutes = date.getUTCMinutes().toString().padStart(2, "0");
            const day = date.getUTCDate().toString().padStart(2, "0");
            const month = date.getUTCMonth().toString().padStart(2, "0");
            const year = date.getUTCFullYear().toString().padStart(2, "0");

            return `${day}.${month}.${year} ${hours}:${minutes}`;
        },

        createEvent() {
            var event = this.event;
            
            event.name = this.nameEventsField;
            event.description = this.descriptionField;
            
            this.$emit('onCreateEvent', event)
        },

        closeDialog(isClose) {
            this.$emit('closeDialog', isClose)
        }
    }
}
</script>

<style lang="scss">

.application-calendar {
    &-events {
        position: absolute;
        display: flex;
        justify-content: center;
        align-items: center;
        z-index: 10;
        width: 100vw;
        height: 100vh;
        background-color: rgb(97, 97, 97);
    }
}

</style>