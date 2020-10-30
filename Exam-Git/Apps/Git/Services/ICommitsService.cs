using Git.ViewModels.Commits;
using System.Collections.Generic;

namespace Git.Services
{
    public interface ICommitsService
    {
        string Create(string description, string userId, string repositoryId);

        IEnumerable<CommitViewModel> GetAll(string userId);

        bool IsCreator(string userId, string commitId);

        bool Delete(string Id);
    }
}
