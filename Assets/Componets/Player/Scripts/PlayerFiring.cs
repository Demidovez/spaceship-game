using UtilsSpace;

namespace PlayerSpace
{
    public class PlayerFiring: Firing
    {
        protected override bool CheckCanFiring()
        {
            return Player.Instance.IsFiring;
        }
    }
}