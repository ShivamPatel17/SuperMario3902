using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace TechSupportMario.SoundItems
{
    class Sound
    {
        public Sound(Song song, bool repeating)
        {
            MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = repeating;
            MediaPlayer.Volume = .4f;
        }
        public void Mute()
        {
            MediaPlayer.IsMuted = true;
            SoundEffect.MasterVolume = 0.0f;
        }
        public void UnMute()
        {
            MediaPlayer.IsMuted = false;
            SoundEffect.MasterVolume = 1f;
        }

        public void Reset()
        {
            MediaPlayer.MoveNext();
        }
    }
}
