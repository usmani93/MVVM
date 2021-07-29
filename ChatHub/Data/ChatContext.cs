using ChatAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAPI.Data
{
    public class ChatContext : DbContext
    {
        public ChatContext(DbContextOptions<ChatContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
