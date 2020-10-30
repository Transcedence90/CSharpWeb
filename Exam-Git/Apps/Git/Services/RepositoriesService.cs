using Git.Data;
using Git.ViewModels.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Git.Services
{
    public class RepositoriesService : IRepositoriesService
    {
        private readonly ApplicationDbContext db;

        public RepositoriesService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public string Create(RepositoryInputModel model, string userId)
        {
            var repository = new Repository()
            {
                Name = model.Name,
                OwnerId = userId,
                CreatedOn = DateTime.UtcNow,
                IsPublic = model.RepositoryType == "Public" ? true : false
            };

            this.db.Repositories.Add(repository);
            this.db.SaveChanges();

            return repository?.Id;
        }

        public IEnumerable<RepositoryViewModel> GetAllPublicRepositories()
        {
            return this.db.Repositories
                .Where(x => x.IsPublic)
                .Select(x => new RepositoryViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    OwnerName = x.Owner.Username,
                    CommitsCount = x.Commits.Count(),
                    CreatedOn = x.CreatedOn
                }).ToList();
        }

        public string GetNameById(string Id)
        {
            return this.db.Repositories.FirstOrDefault(x => x.Id == Id)?.Name;
        }
    }
}
