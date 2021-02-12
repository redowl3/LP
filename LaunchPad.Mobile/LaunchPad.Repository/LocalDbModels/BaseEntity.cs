using SQLite;
using System;

namespace LaunchPad.Repository.LocalDbModels
{
    public class BaseEntity
    {
        [PrimaryKey, AutoIncrement]
        public int? Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public bool Deleted { get; set; }
        public BaseEntity()
        {
            CreatedAt = DateTime.Now;
            ModifiedAt = DateTime.Now;
        }
    }
}
