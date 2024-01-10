using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clock
{
    internal interface IGameModeDependence
    {
        public void OnGameStateChange(GameMode currentGameMode);
    }
}
