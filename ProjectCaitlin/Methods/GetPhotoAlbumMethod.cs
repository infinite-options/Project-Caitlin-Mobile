/*namespace Xamarin_GoogleAuth.Methods
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    public partial class GetPhotoAlbumMethod
    {
        [JsonProperty("albums")]
        public Album[] Albums { get; set; }
    }
    public partial class Album
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("productUrl")]
        public Uri ProductUrl { get; set; }
        [JsonProperty("mediaItemsCount")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long MediaItemsCount { get; set; }
        [JsonProperty("coverPhotoBaseUrl")]
        public Uri CoverPhotoBaseUrl { get; set; }
        [JsonProperty("coverPhotoMediaItemId")]
        public string CoverPhotoMediaItemId { get; set; }
    }
}*/

namespace ProjectCaitlin.Methods
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using ProjectCaitlin.Methods;

    public partial class GetPhotoAlbumMethod
    {
        [JsonProperty("mediaItems")]
        public MediaItem[] MediaItems { get; set; }
    }

    public partial class MediaItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("productUrl")]
        public Uri ProductUrl { get; set; }

        [JsonProperty("baseUrl")]
        public Uri BaseUrl { get; set; }

        [JsonProperty("mimeType")]
        public string MimeType { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("mediaMetadata")]
        public MediaMetadata MediaMetadata { get; set; }

        [JsonProperty("filename")]
        public string Filename { get; set; }
    }

    public partial class MediaMetadata
    {
        [JsonProperty("creationTime")]
        public DateTimeOffset CreationTime { get; set; }

        [JsonProperty("width")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Width { get; set; }

        [JsonProperty("height")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Height { get; set; }

        [JsonProperty("photo")]
        public Photo Photo { get; set; }
    }

    public partial class Photo
    {
    }
}
