<template>
    <v-row justify="center" class="application-files-row">
        <v-file-input
            :value="files"
            small-chips
            show-size
            multiple
            clearable
            label="Select files"
            @change="inputChanged"
        >
            <template v-slot:selection="{ text, index, file }">
                <v-chip
                    small
                    text-color="white"
                    color="#295671"
                    close
                    @click:close="remove(index)"
                >
                {{ text }}
                </v-chip>
            </template>
        </v-file-input>
    </v-row>
</template>

<script>
export default {
    props: ['files'],
    
    data () {
      return {
            previousFiles: []
        }
    },

    created() {
    },

    methods: {
        remove (index) {
            this.$emit("remove", index);
        },
        inputChanged(value) {
            this.$emit('change', value);
        }
    }
  }
</script>

<style>
.application-files-row {
    max-width: 100%;
    margin-top: 10px;
}
</style>