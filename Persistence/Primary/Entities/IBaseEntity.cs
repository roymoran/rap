using System;
namespace RAP.Persistence.Primary.Entities
{
    public interface IBaseEntity
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
