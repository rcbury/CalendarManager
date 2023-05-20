<template>
  <v-row class="fill-height">
    <v-col>
      <v-sheet height="100vh">
        <CalendarSettings @getCalendarName="getCalendarName" :type="type" @setToday="setToday" @prev="prev" @next="next" @changeType="changeType"/>
        
        <v-calendar
          ref="calendar"
          v-model="value"
          color="primary"
          height="100vh"
          :type="type"
          :events="events"
          :event-color="getEventColor"
          :event-ripple="false"
          @click:event="clickEvent"
          @mousedown:event="startDrag"
          @mousedown:time="startTime"
          @mousemove:time="mouseMove"
          @mouseup:time="endDrag"
          @mouseleave.native="cancelDrag"
        >
          <template v-slot:event="{ event, timed, eventSummary }">
            <div class="v-event-draggable">
              <component :is="{ render: eventSummary }"></component>
            </div>
            
            <v-avatar v-for="item of event.users" :key="item.id" :color=stringToColour(item.userName.at(0)) size="20">
              <img v-if="item.avatarPath" :src="item.avatarPath"/>
              <div v-else>
                {{ item.userName.at(0) }}
              </div>
            </v-avatar>

            <div
              v-if="timed"
              class="v-event-drag-bottom"
              @mousedown.stop="extendBottom(event)"
            ></div>
          </template>
          <template v-slot:day-body="{ date, week }">
            <div
              class="v-current-time"
              :class="{ first: date === week[0].date }"
              :style="{ top: nowY }"
            >
          </div>
          </template>
        </v-calendar>

        <CalendarCreate @onCreateEvent="onCreateEvent" @closeDialog="closeDialog" :openDialogType="openDialogType" :event="events[openDialogIndex]" v-if="openDialogType != null"/>
      </v-sheet>
    </v-col>
  </v-row>
</template>

