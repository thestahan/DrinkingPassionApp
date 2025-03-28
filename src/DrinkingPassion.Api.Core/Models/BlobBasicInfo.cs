﻿using System.IO;

namespace DrinkingPassion.Api.Core.Models
{
    public class BlobBasicInfo
    {
        public BlobBasicInfo(Stream content, string contentType)
        {
            Content = content;
            ContentType = contentType;
        }

        public Stream Content { get; set; }
        public string ContentType { get; set; }
    }
}
