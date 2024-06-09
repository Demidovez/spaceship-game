using UtilsSpace;

namespace EnemySpace
{
    public class EnemyFiring: Firing
    {
        protected override bool CheckCanFiring()
        {
            return true;
        }
    }
}