using System.Reflection;
using ContactCalls.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactCalls.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<ContactProfile> ContactProfiles { get; set; }
    public DbSet<Phone> Phones { get; set; }
    public DbSet<Call> Calls  { get; set; }
    public DbSet<Conference> Conferences  { get; set; }
    public DbSet<ConferenceParticipant> ConferenceParticipants  { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}