<script>
  export default {
    data: () => ({
      value: '',
      events: [],
      colors: ['#2196F3', '#3F51B5', '#673AB7', '#00BCD4', '#4CAF50', '#FF9800', '#757575'],
      names: ['Meeting', 'Holiday', 'PTO', 'Travel', 'Event', 'Birthday', 'Conference', 'Party'],
      dragEvent: null,
      dragStart: null,
      createEvent: null,
      createStart: null,
      extendOriginal: null,
      ready: false,
      type: "week",
      openDialogType: null,
      openDialogIndex: null
    }),

    created() {
      console.log("[calendar] loading main component")
    },

    computed: {
      cal () {
        return this.ready ? this.$refs.calendar : null
      },
      nowY () {
        return this.cal ? this.cal.timeToY(this.cal.times.now) + 'px' : '-10px'
      },
    },

    mounted () {
      this.ready = true
      this.scrollToTime()
      this.updateTime()

      this.$refs.calendar.title = "TestCalendar"
    },
    
    async fetch() {
      this.events = [];
      const tasks = await this.$axios.$get(`/Room/${this.$store.state.activeRoom.id}/tasks`);

      for (var task of tasks) {
        const index = this.events.push({
          id: task.id,
          name: task.name,
          description: task.description,
          start: new Date(task.dateStart).getTime(),
          color: this.stringToColour(task.name) + (new Date() > new Date(task.dateEnd) ? '55' : 'ff'),
          end: new Date(task.dateEnd).getTime(),
          files: task.files,
          timed: true,
          users: task.users
        });

        //const data = await this.$axios.$get(`/Room/${this.$store.state.activeRoom.id}/tasks/${task.id}/files`);
        //this.events[index - 1].files = data;
      }
    },

    methods: {
      stringToColour(str) {
        var hash = 0;
        for (var i = 0; i < str.length; i++) {
          hash = str.charCodeAt(i) + ((hash << 5) - hash);
        }
        var colour = '#';
        for (var i = 0; i < 3; i++) {
          var value = (hash >> (i * 8)) & 0xFF;
          colour += ('00' + value.toString(16)).substr(-2);
        }
        return colour;
      },

      clickEvent({ nativeEvent, event }) {
        this.openDialog('open', event.id)
      },

      getCalendarName() {
        return this.$refs.calendar.title;
      },

      changeType(type) {
        this.type = type;
      },

      setToday () {
        this.focus = ''
      },

      prev () {
        this.$refs.calendar.prev()
      },

      next () {
        this.$refs.calendar.next()
      },

      getCurrentTime () {
        return this.cal ? this.cal.times.now.hour * 60 + this.cal.times.now.minute : 0
      },

      scrollToTime () {
        const time = this.getCurrentTime()
        const first = Math.max(0, time - (time % 30) - 30)

        this.cal.scrollToTime(first)
      },

      updateTime () {
        setInterval(() => this.cal.updateTimes(), 60 * 1000)
      },

      startDrag ({ event, timed }) {
        if (event && timed) {
          this.dragEvent = event
          this.dragTime = null
          this.extendOriginal = null
        }
      },

      startTime (tms) {
        const mouse = this.toTime(tms)

        if (this.dragEvent && this.dragTime === null) {
          const start = this.dragEvent.start

          this.dragTime = mouse - start
        } else {
          this.createStart = this.roundTime(mouse)
          this.createEvent = {
            name: `Event #${this.events.length}`,
            color: this.rndElement(this.colors),
            start: this.createStart,
            end: this.createStart,
            timed: true,
          }

          this.events.push(this.createEvent)
        }
      },
      
      extendBottom (event) {
        this.createEvent = event
        this.createStart = event.start
        this.extendOriginal = event.end
      },

      mouseMove (tms) {
        const mouse = this.toTime(tms)

        if (this.dragEvent && this.dragTime !== null) {
          const start = this.dragEvent.start
          const end = this.dragEvent.end
          const duration = end - start
          const newStartTime = mouse - this.dragTime
          const newStart = this.roundTime(newStartTime)
          const newEnd = newStart + duration

          this.dragEvent.start = newStart
          this.dragEvent.end = newEnd
        } else if (this.createEvent && this.createStart !== null) {
          const mouseRounded = this.roundTime(mouse, false)
          const min = Math.min(mouseRounded, this.createStart)
          const max = Math.max(mouseRounded, this.createStart)

          this.createEvent.start = min
          this.createEvent.end = max
        }
      },
      
      openDialog(type, eventId) {
        if (type == 'open') {
          this.openDialogIndex = this.events.findIndex(item => item.id == eventId);
        } else {
          this.openDialogIndex = this.events.length - 1;
          this.events[this.openDialogIndex].id = null;
        }

        if (this.openDialogIndex != -1)
          this.openDialogType = type;
      },

      async closeDialog(event) {
        this.openDialogType = null;
        
        if (event) {
          var elementIndex = this.events.findIndex(item => item.id == event.id);
          if (elementIndex != -1) {
            this.events.splice(elementIndex, 1);
            this.deleteApiTask(event);
          }
        }
      },

      async onCreateEvent(event) {
        this.closeDialog();
        
        var eventIndex = event.id != null ? this.events.findIndex(item => item.id == event.id) : this.events.length;
        
        if (eventIndex != -1) {
          this.events[eventIndex] = { id: null, ...event };
        }

        await this.updateEventApi(eventIndex);
        this.$fetch();
      },

      async deleteApiTask(event) {
        await this.$axios.$delete(`/Room/${this.$store.state.activeRoom.id}/tasks/${event.id}`);
      },  

      async updateEventApi(eventIndex) {
        let requestBody = null;
        
        if (this.events[eventIndex].users)
          this.events[eventIndex].users = this.events[eventIndex].users.filter(item => item != null && item.email && item.userName)
        
        if (this.events[eventIndex].id) {
          requestBody = { 
            id: this.events[eventIndex].id,
            name: this.events[eventIndex].name, 
            description: this.events[eventIndex].description, 
            DateStart: new Date(this.events[eventIndex].start), 
            DateEnd: new Date(this.events[eventIndex].end), 
            RoomId: this.$store.state.activeRoom.id,
            files: this.events[eventIndex].files,
            users: this.events[eventIndex].users
          };

          await this.$axios.$put(`/Room/${this.$store.state.activeRoom.id}/tasks/${this.events[eventIndex].id}`, requestBody);
        } else {
          requestBody = { 
            name: this.events[eventIndex].name, 
            description: this.events[eventIndex].description, 
            DateStart: new Date(this.events[eventIndex].start), 
            DateEnd: new Date(this.events[eventIndex].end), 
            RoomId: this.$store.state.activeRoom.id, 
            users: this.events[eventIndex].users
          };

          var data = await this.$axios.$post(`/Room/${this.$store.state.activeRoom.id}/tasks`, requestBody);
          this.events[eventIndex - 1].id = data.id;

          if (this.events[eventIndex - 1].files && this.events[eventIndex - 1].files.length > 0) {
            for (var item in this.events[eventIndex - 1].files) {
              var bodyFormData = new FormData();

              bodyFormData.append("file", this.events[eventIndex - 1].files[item])
              
              var data = await this.$axios.$post(`/Room/${this.$store.state.activeRoom.id}/tasks/${this.events[eventIndex - 1].id}/files`, bodyFormData)
              this.events[eventIndex - 1].files[item] = data;
            }
          }
        }
      },

      endDrag () {
        if (this.createEvent)
          this.openDialog('create');
        
        this.dragTime = null
        this.dragEvent = null
        this.createEvent = null
        this.createStart = null
        this.extendOriginal = null
      },

      cancelDrag () {
        if (this.createEvent) {
          if (this.extendOriginal) {
            this.createEvent.end = this.extendOriginal
          } else {
            const i = this.events.indexOf(this.createEvent)
            if (i !== -1) {
              this.events.splice(i, 1)
            }
          }
        }

        this.createEvent = null
        this.createStart = null
        this.dragTime = null
        this.dragEvent = null
      },

      roundTime (time, down = true) {
        const roundTo = 15 // minutes
        const roundDownTime = roundTo * 60 * 1000

        return down
          ? time - time % roundDownTime
          : time + (roundDownTime - (time % roundDownTime))
      },
      
      toTime (tms) {
        return new Date(tms.year, tms.month - 1, tms.day, tms.hour, tms.minute).getTime()
      },
      
      getEventColor (event) {
        const rgb = parseInt(event.color.substring(1), 16)
        const r = (rgb >> 16) & 0xFF
        const g = (rgb >> 8) & 0xFF
        const b = (rgb >> 0) & 0xFF

        return event === this.dragEvent
          ? `rgba(${r}, ${g}, ${b}, 0.7)`
          : event === this.createEvent
            ? `rgba(${r}, ${g}, ${b}, 0.7)`
            : event.color
      },

      getEvents ({ start, end }) {
        const events = []

        const min = new Date(`${start.date}T00:00:00`).getTime()
        const max = new Date(`${end.date}T23:59:59`).getTime()
        const days = (max - min) / 86400000
        const eventCount = this.rnd(days, days + 20)

        for (let i = 0; i < eventCount; i++) {
          const timed = this.rnd(0, 3) !== 0
          const firstTimestamp = this.rnd(min, max)
          const secondTimestamp = this.rnd(2, timed ? 8 : 288) * 900000
          const start = firstTimestamp - (firstTimestamp % 900000)
          const end = start + secondTimestamp

          events.push({
            name: this.rndElement(this.names),
            color: this.rndElement(this.colors),
            start,
            end,
            timed,
          })
        }

        this.events = events
      },

      rnd (a, b) {
        return Math.floor((b - a + 1) * Math.random()) + a
      },

      rndElement (arr) {
        return arr[this.rnd(0, arr.length - 1)]
      },
    },
  }
