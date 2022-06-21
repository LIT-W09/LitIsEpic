using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace LITIsEpic.Data
{
    public class ImagesRepository
    {
        private readonly string _connectionString;

        public ImagesRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int Add(string fileName)
        {
            var image = new Image
            {
                FileName = fileName,
                TimesViewed = 0
            };
            using var context = new ImagesDataContext(_connectionString);
            context.Images.Add(image);
            context.SaveChanges();
            return image.Id;
        }

        public void UpdateImageCount(int id)
        {
            using var context = new ImagesDataContext(_connectionString);
            context.Database.ExecuteSqlInterpolated($"UPDATE Images SET TimesViewed = TimesViewed + 1 WHERE Id = {id}");
        }

        public Image GetRandomImage()
        {
            using var context = new ImagesDataContext(_connectionString);
            return context.Images.OrderBy(_ => Guid.NewGuid()).First();
        }

        public Image GetById(int id)
        {
            using var context = new ImagesDataContext(_connectionString);
            return context.Images.FirstOrDefault(i => i.Id == id);
        }
    }
}
