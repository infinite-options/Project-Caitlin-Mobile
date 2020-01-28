namespace ProjectCaitlin.Methods
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
}
