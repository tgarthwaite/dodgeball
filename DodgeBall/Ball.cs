using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisrupterBeam.DodgeBall {

    class Ball {

        const int MIN_THROW_MILLIS = 500;
        const int MAX_THROW_MILLIS = 1000;

        Game _game;
        Player _holder;
        bool _inAir;
        Player _target;
        DateTime _throwStart;
        TimeSpan _travelTime;

        public Player Holder { get { return _holder; } }
        public Player Target { get { return _target; } }
        public bool InAir { get { return _inAir; } }
        public bool OnGround { get { return !_inAir && _holder == null; } }

        public Ball(Game game) {
            _game = game;
            _holder = null;
            _inAir = false;
            _target = null;
        }

        public void Caught() {
            _holder = _target;
            _inAir = false;
            _target = null;
        }

        public void Dropped() {
            _holder = null;
            _inAir = false;
            _target = null;
        }

        public void HitGround() {
            _inAir = false;
            if(_target != null) _game.BallDodged(_target.Team);
            _target = null;
        }

        public void Deflected() {
            // TODO: Randomly determine new target / ground?
            // For now, just make it hit the ground right away (player dodged the ball)
            HitGround();
        }

        public void ThrownAt(Player target) {
            _holder = null;
            _inAir = true;
            _target = target;
            _throwStart = DateTime.Now;
            _travelTime = new TimeSpan(0, 0, 0, 0, _game.Random.Next(MIN_THROW_MILLIS, MAX_THROW_MILLIS));
        }

        public void UpdateBehavior() {
            if(_inAir && DateTime.Now.Subtract(_throwStart) > _travelTime)
                _target.BallArriving(this);
        }
    }
}
