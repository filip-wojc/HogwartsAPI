using HogwartsAPI.Enums;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net;

namespace HogwartsAPI.Entities
{
    public class HogwartDbContext : DbContext
    {
        public HogwartDbContext(DbContextOptions<HogwartDbContext> options) : base(options)
        {
            
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Wand> Wands { get; set; }
        public DbSet<Core> Cores { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<House> Houses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.Email).IsRequired();
            modelBuilder.Entity<User>()
                .Property(u => u.Username).IsRequired();

            modelBuilder.Entity<Student>()
            .HasOne(s => s.House)
            .WithMany(h => h.Students)
            .HasForeignKey(s => s.HouseId)
            .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Student>()
                .HasMany(s => s.Pets)
                .WithOne(p => p.Student)
                .HasForeignKey(p => p.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Student>()
               .HasOne(s => s.Wand)
               .WithMany(w => w.StudentOwners)
               .HasForeignKey(p => p.WandId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Teacher>()
               .HasOne(t => t.Wand)
               .WithMany(w => w.TeacherOwners)
               .HasForeignKey(p => p.WandId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Course>()
               .HasOne(c => c.Teacher)
               .WithOne(t => t.Course)
               .HasForeignKey<Course>(p => p.TeacherId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Wand>()
               .HasOne(w => w.Core)
               .WithMany(c => c.Wands)
               .HasForeignKey(p => p.CoreId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<House>()
                .Property(h => h.Name).HasConversion<string>();

            modelBuilder.Entity<Pet>()
                .Property(h => h.Type).HasConversion<string>();


            modelBuilder.Entity<Core>().HasData(
                new Core { Id = 1, Name = "Phoenix Feather", Description = "A feather from a phoenix" },
                new Core { Id = 2, Name = "Dragon Heartstring", Description = "A heartstring from a dragon" },
                new Core { Id = 3, Name = "Unicorn Hair", Description = "A hair from a unicorn" }
            );


            modelBuilder.Entity<Wand>().HasData(
                new Wand { Id = 1, Price = 150, Length = 11, WoodType = "Holly", Color = "Brown", CoreId = 1 },
                new Wand { Id = 2, Price = 120, Length = 10, WoodType = "Yew", Color = "Black", CoreId = 2 },
                new Wand { Id = 3, Price = 90, Length = 9.5, WoodType = "Elm", Color = "Light Brown", CoreId = 3 },
                new Wand { Id = 4, Price = 80, Length = 12, WoodType = "Oak", Color = "White", CoreId = 2 },
                new Wand { Id = 5, Price = 190, Length = 10.5, WoodType = "Ivory", Color = "Dark Brown", CoreId = 3 }
            );


            modelBuilder.Entity<Teacher>().HasData(
            new { Id = 1, Name = "Albus", Surname = "Dumbledore", DateOfBirth = new DateTime(1881, 7, 31), WandId = 1},
            new { Id = 2, Name = "Severus", Surname = "Snape", DateOfBirth = new DateTime(1930, 1, 9), WandId = 2 },
            new { Id = 3, Name = "Minerva", Surname = "McGonagall", DateOfBirth = new DateTime(1922, 3, 4), WandId = 3},
            new { Id = 4, Name = "Pomona", Surname = "Sprout", DateOfBirth = new DateTime(1951, 10, 20), WandId = 4,},
            new { Id = 5, Name = "Filius", Surname = "Flitwick", DateOfBirth = new DateTime(1943, 12, 10), WandId = 5,}
             );
      


            modelBuilder.Entity<House>().HasData(
           new House { Id = 1, Name = HouseName.Gryffindor, Description = "Brave and daring", CreationDate = new DateTime(990, 1, 1), TrophyCount = 10, TeacherId = 3},
           new House { Id = 2, Name = HouseName.Hufflepuff, Description = "Loyal and fair", CreationDate = new DateTime(990, 1, 1), TrophyCount = 5, TeacherId = 4},
           new House { Id = 3, Name = HouseName.Ravenclaw, Description = "Wise and clever", CreationDate = new DateTime(990, 1, 1), TrophyCount = 7, TeacherId = 5},
           new House { Id = 4, Name = HouseName.Slytherin, Description = "Cunning and ambitious", CreationDate = new DateTime(990, 1, 1), TrophyCount = 8, TeacherId = 2}
           );


            modelBuilder.Entity<Student>().HasData(
            new { Id = 1, Name = "Harry", Surname = "Potter", DateOfBirth = new DateTime(1980, 7, 31), WandId = 1, HouseId = 1, SchoolYear = 1 },
            new { Id = 2, Name = "Hermione", Surname = "Granger", DateOfBirth = new DateTime(1979, 9, 19), WandId = 3, HouseId = 1, SchoolYear = 1 },
            new { Id = 3, Name = "Fred", Surname = "Weasley", DateOfBirth = new DateTime(1976, 3, 4), WandId = 4, HouseId = 1, SchoolYear = 3 },
            new { Id = 4, Name = "Hanna", Surname = "Abbot", DateOfBirth = new DateTime(1978, 9, 19), WandId = 3, HouseId = 2, SchoolYear = 2 },
            new { Id = 5, Name = "Cedric", Surname = "Diggory", DateOfBirth = new DateTime(1975, 1, 1), WandId = 5,HouseId = 2, SchoolYear = 4 },
            new { Id = 6, Name = "Cho", Surname = "Chang", DateOfBirth = new DateTime(1979, 10, 3), WandId = 5, HouseId = 3, SchoolYear = 1 },
            new { Id = 7, Name = "Draco", Surname = "Malfoy", DateOfBirth = new DateTime(1980, 3, 3), WandId = 2, HouseId = 4, SchoolYear = 1 },
            new { Id = 8, Name = "Vincent", Surname = "Crabbe", DateOfBirth = new DateTime(1979, 1, 24), WandId = 2, HouseId = 4, SchoolYear = 1 }
                );


            modelBuilder.Entity<Pet>().HasData(
                new Pet { Id = 1, Name = "Hedwig", Type = PetType.Owl, HasMagicAbility = false, StudentId = 1 },
                new Pet { Id = 2, Name = "Crookshanks", Type = PetType.Cat, HasMagicAbility = true, StudentId = 2 },
                new Pet { Id = 3, Name = "Teodora", Type = PetType.Frog, HasMagicAbility = false, StudentId = 4 },
                new Pet { Id = 4, Name = "Random Owl", Type = PetType.Owl, HasMagicAbility = true, StudentId = 7 }
            );

            modelBuilder.Entity<Course>().HasData(
                new Course { Id = 1, Name = "Defense Against the Dark Arts", Description = "Learn how to defend against dark magic.", DifficultyLevel = 3, Date = new DateTime(2024, 8, 4), TeacherId = 1 },
                new Course { Id = 2, Name = "Potions", Description = "Learn how to brew magical potions.", DifficultyLevel = 5, Date = new DateTime(2024, 8, 5), TeacherId = 2 },
                new Course { Id = 3, Name = "Spells", Description = "Learn how to cast advanced speels.", DifficultyLevel = 2, Date = new DateTime(2024, 8, 6), TeacherId = 5 },
                new Course { Id = 4, Name = "Herbology", Description = "Learn how to take care of plants.", DifficultyLevel = 3, Date = new DateTime(2024, 8, 7), TeacherId = 4 }
            );

            modelBuilder.Entity<Role>().HasData(
                    new Role { Id = 1, Name = "User"},
                    new Role { Id = 2, Name = "Manager"},
                    new Role { Id = 3, Name = "Admin"}
                );

        }
    }
}
