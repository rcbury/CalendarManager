

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
        }
        catch{

        }
    }

}
</script>
