using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisrupterBeam.DodgeBall {

    class Player {

        #region Constants

        const int CATCH_INTERVAL_MILLIS = 100;
        const int REST_INTERVAL_MILLIS = 50;

        #endregion

        #region Instance Variables

        Ball _ball;
        DateTime _catchAttemptStart;
        TimeSpan _catchInterval = new TimeSpan(0, 0, 0, 0, CATCH_INTERVAL_MILLIS);
        Game _game;
        bool _isCatching;
        bool _isIn;
        int _jerseyNumber;
        TimeSpan _restInterval = new TimeSpan(0, 0, 0, 0, REST_INTERVAL_MILLIS);
        Team _team;

        #endregion

        #region Public Properties

        public Ball Ball { get { return _ball; } }
        public bool HasBall { get { return _ball != null; } }
        public bool IsIn { get { return _isIn; } }
        public int JerseyNumber { get { return _jerseyNumber; } }
        public Team Team {
            get { return _team; }
            set { _team = value; }
        }

        #endregion

        #region Public Methods

        public void Initialize(Game game) {
            _game = game;
            bool _isIn = true;
            bool _isCatching = false;
            bool _hasBall = false;
        }

        public void BallArriving(Ball ball) {
            // Only allow catching/deflecting if player attempted catch
            // within the catch interval of the ball's arrival
            if(_isCatching && DateTime.Now.Subtract(_catchAttemptStart) < _catchInterval) {
                if(HasBall) {
                    // Signal to the game that the ball was deflected
                    _game.BallDeflected(this, ball);
                }
                else {
                    // Signal to game that player caught the ball
                    _game.BallCaught(this, ball);
                    _ball = ball;
                }
                // Player may instantly attempt a(nother) deflection
                // (It's hard to hit a player holding a ball)
                _isCatching = false;
            }
            else {
                _isIn = false;
                // Signal to game that player was hit
                _game.PlayerHit(this, ball);
            }
        }

        // Called when player or AI begins to try catching a ball
        public void TryCatch() {
            if(_isCatching) return;
            _isCatching = true;
            _catchAttemptStart = DateTime.Now;
        }

        // Called when the player or AI wants to drop the ball
        public void DropBall() {
            if(HasBall) {
                // If player was attempting to deflect, skip to end of catch interval,
                // making player wait until rest interval is complete until they can catch
                if(_isCatching)
                    _catchAttemptStart = DateTime.Now.Subtract(_catchInterval);
                // Signal to the ball that the player dropped it
                _ball.Dropped();
                _ball = null;
            }
        }

        public void UpdateBehaviors() {
            if(_isIn) {
                UpdateCatch();
                // Other behaviors here
            }
        }

        #endregion

        #region Private Methods

        void UpdateCatch() {
            if(_isCatching) {
                // After a catch attempt, player may not attempt a catch again
                // until waiting for the rest interval to pass
                if(DateTime.Now.Subtract(_catchAttemptStart) > _catchInterval + _restInterval)
                    _isCatching = false;
            }
        }

        void PickUpBall() {
            var ballsOnGround = _game.Balls.Where(o => o.OnGround);
            var numBalls = ballsOnGround.Count();
            if(numBalls > 0)
                _ball = ballsOnGround.ElementAt(_game.Random.Next(numBalls));
        }

        Player SelectTarget() {
            var target = null as Player;
            foreach(var team in _game.Teams)
                if(team != _team) {
                    var activePlayers = team.ActivePlayers;
                    target = activePlayers.ElementAt(_game.Random.Next(activePlayers.Count()));
                    break;
                }
            return target;
        }

        #endregion
    }
}
