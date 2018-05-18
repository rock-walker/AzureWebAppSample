using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using  BookStore.Models;
using System.Data.Entity;
using Microsoft.ApplicationInsights;
using BookStore.App_Start;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace BookStore.Controllers
{
    public class SoccerController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        SoccerContex db = new SoccerContex();
        private TelemetryClient tc = new TelemetryClient();
        private ISearchIndexClient _soccerIndex = AzureSearch.SoccerIndexClient;

		protected override void Dispose(bool disposing)
		{
			if (db != null)
			{
				db.Dispose();
				db = null;
			}
			base.Dispose(disposing);
		}

		public ActionResult Filtes(int? team, string position)
        {
            IQueryable<Player> players = db.Players.Include(p => p.Team);
            if (team != null && team != 0)
            {
                players = players.Where(p => p.TeamId == team);
            }
            if (!String.IsNullOrEmpty(position) && !position.Equals("Все"))
            {
                players = players.Where(p => p.Position == position);
            }

            List<Team> teams = db.Teams.ToList();
            // устанавливаем начальный элемент, который позволит выбрать всех
            teams.Insert(0, new Team { Name = "Все", Id = 0 });

	        PlayersListViewModel plvm = new PlayersListViewModel
	        {
		        Players = players.ToList(),
		        Teams = new SelectList(teams, "Id", "Name"),
		        Positions = new SelectList(new List<string>()
		        {
			        "Все",
			        "Нападающий",
			        "Полузащитник",
			        "Защитник",
			        "Вратарь"
		        })
	        };
            return View(plvm);
        }
        
        
        
        // GET: Soccer
        public ActionResult Index()
        {
            var players = db.Players.Include(p => p.Team);
            return View(players.ToList());
        }

        public ActionResult TeamDetails(int? id)
        {
            id = 2;//?
            if (id == null)
            {
                return HttpNotFound();
            }
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            team.Players = db.Players.Where(m => m.TeamId == team.Id);
            return View(team);
        }

        [HttpGet]
        public ActionResult Create()
        {
            // Формируем список команд для передачи в представление
            SelectList teams = new SelectList(db.Teams, "Id", "Name");
            ViewBag.Teams = teams;
            return View();
        }

        [HttpPost]
        public ActionResult Create(Player player)
        {
            if (string.IsNullOrEmpty(player.Name))
            {
                log.Error("Player name shouldn't be empty");
                throw new ArgumentException("Player name shouldn't be empty");
            }
            player.Id = Guid.NewGuid();
            db.Players.Add(player);
            db.SaveChanges();

            var actions = new IndexAction<Player>[]
            {
                IndexAction.Upload(player)
            };
            var batch = IndexBatch.New(actions);

            try
            {
                 _soccerIndex.Documents.Index(batch);
            }
            catch (IndexBatchException e)
            {
                // Sometimes when your Search service is under load, indexing will fail for some of the documents in
                // the batch. Depending on your application, you can take compensating actions like delaying and
                // retrying. For this simple demo, we just log the failed document keys and continue.
                Console.WriteLine(
                    "Failed to index some of the documents: {0}",
                    String.Join(", ", e.IndexingResults.Where(r => !r.Succeeded).Select(r => r.Key)));
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(string guid)
        {
            var id = Guid.Parse(guid);
            if (id == null)
            {
                return HttpNotFound();
            }
            // Находим в бд футболиста
            Player player = db.Players.Find(id);
            if (player != null)
            {
                // Создаем список команд для передачи в представление
                SelectList teams = new SelectList(db.Teams, "Id", "Name", player.TeamId);
                ViewBag.Teams = teams;
                return View(player);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(Player player)
        {
            tc.TrackEvent("Edit Soccer Player", new Dictionary<string, string>{ {"new Player name", player.Name} });
            db.Entry(player).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}