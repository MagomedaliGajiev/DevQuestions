using Tags.Domain;

namespace Tags;

public interface ITagsReadDbContext
{
    IQueryable<Tag> TagsRead { get; }
}