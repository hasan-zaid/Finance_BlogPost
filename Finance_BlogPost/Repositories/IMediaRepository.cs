namespace Finance_BlogPost.Repositories
{
    public interface IMediaRepository
    {
        Task<string> UploadAsync(IFormFile file);
    }
}
