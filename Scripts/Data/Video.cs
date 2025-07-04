using UnityEngine;
using UnityEngine.Video;

namespace MyThings.Data
{
    /// <summary>
    /// Represents a video that can be either a local MP4 file or a URL.
    /// </summary>
    [System.Serializable]
    public class Video
    {
        // Stores the raw MP4 video file as a Unity TextAsset (optional)
        [field: SerializeField] public VideoClip Mp4File { get; set; }

        // Stores the URL of the video (optional)
        [field: SerializeField] public string VideoUrl { get; set; }

        // Indicates whether this instance uses a file or a URL
        public bool IsUrl => !string.IsNullOrEmpty(VideoUrl);
    }
}