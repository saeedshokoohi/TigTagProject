//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TigTag.DataModel.model
{
    using System;
    using System.Collections.Generic;

    public partial class ImageTable : BaseEntity
    {
        public ImageTable()
        {
            this.Pages = new HashSet<Page>();
            this.Users = new HashSet<User>();
        }
        public ImageTable(Guid _Id, string _ImageName, string _ImageType, byte[] _ThumbnailData)
        {
            this.Id = _Id;
            this.ImageData = null;
            this.ImageName = _ImageName;
            this.ImageType = _ImageType;
            this.ThumbnailData = _ThumbnailData;
            this.Pages = new HashSet<Page>();
            this.Users = new HashSet<User>();

        }

        public System.Guid Id { get; set; }
        public string ImageName { get; set; }
        public string ImageType { get; set; }
        public byte[] ImageData { get; set; }
        public byte[] ThumbnailData { get; set; }

        public virtual ICollection<Page> Pages { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
