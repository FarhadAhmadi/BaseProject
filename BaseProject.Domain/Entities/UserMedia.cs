using BaseProject.Domain.Entities.Auth;
using BaseProject.Domain.Entities.Base;
using BaseProject.Domain.Enums;
using Swashbuckle.AspNetCore.Annotations;

namespace BaseProject.Domain.Entities
{
    /// <summary>
    /// Represents media uploaded by a user.
    /// </summary>
    public class UserMedia : BaseEntity
    {
        [SwaggerSchema("Type of the media, e.g., Image, Video.")]
        public MediaType Type { get; set; }

        [SwaggerSchema("Path to the media file.")]
        public string PathMedia { get; set; }

        [SwaggerSchema("Optional caption for the media.")]
        public string Caption { get; set; }

        [SwaggerSchema("Size of the file in bytes.")]
        public long FileSize { get; set; }

        [SwaggerSchema("Date the media was created.")]
        public DateTime DateCreated { get; set; }

        [SwaggerSchema("The user who uploaded this media.")]
        public ApplicationUser User { get; set; }
    }
}
