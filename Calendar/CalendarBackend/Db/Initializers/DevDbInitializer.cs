using Microsoft.EntityFrameworkCore;

namespace CalendarBackend.Db.Initializers;

public class DevDbInitializer
{
    private readonly ModelBuilder _modelBuilder;

    public DevDbInitializer(ModelBuilder modelBuilder)
    {
        this._modelBuilder = modelBuilder;
    }

    public void Seed()
    {
        _modelBuilder.Entity<UserRole>().HasData(
           new UserRole() { Id = 1, Name = "admin" },
           new UserRole() { Id = 2, Name = "member" }
        );

        _modelBuilder.Entity<Room>().HasData(
           new Room() { Id = 1, Name = "room1" },
           new Room() { Id = 2, Name = "room2" }
        );
    }
}

