﻿using CalendarBackend.Db;
using CalendarBackend.Dto;
using System.Runtime.InteropServices;

namespace CalendarBackend.Repository.Interfaces
{
    public interface IRoomRepository
    {
        public List<RoomDto> GetAll();
        public RoomDto GetById(int id);
        public List<UserRoomDto> GetUsersByRoom(int roomId);
        public List<RoomDto> GetByUser(int userId);
        public RoomDto Update(RoomDto dto);
        public RoomDto Create(RoomDto dto, CalendarUser? user);
        public bool ToggleAdmin(int roomId, int userId);
        public void AddUser(int roomId, int userId);
        public void DeleteUser(int roomId, int userId);
        public void DeleteById(int id);
    }
}
