using System;
using GameStore.Api.Models;

namespace GameStore.Api.Data;

public class GameStoreData
{
    private readonly List<Genre> genres = [
        new Genre { Id = new Guid("d8a77400-e414-4e5a-a695-c3ec90ce6177"), Name = "Fighting" },
        new Genre { Id = new Guid("b8a77400-e414-4e5a-a695-c3ec90ce6177"), Name = "Roleplaying" },
        new Genre { Id = new Guid("c8a77400-e414-4e5a-a695-c3ec90ce6177"), Name = "Sports" },
        new Genre { Id = new Guid("d8a77400-e414-4e5a-a695-c3ec90ce6178"), Name = "Racing" }
    ];

    private readonly List<Game> games;

    public GameStoreData()
    {
        games =
        [
            new Game {
                Id = Guid.NewGuid(),
                Name = "Street Fighter II",
                Genre = genres[0],
                Price = 19.99m,
                ReleaseDate = new DateOnly(1992, 7, 15),
                Description = "Street Fighter 2, the most iconic fighting game of all time, is back on the Nintendo Switch! The newest iteration of SFII in nearly 10 years, Ultra Street Fighter 2 features all of the classic characters, a host of new single player and multiplayer features, as well as two new fighters: Evil Ryu and Violent Ken!"
            },
            new Game {
                Id = Guid.NewGuid(),
                Name = "Final Fantasy XIV",
                Genre = genres[3],
                Price = 59.99m,
                ReleaseDate = new DateOnly(2010, 9, 30),
                Description = "Join over 27 million adventurers worldwide and take part in an epic and ever-changing FINAL FANTASY. Experience an unforgettable story, exhilarating battles, and a myriad of captivating environments to explore." },
            new Game {
                Id = Guid.NewGuid(),
                Name = "FIFA 23",
                Genre = genres[4],
                Price = 69.99m,
                ReleaseDate = new DateOnly(2022, 9, 27),
                Description = "FIFA 23 brings The World's Game to the pitch, with HyperMotion2 Technology, men's and women's FIFA World Cup™, women's club teams, cross-play features, and more." }
        ];
    }

    public IEnumerable<Game> GetGames() => games;
}
