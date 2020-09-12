using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class Particle
    {
        public double velocity { get; set; }
        public double acceleration { get; set; }
        public double position { get; set; }
        
        public Boolean isObstacle { get; set; }

        public Particle(int a, int v, int p)
        {
            acceleration = a;
            velocity = v;
            position = p;

        }
        public Particle()
        {
            this.acceleration = 0;
            this.velocity = 0;
            this.position = 0;
            this.isObstacle = false;
        }

    }
}