</script>

<style scoped lang="scss">
.v-current-time {
  height: 2px;
  background-color: #ea4335;
  position: absolute;
  left: -1px;
  right: 0;
  pointer-events: none;

  &.first::before {
    content: '';
    position: absolute;
    background-color: #ea4335;
    width: 12px;
    height: 12px;
    border-radius: 50%;
    margin-top: -5px;
    margin-left: -6.5px;
  }
}


.col {
  .v-sheet {
    overflow-y: hidden;
    // height: 95vh !important;
  }
}

.v-event-draggable {
  padding-left: 6px;
}

.v-event-timed {
  user-select: none;
  -webkit-user-select: none;
}

.v-event-drag-bottom {
  position: absolute;
  left: 0;
  right: 0;
  bottom: 4px;
  height: 4px;
  cursor: ns-resize;

  &::after {
    display: none;
    position: absolute;
    left: 50%;
    height: 4px;
    border-top: 1px solid white;
    border-bottom: 1px solid white;
    width: 16px;
    margin-left: -8px;
    opacity: 0.8;
    content: '';
  }

  &:hover::after {
    display: block;
  }
}

a.nostyle:link {
    text-decoration: inherit;
    color: inherit;
    cursor: auto;
}

a.nostyle:visited {
    text-decoration: inherit;
    color: inherit;
    cursor: auto;
}
</style>
