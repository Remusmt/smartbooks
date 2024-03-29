﻿using System.ComponentModel.DataAnnotations.Schema;

namespace SmartBooks.Domains.Entities
{
    public class Attachment : AppBaseEntity
    {
        public string RootPath { get; set; }
        public string ContentDisposition { get; set; }
        public string ContentType { get; set; }
        /// <summary>
        /// The filename as was when user uploads
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// A guid generated when saving attachment
        /// </summary>
        public string SystemFileName { get; set; }
        public string Extension { get; set; }
        public long Length { get; set; }
        public string Name { get; set; }

        [NotMapped]
        public string FullPath
        {
            get
            {
                return $"{RootPath}/uploads/{SystemFileName}{Extension}";
            }

        }
    }
}
