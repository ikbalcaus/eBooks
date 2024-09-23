using System;
using System.Collections.Generic;

namespace eBooksAPI.Database.Models
{
    public partial class PublisherFollow
    {
        public int FollowId { get; set; }
        public int? UserId { get; set; }
        public int? PublisherId { get; set; }
        public DateTime? FollowDate { get; set; }

        public virtual User? Publisher { get; set; }
        public virtual User? User { get; set; }
    }
}
