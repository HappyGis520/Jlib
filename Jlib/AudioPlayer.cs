using System;
using System.Windows.Media;

namespace Jlib
{
    /// <summary>
    /// 音频播放器类
    /// </summary>
    public class AudioPlayer
    {
        private static MediaPlayer _player;
        public static void Initialize()
        {
            _player = new MediaPlayer();
        }   
        public static void PlayMp3(string filePath)
        {
            try
            {
                _player.Open(new Uri(filePath, UriKind.RelativeOrAbsolute));
                _player.Volume = 1.0;
                _player.Play();
            }
            catch (Exception ex)
            {
                // 你可以记录或提示错误
                Console.WriteLine($"播放MP3出错: {ex.Message}");
            }
        }
    }

}
