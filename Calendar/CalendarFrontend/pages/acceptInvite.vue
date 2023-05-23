

<template>
    <RoomManagementMain/>  
</template>
    
<script>
export default {
    name: 'accept invite',

    async mounted() {
        const roomId = this.$nuxt._route.query.roomId
        const token = this.$nuxt._route.query.token
        
        try
        {
            const result = await this.$axios.$get(`/Room/${roomId}/acceptInvite?token=${token}`)
            this.$router.push({ path: '/'})
            //window.location = "http://localhost:3000/"
            //window.history.pushState({},'',"")
            //window.location.reload()
        }
        catch{

        }
    }

}
</script>
