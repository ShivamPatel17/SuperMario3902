using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using TechSupportMario.SoundItems;

namespace TechSupportMario
{
    class MuteCommand : ICommand
    {
        private readonly Sound sound;
        public MuteCommand(Sound sound)
        {
            this.sound = sound;
        }

        public void Execute()
        {
            if (!MediaPlayer.IsMuted)
            {
                sound.Mute();
            }
            else
            {
                sound.UnMute();
            }
        }
    }
}
