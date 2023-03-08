using CalendarBackend.Db;

class UserRepository
{
    private readonly CalendarDevContext _context;

    public UserRepository(CalendarDevContext context)
    {
        _context = context;
    }

    /* public User? GetUser(int id) */
    /* { */
    /*     return _context.Users.Where(x => x.Id == id).FirstOrDefault(); */
    /* } */

}
