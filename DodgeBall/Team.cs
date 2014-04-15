using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisrupterBeam.DodgeBall {

    class Team {

        #region Instance Variables

        int _caughtBalls;
        int _deflectedBalls;
        int _dodgedBalls;
        Game _game;
        string _name;
        List<Player> _players;

        #endregion

        #region Public Properties

        public string Name { get { return _name; } }
        public IEnumerable<Player> Players { get { return _players; } }
        public IEnumerable<Player> ActivePlayers { get { return _players.Where(o => o.IsIn); } }

        #endregion

        #region Public Methods

        public Team(string name) {
            _name = name;
            _players = new List<Player>();
        }

        public void Initialize(Game game) {
            _caughtBalls = 0;
            _dodgedBalls = 0;
            _game = game;
            foreach(var player in _players)
                player.Initialize(game);
        }

        // Returns the player's jersey number
        public int AddPlayer(Player player) {
            player.Team = this;
            _players.Add(player);
            return _players.Count();
        }

        public void BallCaught() {
            _caughtBalls++;
        }

        public void BallDeflected() {
            _deflectedBalls++;
        }

        public void BallDodged() {
            _dodgedBalls++;
        }

        #endregion
    }
}
