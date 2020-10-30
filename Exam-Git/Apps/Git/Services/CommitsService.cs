using Git.Data;
using Git.ViewModels.Commits;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Git.Services
{
    public class CommitsService : ICommitsService
    {
        private readonly ApplicationDbContext db;

        public CommitsService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public string Create(string description, string userId, string repositoryId)
        {
            var commit = new Commit()
            {
                Description = description,
                CreatedOn = DateTime.UtcNow,
                CreatorId = userId,
                RepositoryId = repositoryId
            };

            this.db.Commits.Add(commit);
            this.db.SaveChanges();

            return commit?.Id;
        }

        public bool Delete(string Id)
        {
            var commit = this.db.Commits.FirstOrDefault(x => x.Id == Id);

            if (commit == null)
            {
                return false;
            }

            this.db.Commits.Remove(commit);
            this.db.SaveChanges();

            return true;
        }

        public IEnumerable<CommitViewModel> GetAll(string userId)
        {
            return this.db.Commits
                .Where(x => x.CreatorId == userId)
                .Select(x => new CommitViewModel()
                {
                    Id = x.Id,
                    CreatedOn = x.CreatedOn,
                    RepositoryName = x.Repository.Name,
                    Description = x.Description
                }).ToList();
        }

        public bool IsCreator(string userId, string commitId)
        {
            var commit = this.db.Commits.FirstOrDefault(x => x.Id == commitId);

            if (commit == null || commit.CreatorId != userId)
            {
                return false;
            }

            return true;
        }
    }
}
