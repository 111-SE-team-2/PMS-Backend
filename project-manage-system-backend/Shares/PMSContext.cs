using Microsoft.EntityFrameworkCore;
using project_manage_system_backend.Models;

namespace project_manage_system_backend.Shares
{
    public class PMSContext : DbContext
    {
        public PMSContext(DbContextOptions<PMSContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Jira> Jiras { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Repo> Repositories { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<ScheduleOption> ScheduleOptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserProject>().HasKey(userProject => new { userProject.ProjectId, userProject.Account });
            modelBuilder.Entity<UserProject>()
                .HasOne(userProject => userProject.Project)
                .WithMany(project => project.Users)
                .HasForeignKey(userProject => userProject.ProjectId);
            modelBuilder.Entity<UserProject>()
                .HasOne(userProject => userProject.User)
                .WithMany(user => user.Projects)
                .HasForeignKey(userProject => userProject.Account);

            modelBuilder.Entity<UserScheduleOption>().HasKey(userScheduleOption => new { userScheduleOption.ScheduleOptionId, userScheduleOption.Account });
            modelBuilder.Entity<UserScheduleOption>()
                .HasOne(userScheduleOption => userScheduleOption.ScheduleOption)
                .WithMany(scheduleOption => scheduleOption.Users)
                .HasForeignKey(ut => ut.ScheduleOptionId);
            modelBuilder.Entity<UserScheduleOption>()
                .HasOne(userScheduleOption => userScheduleOption.User)
                .WithMany(user => user.ScheduleOptions)
                .HasForeignKey(userScheduleOption => userScheduleOption.Account);
        }
    }
}
