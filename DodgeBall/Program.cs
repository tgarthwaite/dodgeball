using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisrupterBeam.DodgeBall {
    class Program {
        const int BALLS_IN_GAME = 6;
        const int PLAYERS_PER_TEAM = 6;
        static void Main(string[] args) {
            var teamA = new Team("A-team");
            var teamB = new Team("B-team");
            for(var playerNumber = 0 ; playerNumber < PLAYERS_PER_TEAM ; playerNumber++) {
                teamA.AddPlayer(new Player());
                teamB.AddPlayer(new Player());
            }
            var game = new Game(teamA, teamB, BALLS_IN_GAME);
            game.Start();
            Debug.WriteLine("Game over");
        }
    }
}
