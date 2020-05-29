using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace docker_compose_api_mssql
{
    public class CoreContext : DbContext
    {
        public CoreContext() { }

        public CoreContext(DbContextOptions options)
            : base(options)
        {
        }

        public virtual DbSet<Post> Post { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
                    .AddJsonFile("appsettings.json", optional: false).Build();

                optionsBuilder.UseSqlServer(config["ConnectionString"]);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            #region Seeds

            string[] titles = new string[] {
                "本教程由Siegrain傾情奉獻?️",
                "感謝大家關註~",
                "博客地址為 http://siegrain.wang",
                "本教程Github地址為 https://github.com/Seanwong933/.NET-Core-with-Docker"
            };

            for (var i = 0; i < titles.Length; i++)
            {
                builder.Entity<Post>().HasData(new Post
                {
                    Id = i + 1,
                    Title = titles[i],
                    Content = Guid.NewGuid().ToString()
                });
            }

            #endregion
        }
    }
}