import { HttpParams } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { Statistics } from "src/app/_models/statistics";
import { MatchService } from "src/app/_services/match.service";
import { NotifyService } from "src/app/_services/notify.service";

@Component({
  selector: "app-played-match",
  templateUrl: "./played-match.component.html",
  styleUrls: ["./played-match.component.css"],
})
export class PlayedMatchComponent implements OnInit {
  homeTeam: any = [];
  awayTeam: any = [];
  matchdayId: number;
  matchInformation: any;
  upperSection: any;

  constructor(
    private matchService: MatchService,
    private route: ActivatedRoute,
    private router: Router,
    private notify: NotifyService
  ) {}

  ngOnInit() {
    this.route.params.subscribe((params: HttpParams) => {
      this.matchdayId = params["matchdayId"];
      this.matchService.getOrganizedMatchInformation(this.matchdayId).subscribe(
        (res) => {
          if (res.length) {
            this.matchInformation = res;
            this.mapOrganizedMatchToTeams(this.matchInformation);
            this.upperSection = this.getUpperSection();
          } else {
            this.notify.showError('Match has not been played yet');
            this.router.navigate(["/app/match-history"], { relativeTo: this.route });
          }

        },
        (err) => {
          this.notify.showError(err);
          this.router.navigate(["/app/match-history"], { relativeTo: this.route });
        }
      );
    });
  }

  private mapOrganizedMatchToTeams(res: any) {
    res.forEach((element) => {
      var player = {
        username: element.username,
        statistics: new Statistics(
          element.goals,
          element.assists,
          element.minutesPlayed
        ),
        mainPhoto: element.image
          ? {
              image: element.image,
            }
          : null,
        firstname: element.firstname,
        lastname: element.lastname,
        created: element.created,
        id: element.userId
      };
      if (element.homeId) {
        this.addPlayerToTeam("home", player);
      }

      if (element.awayId) {
        this.addPlayerToTeam("away", player);
      }
    });
  }

  private addPlayerToTeam(team: string, player: any) {
    if (team === "home") {
      this.homeTeam.push(player);
    }

    if (team === "away") {
      this.awayTeam.push(player);
    }
  }

  public getMatchPlayedLocation() {
    if (!this.matchInformation) {
      return null;
    }
    return this.matchInformation.pop().place;
  }

  public getUpperSection() {
    return {
      homeGoals: this.matchInformation[0].homeGoals,
      awayGoals: this.matchInformation[0].awayGoals,
      homeScorers: this.getHomeScorers(),
      awayScorers: this.getAwayScorers()
    }
  }

  private getHomeScorers() {
    var homeScorers = [];
    this.matchInformation.forEach(element => {
      if (element.goals > 0 && element.homeId !== null) {
        homeScorers.push({
          name: element.firstname + " " + element.lastname,
          goals: element.goals
        })
      }
    });

    return homeScorers;
  }
  
  private getAwayScorers() {
    var awayScorers = [];
    this.matchInformation.forEach(element => {
      if (element.goals > 0 && element.awayId !== null) {
        awayScorers.push({
          name: element.firstname + " " + element.lastname,
          goals: element.goals
        })
      }
    });
    
    return awayScorers;
  }
}
