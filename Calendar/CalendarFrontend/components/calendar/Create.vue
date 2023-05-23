<template>
  <div class="text-center">
    <v-dialog
      width="500"
      v-model="dialog"
      @click:outside="createEvent"
    >
      <v-card>
        <v-card-title class="text-h5">
            {{ event.id == null ? "Create event" : "Edit event" }}
        </v-card-title>

        <v-card-text>
          <CalendarDateTimePicker 
            @save="changeTimeStart"
            :time="convertTime(event.start)"
            :date="new Date(event.start).toISOString().substr(0, 10)"
            label="Start time"
          />
          <CalendarDateTimePicker 
            @save="changeTimeEnd"
            :time="convertTime(event.end)"
            :date="new Date(event.end).toISOString().substr(0, 10)"
            label="End time"
          />
          <v-text-field
              v-model="nameEventsField"
              :counter="32"
              label="Name event"
              required
          ></v-text-field>

          <v-textarea
              v-model="descriptionField"
              :counter="144"
              label="Description"
              required
          ></v-textarea>
          
          <CalendarUserList @change="changeUsers" :selectUsers="users"/>
          <CalendarFileInput @change="changeFiles" @remove="removeFiles" :files="files"/>
          <v-list>
            <v-subheader>Download files</v-subheader>
            <v-list-item v-for="file in event.files">
              <v-list-item-content>
                <v-list-item-title><a :href="file.link"> {{ file.name }} </a></v-list-item-title>
              </v-list-item-content>
            </v-list-item>

          </v-list>
        </v-card-text>
        <v-divider></v-divider>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn
            color="primary"
            text
            @click="createEvent()"
          >
            {{ event.id == null ? "Create" : "Close" }}
          </v-btn>

          <v-btn
            color="error"
            text
            @click="closeDialog()"
          >
            {{ event.id == null ? "Cancel" : "Delete" }}
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
        taskId: null,
        nameEventsField: "",
        descriptionField: "",
        files: [],
        users: [],
        eventStart: null,
        eventEnd: null,
        dialog: true
    }),

    created() {
      this.taskId = this.event.id;
      this.nameEventsField = this.event.name;
      this.descriptionField = this.event.description;
      this.eventStart = this.event.start;
      this.eventEnd = this.event.end;
      this.files = this.event.files;
      this.users = this.event.users;
    },

    methods: {
        changeUsers(users) {
          this.users = users;
        },

        changeTimeStart(timestamp) {
          console.log("timestamp", timestamp)
          this.eventStart = timestamp;
        },

        changeTimeEnd(timestamp) {
          this.eventEnd = timestamp;
        },

        convertTime(timestamp) {
          return `${new Date(timestamp).getHours()}:${new Date(timestamp).getMinutes().toString().padStart(2, '0')}`;
        },

        formatedTime(time) {
            const date = new Date(time);
            const hours = date.getUTCHours().toString().padStart(2, "0");
            const minutes = date.getUTCMinutes().toString().padStart(2, "0");
            const day = date.getUTCDate().toString().padStart(2, "0");
            const month = date.getUTCMonth().toString().padStart(2, "0");
            const year = date.getUTCFullYear().toString().padStart(2, "0");

            return `${day}.${month}.${year} ${hours}:${minutes}`;
        },

        async removeFiles(fileIndex) {
          if (this.files[fileIndex].id) {
            await this.$axios.$delete(`/Room/${this.$store.state.activeRoom.id}/tasks/${this.event.id}/files/${this.files[fileIndex].id}`);          
            this.files.splice(fileIndex, 1)
          }
        },

        async changeFiles(newFiles) {
          
          if (!this.taskId) {
            this.files = newFiles;
            return;
          }
          
          for (var item in this.files) {
            if (this.files[item].id) {
              await this.removeFiles(item)
            }
          }

          this.files = newFiles;

          for (var item in newFiles) {
              var bodyFormData = new FormData();

              bodyFormData.append("file", newFiles[item])
              var data = await this.$axios.$post(`/Room/${this.$store.state.activeRoom.id}/tasks/${this.taskId}/files`, bodyFormData)
              this.files[item] = data;
          }
        },

        createEvent() {
            var event = this.event;
            
            event.name = this.nameEventsField;
            event.description = this.descriptionField;
            event.start = this.eventStart;
            event.end = this.eventEnd;
            event.files = this.files;
            event.users = this.users;
            
            this.$emit('onCreateEvent', event)
        },

        closeDialog() {
          this.$emit('closeDialog', this.event)
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