using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ImgCognitiveWeb.Models
{
    public class ImageStorageContext : DbContext
    {
        public ImageStorageContext (DbContextOptions<ImageStorageContext> options)
            : base(options)
        {
        }

        public DbSet<ImgCognitiveWeb.Models.Picture> Picture { get; set; }
    }
}
