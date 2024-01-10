using Clock.Entities.Arrows;
using System;

namespace Clock.Entities.Clocks
{
    internal static class ArrowControllerExtensions
    {
        public static T SetController<T>(this ArrowController controller, params object[] param) where T : ArrowController
        { 
            T newController = (T)Activator.CreateInstance(typeof(T), param);
            controller = newController;
            return newController;
        }
    }
}
