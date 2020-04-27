using Microsoft.Xna.Framework.Audio;

namespace TechSupportMario.SoundItems
{
    class SoundEvent
    {
        public SoundEvent(SoundEffect noise)
        {
            noise.Play();
        }
    }
}
