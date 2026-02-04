using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecetArreAPI.Models;

namespace RecetArreAPI.Context
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            // ---------- ApplicationUser ----------
            builder.Entity<ApplicationUser>(e =>
            {
                e.Property(x => x.DisplayName).HasMaxLength(60).IsRequired();
                e.Property(x => x.CreatedAtUtc).HasDefaultValueSql("now()");
                e.HasIndex(x => x.DisplayName);
            });

            // ---------- UserProfile (1-1) ----------
            builder.Entity<UserProfile>(e =>
            {
                e.HasKey(x => x.UserId);

                e.Property(x => x.Bio).HasMaxLength(500);
                e.Property(x => x.AvatarUrl).HasMaxLength(300);
                e.Property(x => x.Location).HasMaxLength(120);
                e.Property(x => x.WebsiteUrl).HasMaxLength(200);
                e.Property(x => x.DietaryPreferences).HasMaxLength(200);

                e.Property(x => x.CreatedAtUtc).HasDefaultValueSql("now()");
                e.Property(x => x.UpdatedAtUtc).HasDefaultValueSql("now()");

                e.HasOne(x => x.User)
                    .WithOne(u => u.Profile)
                    .HasForeignKey<UserProfile>(x => x.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ---------- Recipe ----------
            builder.Entity<Recipe>(e =>
            {
                e.Property(x => x.Id).ValueGeneratedOnAdd();
                e.Property(x => x.Title).HasMaxLength(120).IsRequired();
                e.Property(x => x.Description).HasMaxLength(1000);
                e.Property(x => x.Instructions).HasMaxLength(15000).IsRequired();
                e.Property(x => x.CreatedAtUtc).HasDefaultValueSql("now()");
                e.Property(x => x.UpdatedAtUtc).HasDefaultValueSql("now()");

                e.HasOne(x => x.Author)
                    .WithMany(u => u.Recipes)
                    .HasForeignKey(x => x.AuthorId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasIndex(x => x.CreatedAtUtc);
                e.HasIndex(x => new { x.AuthorId, x.CreatedAtUtc });
            });

            // ---------- Ingredient ----------
            builder.Entity<Ingredient>(e =>
            {
                e.Property(x => x.Id).ValueGeneratedOnAdd();
                e.Property(x => x.Name).HasMaxLength(80).IsRequired();
                e.Property(x => x.Notes).HasMaxLength(250);
                e.Property(x => x.CreatedAtUtc).HasDefaultValueSql("now()");
                e.HasIndex(x => x.Name).IsUnique();
            });

            // ---------- Category ----------
            builder.Entity<Category>(e =>
            {
                e.Property(x => x.Id).ValueGeneratedOnAdd();
                e.Property(x => x.Name).HasMaxLength(60).IsRequired();
                e.Property(x => x.Description).HasMaxLength(250);
                e.Property(x => x.CreatedAtUtc).HasDefaultValueSql("now()");
                e.HasIndex(x => x.Name).IsUnique();
            });

            // ---------- RecipeIngredient ----------
            builder.Entity<RecipeIngredient>(e =>
            {
                e.HasKey(x => new { x.RecipeId, x.IngredientId });

                e.Property(x => x.Quantity).HasPrecision(10, 2);
                e.Property(x => x.Unit).HasMaxLength(20).IsRequired();
                e.Property(x => x.Preparation).HasMaxLength(200);

                e.HasOne(x => x.Recipe)
                    .WithMany(r => r.RecipeIngredients)
                    .HasForeignKey(x => x.RecipeId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(x => x.Ingredient)
                    .WithMany(i => i.RecipeIngredients)
                    .HasForeignKey(x => x.IngredientId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ---------- RecipeCategory ----------
            builder.Entity<RecipeCategory>(e =>
            {
                e.HasKey(x => new { x.RecipeId, x.CategoryId });

                e.HasOne(x => x.Recipe)
                    .WithMany(r => r.RecipeCategories)
                    .HasForeignKey(x => x.RecipeId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(x => x.Category)
                    .WithMany(c => c.RecipeCategories)
                    .HasForeignKey(x => x.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ---------- Comment ----------
            builder.Entity<Comment>(e =>
            {
                e.Property(x => x.Id).ValueGeneratedOnAdd();
                e.Property(x => x.Content).HasMaxLength(1000).IsRequired();
                e.Property(x => x.CreatedAtUtc).HasDefaultValueSql("now()");

                e.HasOne(x => x.Recipe)
                    .WithMany(r => r.Comments)
                    .HasForeignKey(x => x.RecipeId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(x => x.User)
                    .WithMany(u => u.Comments)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasIndex(x => new { x.RecipeId, x.CreatedAtUtc });
            });

            // ---------- Rating ----------
            builder.Entity<Rating>(e =>
            {
                e.Property(x => x.Id).ValueGeneratedOnAdd();
                e.Property(x => x.CreatedAtUtc).HasDefaultValueSql("now()");

                e.HasOne(x => x.Recipe)
                    .WithMany(r => r.Ratings)
                    .HasForeignKey(x => x.RecipeId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(x => x.User)
                    .WithMany(u => u.Ratings)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasIndex(x => new { x.UserId, x.RecipeId }).IsUnique();
                e.ToTable(t => t.HasCheckConstraint("CK_Ratings_Stars", "\"Stars\" >= 1 AND \"Stars\" <= 5"));
            });

            // ---------- Medal ----------
            builder.Entity<Medal>(e =>
            {
                e.Property(x => x.Id).ValueGeneratedOnAdd();
                e.Property(x => x.Code).HasMaxLength(40).IsRequired();
                e.Property(x => x.Name).HasMaxLength(80).IsRequired();
                e.Property(x => x.Description).HasMaxLength(300);
                e.Property(x => x.IconUrl).HasMaxLength(300);
                e.Property(x => x.CreatedAtUtc).HasDefaultValueSql("now()");
                e.HasIndex(x => x.Code).IsUnique();
            });

            // ---------- UserMedal ----------
            builder.Entity<UserMedal>(e =>
            {
                e.HasKey(x => new { x.UserId, x.MedalId });

                e.Property(x => x.AssignedAtUtc).HasDefaultValueSql("now()");

                e.HasOne(x => x.User)
                    .WithMany(u => u.UserMedals)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(x => x.Medal)
                    .WithMany(m => m.UserMedals)
                    .HasForeignKey(x => x.MedalId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(x => x.AssignedByUser)
                    .WithMany()
                    .HasForeignKey(x => x.AssignedByUserId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // ---------- WeeklyRankingEntry ----------
            builder.Entity<WeeklyRankingEntry>(e =>
            {
                e.Property(x => x.Id).ValueGeneratedOnAdd();
                e.Property(x => x.CreatedAtUtc).HasDefaultValueSql("now()");

                e.HasOne(x => x.User)
                    .WithMany(u => u.WeeklyRankingEntries)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasIndex(x => new { x.WeekStartDate, x.Position }).IsUnique();
                e.HasIndex(x => new { x.WeekStartDate, x.UserId }).IsUnique();
            });

        }


        public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
        public DbSet<Recipe> Recipes => Set<Recipe>();
        public DbSet<Ingredient> Ingredients => Set<Ingredient>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<RecipeIngredient> RecipeIngredients => Set<RecipeIngredient>();
        public DbSet<RecipeCategory> RecipeCategories => Set<RecipeCategory>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<Rating> Ratings => Set<Rating>();
        public DbSet<Medal> Medals => Set<Medal>();
        public DbSet<UserMedal> UserMedals => Set<UserMedal>();
        public DbSet<WeeklyRankingEntry> WeeklyRankingEntries => Set<WeeklyRankingEntry>();

    }
}
