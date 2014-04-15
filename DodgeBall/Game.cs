using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisrupterBeam.DodgeBall {

    class Game {

        List<Ball> _balls;
        DateTime _gameStart;
        DateTime _lastTick;
        Random _random = new Random();
        bool _running;
        List<Team> _teams;

        public IEnumerable<Ball> Balls { get { return _balls; } }
        public Random Random { get { return _random; } }
        public IEnumerable<Team> Teams { get { return _teams; } }

        public Game(Team teamA, Team teamB, int numBalls) {
            _teams = new List<Team>(2);
            _teams.Add(teamA);
            _teams.Add(teamB);
            for(var ballNumber = 0 ; ballNumber < numBalls ; ballNumber++)
                _balls.Add(new Ball(this));
        }

        public void BallCaught(Player player, Ball ball) {
            // Signal to the ball that it was caught
            ball.CaughtBy(player);
            // Signal to the team that they caught a ball
            player.Team.BallCaught();
        }

        public void BallDeflected(Player player, Ball ball) {
            player.Team.BallDeflected();
            ball.Deflected();
        }

        public void BallDodged(Team dodgingTeam) {
            dodgingTeam.BallDodged();
        }

        public void PlayerHit(Player player, Ball ball) {
            // Signal to the ball that it was dropped
            ball.Dropped();
        }

        public void UpdateBahaviors() {
            foreach(var team in _teams) {
                if(team.ActivePlayers.Count() < 1) {
                    _running = false;
                }
            }
        }

        public void Start() {
            _running = true;
            while(_running) {
                // TODO: Handle input
                UpdateBahaviors();
                // TODO: Render
            }
        }
    }
}
